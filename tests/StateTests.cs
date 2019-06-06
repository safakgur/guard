namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed class StateTests : BaseTests
    {
        [Fact(DisplayName = "State: Operation")]
        public void TestOperation()
        {
            Guard.Operation(true);
            Guard.Operation(true, RandomMessage);
            Guard.Operation(true, RandomMessage, RandomMessage);

            var exceptions = ThrowsException<InvalidOperationException>(
                () => Guard.Operation(false),
                message => Guard.Operation(false, message));

            Assert.Contains(nameof(TestOperation), exceptions[0].Message);
        }

        [Fact(DisplayName = "State: Support")]
        public void TestSupport()
        {
            Guard.Support(true);
            Guard.Support(true, RandomMessage);
            Guard.Support(true, RandomMessage, RandomMessage);

            var exceptions = ThrowsException<NotSupportedException>(
                () => Guard.Support(false),
                message => Guard.Support(false, message));

            Assert.Contains(nameof(TestSupport), exceptions[0].Message);
        }

        [Fact(DisplayName = "State: Disposal")]
        public void TestDisposal()
        {
            var objectName = RandomMessage;

            Guard.Disposal(false);
            Guard.Disposal(false, objectName);
            Guard.Disposal(false, objectName, RandomMessage);

            var exceptions = ThrowsException<ObjectDisposedException>(
                () => Guard.Disposal(true),
                message => Guard.Disposal(true, message: message));

            Assert.Empty(exceptions[0].ObjectName);
            Assert.Empty(exceptions[1].ObjectName);

            exceptions = ThrowsException<ObjectDisposedException>(
                () => Guard.Disposal(true, objectName),
                message => Guard.Disposal(true, objectName, message));

            Assert.Same(objectName, exceptions[0].ObjectName);
            Assert.Same(objectName, exceptions[1].ObjectName);
        }

        private static TException[] ThrowsException<TException>(
            Action testWithoutMessage, Action<string> testWithMessage)
            where TException : Exception
        {
            var exWithoutMessage = Assert.Throws<TException>(() => testWithoutMessage());

            var message = RandomMessage;
            var exWithMessage = Assert.Throws<TException>(() => testWithMessage(message));
            Assert.StartsWith(message, exWithMessage.Message);

            return new[] { exWithoutMessage, exWithMessage };
        }
    }
}
