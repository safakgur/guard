namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = T + "Guard supports booleans.")]
        public void GuardSupportsBooleans()
        {
            var message = RandomMessage;

            // Non-nullable.
            var @true = true;
            var @false = false;

            Guard.Argument(() => @true).True();
            Assert.Throws<ArgumentException>(nameof(@true), ()
                => Guard.Argument(() => @true).False());

            var ex = Assert.Throws<ArgumentException>(nameof(@true), ()
                => Guard.Argument(() => @true).False(message));

            Assert.StartsWith(message, ex.Message);

            Guard.Argument(() => @false).False();
            Assert.Throws<ArgumentException>(nameof(@false), ()
                => Guard.Argument(() => @false).True());

            ex = Assert.Throws<ArgumentException>(nameof(@false), ()
                => Guard.Argument(() => @false).True(message));

            Assert.StartsWith(message, ex.Message);

            // Null allows all.
            bool? ntrue = null;
            bool? nfalse = null;

            Guard.Argument(() => ntrue).True().False();
            Guard.Argument(() => nfalse).True().False();

            // Nullable with value.
            ntrue = true;
            nfalse = false;

            Guard.Argument(() => ntrue).True();
            Assert.Throws<ArgumentException>(nameof(ntrue), ()
                => Guard.Argument(() => ntrue).False());

            ex = Assert.Throws<ArgumentException>(nameof(ntrue), ()
                => Guard.Argument(() => ntrue).False(message));

            Assert.StartsWith(message, ex.Message);

            Guard.Argument(() => nfalse).False();
            Assert.Throws<ArgumentException>(nameof(nfalse), ()
                => Guard.Argument(() => nfalse).True());

            ex = Assert.Throws<ArgumentException>(nameof(nfalse), ()
                => Guard.Argument(() => nfalse).True(message));

            Assert.StartsWith(message, ex.Message);
        }
    }
}
