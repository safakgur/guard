namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        #region Methods

        [Fact(DisplayName = "Guard can initialize arguments.")]
        public void GuardCanInitializeArguments()
        {
            var i = 1;

            // Assigns the right value and name.
            var int32Arg = Guard.Argument(i, nameof(i));
            Assert.Equal(i, int32Arg.Value);
            Assert.Equal(nameof(i), int32Arg.Name);

            int32Arg = Guard.Argument(() => i);
            Assert.Equal(i, int32Arg.Value);
            Assert.Equal(nameof(i), int32Arg.Name);

            int32Arg = Guard.Argument(i);
            Assert.Equal(i, int32Arg.Value);
            Assert.Equal($"The {typeof(int)} argument", int32Arg.Name);

            Assert.Throws<ArgumentNullException>("e", () => Guard.Argument<int>(null));
            Assert.Throws<ArgumentException>("e", () => Guard.Argument(() => 1));

            // Allows null values.
            string s = null;

            var stringArg = Guard.Argument(s, nameof(s));
            Assert.Equal(s, stringArg.Value);
            Assert.Equal(nameof(s), stringArg.Name);

            stringArg = Guard.Argument(() => s);
            Assert.Equal(s, stringArg.Value);
            Assert.Equal(nameof(s), stringArg.Name);

            stringArg = Guard.Argument(s);
            Assert.Equal(s, stringArg.Value);
            Assert.Equal($"The {typeof(string)} argument", stringArg.Name);

            Assert.Throws<ArgumentNullException>("e", () => Guard.Argument<string>(null));
            Assert.Throws<ArgumentException>("e", () => Guard.Argument<string>(() => null));

            // Has the correct name when unitialized.
            int32Arg = default;
            Assert.Equal(0, int32Arg.Value);
            Assert.Equal($"The {typeof(int)} argument", int32Arg.Name);

            stringArg = default;
            Assert.Null(stringArg.Value);
            Assert.Equal($"The {typeof(string)} argument", stringArg.Name);
        }

        [Fact(DisplayName = "Guard supports custom preconditions.")]
        public void GuardSupportsCustomPreconditions()
        {
            var number = 1;

            Guard.Argument(() => number).Require(i => i < 2, i => i.ToString());

            // The default exception.
            Assert.Throws<ArgumentException>(nameof(number), () =>
                Guard.Argument(() => number).Require(i => i > 2));

            var argEx = Assert.Throws<ArgumentException>(nameof(number), () =>
                Guard.Argument(() => number).Require(i => i > 2, i => i.ToString()));

            Assert.StartsWith(number.ToString(), argEx.Message);

            // Custom argument exception.
            Assert.Throws<TestArgException>(nameof(number), () =>
                Guard.Argument(() => number).Require<TestArgException>(i => i > 2));

            var customArgEx = Assert.Throws<TestArgException>(nameof(number), () =>
                Guard.Argument(() => number).Require<TestArgException>(i => i > 2, i => i.ToString()));

            Assert.StartsWith(number.ToString(), customArgEx.Message);

            // Custom argument exception without message.
            Assert.Throws<TestArgExceptionNoMessage>(nameof(number), () =>
                Guard.Argument(() => number).Require<TestArgExceptionNoMessage>(i => i > 2));

            var customArgExNoMsg = Assert.Throws<TestArgExceptionNoMessage>(nameof(number), () =>
                Guard.Argument(() => number).Require<TestArgExceptionNoMessage>(i => i > 2, i => i.ToString()));

            // Custom exception.
            Assert.Throws<TestException>(() =>
                Guard.Argument(() => number).Require<TestException>(i => i > 2));

            var customEx = Assert.Throws<TestException>(() =>
                Guard.Argument(() => number).Require<TestException>(i => i > 2, i => i.ToString()));

            Assert.Equal(number.ToString(), customEx.Message);

            // Custom exception without message.
            Assert.Throws<TestExceptionNoMessage>(() =>
                Guard.Argument(() => number).Require<TestExceptionNoMessage>(i => i > 2));

            var customExNoMsg = Assert.Throws<TestExceptionNoMessage>(() =>
                Guard.Argument(() => number).Require<TestExceptionNoMessage>(i => i > 2, i => i.ToString()));

            Assert.NotEqual(number.ToString(), customExNoMsg.Message);

            // Custom exception without public constructor.
            Assert.Throws<ArgumentException>(() =>
                Guard.Argument(() => number).Require<TestExceptionNoCtor>(i => i > 2));

            var customExNoCtor = Assert.Throws<ArgumentException>(() =>
                Guard.Argument(() => number).Require<TestExceptionNoCtor>(i => i > 2, i => i.ToString()));

            Assert.Null(customExNoCtor.ParamName);
            Assert.NotEqual(number.ToString(), customExNoCtor.Message);

            var inner = Assert.IsType<ArgumentException>(customExNoCtor.InnerException);
            Assert.Equal(nameof(number), inner.ParamName);
            Assert.StartsWith(number.ToString(), inner.Message);
        }

        #endregion Methods

        #region Classes

        private sealed class TestArgException : ArgumentException
        {
            public TestArgException(string paramName, string message)
                : base(message, paramName)
            {
            }
        }

        private sealed class TestArgExceptionNoMessage : ArgumentException
        {
            public TestArgExceptionNoMessage(string paramName)
                : base(null, paramName)
            {
            }
        }

        private sealed class TestException : Exception
        {
            public TestException(string message)
                : base(message)
            {
            }
        }

        private sealed class TestExceptionNoMessage : Exception
        {
        }

        private sealed class TestExceptionNoCtor : Exception
        {
            private TestExceptionNoCtor()
            {
            }
        }

        #endregion Classes
    }
}
