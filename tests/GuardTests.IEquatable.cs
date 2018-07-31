namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = T + "Guard supports equality preconditions.")]
        public void GuardSupportsEquatables()
        {
            var message = RandomMessage;

            // Default.
            var zero = 0;
            var zeroArg = Guard.Argument(() => zero);
            zeroArg.Default();

            var one = 1;
            Assert.Throws<ArgumentException>(
                nameof(one), () => Guard.Argument(() => one).Default());

            var ex = Assert.Throws<ArgumentException>(
                nameof(one), () => Guard.Argument(() => one).Default(i =>
                {
                    Assert.Equal(one, i);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Not default.
            var oneArg = Guard.Argument(() => one);
            oneArg.NotDefault();

            Assert.Throws<ArgumentException>(
                nameof(zero), () => Guard.Argument(() => zero).NotDefault());

            ex = Assert.Throws<ArgumentException>(
                nameof(zero), () => Guard.Argument(() => zero).NotDefault(i =>
                {
                    Assert.Equal(zero, i);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Class equals.
            var nullRef = null as string;
            var nullRefArg = Guard.Argument(() => nullRef);

            var a = "A";
            var aArg = Guard.Argument(() => a);

            var a2 = 'A'.ToString();
            nullRefArg.Equal(a2);
            aArg.Equal(a2);

            var b = "B";
            Assert.Throws<ArgumentException>(
                nameof(a), () => Guard.Argument(() => a).Equal(b));

            ex = Assert.Throws<ArgumentException>(
                nameof(a), () => Guard.Argument(() => a).Equal(b, (s1, s2) =>
                {
                    Assert.Equal(a, s1);
                    Assert.Equal(b, s2);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Class not equals.
            nullRefArg.NotEqual(b);
            aArg.NotEqual(b);

            Assert.Throws<ArgumentException>(
                nameof(a), () => Guard.Argument(() => a).NotEqual(a2));

            ex = Assert.Throws<ArgumentException>(
                nameof(a), () => Guard.Argument(() => a).NotEqual(a2, s =>
                {
                    Assert.Equal(a, s);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);
        }
    }
}
