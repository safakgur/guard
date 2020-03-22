using Xunit;

namespace Dawn.Tests
{
    public sealed class ComparableTests : BaseTests
    {
        [Theory(DisplayName = "Comparable: Min")]
        [InlineData(null, 3, 4)]
        [InlineData(3, 3, 4)]
        [InlineData(3, 2, 5)]
        public void Min(int? value, int valueOrLess, int greaterThanValue)
        {
            var nullableValueArg = Guard.Argument(() => value).Min(valueOrLess);
            if (!value.HasValue)
            {
                nullableValueArg.Min(greaterThanValue);
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableValueArg,
                arg => arg.Min(greaterThanValue),
                (arg, message) => arg.Min(greaterThanValue, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(greaterThanValue, m);
                    return message;
                }));

            var valueArg = Guard.Argument(value.Value, nameof(value)).Min(valueOrLess);
            ThrowsArgumentOutOfRangeException(
                valueArg,
                arg => arg.Min(greaterThanValue),
                (arg, message) => arg.Min(greaterThanValue, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(greaterThanValue, m);
                    return message;
                }));
        }

        [Theory(DisplayName = "Comparable: GreaterThan")]
        [InlineData(null, 3, 4)]
        [InlineData(3, 2, 3)]
        [InlineData(3, 1, 4)]
        public void GreaterThan(int? value, int lessThanValue, int valueOrGreater)
        {
            var nullableValueArg = Guard.Argument(() => value).GreaterThan(lessThanValue);
            if (!value.HasValue)
            {
                nullableValueArg.GreaterThan(valueOrGreater);
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableValueArg,
                arg => arg.GreaterThan(valueOrGreater),
                (arg, message) => arg.GreaterThan(valueOrGreater, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(valueOrGreater, m);
                    return message;
                }));

            var valueArg = Guard.Argument(value.Value, nameof(value)).GreaterThan(lessThanValue);
            ThrowsArgumentOutOfRangeException(
                valueArg,
                arg => arg.GreaterThan(valueOrGreater),
                (arg, message) => arg.GreaterThan(valueOrGreater, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(valueOrGreater, m);
                    return message;
                }));
        }

        [Theory(DisplayName = "Comparable: Max")]
        [InlineData(null, 3, 2)]
        [InlineData(3, 3, 2)]
        [InlineData(3, 4, 1)]
        public void Max(int? value, int valueOrGreater, int lessThanValue)
        {
            var nullableValueArg = Guard.Argument(() => value).Max(valueOrGreater);
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

            var valueArg = Guard.Argument(value.Value, nameof(value)).Max(valueOrGreater);
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

        [Theory(DisplayName = "Comparable: LessThan")]
        [InlineData(null, 3, 2)]
        [InlineData(3, 4, 3)]
        [InlineData(3, 5, 2)]
        public void LessThan(int? value, int greaterThanValue, int valueOrLess)
        {
            var nullableValueArg = Guard.Argument(() => value).LessThan(greaterThanValue);
            if (!value.HasValue)
            {
                nullableValueArg.LessThan(valueOrLess);
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableValueArg,
                arg => arg.LessThan(valueOrLess),
                (arg, message) => arg.LessThan(valueOrLess, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(valueOrLess, m);
                    return message;
                }));

            var valueArg = Guard.Argument(value.Value, nameof(value)).LessThan(greaterThanValue);
            ThrowsArgumentOutOfRangeException(
                valueArg,
                arg => arg.LessThan(valueOrLess),
                (arg, message) => arg.LessThan(valueOrLess, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(valueOrLess, m);
                    return message;
                }));
        }

        [Theory(DisplayName = "Comparable: InRange")]
        [InlineData(null, 2, 4)]
        [InlineData(3, 2, 4)]
        [InlineData(3, 1, 5)]
        public void InRange(int? value, int lessThanValue, int greaterThanValue)
        {
            var nullableValueArg = Guard.Argument(() => value).InRange(lessThanValue, greaterThanValue);
            if (!value.HasValue)
            {
                for (var i = 0; i < 2; i++)
                {
                    var limit = i == 0 ? lessThanValue : greaterThanValue;
                    nullableValueArg.InRange(limit, limit);
                }

                return;
            }

            nullableValueArg
                .InRange(lessThanValue, value.Value)
                .InRange(value.Value, value.Value)
                .InRange(value.Value, greaterThanValue);

            var valueArg = Guard.Argument(value.Value, nameof(value))
                .InRange(lessThanValue, value.Value)
                .InRange(value.Value, value.Value)
                .InRange(value.Value, greaterThanValue)
                .InRange(lessThanValue, greaterThanValue);

            for (var i = 0; i < 2; i++)
            {
                var limit = i == 0 ? lessThanValue : greaterThanValue;
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
                    valueArg,
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
    }
}
