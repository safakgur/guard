# Guard - Extensibility

This document describes how to add custom validations to Guard by writing simple extension methods.

## A Basic Validation

Here is a basic extension that throws an `ArgumentException` if a GUID argument is
passed uninitialized. It is not included among the standard validations because the
`NotDefault` method defined for `IEquatable<T>` arguments covers its functionality.

```c#
public static class GuardExtensions
{
    public static Guard.ArgumentInfo<Guid> NotEmpty(this Guard.ArgumentInfo<Guid> argument)
    {
        if (argument.Value == default)
            throw new ArgumentException(
                $"{argument.Name} is not initialized. " +
                "Consider using the static Guid.NewGuid method.",
                argument.Name);

        return argument;
    }
}

public class Program
{
    public Record GetRecord(Guid id)
    {
        Guard.Argument(() => id).NotEmpty();
    }
}
```

But what if the argument was nullable?

```c#
public class Program
{
    public Record GetRecord(Guid? id)
    {
        // This won't compile since the id is not a Guid, it's a Nullable<Guid>.
        Guard.Argument(() => id).Valid();
    
        // We can do this instead:
        Guard.Argument(() => id)
            // Calling NotNull converts the ArgumentInfo<Guid?> to an ArgumentInfo<Guid>.
            .NotNull()
            // After that we can use our NotEmpty extension.
            .NotEmpty();
    }
}
```

In the above example we forced the argument to be non-null. Standard validations
in Guard follow a convention where the validations on null arguments are ignored.
See the [relevant section in the design decisions][1] for the rationale.

Let's write another extension to support nullable GUIDs.

```c#
public static class GuardExtensions
{
    // This time we accept and return a Nullable<Guid>.
    public static Guard.ArgumentInfo<Guid?> NotEmpty(this Guard.ArgumentInfo<Guid?> argument)
    {
        // NotNull gets an `ArgumentInfo<Guid>` if the argument value
        // is not null, so we can call our original NotEmpty extension.
        if (argument.NotNull(out var a))
            a.NotEmpty();

        return argument;
    }
}

public class Program
{
    public Record GetRecord(Guid? id)
    {
        // Will only validate id if it is not null.
        Guard.Argument(() => id).NotEmpty();
    }
}
```

## Null Arguments

`ArgumentInfo<T>` provides two methods that you can use to check whether the
value is null: `IsNull` and its opposite, `HasValue`. The reason why these
methods are recommended over direct checks like `argument.Value != null`
is because they don't cause boxing when the argument value is a struct.

```c#
public interface IFoo
{
    bool Bar();
}

public static class GuardExtensions
{
    public static Guard.ArgumentInfo<T> Bar<T>(this Guard.ArgumentInfo<T> argument)
        where T : IFoo
    {
        // Ignore if the value is null.
        if (argument.HasValue() && !argument.Value.Bar())
            throw new ArgumentException($"{argument.Name} must bar.", argument.Name);

        return argument;
    }
}

public class Program
{
    public void FooBar(IFoo foo)
    {
        // Ignored if foo is null.
        // Throws if it's not *and* foo.Bar() returns false
        Guard.Argument(() => foo).Bar();
    }
}
```

## Accepting and Returning the Argument by Reference

In the following example, we accept the argument using the `in` keyword which makes the
caller to pass the argument as a readonly reference. So the argument and therefore, its
value, are not copied for the method call. We also mark the return type as `ref readonly`,
so the return value is also a reference to the argument we accepted with the `in` keyword.
You can do this to prevent copying of large struct values with each validation.

```c#
public static class GuardExtensions
{
    public static ref readonly Guard.ArgumentInfo<MyLargeStruct> Valid(
        in this Guard.ArgumentInfo<MyLargeStruct> argument,
        Func<MyLargeStruct, string> message = null)
    {
        if (! /** Custom validation logic. **/)
        {
            var m = message?.Invoke(argument.Value)
                ?? $"{argument.Name} is not a valid MyLargeStruct.";

            throw new ArgumentException(m, argument.Name);
        }

        return ref argument;
    }
}

public class Program
{
    public void Process(MyLargeStruct value)
    {
        // Initialize the guarded argument.
        // The value is copied once.
        Guard.Argument(() => value)
            // Call the extension, the value is not copied.
            .Valid()
            // Chain any standard validation that accepts the
            // argument by ref and the value is still not copied.
            .NotEqual(MyLargeStruct.InvalidValue);
    }
}
```

We can do the same as we did for our GUID extension to support nullable `MyLargeStruct`
arguments. But if we made this much effort to pass our argument by reference, calling
`NotNull(out var a)` may not be desirable since it will copy the argument value.
Here is an example that supports both regular and nullable `MyLargeStruct`
arguments without copying their values:

```c#
public static class GuardExtensions
{
    public static ref readonly Guard.ArgumentInfo<MyLargeStruct> Valid(
        in this Guard.ArgumentInfo<MyLargeStruct> argument,
        Func<MyLargeStruct, string> message = null)
    {
        Valid(argument.Value, argument.Name, message);
        return ref argument;
    }

    public static ref readonly Guard.ArgumentInfo<MyLargeStruct?> Valid(
        in this Guard.ArgumentInfo<MyLargeStruct?> argument,
        Func<MyLargeStruct, string> message = null)
    {
        if (argument.HasValue())
            Valid(argument.Value.Value, argument.Name, message);

        return ref argument;
    }

    private static void Valid(
        in MyLargeStruct value,
        string name,
        Func<MyLargeStruct, string> message)
    {
        if (! /** Custom validation logic. **/)
        {
            var m = message?.Invoke(value)
                ?? $"{name} is not a valid MyLargeStruct.";

            throw new ArgumentException(m, name);
        }
    }
}
```

[1]: design-decisions.md#optional-preconditions
