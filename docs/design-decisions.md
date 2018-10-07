# Guard - Design Decisions

This document explains the motives behind the Guard's API design.

## To Fluent or Not to Fluent

A fluent API is much more readable for validating arguments, especially when testing the same
argument against multiple preconditions like the following example:

```c#
Guard.Argument(arg, nameof(arg)).NotNull().NotEmpty();
// vs
Guard.ValidateStringNotNull(arg, nameof(arg));
Guard.ValidateStringNotEmpty(arg, nameof(arg));
```

Notice that the argument name and value is passed only once in the first sample where they are
passed for each validation in the second sample. The throwback of the fluent API is that in order
to chain validations like that, we need to return something after each validation. A class means at
least one heap allocation per argument. Albeit cheap, I dislike the idea to force a user to create
an allocation every time they need to guard an argument.

An alternative is to return a struct but this time, the value type semantics cause it to be copied
for each validation and that too may not be desirable. Fortunately we can use the `in` parameters,
`ref readonly` returns, `readonly` structs and `ref`/`in` extensions that are introduced with
C# 7.2. So given a `readonly struct ArgumentInfo<T>`, we can now write extension methods like
the following:

```c#
public static ref readonly ArgumentInfo<T> Test(
    in this ArgumentInfo<T> argument) // Accept by reference.
{
    // [Validate the argument]
    return ref argument; // Return by reference.
}
```

This takes away all the usability of Guard for people using the previous versions of C# but it is
somewhat acceptable because a) there is no other way that provides this level of readability with so
little performance impact and b) as far as the binary compatibility is concerned, people can use
Guard with C# 7.2 and still be able to target down to .NET Standard 1.0.

A compile-time symbol to toggle byref arguments/returns may be added in the future, so a version
that doesn't take advantage of the new features can be compiled with copy-by-value semantics and
live as a separate package.

## Initializing a Guarded Argument

Guard needs to know the argument's value to test it against preconditions and its name to include in
a potential exception. There are three ways to initialize a guarded argument:

```c#
// First, by specifying the argument value and name separately.
Guard.Argument(arg, nameof(arg));

// Second, omitting the optional argument name.
Guard.Argument(arg);

// Third, creating a MemberExpression via a lambda expression.
Guard.Argument(() => arg);
```

* The first sample initializes a guarded argument by specifying both the argument's value and name.
* The second sample does not specify the argument name. This is allowed but not recommended since
the argument name proves a valuable piece of information when you try to identify the error cause
from logs or crash dumps.
* The third sample initializes a `MemberExpression` that provides both the argument's value and
  name. Although compiling an expression tree is an expensive operation, it is a convenient
  alternative that can be used in applications that are not performance-critical.

## Implicit Conversion to Value's Type

Most constructors consist of code that first, validate the arguments and second, assign them to
their corresponding fields/properties. So it seems convenient to allow the guarded arguments to be
assigned directly as argument values like this:

```c#
public Person(string name)
    => Name = Guard.Argument(() => name).NotNull().NotEmpty();
```

## Optional Preconditions

I opted to ignore preconditions when the argument value is null. This allows us to specify more
complex preconditions like "Allow the argument to be unspecified (null), but if it is specified
(non-null), then require it to be a non-empty string."

```c#
// Throws if arg is null or empty.
Guard.Argument(() => arg).NotNull().NotEmpty();

// Ignored if arg is null but throws if it's an empty string.
Guard.Argument(() => arg).NotEmpty());
```

## Exception Types

