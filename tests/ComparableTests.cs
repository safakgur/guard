namespace Dawn.Tests
{
    using Xunit;

    public sealed class ComparableTests : BaseTests
    {
        [Theory(DisplayName = T + "Comparable: Min")]
        [InlineData(null, 3, 4)]
        [InlineData(3, 3, 4)]
        [InlineData(3, 2, 5)]
        public void Min(int? value, int valueOrLess, int moreThanValue)
        {
            var nullableValueArg = Guard.Argument(() => value).Min(valueOrLess);
            if (!value.HasValue)
            {
                nullableValueArg.Min(moreThanValue);
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableValueArg,
                arg => arg.Min(moreThanValue),
                (arg, message) => arg.Min(moreThanValue, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(moreThanValue, m);
                    return message;
                }));

            var valueArg = Guard.Argument(value.Value, nameof(value)).Min(valueOrLess);
            ThrowsArgumentOutOfRangeException(
                valueArg,
                arg => arg.Min(moreThanValue),
                (arg, message) => arg.Min(moreThanValue, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(moreThanValue, m);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Comparable: Max")]
        [InlineData(null, 3, 2)]
        [InlineData(3, 3, 2)]
        [InlineData(3, 4, 1)]
        public void Max(int? value, int valueOrMore, int lessThanValue)
        {
            var nullableValueArg = Guard.Argument(() => value).Max(valueOrMore);
            if (!value.HasValue)
            {
                nullableValueArg.Max(lessThanValue);
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableValueArg,
                arg => arg.Max(lessThanValue),
                (arg, message) => arg.Max(lessThanValue, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(lessThanValue, m);
                    return message;
                }));

            var valueArg = Guard.Argument(value.Value, nameof(value)).Max(valueOrMore);
            ThrowsArgumentOutOfRangeException(
                valueArg,
                arg => arg.Max(lessThanValue),
                (arg, message) => arg.Max(lessThanValue, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(lessThanValue, m);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Comparable: InRange")]
        [InlineData(null, 2, 4)]
        [InlineData(3, 2, 4)]
        [InlineData(3, 1, 5)]
        public void InRange(int? value, int lessThanValue, int moreThanValue)
        {
            var nullableValueArg = Guard.Argument(() => value).InRange(lessThanValue, moreThanValue);
            if (!value.HasValue)
            {
                for (var i = 0; i < 2; i++)
                {
                    var limit = i == 0 ? lessThanValue : moreThanValue;
                    nullableValueArg.InRange(limit, limit);
                }

                return;
            }

            nullableValueArg
                .InRange(lessThanValue, value.Value)
                .InRange(value.Value, value.Value)
                .InRange(value.Value, moreThanValue);

            var valueArg = Guard.Argument(value.Value, nameof(value)).InRange(lessThanValue, moreThanValue);
            for (var i = 0; i < 2; i++)
            {
                var limit = i == 0 ? lessThanValue : moreThanValue;
                ThrowsArgumentOutOfRangeException(
                    nullableValueArg,
                    arg => arg.InRange(limit, limit),
                    (arg, message) => arg.InRange(limit, limit, (v, min, max) =>
                    {
                        Assert.Equal(value, v);
                        Assert.Equal(limit, min);
                        Assert.Equal(limit, max);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    nullableValueArg,
                    arg => arg.InRange(limit, limit),
                    (arg, message) => arg.InRange(limit, limit, (v, min, max) =>
                    {
                        Assert.Equal(value, v);
                        Assert.Equal(limit, min);
                        Assert.Equal(limit, max);
                        return message;
                    }));
            }
        }

        [Theory(DisplayName = T + "Comparable: Zero/NotZero")]
        [InlineData(null, null)]
        [InlineData(0, -1)]
        [InlineData(0, 1)]
        public void Zero(int? zero, int? nonZero)
        {
            var nullableZeroArg = Guard.Argument(zero).Zero();
            var nullableNonZeroArg = Guard.Argument(nonZero).NotZero();
            if (!zero.HasValue)
            {
                nullableZeroArg.NotZero();
                nullableNonZeroArg.Zero();
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableNonZeroArg,
                arg => arg.Zero(),
                (arg, message) => arg.Zero(i =>
                {
                    Assert.Equal(nonZero, i);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                nullableZeroArg,
                arg => arg.NotZero(),
                (arg, message) => arg.NotZero(message));

            var zeroArg = Guard.Argument(zero.Value, nameof(zero)).Zero();
            var nonZeroArg = Guard.Argument(nonZero.Value, nameof(nonZero)).NotZero();
            ThrowsArgumentOutOfRangeException(
                nonZeroArg,
                arg => arg.Zero(),
                (arg, message) => arg.Zero(i =>
                {
                    Assert.Equal(nonZero, i);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                zeroArg,
                arg => arg.NotZero(),
                (arg, message) => arg.NotZero(message));
        }

        [Theory(DisplayName = T + "Comparable: Positive/NotPositive")]
        [InlineData(null, null)]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public void Positive(int? positive, int? nonPositive)
        {
            var nullablePositiveArg = Guard.Argument(() => positive).Positive();
            var nullableNonPositiveArg = Guard.Argument(() => nonPositive).NotPositive();
            if (!positive.HasValue)
            {
                nullablePositiveArg.NotPositive();
                nullableNonPositiveArg.Positive();
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableNonPositiveArg,
                arg => arg.Positive(),
                (arg, message) => arg.Positive(i =>
                {
                    Assert.Equal(nonPositive, i);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                nullablePositiveArg,
                arg => arg.NotPositive(),
                (arg, message) => arg.NotPositive(i =>
                {
                    Assert.Equal(positive, i);
                    return message;
                }));

            var positiveArg = Guard.Argument(positive.Value, nameof(positive));
            var nonPositiveArg = Guard.Argument(nonPositive.Value, nameof(nonPositive));
            ThrowsArgumentOutOfRangeException(
                nonPositiveArg,
                arg => arg.Positive(),
                (arg, message) => arg.Positive(i =>
                {
                    Assert.Equal(nonPositive, i);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                positiveArg,
                arg => arg.NotPositive(),
                (arg, message) => arg.NotPositive(i =>
                {
                    Assert.Equal(positive, i);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Comparable: Negative/NotNegative")]
        [InlineData(null, null)]
        [InlineData(-1, 0)]
        [InlineData(-1, 1)]
        public void Negative(int? negative, int? nonNegative)
        {
            var nullableNegativeArg = Guard.Argument(() => negative).Negative();
            var nullableNonNegativeArg = Guard.Argument(() => nonNegative).NotNegative();
            if (!negative.HasValue)
            {
                nullableNegativeArg.NotNegative();
                nullableNonNegativeArg.Negative();
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableNonNegativeArg,
                arg => arg.Negative(),
                (arg, message) => arg.Negative(i =>
                {
                    Assert.Equal(nonNegative, i);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                nullableNegativeArg,
                arg => arg.NotNegative(),
                (arg, message) => arg.NotNegative(i =>
                {
                    Assert.Equal(negative, i);
                    return message;
                }));

            var NegativeArg = Guard.Argument(negative.Value, nameof(negative));
            var nonNegativeArg = Guard.Argument(nonNegative.Value, nameof(nonNegative));
            ThrowsArgumentOutOfRangeException(
                nonNegativeArg,
                arg => arg.Negative(),
                (arg, message) => arg.Negative(i =>
                {
                    Assert.Equal(nonNegative, i);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                NegativeArg,
                arg => arg.NotNegative(),
                (arg, message) => arg.NotNegative(i =>
                {
                    Assert.Equal(negative, i);
                    return message;
                }));
        }
    }
}
