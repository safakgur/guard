namespace Dawn.Tests
{
    using Xunit;

    public sealed class DoubleTests : BaseTests
    {
        [Theory(DisplayName = T + "Double: NaN/NotNaN.")]
        [InlineData(null, null)]
        [InlineData(double.NaN, double.NegativeInfinity)]
        [InlineData(double.NaN, -1.0)]
        [InlineData(double.NaN, 0.0)]
        [InlineData(double.NaN, 1.0)]
        [InlineData(double.NaN, double.PositiveInfinity)]
        public void NaN(double? nan, double? nonNaN)
        {
            var nullableNaNArg = Guard.Argument(() => nan).NaN();
            var nullableNonNaNArg = Guard.Argument(() => nonNaN).NotNaN();
            if (nan.HasValue)
            {
                ThrowsArgumentOutOfRangeException(
                    nullableNonNaNArg,
                    arg => arg.NaN(),
                    (arg, message) => arg.NaN(d =>
                    {
                        Assert.Equal(nonNaN, d);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    nullableNaNArg,
                    arg => arg.NotNaN(),
                    (arg, message) => arg.NotNaN(message));

                var nanArg = Guard.Argument(nan.Value, nameof(nan)).NaN();
                var nonNaNArg = Guard.Argument(nonNaN.Value, nameof(nonNaN)).NotNaN();
                ThrowsArgumentOutOfRangeException(
                    nonNaNArg,
                    arg => arg.NaN(),
                    (arg, message) => arg.NaN(d =>
                    {
                        Assert.Equal(nonNaN, d);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    nanArg,
                    arg => arg.NotNaN(),
                    (arg, message) => arg.NotNaN(message));
            }
        }

        [Theory(DisplayName = T + "Double: Infinity/NotInfinity.")]
        [InlineData(null, null)]
        [InlineData(double.NegativeInfinity, double.NaN)]
        [InlineData(double.NegativeInfinity, -1.0)]
        [InlineData(double.NegativeInfinity, 0.0)]
        [InlineData(double.NegativeInfinity, 1.0)]
        [InlineData(double.PositiveInfinity, double.NaN)]
        [InlineData(double.PositiveInfinity, -1.0)]
        [InlineData(double.PositiveInfinity, -0.0)]
        [InlineData(double.PositiveInfinity, 1.0)]
        public void Infinity(double? infinity, double? nonInfinity)
        {
            var nullableInfinityArg = Guard.Argument(() => infinity).Infinity();
            var nullableNonInfinityArg = Guard.Argument(() => nonInfinity).NotInfinity();
            if (infinity.HasValue)
            {
                ThrowsArgumentOutOfRangeException(
                    nullableNonInfinityArg,
                    arg => arg.Infinity(),
                    (arg, message) => arg.Infinity(d =>
                    {
                        Assert.Equal(nonInfinity, d);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    nullableInfinityArg,
                    arg => arg.NotInfinity(),
                    (arg, message) => arg.NotInfinity(d =>
                    {
                        Assert.Equal(infinity, d);
                        return message;
                    }));

                var infinityArg = Guard.Argument(infinity.Value, nameof(infinity)).Infinity();
                var nonInfinityArg = Guard.Argument(nonInfinity.Value, nameof(nonInfinity)).NotInfinity();
                ThrowsArgumentOutOfRangeException(
                    nonInfinityArg,
                    arg => arg.Infinity(),
                    (arg, message) => arg.Infinity(d =>
                    {
                        Assert.Equal(nonInfinity, d);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    infinityArg,
                    arg => arg.NotInfinity(),
                    (arg, message) => arg.NotInfinity(d =>
                    {
                        Assert.Equal(infinity, d);
                        return message;
                    }));
            }
        }

        [Theory(DisplayName = T + "Double: PositiveInfinity/NotPositiveInfinity.")]
        [InlineData(null, null)]
        [InlineData(double.PositiveInfinity, double.NaN)]
        [InlineData(double.PositiveInfinity, double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity, -1.0)]
        [InlineData(double.PositiveInfinity, 0.0)]
        [InlineData(double.PositiveInfinity, 1.0)]
        public void PositiveInfinity(double? infinity, double? nonInfinity)
        {
            var nullableInfinityArg = Guard.Argument(() => infinity).PositiveInfinity();
            var nullableNonInfinityArg = Guard.Argument(() => nonInfinity).NotPositiveInfinity();
            if (infinity.HasValue)
            {
                ThrowsArgumentOutOfRangeException(
                    nullableNonInfinityArg,
                    arg => arg.PositiveInfinity(),
                    (arg, message) => arg.PositiveInfinity(d =>
                    {
                        Assert.Equal(nonInfinity, d);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    nullableInfinityArg,
                    arg => arg.NotPositiveInfinity(),
                    (arg, message) => arg.NotPositiveInfinity(message));

                var infinityArg = Guard.Argument(infinity.Value, nameof(infinity)).PositiveInfinity();
                var nonInfinityArg = Guard.Argument(nonInfinity.Value, nameof(nonInfinity)).NotPositiveInfinity();
                ThrowsArgumentOutOfRangeException(
                    nonInfinityArg,
                    arg => arg.PositiveInfinity(),
                    (arg, message) => arg.PositiveInfinity(d =>
                    {
                        Assert.Equal(nonInfinity, d);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    infinityArg,
                    arg => arg.NotPositiveInfinity(),
                    (arg, message) => arg.NotPositiveInfinity(message));
            }
        }

        [Theory(DisplayName = T + "Double: NegativeInfinity/NotNegativeInfinity.")]
        [InlineData(null, null)]
        [InlineData(double.NegativeInfinity, double.NaN)]
        [InlineData(double.NegativeInfinity, -1.0)]
        [InlineData(double.NegativeInfinity, 0.0)]
        [InlineData(double.NegativeInfinity, 1.0)]
        [InlineData(double.NegativeInfinity, double.PositiveInfinity)]
        public void NegativeInfinity(double? infinity, double? nonInfinity)
        {
            var nullableInfinityArg = Guard.Argument(() => infinity).NegativeInfinity();
            var nullableNonInfinityArg = Guard.Argument(() => nonInfinity).NotNegativeInfinity();
            if (infinity.HasValue)
            {
                ThrowsArgumentOutOfRangeException(
                    nullableNonInfinityArg,
                    arg => arg.NegativeInfinity(),
                    (arg, message) => arg.NegativeInfinity(d =>
                    {
                        Assert.Equal(nonInfinity, d);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    nullableInfinityArg,
                    arg => arg.NotNegativeInfinity(),
                    (arg, message) => arg.NotNegativeInfinity(message));

                var infinityArg = Guard.Argument(infinity.Value, nameof(infinity)).NegativeInfinity();
                var nonInfinityArg = Guard.Argument(nonInfinity.Value, nameof(nonInfinity)).NotNegativeInfinity();
                ThrowsArgumentOutOfRangeException(
                    nonInfinityArg,
                    arg => arg.NegativeInfinity(),
                    (arg, message) => arg.NegativeInfinity(d =>
                    {
                        Assert.Equal(nonInfinity, d);
                        return message;
                    }));

                ThrowsArgumentOutOfRangeException(
                    infinityArg,
                    arg => arg.NotNegativeInfinity(),
                    (arg, message) => arg.NotNegativeInfinity(message));
            }
        }
    }
}
