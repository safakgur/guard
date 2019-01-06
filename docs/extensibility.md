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
        if (argument.HasValue() && // Ignore if the GUID is null.
            argument.Value.Value == default) // Check whether the GUID is empty.
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
* We used the [HasValue](#the-hasvalue-method) method to check whether the GUID is null.
* We ignored the arguments that are null.
* The rest is the same with our non-nullable validation.

## Accepting and Returning the Argument by Reference

Being a struct, `ArgumentInfo<T>` is subject to copy-by-value semantics. This means that it would
get copied once to send it as a parameter, and once to return it to the caller with each validation.
Think of a validation chain like `.NotNull().CountInRange(1, 5).DoesNotContainNull()`.
This would cause our argument instance to be copied six times if we didn't accept and returned
it as reference.

Sending and returning values as reference add a small overhead but it's negligible for values
heavier than four bytes and the benefits start to overweight this overhead as the value gets bigger.
An `ArgumentInfo<T>` instance contains three fields:
* The value of the argument of type `T`.
* A string that contains the argument name.
* A boolean that is used to determine whether the argument is [modified][3].
* A boolean that is used to determine whether the exception messages should not contain [sensitive
  information][4].

So an `ArgumentInfo<int>` instance on a 32-bit system is _at least_ 10 bytes and an
`ArgumentInfo<long>` instance on a 64-bit system is _at least_ 18 bytes. Even more if we use heavier
structs like a `Guid` or `decimal`. So accepting and returning our validation arguments as reference
allows us to avoid copying heavier instances around.

## The HasValue Method

In our examples above where we specifically targeted GUID arguments, we could just check whether the
argument is null by writing `argument.Value != null`.  Using `argument.HasValue()` here made no
difference. But if we targeted a generic argument `T` where `T` is a struct, the `argument.Value != null` check
would cause boxing.

```c#
public interface IDuck
{
    bool CanQuack { get; }

    string Quack();
}

public class RefDuck : IDuck { /*...*/ }

public struct ValueDuck : IDuck { /*...*/ }

public static class GuardExtensions
{
    public static ref readonly Guard.ArgumentInfo<T> CanQuack<T>(
        in this Guard.ArgumentInfo<T> argument)
        where T : IDuck
    {
        // Writing `argument.Value != null` here would box a `ValueDuck`.
        if (argument.HasValue() && !argument.Value.CanQuack)
        {
            // Throw is it is a non-null duck who sadly cannot quack.
            throw Guard.Fail(new ArgumentException(
                $"{argument.Name} must be able to quack.", argument.Name));
        }

        return ref argument;
    }
}

public class Program
{
    public static void Main()
    {
        var refDuck = new RefDuck();
        MakeItQuack(refDuck);

        var valueDuck = new ValueDuck();
        MakeItQuack(valueDuck); // No boxing.
    }

    public static void MakeItQuack<T>(T duck)
        where T : IDuck
    {
        Guard.Argument(() => duck).CanQuack();

        Console.WriteLine(duck.Quack());
    }
}
```

[1]: design-decisions.md#guarding-scopes
[2]: design-decisions.md#optional-preconditions
[3]: design-decisions.md#modifying-arguments
[4]: design-decisions.md#secure-arguments