Each validation in Guard has a specific exception type it throws when its precondition is not
satisfied. `NotNull` throws an `ArgumentNullException`. The validations on `IComparable<T>`
arguments like `MinValue` and `NotZero` throw `ArgumentOutOfRangeException`s. Most others
throw `ArgumentException`s. (See [Modifying Arguments](#modifying-arguments) for exceptional cases.)

Throwing custom exceptions from standard validations seems counter-intuitive and right now, the only
way to do so is to use the generic `Require<TException>` validation.

```c#
Guard.Argument(() => arg).Require<KeyNotFoundException>(a => a != 0);
```

The above code throws a `KeyNotFoundException` if the `arg` is passed `0`.

## Exception Messages

Guard creates a meaningful exception message that contains the argument name and a description
specific to the validation when a precondition can't be satisfied. Additionaly, every validation in
Guard accepts an optional parameter letting the user specify a custom error message.

```c#
// Throws an ArgumentException if the arg is not null.
Guard.Argument(() => arg).Null(a => "The argument must be null but it is: " + a);

// Throws an ArgumentNullException if the arg is null.
Guard.Argument(() => arg).NotNull("The argument cannot be null.");
```

In the first example above, we specify a factory that will create the error message if the
validation fails. `arg` is passed to the factory as `a` so it can be used in the error message. We
could of course use `arg` directly but that would cause it to be captured by the lambda expression,
thus prevent the expression from being cached. We could make the `Null` validation accept a
`string` parameter instead of a `Func<T, string>`, but that would require the error message to
be initialized even when the precondition is satisfied, i.e. when the argument is null.

In the second example, we see that the `NotNull` validation accepts the error message as a string
instead of a factory. This is because it only throws an exception if the argument value is null.
Therefore the only possible value that can be passed to a factory would be null.

## Secure Arguments

Exceptions thrown for failed Guard validations contain very descriptive messages.

```c#
// Throws with message: "token must be a2C-p."
Guard.Argument("abc", "token").Equal("a2C-p");

// Throws with message: "number must be one of the following: 1, 2, 3"
Guard.Argument(0, "number").In(1, 2, 3);
```

There may be cases where you don't want to expose that additional data to the caller. For these
scenarios, you can specify the optional "secure" flag when you initialize the argument.

```c#
// Throws with message: "token is invalid."
Guard.Argument("abc", "token", true).Equal("a2C-p");

// Throws with message: "number is invalid."
Guard.Argument(0, "number", true).In(1, 2, 3);
```

Things to note:

* Parameter names are never secured.
* Min/Max values of range checks are never secured.
* Type names are never secured.
* Exceptions that are not directly thrown by the library are never secured.
* When in doubt, see [the source][1] that provides the default messages.

## Automatic Nullable Value Conversions

Using the `NotNull` validation on a nullable value type would convert the `ArgumentInfo<T?>` to
an `ArgumentInfo<T>` since the validation being successful means that the argument is not null.

```c#
public class SomeService
{
    public SomeService(int? timeout)
    {
        // Guard.Argument creates an ArgumentInfo<int?> but NotNull converts it to an
        // ArgumentInfo<int>, so it can be assigned to a non-nullable Int32.
        Timeout = Guard.Argument(() => timeout).NotNull();
    }

    public int Timeout { get; }
}
```

## Modifying Arguments

A method that validates its arguments can also apply some normalization routines before using them.
Trimming a string before assigning it to a field/property is a good example for that. Guard provides
the `Modify` overloads that can be used for normalizing argument values.

```c#
public Person(string name)
{
    Name = Guard.Argument(() => name)
        .NotNull()
        .Modify(s => s.Trim())
        .MinLength(3); // Validates the trimmed version.
}
```

Since the arguments can be modified to have any value, including null, `NotNull` validations
applied to modified arguments shouldn't throw `ArgumentNullException`s.

```c#
public Person GetOwner(Car car)
{
    return Guard.Argument(() => car)
        .NotNull()
        .Modify(c => c.Owner)
        .NotNull();
}
```

The first call to `NotNull` in the above example throws an `ArgumentNullException` if `car` is
null but the second call to `NotNull` should throw an `ArgumentException`. This is because
throwing an `ArgumentNullException` there would indicate that `car` is null when in fact its
`Owner` is null.

The same goes for `ArgumentOutOfRangeException`s. If the original argument is modified, an
`ArgumentException` is thrown instead of a more specialized exception. For validations to detect
whether the argument is modified, `ArgumentInfo<T>` contains a boolean `Modified` flag along
with the argument's name and value.

## Validating Argument Members

Some arguments may contain fields/properties that we want to validate individually. Guard provides
`Member` overloads that can be used to validate these members without modifying the arguments.

```c#
public void BuyCar(Person buyer, Car car)
{
    Guard.Argument(() => buyer)
        .NotNull()
        .Member(p => p.Age, a => a.Min(18))
        .Member(p => p.Address.City, c => c.NotNull().NotEmpty());

    Guard.Argument(() => car)
        .NotNull()
        .Member(c => c.Owner, o => o.Null());

    car.Owner = buyer;
}
```

What makes `Member` overloads powerful is that they provide members as guarded arguments so you can
directly start chaining validations. What's better is when a member validation fails, the exception
is still thrown for the original argument (same `ParamName`) but also with a clear error message
that contains the actual member's name.

```c#
var address = new Address { City = null };
var buyer = new Person { Age = 18, Address = address };
var car = new Car("Dodge", "Power Wagon");
BuyCar(buyer, car);
```

The above code throws an `ArgumentException` with the parameter name "buyer" and message
"Address.City cannot be null.".

Keep in mind that member validations require building `MemberExpression`s. Even though the
compiled delegates get cached and reused, creating expression trees may still be expensive for your
particular application.

[1]: ../src/Guard.Messages.cs
