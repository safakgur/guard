namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = T + "Guard supports null preconditions.")]
        public void GuardSupportsNullPreconditions()
        {
            var s = null as string;
            var message = RandomMessage;

            Guard.Argument(() => s).Null();
            Assert.Throws<ArgumentNullException>(nameof(s), ()
                => Guard.Argument(() => s).NotNull());

            var nullEx = Assert.Throws<ArgumentNullException>(nameof(s), ()
                => Guard.Argument(() => s).NotNull(message));

            Assert.StartsWith(message, nullEx.Message);

            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Modify(s).NotNull());

            var argEx = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Modify(s).NotNull(message));

            Assert.StartsWith(message, argEx.Message);

            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Modify(a => a).NotNull());

            argEx = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Modify(a => a).NotNull(message));

            Assert.StartsWith(message, argEx.Message);

            s = "s";
            Guard.Argument(() => s).NotNull();
            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Null());

            argEx = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Null(v => v));

            Assert.StartsWith(s, argEx.Message);

            int? i = null;
            Guard.Argument(() => i).Null();
            Assert.Throws<ArgumentNullException>(nameof(i), ()
                => Guard.Argument(() => i).NotNull());

            nullEx = Assert.Throws<ArgumentNullException>(nameof(i), ()
                => Guard.Argument(() => i).NotNull(message));

            Assert.StartsWith(message, nullEx.Message);

            Assert.Throws<ArgumentException>(nameof(i), ()
                => Guard.Argument(() => i).Modify(i).NotNull());

            argEx = Assert.Throws<ArgumentException>(nameof(i), ()
                 => Guard.Argument(() => i).Modify(i).NotNull(message));

            Assert.StartsWith(message, argEx.Message);

            Assert.Throws<ArgumentException>(nameof(i), ()
                => Guard.Argument(() => i).Modify(a => a).NotNull());

            argEx = Assert.Throws<ArgumentException>(nameof(i), ()
                => Guard.Argument(() => i).Modify(a => a).NotNull(message));

            Assert.StartsWith(message, argEx.Message);

            i = 0;
            Assert.IsType<int>(Guard.Argument(() => i).NotNull().Value);
            Assert.Throws<ArgumentException>(nameof(i), ()
                => Guard.Argument(() => i).Null());

            argEx = Assert.Throws<ArgumentException>(nameof(i), ()
                => Guard.Argument(() => i).Null(a => a.ToString()));

            Assert.StartsWith(i.ToString(), argEx.Message);
        }
    }
}
