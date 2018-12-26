using System;
using Xunit;

namespace Dawn.Tests
{
    public class InterceptorBasicTests : BaseTests
    {
        [Theory(DisplayName = "Intercept test")]
        [InlineData("A")]
        public void Intercept(string argument)
        {
            var intercepted = false;
            Exception interceptedException = null;

            var emptyArg = Guard.Argument(
                () => argument,
                exceptionInterceptor: (ex) =>
                 {
                     intercepted = true;
                     interceptedException = ex;
                 });

            var exception = Assert.Throws<ArgumentException>(() => emptyArg.Empty());
            Assert.True(intercepted);
            Assert.Equal(exception, interceptedException);
        }

        [Theory(DisplayName = "No exception")]
        [InlineData("")]
        public void NoException(string argument)
        {
            var intercepted = false;
            Exception interceptedException = null;

            var emptyArg = Guard.Argument(
                () => argument,
                exceptionInterceptor: (ex) =>
                {
                    intercepted = true;
                    interceptedException = ex;
                });

            Assert.False(intercepted);
            Assert.Null(interceptedException);
        }

        [Theory(DisplayName = "Modified argument info test")]
        [InlineData("A")]
        public void Modified(string argument)
        {
            var intercepted = false;
            Exception interceptedException = null;

            var emptyArg = Guard.Argument(
                () => argument,
                exceptionInterceptor: (ex) =>
                {
                    intercepted = true;
                    interceptedException = ex;
                });

            var exception = Assert.Throws<ArgumentException>(() =>
                emptyArg
                    .NotEmpty()
                    .Modify(s => "")
                    .NotEmpty()
                    );

            Assert.True(intercepted);
            Assert.Equal(exception, interceptedException);
        }

        [Theory(DisplayName = "No interceptor test")]
        [InlineData("A")]
        public void NoInterceptor(string argument)
        {
            var emptyArg = Guard.Argument(
                () => argument,
                exceptionInterceptor: null);

            var exception = Assert.Throws<ArgumentException>(() => emptyArg.Empty());
        }

        [Theory(DisplayName = "Exception in interceptor test")]
        [InlineData("A")]
        public void ExceptionInInterceptor(string argument)
        {
            var emptyArg = Guard.Argument(
                () => argument,
                exceptionInterceptor: (ex) =>
            {
                throw new InvalidOperationException();
            });

            //When the interceptor fails, its exception is thrown instead of the Guard exception (it's not "hidded" by design)
            var exception = Assert.Throws<InvalidOperationException>(() => emptyArg.Empty());
        }
    }
}
