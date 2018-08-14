namespace Dawn.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public sealed class GenericTests : BaseTests
    {
        [ThreadStatic]
        private static object currentValue;

        [Theory(DisplayName = T + "Generic: Require (default exception)")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireDefaultException<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value).Require(Success);

            ThrowsArgumentException(
                valueArg,
                arg => arg.Require(Fail),
                (arg, message) => arg.Require(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Generic: Require (argument exception w/ message)")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireArgumentExceptionWithMessage<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value).Require<TestArgException>(Success);

            ThrowsException<T, TestArgException>(
                valueArg,
                arg => arg.Require<TestArgException>(Fail),
                (arg, message) => arg.Require<TestArgException>(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Generic: Require (argument exception w/o message)")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireArgumentExceptionWithoutMessage<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value).Require<TestArgExceptionNoMessage>(Success);

            ThrowsException<T, TestArgExceptionNoMessage>(
                valueArg,
                arg => arg.Require<TestArgExceptionNoMessage>(Fail),
                (arg, message) => arg.Require<TestArgExceptionNoMessage>(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }), true);
        }

        [Theory(DisplayName = T + "Generic: Require (common exception w/ message)")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireCommonExceptionWithMessage<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value).Require<TestException>(Success);

            ThrowsException<T, TestException>(
                valueArg,
                arg => arg.Require<TestException>(Fail),
                (arg, message) => arg.Require<TestException>(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Generic: Require (common exception w/o message)")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireCommonExceptionWithoutMessage<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value).Require<TestExceptionNoMessage>(Success);

            ThrowsException<T, TestExceptionNoMessage>(
                valueArg,
                arg => arg.Require<TestExceptionNoMessage>(Fail),
                (arg, message) => arg.Require<TestExceptionNoMessage>(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }), true);
        }

        [Theory(DisplayName = T + "Generic: Require (exception w/o ctor)")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireExceptionWithoutConstructor<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value).Require<TestExceptionNoCtor>(Success);

            ThrowsArgumentException(
                valueArg,
                arg =>
                {
                    try
                    {
                        arg.Require<TestExceptionNoCtor>(Fail);
                    }
                    catch (ArgumentException ex)
                    {
                        Assert.Null(ex.ParamName);
                        Assert.Contains(typeof(TestExceptionNoCtor).ToString(), ex.Message);
                        throw ex.InnerException;
                    }
                },
                (arg, message) =>
                {
                    try
                    {
                        arg.Require<TestExceptionNoCtor>(Fail, v =>
                        {
                            Assert.Equal(value, v);
                            return message;
                        });
                    }
                    catch (ArgumentException ex)
                    {
                        Assert.Null(ex.ParamName);
                        Assert.Contains(typeof(TestExceptionNoCtor).ToString(), ex.Message);
                        throw ex.InnerException;
                    }
                });
        }

        [Fact(DisplayName = T + "Generic: Compatible/NotCompatible")]
        public void Compatible()
        {
            using (var memory = new MemoryStream() as Stream)
            {
                var @null = null as Stream;
                var nullArg = Guard.Argument(() => @null)
                    .Compatible<object>()
                    .Compatible<MemoryStream>()
                    .Compatible<string>();

                var memoryArg = Guard.Argument(() => memory)
                    .Compatible<object>()
                    .Compatible<MemoryStream>();

                ThrowsArgumentException(
                    memoryArg,
                    arg => arg.Compatible<string>(),
                    (arg, message) => arg.Compatible<string>(s =>
                    {
                        Assert.Same(memory, s);
                        return message;
                    }));

                nullArg
                    .NotCompatible<object>()
                    .NotCompatible<MemoryStream>()
                    .NotCompatible<string>();

                memoryArg.NotCompatible<string>();

                ThrowsArgumentException(
                    memoryArg,
                    arg => arg.NotCompatible<object>(),
                    (arg, message) => arg.NotCompatible<object>(o =>
                    {
                        Assert.Same(memory, o);
                        return message;
                    }));

                ThrowsArgumentException(
                    memoryArg,
                    arg => arg.NotCompatible<MemoryStream>(),
                    (arg, message) => arg.NotCompatible<MemoryStream>(s =>
                    {
                        Assert.Same(memory, s);
                        return message;
                    }));
            }
        }

        [Fact(DisplayName = T + "Generic: Cast")]
        public void Cast()
        {
            using (var memory = new MemoryStream() as Stream)
            {
                var @null = null as Stream;
                var nullArg = Guard.Argument(() => @null);

                ThrowsArgumentException(
                    nullArg,
                    arg => arg.Cast<object>(),
                    (arg, message) => arg.Cast<object>(s =>
                    {
                        Assert.Null(s);
                        return message;
                    }));

                ThrowsArgumentException(
                    nullArg,
                    arg => arg.Cast<MemoryStream>(),
                    (arg, message) => arg.Cast<MemoryStream>(s =>
                    {
                        Assert.Null(s);
                        return message;
                    }));

                var memoryArg = Guard.Argument(() => memory);
                Assert.Same(memory, memoryArg.Cast<object>().Value);
                Assert.Same(memory, memoryArg.Cast<MemoryStream>().Value);

                ThrowsArgumentException(
                    memoryArg,
                    arg => arg.Cast<string>(),
                    (arg, message) => arg.Cast<string>(s =>
                    {
                        Assert.Same(memory, s);
                        return message;
                    }));
            }
        }

        private static bool Success<T>(T v)
        {
            Assert.Equal((T)currentValue, v);
            return true;
        }

        private static bool Fail<T>(T v)
        {
            Assert.Equal((T)currentValue, v);
            return false;
        }

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
    }
}
