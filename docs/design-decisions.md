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

## Experimenting with `ref struct`s

C# 7.2 also introduced the concept of `ref struct`s. Value types declared as `ref struct` must be
stack allocated, so they can never be created on the heap as a member of another class. They can't
implement interfaces, be boxed, or be declared as local variables inside state machines
(iterators/async methods). They are also not allowed as delegate parameters.

Although these restrictions are introduced mainly for structures that interop with unmanaged memory,
I thought they could also work for us, preventing many ways of misusing the guarded arguments.

But unfortunately, these limitations make the library very hard to test for so little benefits.
I may look into writing some Roslyn analyzers to check for correct usage at some point but I think
that too wouldn't worth the effort, considering the use of this library being pretty straightforward.

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
* The third sample initializes a `MemberExpression` that provides both the argument's value and name.
Although I see the importance to provide an option for initializing guarded arguments without any
heap allocations, I expect the performance implications to be negligible for most projects.
So I recommend this to be used unless it is actually measured to be slowing things down.

## Implicit Conversion to Value's Type

Most constructors consist of code that first, validate the arguments and second, assign them to
their corresponding fields/properties. So it seems convenient to allow the guarded arguments to be
assigned directly as argument values like this:

```c#
public Person(string firstName, string lastName)
{
    this.firstName = Guard.Argument(() => firstName).NotNull().NotEmpty();
    this.lastName = Guard.Argument(() => lastName).NotNull().NotEmpty();
} 
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
arguments like `MinValue` and `NotZero` throw `ArgumentOutOfRangeException`s. Most others throw
`ArgumentException`s. (See [Modifying Arguments](#modifying-arguments) for exceptional cases.)

Throwing custom exceptions from standard validations seems counter-intuitive and right now, the only
way to do so is to use the generic `Require<TException>` validation.

```c#
Guard.Argument(() => arg)
    .Require<KeyNotFoundException>(a => a != 0);
```

The above code throws a `KeyNotFoundException` if the `arg` is passed `0`.

## Exception Messages

Guard creates a meaningful exception message that contains the argument name and a description
specific to the validation when a precondition can't be satisfied. Additionaly, every validation in
Guard accepts an optional parameter letting the user specify a custom error message.

```c#
// Throws an ArgumentException if the arg is not null.
Guard.Argument(() => arg)
    .Null(a => "The argument must be null but it is: " + a);

// Throws an ArgumentNullException if the arg is null.
Guard.Argument(() => arg)
    .NotNull("The argument cannot be null.");
```

In the first example above, we specify a factory that will create the error message if the
validation fails. `arg` is passed to the factory as `a` so it can be used in the error message.
We could of course use `arg` directly but that would cause it to be captured by the lambda
expression, thus prevent the expression from being cached. We could make the `Null` validation
accept a `string` parameter instead of a `Func<T, string>`, but that would require the error message
to be initialized even when the precondition is satisfied, i.e. when the argument is null.

In the second example, we see that the `NotNull` validation accepts the error message as a string
instead of a factory. This is because it only throws an exception if the argument value is null.
Therefore the only possible value that can be passed to a factory would be null.

## Automatic Nullable Value Conversions

Using the `NotNull` validation on a nullable value type would convert the `ArgumentInfo<T?>` to an
`ArgumentInfo<T>` since the validation being successful means that the argument is not null.

```c#
public class SomeService
{
    public SomeService(int? timeout)
    {
        // Guard.Argument creates an ArgumentInfo<int?>
        this.Timeout = Guard.Argument(() => timeout)
            // NotNull converts it to an ArgumentInfo<int>
            .NotNull();
    }

    public int Timeout { get; }
}
```

## Modifying Arguments

A method that validates its arguments can also apply some normalization routines before using them.
Trimming a string before assigning it to a field is a good example for that. Guard provides the
`Modify` overloads that can be used for normalizing argument values.

```c#
public Person(string name)
{
    this.name = Guard.Argument(() => name)
        .NotNull()
        .Modify(s => s.Trim())
        .MinLength(3); // Validates the trimmed version.
}
```

Since the arguments can be modified to have any value, including null, `NotNull` validations applied
to modified arguments shouldn't throw `ArgumentNullException`s.

```c#
public Person GetOwner(Car car)
{
    return Guard.Argument(() => car)
        .NotNull()
        .Modify(c => c.Owner)
        .NotNull();
}
```

The first call to `NotNull` in the above example throws an `ArgumentNullException` if `car` is null
but the second call to `NotNull` should throw an `ArgumentException`. This is because throwing an
`ArgumentNullException` there would indicate that `car` is null when in fact its `Owner` is null.

The same goes for `ArgumentOutOfRangeException`s. If the original argument is modified, an
`ArgumentException` is thrown instead of a more specialized exception. For validations to detect
whether the argument is modified, `ArgumentInfo<T>` contains a boolean `Modified` flag along with
the argument's name and value.
