using System;
using Xunit;

namespace Dawn.Tests
{
    public sealed class NullTests : BaseTests
    {
        [Fact(DisplayName = "Nullable class: Null/NotNull")]
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

        [Fact(DisplayName = "Nullable struct: Null/NotNull")]
        public void NullValue()
        {
            var @null = null as int?;
            var nullArg = Guard.Argument(() => @null).Null();
            Assert.False(nullArg.HasValue);

            ThrowsArgumentNullException(
                nullArg,
                arg => arg.NotNull(),
                (arg, message) => arg.NotNull(message));

            var one = 1 as int?;
            for (var i = 0; i < 2; i++)
            {
                var nullableOneArg = Guard.Argument(() => one, i == 1);
                Assert.IsType<Guard.ArgumentInfo<int?>>(nullableOneArg);
                Assert.True(nullableOneArg.HasValue);

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
                Assert.True(oneArg.HasValue);
                Assert.Equal(nullableOneArg.Value, oneArg.Value);
                Assert.Equal(nullableOneArg.Secure, oneArg.Secure);
            }
        }

        [Theory(DisplayName = "Null: NotAllNull`2")]
        [InlineData(1, "A", true)]
        [InlineData(null, "A", true)]
        [InlineData(1, null, true)]
        [InlineData(null, null, false)]
        public void NotAllNull2<T1, T2>(T1 val1, T2 val2, bool valid)
        {
            if (!valid)
            {
                var paramName = $"{nameof(val1)}, {nameof(val2)}";
                Assert.Throws<ArgumentNullException>(paramName, () => Test());
                return;
            }

            Test();

            void Test() => Guard.NotAllNull(
                Guard.Argument(() => val1),
                Guard.Argument(() => val2));
        }

        [Theory(DisplayName = "Null: NotAllNull`3")]
        [InlineData(1, "A", 1.0, true)]
        [InlineData(null, "A", 1.0, true)]
        [InlineData(1, null, 1.0, true)]
        [InlineData(1, "A", null, true)]
        [InlineData(null, null, 1.0, true)]
        [InlineData(1, null, null, true)]
        [InlineData(null, "A", null, true)]
        [InlineData(null, null, null, false)]
        public void NotAllNull3<T1, T2, T3>(T1 val1, T2 val2, T3 val3, bool valid)
        {
            if (!valid)
            {
                var paramName = $"{nameof(val1)}, {nameof(val2)}, {nameof(val3)}";
                Assert.Throws<ArgumentNullException>(paramName, () => Test());
                return;
            }

            Test();

            void Test() => Guard.NotAllNull(
                Guard.Argument(() => val1),
                Guard.Argument(() => val2),
                Guard.Argument(() => val3));
        }
    }
}
