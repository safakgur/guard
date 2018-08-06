namespace Dawn.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;

    public sealed class InitializationTests : BaseTests
    {
        [Fact(DisplayName = T + "Argument: Uninitialized")]
        public void Uninitialized()
        {
            var int32Arg = default(Guard.ArgumentInfo<int>);
            Assert.Equal(default, int32Arg.Value);
            Assert.Contains(typeof(int).ToString(), int32Arg.Name);
            Assert.False(int32Arg.Modified);

            var nullableInt32Arg = default(Guard.ArgumentInfo<int?>);
            Assert.Equal(default, nullableInt32Arg.Value);
            Assert.Contains(typeof(int?).ToString(), nullableInt32Arg.Name);
            Assert.False(nullableInt32Arg.Modified);

            var stringArg = default(Guard.ArgumentInfo<string>);
            Assert.Equal(default, stringArg.Value);
            Assert.Contains(typeof(string).ToString(), stringArg.Name);
            Assert.False(stringArg.Modified);
        }

        [Theory(DisplayName = T + "Argument: Member expression")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("S")]
        public void MemberExpression<T>(T value)
        {
            var arg = Guard.Argument(() => value);
            Assert.Equal(value, arg.Value);
            Assert.Equal(nameof(value), arg.Name);
            Assert.False(arg.Modified);
        }

        [Fact(DisplayName = T + "Argument: Validates member expression")]
        public void ValidatesExpression()
        {
            Assert.Throws<ArgumentNullException>("e", () => Guard.Argument<int>(null));
            Assert.Throws<ArgumentException>("e", () => Guard.Argument(() => 1));
        }

        [Theory(DisplayName = T + "Argument: Value and name")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("S")]
        public void ValueAndName<T>(T value)
        {
            var arg = Guard.Argument(value, nameof(value));
            Assert.Equal(value, arg.Value);
            Assert.Equal(nameof(value), arg.Name);
            Assert.False(arg.Modified);

            for (var i = 0; i < 3; i++)
            {
                arg = i == 0 ? new Guard.ArgumentInfo<T>(value, nameof(value))
                    : i == 1 ? new Guard.ArgumentInfo<T>(value, nameof(value), false)
                    : new Guard.ArgumentInfo<T>(value, nameof(value), true);

                Assert.Equal(value, arg.Value);
                Assert.Equal(nameof(value), arg.Name);
                Assert.Equal(i == 2, arg.Modified);
            }
        }

        [Theory(DisplayName = T + "Argument: Value-only")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("S")]
        public void ValueOnly<T>(T value)
        {
            var arg = Guard.Argument(value);
            Assert.Equal(value, arg.Value);
            Assert.Contains(typeof(T).ToString(), arg.Name);
            Assert.False(arg.Modified);

            for (var i = 0; i < 2; i++)
            {
                arg = i == 0 ? new Guard.ArgumentInfo<T>(value, null)
                   : i == 1 ? new Guard.ArgumentInfo<T>(value, null, false)
                   : new Guard.ArgumentInfo<T>(value, null, true);

                Assert.Equal(value, arg.Value);
                Assert.Contains(typeof(T).ToString(), arg.Name);
                Assert.Equal(i == 2, arg.Modified);
            }
        }
    }
}
