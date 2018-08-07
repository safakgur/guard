namespace Dawn.Tests
{
    using Xunit;

    public sealed class BooleanTests : BaseTests
    {
        [Theory(DisplayName = T + "Boolean: True/False")]
        [InlineData(null, null)]
        [InlineData(true, false)]
        public void GuardSupportsBooleans(bool? @true, bool? @false)
        {
            var nullableTrueArg = Guard.Argument(() => @true).True();
            var nullableFalseArg = Guard.Argument(() => @false).False();
            if (!@true.HasValue)
            {
                nullableTrueArg.False();
                nullableFalseArg.True();
                return;
            }

            ThrowsArgumentException(
                nullableFalseArg,
                arg => arg.True(),
                (arg, message) => arg.True(message));

            ThrowsArgumentException(
                nullableTrueArg,
                arg => arg.False(),
                (arg, message) => arg.False(message));

            var trueArg = Guard.Argument(@true.Value, nameof(@true)).True();
            var falseArg = Guard.Argument(@false.Value, nameof(@false)).False();
            ThrowsArgumentException(
                falseArg,
                arg => arg.True(),
                (arg, message) => arg.True(message));

            ThrowsArgumentException(
                trueArg,
                arg => arg.False(),
                (arg, message) => arg.False(message));
        }
    }
}
