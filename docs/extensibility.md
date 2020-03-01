# Guard - Extensibility

This document describes how to add custom validations to Guard by writing simple extension methods.

## A Basic Validation

Here is a basic extension that throws an `ArgumentException` if a GUID argument is passed
uninitialized. It is not included among the standard validations because the `NotDefault` method
defined for structs covers its functionality.

```c#
public static class GuardExtensions
{
    public static ref readonly Guard.ArgumentInfo<Guid> NotEmpty(
        in this Guard.ArgumentInfo<Guid> argument)
    {
        if (argument.Value == default) // Check whether the GUID is empty.
        {
            throw Guard.Fail(new ArgumentException(
                $"{argument.Name} is not initialized. " +
                "Consider using the static Guid.NewGuid method.",
                argument.Name));
        }

        return ref argument;
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

What Did We Do?

* We wrote an extension method for `ArgumentInfo<Guid>`.
* We accepted the argument as a [readonly reference](#accepting-and-returning-the-argument-by-reference)
  and returned the same reference.
* We passed the argument name to the `ArgumentException`, also mentioning it in the exception message.
* We passed the exception to `Guard.Fail` before throwing it to support [scopes][1].

What if the argument was nullable?

```c#
public class Program
{
    public Record GetRecord(Guid? id)
    {
        // This won't compile since the id is not a Guid, it's a Nullable<Guid>.
        Guard.Argument(() => id).Valid();
    
        // Calling NotNull converts the ArgumentInfo<Guid?> to an ArgumentInfo<Guid>.
        // After that we can use our NotEmpty extension.
        Guard.Argument(() => id).NotNull().NotEmpty();
    }
}
```

But forcing the argument to be non-null contradicts the convention followed by the standard
validations where null arguments are ignored. See the [relevant section in the design decisions][2]
for the rationale.

Let's add an overload to our extension, this time specifically for nullable GUIDs.

```c#
public static class GuardExtensions
{
    public static ref readonly Guard.ArgumentInfo<Guid?> NotEmpty(
        in this Guard.ArgumentInfo<Guid?> argument)
    {
        // Ignore if the GUID is null.
        if (argument.TryGetValue(out var value) && value == default)
        {
            throw Guard.Fail(new ArgumentException(
                $"{argument.Name} is not initialized. " +
                "Consider using the static Guid.NewGuid method.",
                argument.Name));
        }

        return ref argument;
    }
}

public class Program
{
    public Record GetRecord(Guid? id)
    {
        // Ignored if `id` is null.
        Guard.Argument(() => id).NotEmpty();
    }
}
```

What Did We Do?

* We wrote an extension method for `ArgumentInfo<Guid?>`.
* We checked whether the GUID is null and skipped the validation if it is so.
* The rest is the same with our non-nullable validation.

[1]: design-decisions.md#guarding-scopes
[2]: design-decisions.md#optional-preconditions
