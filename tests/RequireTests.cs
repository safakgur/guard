namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed class RequireTests : BaseTests
    {
        [ThreadStatic]
        private static object currentValue;

        [Theory(DisplayName = T + "Require: Default exception")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireDefaultException<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value).Require(true).Require(Success);
            if (value == null)
            {
                valueArg.Require(false).Require(Fail);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.Require(false),
                (arg, message) => arg.Require(false, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.Require(Fail),
                (arg, message) => arg.Require(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Require: Argument exception w/ message")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireArgumentExceptionWithMessage<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value)
                .Require<TestArgException>(true)
                .Require<TestArgException>(Success);

            if (value == null)
            {
                valueArg
                    .Require<TestArgException>(false)
                    .Require<TestArgException>(Fail);

                return;
            }

            ThrowsException<T, TestArgException>(
                valueArg,
                arg => arg.Require<TestArgException>(false),
                (arg, message) => arg.Require<TestArgException>(false, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));

            ThrowsException<T, TestArgException>(
                valueArg,
                arg => arg.Require<TestArgException>(Fail),
                (arg, message) => arg.Require<TestArgException>(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Require: Argument exception w/o message")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireArgumentExceptionWithoutMessage<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value)
                .Require<TestArgExceptionNoMessage>(true)
                .Require<TestArgExceptionNoMessage>(Success);

            if (value == null)
            {
                valueArg
                    .Require<TestArgExceptionNoMessage>(false)
                    .Require<TestArgExceptionNoMessage>(Fail);

                return;
            }

            ThrowsException<T, TestArgExceptionNoMessage>(
                valueArg,
                arg => arg.Require<TestArgExceptionNoMessage>(false),
                (arg, message) => arg.Require<TestArgExceptionNoMessage>(false, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }), true);

            ThrowsException<T, TestArgExceptionNoMessage>(
                valueArg,
                arg => arg.Require<TestArgExceptionNoMessage>(Fail),
                (arg, message) => arg.Require<TestArgExceptionNoMessage>(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }), true);
        }

        [Theory(DisplayName = T + "Require: Common exception w/ message")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireCommonExceptionWithMessage<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value)
                .Require<TestException>(true)
                .Require<TestException>(Success);

            if (value == null)
            {
                valueArg
                    .Require<TestException>(false)
                    .Require<TestException>(Fail);

                return;
            }

            ThrowsException<T, TestException>(
                valueArg,
                arg => arg.Require<TestException>(false),
                (arg, message) => arg.Require<TestException>(false, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));

            ThrowsException<T, TestException>(
                valueArg,
                arg => arg.Require<TestException>(Fail),
                (arg, message) => arg.Require<TestException>(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Require: Common exception w/o message")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireCommonExceptionWithoutMessage<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value)
                .Require<TestExceptionNoMessage>(true)
                .Require<TestExceptionNoMessage>(Success);

            if (value == null)
            {
                valueArg
                    .Require<TestExceptionNoMessage>(false)
                    .Require<TestExceptionNoMessage>(Fail);

                return;
            }

            ThrowsException<T, TestExceptionNoMessage>(
                valueArg,
                arg => arg.Require<TestExceptionNoMessage>(false),
                (arg, message) => arg.Require<TestExceptionNoMessage>(false, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }), true);

            ThrowsException<T, TestExceptionNoMessage>(
                valueArg,
                arg => arg.Require<TestExceptionNoMessage>(Fail),
                (arg, message) => arg.Require<TestExceptionNoMessage>(Fail, v =>
                {
                    Assert.Equal(value, v);
                    return message;
                }), true);
        }

        [Theory(DisplayName = T + "Require: Exception w/o ctor")]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("A")]
        public void RequireExceptionWithoutConstructor<T>(T value)
        {
            currentValue = value;
            var valueArg = Guard.Argument(() => value)
                .Require<TestExceptionNoCtor>(true)
                .Require<TestExceptionNoCtor>(Success);

            if (value == null)
            {
                valueArg
                    .Require<TestExceptionNoCtor>(false)
                    .Require<TestExceptionNoCtor>(Fail);

                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg =>
                {
                    try
                    {
                        arg.Require<TestExceptionNoCtor>(false);
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
                        arg.Require<TestExceptionNoCtor>(false, v =>
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
                },
                doNotTestScoping: true);

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
                },
                doNotTestScoping: true);
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
