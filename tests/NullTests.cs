namespace Dawn.Tests
{
    using System;
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
            var nullArg = Guard.Argument(@null).Null();
            Assert.False(nullArg.NotNull(out var a));
            Assert.Equal(default, a);

            var nonNull = 1 as int?;
            var nonNullArg = Guard.Argument(nonNull);
            Assert.IsType<Guard.ArgumentInfo<int?>>(nonNullArg);
            Assert.IsType<Guard.ArgumentInfo<int>>(nonNullArg.NotNull());
            Assert.True(nonNullArg.NotNull(out a));
            Assert.IsType<Guard.ArgumentInfo<int>>(a);

            ThrowsArgumentException(
                nonNullArg,
                arg => arg.Null(),
                (arg, message) => arg.Null(i =>
                {
                    Assert.Equal(nonNull, i);
                    return message;
                }));

            ThrowsArgumentNullException(
                nullArg,
                arg => arg.NotNull(),
                (arg, message) => arg.NotNull(message));
        }
    }
}
