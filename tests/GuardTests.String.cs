namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = T + "Guard supports strings.")]
        public void GuardSupportsStrings()
        {
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
                .NotEqual("s", RandomStringComparison)
                .StartsWith("s")
                .StartsWith("s", RandomStringComparison)
                .DoesNotStartWith("s")
                .DoesNotStartWith("s", RandomStringComparison)
                .EndsWith("s")
                .EndsWith("s", RandomStringComparison)
                .DoesNotEndWith("s")
                .DoesNotEndWith("s", RandomStringComparison);

            // Empty string.
            s = string.Empty;
            Guard.Argument(() => s).Empty().WhiteSpace();
            Assert.Throws<ArgumentException>(nameof(s), ()
                => Guard.Argument(() => s).NotEmpty());

            var message = RandomMessage;
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

            // Starts with
            s = "AB";
            Guard.Argument(() => s)
                .StartsWith("A")
                .StartsWith("A", RandomStringComparison)
                .StartsWith("AB")
                .StartsWith("AB", RandomStringComparison);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).StartsWith("B"));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).StartsWith("B", (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("B", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).StartsWith("B", RandomStringComparison));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).StartsWith("B", RandomStringComparison, (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("B", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Does not start with
            Guard.Argument(() => s)
                .DoesNotStartWith("B")
                .DoesNotStartWith("B", RandomStringComparison);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotStartWith("A"));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotStartWith("A", (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("A", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotStartWith("A", RandomStringComparison));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotStartWith("A", RandomStringComparison, (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("A", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotStartWith("AB"));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotStartWith("AB", (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("AB", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotStartWith("AB", RandomStringComparison));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotStartWith("AB", RandomStringComparison, (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("AB", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Ends with
            Guard.Argument(() => s)
                .EndsWith("B")
                .EndsWith("B", RandomStringComparison)
                .EndsWith("AB")
                .EndsWith("AB", RandomStringComparison);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).EndsWith("A"));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).EndsWith("A", (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("A", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Does not end with
            Guard.Argument(() => s).DoesNotEndWith("A");
            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotEndWith("B"));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotEndWith("B", (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("B", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotEndWith("B", RandomStringComparison));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotEndWith("B", RandomStringComparison, (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("B", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotEndWith("AB"));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotEndWith("AB", (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("AB", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotEndWith("AB", RandomStringComparison));

            message = RandomMessage;
            ex = Assert.Throws<ArgumentException>(
                nameof(s), () => Guard.Argument(() => s).DoesNotEndWith("AB", RandomStringComparison, (a, value) =>
                {
                    Assert.Equal(s, a);
                    Assert.Equal("AB", value);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

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
