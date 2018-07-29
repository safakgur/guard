# Guard

[![NuGet Status](https://img.shields.io/nuget/v/Dawn.Guard.svg?style=flat)](https://www.nuget.org/packages/Dawn.Guard/)

Guard is a fluent argument validation library that is intuitive, fast and extensible.

* [Introduction](#introduction)
* [What's Wrong with Vanilla?](#whats-wrong-with-vanilla)
* [Requirements](#requirements)
* [Standard Validations](#standard-validations)
* [Design Decisions][1]
* [Extensibility][2]

## Introduction

Here is a sample constructor that validates its arguments without Guard:

```c#
public Person(string firstName, string lastName)
{
    if (firstName == null)
        throw new ArgumentNullException(
            nameof(firstName), "The first name cannot be null.");

    if (firstName.Length == 0)
        throw new ArgumentException(
            "The first name cannot be empty.", nameof(firstName));

    if (lastName == null)
        throw new ArgumentNullException(
            nameof(lastName), "The last name cannot be null.");

    if (lastName.Length == 0)
        throw new ArgumentException(
            "The last name cannot be empty.", nameof(lastName));

    this.firstName = firstName;
    this.lastName = lastName;
}
```

And this is how we write the same constructor with Guard:

```c#
public Person(string firstName, string lastName)
{
    this.firstName = Guard.Argument(() => firstName).NotNull().NotEmpty();
    this.lastName = Guard.Argument(() => lastName).NotNull().NotEmpty();
}
```

If this looks like too much allocations to you, you can use the overload that
accepts the argument name as a separate parameter and write:
`Guard.Argument(firstName, nameof(firstName))`.

See the [design decisions][1] for details.

## What's Wrong with Vanilla?

There is nothing wrong with writing your own checks but when you have lots
of types you need to validate, the task gets very tedious, very quickly.

Let's analyze the example above.
* We have an argument (firstName) that we need to be a non-null, non-empty string.
* We check if it's null and throw an `ArgumentNullException` if it is.
* We then check if it's empty and throw an `ArgumentException` if it is.
* We specify the same parameter name for each validation.
* We write an error message for each validation.
* `ArgumentNullException` accepts the parameter name as its first argument and error message as its second while it's the other way around for the `ArgumentException`. An inconsistency that many of us sometimes find it hard to remember.

In reality, all we need to express should be the first bullet,
that we want our argument non-null and non-empty.

With Guard, if you want to guard an argument against null, you just write `NotNull`
and that's it. If the argument is passed null, you'll get an `ArgumentNullException`
thrown with the correct parameter name and a clear error message out of the box.
The [standard validations](#standard-validations) have fully documented, meaningful
defaults that get out of your way and let you focus on your business logic.

## Requirements

**C# 7.2 or later is required.**  
Guard takes advantage of almost all the new features introduced in C# 7.2.
So in order to use Guard, you need to make sure your Visual Studio is up to date
and you have `<LangVersion>7.2</LangVersion>` added in your .csproj file.

Supported targets:
* .NET Standard 1.0 and above - See [.NET Standard compatibility table][3].

## Standard Validations

### All Arguments

For `ArgumentInfo<T>`
* `Require(Func<T, bool>)`
* `Require<TException>(Func<T, bool>)`
* `Compatible<TTarget>()`
* `NotCompatible<TTarget>()`
* `Cast<TTarget>` - Returns an argument of `TTarget`

### Nullable Arguments

For `ArgumentInfo<T> where T : class` and `ArgumentInfo<T?> where T : struct`
* `Null()`
* `NotNull()` - When called for an argument of `T?`, returns an argument of `T`.

### Equatable Arguments

For `ArgumentInfo<T> where T : IEquatable<T>`
* `Equal(T)`
* `NotEqual(T)`

For `ArgumentInfo<T> where T : struct, IEquatable<T>`
* `Default()`
* `NotDefault()`

### Comparable Arguments

For `ArgumentInfo<T> where T : IComparable<T>`
* `Min(T)`
* `Max(T)`
* `InRange(T, T)`

For `ArgumentInfo<T> where T : struct, IComparable<T>`
* `Zero()`
* `NotZero()`
* `Positive()`
* `Negative()`

### Collection Arguments

For `ArgumentInfo<T> where T : IEnumerable`
* `Empty()`
* `NotEmpty()`
* `MinCount(int)`
* `MaxCount(int)`
* `CountInRange(int, int)`
* `ContainsNull()`
* `DoesNotContainNull()`

For `ArgumentInfo<TCollection> where TCollection : IEnumerable<TItem>`
* `Contains(TItem)`
* `DoesNotContain(TItem)`

### String Arguments

For `ArgumentInfo<string>`
* `Empty()`
* `NotEmpty()`
* `WhiteSpace()`
* `NotWhiteSpace()`
* `MinLength(int)`
* `MaxLength(int)`
* `LengthInRange(int, int)`
* `Equal(string, StringComparison)`
* `NotEqual(string, StringComparison)`

### Boolean Arguments

For `ArgumentInfo<bool>`
* `True()`
* `False()`

### URI Arguments

For `ArgumentInfo<Uri>`
* `Absolute`
* `Relative`
* `Scheme(string)`
* `Http()`
* `Http(bool)`
* `Https()`

### Enum Arguments

For `ArgumentInfo<T> where T : struct, IComparable, IFormattable, IConvertible`
* `Enum()` - Returns an `EnumArgumentInfo<T>`.

For `EnumArgumentInfo<T>`
* `Defined()`
* `None()`
* `NotNone()`
* `Equal(T)`
* `NotEqual(T)`
* `HasFlag(T)`
* `DoesNotHaveFlag(T)`

### Object Arguments

For `ArgumentInfo<object>`
* `Type<T>()` - Returns an argument of `T`.
* `NotType<T>()`
* `Type(Type)`
* `NotType(Type)`

### Modifications

For `ArgumentInfo<T>`
* `Modify(T value)`
* `Modify<TTarget>(Func<T, TTarget>)` - Returns an argument of `TTarget`
* `Wrap<TTarget>(Func<T, TTarget>)` - Returns an argument of `TTarget`

For `ArgumentInfo<T> where T : class, ICloneable`
* `Clone()`

[1]: docs/design-decisions.md
[2]: docs/extensibility.md
[3]: https://docs.microsoft.com/dotnet/standard/net-standard
