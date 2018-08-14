namespace Dawn.Tests
{
    using Xunit;

    public sealed class SingleTests : BaseTests
    {
        [Theory(DisplayName = T + "Single: NaN/NotNaN")]
        [InlineData(null, null)]
        [InlineData(float.NaN, float.NegativeInfinity)]
        [InlineData(float.NaN, -1.0f)]
        [InlineData(float.NaN, 0.0f)]
        [InlineData(float.NaN, 1.0f)]
        [InlineData(float.NaN, float.PositiveInfinity)]
        public void NaN(float? nan, float? nonNaN)
        {
            var nullableNaNArg = Guard.Argument(() => nan).NaN();
            var nullableNonNaNArg = Guard.Argument(() => nonNaN).NotNaN();
            if (!nan.HasValue)
            {
                nullableNaNArg.NotNaN();
                nullableNonNaNArg.NaN();
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableNonNaNArg,
                arg => arg.NaN(),
                (arg, message) => arg.NaN(f =>
                {
                    Assert.Equal(nonNaN, f);
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
                (arg, message) => arg.NaN(f =>
                {
                    Assert.Equal(nonNaN, f);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                nanArg,
                arg => arg.NotNaN(),
                (arg, message) => arg.NotNaN(message));
        }

        [Theory(DisplayName = T + "Single: Infinity/NotInfinity")]
        [InlineData(null, null)]
        [InlineData(float.NegativeInfinity, float.NaN)]
        [InlineData(float.NegativeInfinity, -1.0f)]
        [InlineData(float.NegativeInfinity, 0.0f)]
        [InlineData(float.NegativeInfinity, 1.0f)]
        [InlineData(float.PositiveInfinity, float.NaN)]
        [InlineData(float.PositiveInfinity, -1.0f)]
        [InlineData(float.PositiveInfinity, -0.0f)]
        [InlineData(float.PositiveInfinity, 1.0f)]
        public void Infinity(float? infinity, float? nonInfinity)
        {
            var nullableInfinityArg = Guard.Argument(() => infinity).Infinity();
            var nullableNonInfinityArg = Guard.Argument(() => nonInfinity).NotInfinity();
            if (!infinity.HasValue)
            {
                nullableInfinityArg.NotInfinity();
                nullableNonInfinityArg.Infinity();
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableNonInfinityArg,
                arg => arg.Infinity(),
                (arg, message) => arg.Infinity(f =>
                {
                    Assert.Equal(nonInfinity, f);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                nullableInfinityArg,
                arg => arg.NotInfinity(),
                (arg, message) => arg.NotInfinity(f =>
                {
                    Assert.Equal(infinity, f);
                    return message;
                }));

            var infinityArg = Guard.Argument(infinity.Value, nameof(infinity)).Infinity();
            var nonInfinityArg = Guard.Argument(nonInfinity.Value, nameof(nonInfinity)).NotInfinity();
            ThrowsArgumentOutOfRangeException(
                nonInfinityArg,
                arg => arg.Infinity(),
                (arg, message) => arg.Infinity(f =>
                {
                    Assert.Equal(nonInfinity, f);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                infinityArg,
                arg => arg.NotInfinity(),
                (arg, message) => arg.NotInfinity(f =>
                {
                    Assert.Equal(infinity, f);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Single: PositiveInfinity/NotPositiveInfinity")]
        [InlineData(null, null)]
        [InlineData(float.PositiveInfinity, float.NaN)]
        [InlineData(float.PositiveInfinity, float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity, -1.0f)]
        [InlineData(float.PositiveInfinity, 0.0f)]
        [InlineData(float.PositiveInfinity, 1.0f)]
        public void PositiveInfinity(float? infinity, float? nonInfinity)
        {
            var nullableInfinityArg = Guard.Argument(() => infinity).PositiveInfinity();
            var nullableNonInfinityArg = Guard.Argument(() => nonInfinity).NotPositiveInfinity();
            if (!infinity.HasValue)
            {
                nullableInfinityArg.NotPositiveInfinity();
                nullableNonInfinityArg.PositiveInfinity();
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableNonInfinityArg,
                arg => arg.PositiveInfinity(),
                (arg, message) => arg.PositiveInfinity(f =>
                {
                    Assert.Equal(nonInfinity, f);
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
                (arg, message) => arg.PositiveInfinity(f =>
                {
                    Assert.Equal(nonInfinity, f);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                infinityArg,
                arg => arg.NotPositiveInfinity(),
                (arg, message) => arg.NotPositiveInfinity(message));
        }

        [Theory(DisplayName = T + "Single: NegativeInfinity/NotNegativeInfinity")]
        [InlineData(null, null)]
        [InlineData(float.NegativeInfinity, float.NaN)]
        [InlineData(float.NegativeInfinity, -1.0f)]
        [InlineData(float.NegativeInfinity, 0.0f)]
        [InlineData(float.NegativeInfinity, 1.0f)]
        [InlineData(float.NegativeInfinity, float.PositiveInfinity)]
        public void NegativeInfinity(float? infinity, float? nonInfinity)
        {
            var nullableInfinityArg = Guard.Argument(() => infinity).NegativeInfinity();
            var nullableNonInfinityArg = Guard.Argument(() => nonInfinity).NotNegativeInfinity();
            if (!infinity.HasValue)
            {
                nullableInfinityArg.NotNegativeInfinity();
                nullableNonInfinityArg.NegativeInfinity();
                return;
            }

            ThrowsArgumentOutOfRangeException(
                nullableNonInfinityArg,
                arg => arg.NegativeInfinity(),
                (arg, message) => arg.NegativeInfinity(f =>
                {
                    Assert.Equal(nonInfinity, f);
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
                (arg, message) => arg.NegativeInfinity(f =>
                {
                    Assert.Equal(nonInfinity, f);
                    return message;
                }));

            ThrowsArgumentOutOfRangeException(
                infinityArg,
                arg => arg.NotNegativeInfinity(),
                (arg, message) => arg.NotNegativeInfinity(message));
        }
    }
}
