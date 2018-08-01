namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = T + "Guard supports enum preconditions.")]
        public void GuardSupportsEnums()
        {
            // Defined.
            var @null = default(Colors?);
            var nullArg = Guard.Argument(() => @null);
            nullArg.Defined();

            var red = Colors.Red;
            var redArg = Guard.Argument(() => red);
            redArg.Defined();

            var nullableRed = red as Colors?;
            var nullableRedArg = Guard.Argument(() => nullableRed);
            nullableRedArg.Defined();

            var undefined = (Colors)10;
            Guard.Argument(() => undefined);
            Assert.Throws<ArgumentException>(
                nameof(undefined), () => Guard.Argument(() => undefined).Defined());

            var message = RandomMessage;
            var ex = Assert.Throws<ArgumentException>(
                nameof(undefined), () => Guard.Argument(() => undefined).Defined(c =>
                {
                    Assert.Equal(undefined, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            var nullableUndefined = undefined as Colors?;
            Guard.Argument(() => nullableUndefined);
            Assert.Throws<ArgumentException>(
                nameof(nullableUndefined), () => Guard.Argument(() => nullableUndefined).Defined());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullableUndefined), () => Guard.Argument(() => nullableUndefined).Defined(c =>
                {
                    Assert.Equal(nullableUndefined, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Has flag.
            nullArg.HasFlag(Colors.None).HasFlag(Colors.Red).HasFlag(Colors.All);

            var purple = Colors.Red | Colors.Blue;
            var purpleArg = Guard.Argument(() => purple);
            purpleArg.HasFlag(Colors.None).HasFlag(Colors.Red).HasFlag(Colors.Blue).HasFlag(Colors.Red | Colors.Blue);

            Assert.Throws<ArgumentException>(
                nameof(purple), () => Guard.Argument(() => purple).DoesNotHaveFlag(Colors.Red));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(purple), () => Guard.Argument(() => purple).DoesNotHaveFlag(Colors.Red, (c1, c2) =>
                {
                    Assert.Equal(purple, c1);
                    Assert.Equal(Colors.Red, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            var nullablePurple = purple as Colors?;
            var nullablePurpleArg = Guard.Argument(() => nullablePurple);
            nullablePurpleArg.HasFlag(Colors.None).HasFlag(Colors.Red).HasFlag(Colors.Blue).HasFlag(Colors.Red | Colors.Blue);
            Assert.Throws<ArgumentException>(
                nameof(nullablePurple), () => Guard.Argument(() => nullablePurple).DoesNotHaveFlag(Colors.Red));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullablePurple), () => Guard.Argument(() => nullablePurple).DoesNotHaveFlag(Colors.Red, (c1, c2) =>
                {
                    Assert.Equal(nullablePurple, c1);
                    Assert.Equal(Colors.Red, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Does not have flag.
            nullArg.DoesNotHaveFlag(Colors.Green).DoesNotHaveFlag(Colors.All);

            purpleArg
                .DoesNotHaveFlag(Colors.Green)
                .DoesNotHaveFlag(Colors.All);

            Assert.Throws<ArgumentException>(
                nameof(purple), () => Guard.Argument(() => purple).HasFlag(Colors.Green));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(purple), () => Guard.Argument(() => purple).HasFlag(Colors.Green, (c1, c2) =>
                {
                    Assert.Equal(purple, c1);
                    Assert.Equal(Colors.Green, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            nullablePurpleArg
                .DoesNotHaveFlag(Colors.Green)
                .DoesNotHaveFlag(Colors.All);

            Assert.Throws<ArgumentException>(
                nameof(nullablePurple), () => Guard.Argument(() => nullablePurple).HasFlag(Colors.Green));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullablePurple), () => Guard.Argument(() => nullablePurple).HasFlag(Colors.Green, (c1, c2) =>
                {
                    Assert.Equal(nullablePurple, c1);
                    Assert.Equal(Colors.Green, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        [Fact(DisplayName = T + "Guard supports legacy enum preconditions.")]
        public void GuardSupportsLegacyEnums()
        {
            // Non-enum types.
            var integer = 1;
            Assert.Throws<ArgumentException>(
                nameof(integer), () => Guard.Argument(() => integer).Enum());

            var message = RandomMessage;
            var ex = Assert.Throws<ArgumentException>(
                nameof(integer), () => Guard.Argument(() => integer).Enum(i =>
                {
                    Assert.Equal(integer, i);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Null.
            var @null = default(Colors?);
            var nullArg = Guard.Argument(() => @null).Enum();

            nullArg.Null();
            Assert.Throws<ArgumentNullException>(
               nameof(@null), () => Guard.Argument(() => @null).Enum().NotNull());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentNullException>(
               nameof(@null), () => Guard.Argument(() => @null).Enum().NotNull(message));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
               nameof(@null), () => Guard.Argument(() => @null).Modify(@null).Enum().NotNull());

            // Not null.
            var red = Colors.Red;
            var redArg = Guard.Argument(() => red).Enum();

            var nullableRed = red as Colors?;
            var nullableRedArg = Guard.Argument(() => nullableRed).Enum();

            Assert.Equal(red, nullableRedArg.NotNull());
            Assert.Throws<ArgumentException>(
               nameof(nullableRed), () => Guard.Argument(() => nullableRed).Enum().Null());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
               nameof(nullableRed), () => Guard.Argument(() => nullableRed).Enum().Null(c =>
               {
                   Assert.Equal(nullableRed, c);
                   return message;
               }));

            Assert.StartsWith(message, ex.Message);

            // Defined.
            nullArg.Defined();
            redArg.Defined();
            nullableRedArg.Defined();

            var undefined = (Colors)10;
            Guard.Argument(() => undefined).Enum();
            Assert.Throws<ArgumentException>(
                nameof(undefined), () => Guard.Argument(() => undefined).Enum().Defined());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(undefined), () => Guard.Argument(() => undefined).Enum().Defined(c =>
                {
                    Assert.Equal(undefined, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            var nullableUndefined = undefined as Colors?;
            Guard.Argument(() => nullableUndefined).Enum();
            Assert.Throws<ArgumentException>(
                nameof(nullableUndefined), () => Guard.Argument(() => nullableUndefined).Enum().Defined());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullableUndefined), () => Guard.Argument(() => nullableUndefined).Enum().Defined(c =>
                {
                    Assert.Equal(nullableUndefined, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // None.
            nullArg.None();

            var none = Colors.None;
            Guard.Argument(() => none).Enum().None();
            Assert.Throws<ArgumentException>(
                nameof(none), () => Guard.Argument(() => none).Enum().NotNone());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(none), () => Guard.Argument(() => none).Enum().NotNone(c =>
                {
                    Assert.Equal(none, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            var nullableNone = none as Colors?;
            Guard.Argument(() => nullableNone).Enum().None();
            Assert.Throws<ArgumentException>(
                nameof(nullableNone), () => Guard.Argument(() => nullableNone).Enum().NotNone());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullableNone), () => Guard.Argument(() => nullableNone).Enum().NotNone(c =>
                {
                    Assert.Equal(nullableNone, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Not none.
            nullArg.NotNone();

            redArg.NotNone();
            Assert.Throws<ArgumentException>(
                nameof(red), () => Guard.Argument(() => red).Enum().None());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(red), () => Guard.Argument(() => red).Enum().None(c =>
                {
                    Assert.Equal(red, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            nullableRedArg.NotNone();
            Assert.Throws<ArgumentException>(
                nameof(nullableRed), () => Guard.Argument(() => nullableRed).Enum().None());

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullableRed), () => Guard.Argument(() => nullableRed).Enum().None(c =>
                {
                    Assert.Equal(nullableRed, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Equal.
            nullArg.Equal(Colors.Red);

            redArg.Equal(Colors.Red);
            Assert.Throws<ArgumentException>(
                nameof(red), () => Guard.Argument(() => red).Enum().NotEqual(Colors.Red));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(red), () => Guard.Argument(() => red).Enum().NotEqual(Colors.Red, c =>
                {
                    Assert.Equal(red, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            nullableRedArg.Equal(Colors.Red);
            Assert.Throws<ArgumentException>(
                nameof(nullableRed), () => Guard.Argument(() => nullableRed).Enum().NotEqual(Colors.Red));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullableRed), () => Guard.Argument(() => nullableRed).Enum().NotEqual(Colors.Red, c =>
                {
                    Assert.Equal(nullableRed, c);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Not equal.
            nullArg.NotEqual(Colors.Green);

            redArg.NotEqual(Colors.Green);
            Assert.Throws<ArgumentException>(
                nameof(red), () => Guard.Argument(() => red).Enum().Equal(Colors.Green));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(red), () => Guard.Argument(() => red).Enum().Equal(Colors.Green, (c1, c2) =>
                {
                    Assert.Equal(red, c1);
                    Assert.Equal(Colors.Green, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            nullableRedArg.NotEqual(Colors.Green);
            Assert.Throws<ArgumentException>(
                nameof(nullableRed), () => Guard.Argument(() => nullableRed).Enum().Equal(Colors.Green));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullableRed), () => Guard.Argument(() => nullableRed).Enum().Equal(Colors.Green, (c1, c2) =>
                {
                    Assert.Equal(nullableRed, c1);
                    Assert.Equal(Colors.Green, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Has flag.
            nullArg.HasFlag(Colors.None).HasFlag(Colors.Red).HasFlag(Colors.All);

            var purple = Colors.Red | Colors.Blue;
            var purpleArg = Guard.Argument(() => purple).Enum();
            purpleArg.HasFlag(Colors.None).HasFlag(Colors.Red).HasFlag(Colors.Blue).HasFlag(Colors.Red | Colors.Blue);
            Assert.Throws<ArgumentException>(
                nameof(purple), () => Guard.Argument(() => purple).Enum().DoesNotHaveFlag(Colors.Red));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(purple), () => Guard.Argument(() => purple).Enum().DoesNotHaveFlag(Colors.Red, (c1, c2) =>
                {
                    Assert.Equal(purple, c1);
                    Assert.Equal(Colors.Red, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            var nullablePurple = purple as Colors?;
            var nullablePurpleArg = Guard.Argument(() => nullablePurple).Enum();
            nullablePurpleArg.HasFlag(Colors.None).HasFlag(Colors.Red).HasFlag(Colors.Blue).HasFlag(Colors.Red | Colors.Blue);
            Assert.Throws<ArgumentException>(
                nameof(nullablePurple), () => Guard.Argument(() => nullablePurple).Enum().DoesNotHaveFlag(Colors.Red));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullablePurple), () => Guard.Argument(() => nullablePurple).Enum().DoesNotHaveFlag(Colors.Red, (c1, c2) =>
                {
                    Assert.Equal(nullablePurple, c1);
                    Assert.Equal(Colors.Red, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Does not have flag.
            nullArg.DoesNotHaveFlag(Colors.Green).DoesNotHaveFlag(Colors.All);

            purpleArg
                .DoesNotHaveFlag(Colors.Green)
                .DoesNotHaveFlag(Colors.All);

            Assert.Throws<ArgumentException>(
                nameof(purple), () => Guard.Argument(() => purple).Enum().HasFlag(Colors.Green));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(purple), () => Guard.Argument(() => purple).Enum().HasFlag(Colors.Green, (c1, c2) =>
                {
                    Assert.Equal(purple, c1);
                    Assert.Equal(Colors.Green, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            nullablePurpleArg
                .DoesNotHaveFlag(Colors.Green)
                .DoesNotHaveFlag(Colors.All);

            Assert.Throws<ArgumentException>(
                nameof(nullablePurple), () => Guard.Argument(() => nullablePurple).Enum().HasFlag(Colors.Green));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(nullablePurple), () => Guard.Argument(() => nullablePurple).Enum().HasFlag(Colors.Green, (c1, c2) =>
                {
                    Assert.Equal(nullablePurple, c1);
                    Assert.Equal(Colors.Green, c2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);
        }
#pragma warning restore CS0618 // Type or member is obsolete

        [Flags]
        private enum Colors
        {
            None = 0,

            Red = 1,

            Green = 2,

            Blue = 4,

            All = Red | Green | Blue
        }
    }
}
