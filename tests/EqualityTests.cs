namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed class EqualityTests : BaseTests
    {
        [Theory(DisplayName = T + "Equality: Default/NotDefault")]
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

        [Theory(DisplayName = T + "Equality: Equal/NotEqual w/o comparer")]
        [InlineData(null, null, null, false)]
        [InlineData("AB", "AB", "BC", false)]
        [InlineData("AB", "AB", "BC", true)]
        public void EqualWithoutComparer(string value, string equal, string unequal, bool secure)
        {
            var valueArg = Guard.Argument(() => value, secure).Equal(equal).NotEqual(unequal);
            if (value == null)
            {
                valueArg.Equal(unequal);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.Equal(unequal),
                m => secure != m.Contains(unequal),
                (arg, message) => arg.Equal(unequal, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(unequal, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotEqual(equal),
                m => secure != m.Contains(equal),
                (arg, message) => arg.NotEqual(equal, v =>
                {
                    Assert.Same(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Equality: Equal/NotEqual w/ comparer")]
        [InlineData(null, null, null, StringComparison.Ordinal, false)]
        [InlineData("AB", "AB", "ab", StringComparison.Ordinal, false)]
        [InlineData("AB", "AB", "ab", StringComparison.Ordinal, true)]
        [InlineData("AB", "ab", "BC", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("AB", "ab", "BC", StringComparison.OrdinalIgnoreCase, true)]
        public void EqualWithComparer(
            string value, string equal, string unequal, StringComparison comparison, bool secure)
        {
            var valueArg = Guard.Argument(() => value, secure);
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
                m => secure != m.Contains(unequal),
                (arg, message) => arg.Equal(unequal, comparer, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(unequal, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotEqual(equal, comparer),
                m => secure != m.Contains(equal),
                (arg, message) => arg.NotEqual(equal, comparer, v =>
                {
                    Assert.Same(value, v);
                    return message;
                }));
        }

        [Fact(DisplayName = T + "Equality: Same/NotSame")]
        public void Same()
        {
            Test(null, null, null, false);
            Test("1", "1", 1.ToString(), false);
            Test("1", "1", 1.ToString(), true);

            void Test(string value, string same, string nonSame, bool secure)
            {
                same = same?.ToString();
                nonSame = nonSame?.ToString();

                var valueArg = Guard.Argument(() => value, secure);
                valueArg
                    .Equal(same)
                    .Equal(nonSame)
                    .Same(same)
                    .NotSame(nonSame);

                if (value is null)
                {
                    valueArg
                        .Same(nonSame)
                        .NotSame(same);

                    return;
                }

                ThrowsArgumentException(
                    valueArg,
                    arg => arg.Same(nonSame),
                    m => secure != m.Contains(nonSame),
                    (arg, message) => arg.Same(nonSame, (v, other) =>
                    {
                        Assert.Same(value, v);
                        Assert.Same(nonSame, other);
                        return message;
                    }));

                ThrowsArgumentException(
                    valueArg,
                    arg => arg.NotSame(same),
                    m => secure != m.Contains(same),
                    (arg, message) => arg.NotSame(same, (v, other) =>
                    {
                        Assert.Same(value, v);
                        Assert.Same(same, other);
                        return message;
                    }));
            }
        }
    }
}
