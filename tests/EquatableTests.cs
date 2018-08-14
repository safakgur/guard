namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed class EquatableTests : BaseTests
    {
        [Theory(DisplayName = T + "Equatable: Default/NotDefault")]
        [InlineData(null, null)]
        [InlineData(0, 1)]
        public void Default(int? @default, int? nonDefault)
        {
            var nullableDefaultArg = Guard.Argument(() => @default).Default();
            var nullableNonDefaultArg = Guard.Argument(() => nonDefault).NotDefault();
            if (!@default.HasValue)
            {
                nullableDefaultArg.NotDefault();
                nullableNonDefaultArg.Default();
                return;
            }

            ThrowsArgumentException(
                nullableNonDefaultArg,
                arg => arg.Default(),
                (arg, message) => arg.Default(i =>
                {
                    Assert.Equal(nonDefault, i);
                    return message;
                }));

            ThrowsArgumentException(
                nullableDefaultArg,
                arg => arg.NotDefault(),
                (arg, message) => arg.NotDefault(message));

            var defaultArg = Guard.Argument(@default.Value, nameof(@default)).Default();
            var nonDefaultArg = Guard.Argument(nonDefault.Value, nameof(nonDefault)).NotDefault();
            ThrowsArgumentException(
                nonDefaultArg,
                arg => arg.Default(),
                (arg, message) => arg.Default(i =>
                {
                    Assert.Equal(nonDefault, i);
                    return message;
                }));

            ThrowsArgumentException(
                defaultArg,
                arg => arg.NotDefault(),
                (arg, message) => arg.NotDefault(message));
        }

        [Theory(DisplayName = T + "Equatable: Equal/NotEqual w/o comparer")]
        [InlineData(null, null, null)]
        [InlineData("A", "A", "B")]
        public void EqualWithoutComparer(string value, string equal, string unequal)
        {
            var valueArg = Guard.Argument(() => value).Equal(equal).NotEqual(unequal);
            if (value == null)
            {
                valueArg.Equal(unequal);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.Equal(unequal),
                (arg, message) => arg.Equal(unequal, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(unequal, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotEqual(equal),
                (arg, message) => arg.NotEqual(equal, v =>
                {
                    Assert.Same(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Equatable: Equal/NotEqual w/ comparer")]
        [InlineData(null, null, null, StringComparison.Ordinal)]
        [InlineData("A", "A", "a", StringComparison.Ordinal)]
        [InlineData("A", "a", "B", StringComparison.OrdinalIgnoreCase)]
        public void EqualWithComparer(string value, string equal, string unequal, StringComparison comparison)
        {
            var valueArg = Guard.Argument(() => value);
            var comparer = comparison == StringComparison.Ordinal
                ? StringComparer.Ordinal
                : StringComparer.OrdinalIgnoreCase;

            valueArg.Equal(equal, comparer).NotEqual(unequal, comparer);

            if (value == null)
            {
                valueArg.Equal(unequal, comparer);
                valueArg.NotEqual(equal, comparer);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.Equal(unequal, comparer),
                (arg, message) => arg.Equal(unequal, comparer, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(unequal, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotEqual(equal, comparer),
                (arg, message) => arg.NotEqual(equal, comparer, v =>
                {
                    Assert.Same(value, v);
                    return message;
                }));
        }
    }
}
