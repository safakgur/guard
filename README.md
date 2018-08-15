# Guard

[![NuGet Status](https://img.shields.io/nuget/v/Dawn.Guard.svg?style=flat)](https://www.nuget.org/packages/Dawn.Guard/)
[![Build status](https://ci.appveyor.com/api/projects/status/add0vx8i2yacvprf/branch/master?svg=true)](https://ci.appveyor.com/project/safak/guard/branch/master)

![Logo](media/guard-64.png)

Guard is a fluent argument validation library that is intuitive, fast and extensible.

* [Introduction](#introduction)
* [What's Wrong with Vanilla?](#whats-wrong-with-vanilla)
* [Requirements](#requirements)
* [Standard Validations](#standard-validations)
* [Design Decisions][1]
* [Extensibility][2]
* [Future](#future)

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

If this looks like too much allocations to you, you can use the overload that accepts the argument
name as a separate parameter and write: `Guard.Argument(firstName, nameof(firstName))`.

See the [design decisions][1] for details.

## What's Wrong with Vanilla?

There is nothing wrong with writing your own checks but when you have lots of types you need to
validate, the task gets very tedious, very quickly.

Let's analyze the example above.
* We have an argument (firstName) that we need to be a non-null, non-empty string.
* We check if it's null and throw an `ArgumentNullException` if it is.
* We then check if it's empty and throw an `ArgumentException` if it is.
* We specify the same parameter name for each validation.
* We write an error message for each validation.
* `ArgumentNullException` accepts the parameter name as its first argument and error message as its
second while it's the other way around for the `ArgumentException`. An inconsistency that many of us
sometimes find it hard to remember.

In reality, all we need to express should be the first bullet, that we want our argument non-null
and non-empty.

With Guard, if you want to guard an argument against null, you just write `NotNull` and that's it.
If the argument is passed null, you'll get an `ArgumentNullException` thrown with the correct
parameter name and a clear error message out of the box. The [standard validations](#standard-validations)
have fully documented, meaningful defaults that get out of your way and let you focus on your project.

## Requirements

**C# 7.2 or later is required.** Guard takes advantage of almost all the new features introduced in
C# 7.2. So in order to use Guard, you need to make sure your Visual Studio is up to date and you
have `<LangVersion>7.2</LangVersion>` or later added in your .csproj file.

**.NET Standard 1.0** and above are supported. [Microsoft Docs][3] lists the following platform
versions as .NET Standard 1.0 compliant but keep in mind that currently, the unit tests are only
targeting .NET Core 1.0 and 2.0.

| Platform                   | Version |
| -------------------------- | ------- |
| .NET Core                  | `1.0`   |
| .NET Framework             | `4.5`   |
| Mono                       | `4.6`   |
| Xamarin.iOS                | `10.0`  |
| Xamarin.Mac                | `3.0`   |
| Xamarin.Android            | `7.0`   |
| Universal Windows Platform | `10.0`  |
| Windows                    | `8.0`   |
| Windows Phone              | `8.1`   |
| Windows Phone Silverlight  | `8.0`   |

## Standard Validations

Below is a complete list of validations that are included with the library.

### Null Guards

For `ArgumentInfo<T> where T : class` and `ArgumentInfo<T?> where T : struct`
* `Null()`
* `NotNull()` - When called for an argument of `T?`, returns an argument of `T`.

### Equality Guards

For `ArgumentInfo<T>`
* `Equal(T)`
* `Equal(T, IEqualityComparer<T>)`
* `NotEqual(T)`
* `NotEqual(T, IEqualityComparer<T>)`

For `ArgumentInfo<T|T?> where T : struct`
* `Default()`
* `NotDefault()`

### Comparison Guards

For `ArgumentInfo<T> where T : IComparable<T>`
* `Min(T)`
* `Max(T)`
* `InRange(T, T)`

For `ArgumentInfo<T|T?> where T : struct, IComparable<T>`
* `Zero()`
* `NotZero()`
* `Positive()`
* `NotPositive()`
* `Negative()`
* `NotNegative()`

### Collection Guards

For `ArgumentInfo<T> where T : IEnumerable`
* `Empty()`
* `NotEmpty()`
* `MinCount(int)`
* `MaxCount(int)`
* `CountInRange(int, int)`
* `Contains(TItem)`
* `Contains(TItem, IEqualityComparer<TItem>)`
* `DoesNotContain(TItem)`
* `DoesNotContain(TItem, IEqualityComparer<TItem>)`
* `ContainsNull()`
* `DoesNotContainNull()`

### String Guards

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
* `StartsWith(string)`
* `StartsWith(string, StringComparison)`
* `DoesNotStartWith(string)`
* `DoesNotStartWith(string, StringComparison)`
* `EndsWith(string)`
* `EndsWith(string, StringComparison)`
* `DoesNotEndWith(string)`
* `DoesNotEndWith(string, StringComparison)`

### Floating-Point Number Guards

For `ArgumentInfo<float|float?|double|double?>`
* `NaN()`
* `NotNaN()`
* `Infinity()`
* `NotInfinity()`
* `PositiveInfinity()`
* `NotPositiveInfinity()`
* `NegativeInfinity()`
* `NotNegativeInfinity()`

### Boolean Guards

For `ArgumentInfo<bool|bool?>`
* `True()`
* `False()`

### URI Guards

For `ArgumentInfo<Uri>`
* `Absolute`
* `Relative`
* `Scheme(string)`
* `NotScheme(string)`
* `Http()`
* `Http(bool)`
* `Https()`

### Enum Guards
For `ArgumentInfo<T|T?> where T : enum`
* `Defined()`
* `HasFlag(T)`
* `DoesNotHaveFlag(T)`

### Email Guards
For `ArgumentInfo<MailAddress>`
* `HasHost(string)`
* `DoesNotHaveHost(string)`
* `HostIn(IEnumerable<string>)`
* `HostNotIn(IEnumerable<string>)`

### Type Guards

For `ArgumentInfo<T>`
* `Compatible<TTarget>()`
* `NotCompatible<TTarget>()`
* `Cast<TTarget>` - Returns an argument of `TTarget`

For `ArgumentInfo<object>`
* `Type<T>()` - Returns an argument of `T`.
* `NotType<T>()`
* `Type(Type)`
* `NotType(Type)`

### Normalization Guards

For `ArgumentInfo<T>`
* `Modify(T value)`
* `Modify<TTarget>(Func<T, TTarget>)` - Returns an argument of `TTarget`
* `Wrap<TTarget>(Func<T, TTarget>)` - Returns an argument of `TTarget`

For `ArgumentInfo<T> where T : class, ICloneable`
* `Clone()`

### Predicate Guards

For `ArgumentInfo<T>`
* `Require(Func<T, bool>)`
* `Require<TException>(Func<T, bool>)`

## Future

The development branch where you can see the works in progress is [`dev`][5]. [`master`][4] is only
updated for releases.

### What Is to Come

* More validations.
* Coverage checks per push.
* Online documentation.
* Performance benchmarks.

### What Is Not to Come

* Compound validations.  
  E.g. `TrimmedNotNullOrEmpty` for strings.
* Validations for types where a better suited type exists.  
  E.g. `IPAddress` for strings since there is already an `IPAddress` class.

[1]: docs/design-decisions.md
[2]: docs/extensibility.md
[3]: https://docs.microsoft.com/dotnet/standard/net-standard
[4]: https://github.com/safakgur/guard/tree/master
[5]: https://github.com/safakgur/guard/tree/dev
