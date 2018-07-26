namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = "Guard supports strings.")]
        public void GuardSupportsStrings()
        {
            var message = RandomMessage;

            // Null allows all.
            string s = null;
            Guard.Argument(() => s)
                .Empty()
                .NotEmpty()
                .WhiteSpace()
                .NotWhiteSpace()
                .MinLength(RandomNumber)
                .MaxLength(RandomNumber)
                .LengthInRange(RandomNumber, RandomNumber)
                .Equal("s", RandomStringComparison)
                .NotEqual("s", RandomStringComparison);

            // Empty string.
            s = string.Empty;
            Guard.Argument(() => s).Empty().WhiteSpace();
            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).NotEmpty());

            var ex = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).NotEmpty(message));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).NotWhiteSpace());

            ex = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).NotWhiteSpace(a => a));

            Assert.StartsWith(Environment.NewLine, ex.Message);

            TestEqual();
            TestLength();

            // White-space.
            s = " ";
            Guard.Argument(() => s).NotEmpty().WhiteSpace();
            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Empty());

            ex = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Empty(a => a));

            Assert.StartsWith(s, ex.Message);

            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).NotWhiteSpace());

            ex = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).NotWhiteSpace(a => a));

            Assert.StartsWith(s, ex.Message);

            TestEqual();
            TestLength();

            // Non white-space.
            s = "s";
            Guard.Argument(() => s).NotEmpty().NotWhiteSpace();
            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Empty());

            ex = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).Empty(a => a));

            Assert.StartsWith(s, ex.Message);

            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).WhiteSpace());

            ex = Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).WhiteSpace(a => a));

            Assert.StartsWith(s, ex.Message);

            TestEqual();
            TestLength();

            void TestEqual()
            {
                const string test = "ss";

                Guard.Argument(() => s)
                    .Equal(s, RandomStringComparison)
                    .NotEqual(test, RandomStringComparison);

                Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).Equal(test, RandomStringComparison));

                ex = Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).Equal(test, RandomStringComparison, (a, b) => a + b));

                Assert.StartsWith(s + test, ex.Message);

                Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).NotEqual(s, RandomStringComparison));

                ex = Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).NotEqual(s, RandomStringComparison, a => a));

                Assert.StartsWith(s, ex.Message);
            }

            void TestLength()
            {
                Guard.Argument(() => s)
                    .MinLength(s.Length)
                    .MinLength(s.Length - 1)
                    .MaxLength(s.Length)
                    .MaxLength(s.Length + 1)
                    .LengthInRange(s.Length, s.Length)
                    .LengthInRange(s.Length - 1, s.Length + 1);

                var min = s.Length + 1;
                Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).MinLength(min));

                ex = Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).MinLength(min, (a, amin) => a + amin));

                Assert.StartsWith(s + min, ex.Message);

                var max = s.Length - 1;
                Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).MaxLength(max));

                ex = Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).MaxLength(max, (a, amax) => a + amax));

                Assert.StartsWith(s + max, ex.Message);

                max = s.Length + 2;
                Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).LengthInRange(min, max));

                ex = Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).LengthInRange(min, max, (a, amin, amax) => a + amin + amax));

                Assert.StartsWith(s + min + max, ex.Message);

                min = s.Length - 2;
                max = s.Length - 1;
                Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).LengthInRange(min, max));

                ex = Assert.Throws<ArgumentException>(nameof(s), ()
                    => Guard.Argument(() => s).LengthInRange(min, max, (a, amin, amax) => a + amin + amax));

                Assert.StartsWith(s + min + max, ex.Message);
            }
        }
    }
}
