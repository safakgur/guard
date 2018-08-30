namespace Dawn.Tests
{
    using Xunit;

    public sealed class NullTests : BaseTests
    {
        [Fact(DisplayName = T + "Nullable class: Null/NotNull")]
        public void NullReference()
        {
            var @null = null as string;
            var nullArg = Guard.Argument(@null).Null();

            var nonNull = "A";
            var nonNullArg = Guard.Argument(nonNull).NotNull();

            ThrowsArgumentException(
                nonNullArg,
                arg => arg.Null(),
                (arg, message) => arg.Null(s =>
                {
                    Assert.Same(nonNull, s);
                    return message;
                }));

            ThrowsArgumentNullException(
                nullArg,
                arg => arg.NotNull(),
                (arg, message) => arg.NotNull(message));
        }

        [Fact(DisplayName = T + "Nullable struct: Null/NotNull")]
        public void NullValue()
        {
            var @null = null as int?;
            var nullArg = Guard.Argument(() => @null).Null();
            Assert.False(nullArg.HasValue());

            ThrowsArgumentNullException(
                nullArg,
                arg => arg.NotNull(),
                (arg, message) => arg.NotNull(message));

            var one = 1 as int?;
            for (var i = 0; i < 2; i++)
            {
                var nullableOneArg = Guard.Argument(() => one, i == 1);
                Assert.IsType<Guard.ArgumentInfo<int?>>(nullableOneArg);
                Assert.True(nullableOneArg.HasValue());

                ThrowsArgumentException(
                    nullableOneArg,
                    arg => arg.Null(),
                    (arg, message) => arg.Null(v =>
                    {
                        Assert.Equal(one, v);
                        return message;
                    }));

                var oneArg = nullableOneArg.NotNull();
                Assert.IsType<Guard.ArgumentInfo<int>>(oneArg);
                Assert.True(oneArg.HasValue());
                Assert.Equal(nullableOneArg.Value, oneArg.Value);
                Assert.Equal(nullableOneArg.Secure, oneArg.Secure);
            }
        }
    }
}
