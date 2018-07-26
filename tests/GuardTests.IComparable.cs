namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = "Guard supports comparing preconditions.")]
        public void GuardSupportsComparables()
        {
            var message = RandomMessage;

            // Class constants.
            var refNull = null as string;
            var refNullArg = Guard.Argument(() => refNull);

            var refMin2 = "A";
            var refMin1 = "B";
            var ref0 = "C";
            var ref1 = "D";
            var ref2 = "E";
            var ref0Arg = Guard.Argument(() => ref0);

            // Struct constants.
            var valNull = default(int?);
            var valNullArg = Guard.Argument(() => valNull);

            var valMin2 = -2;
            var valMin1 = -1;
            var val0 = 0;
            var val1 = 1;
            var val2 = 2;
            var valMin1Arg = Guard.Argument(() => valMin1);
            var val0Arg = Guard.Argument(() => val0);
            var val1Arg = Guard.Argument(() => val1);

            var nilMin2 = valMin2 as int?;
            var nilMin1 = valMin1 as int?;
            var nil0 = val0 as int?;
            var nil1 = val1 as int?;
            var nil2 = val2 as int?;
            var nilMin1Arg = Guard.Argument(() => nilMin1);
            var nil0Arg = Guard.Argument(() => nil0);
            var nil1Arg = Guard.Argument(() => nil1);

            // Min (class).
            refNullArg.Min(refMin1).Min(ref0).Min(ref1);

            val0Arg.Min(val0).Min(valMin1);
            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).Min(val1));

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).Min(val1, (a, b) =>
                {
                    Assert.Equal(val0, a);
                    Assert.Equal(val1, b);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Min (struct).
            valNullArg.Min(valMin1).Min(val0).Min(val1);

            ref0Arg.Min(ref0).Min(refMin1);
            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(ref0), () => Guard.Argument(() => ref0).Min(ref1));

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(ref0), () => Guard.Argument(() => ref0).Min(ref1, (a, b) =>
                {
                    Assert.Equal(ref0, a);
                    Assert.Equal(ref1, b);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Max (class).
            refNullArg.Max(refMin1).Max(ref0).Max(ref1);

            ref0Arg.Max(ref0).Max(ref1);
            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(ref0), () => Guard.Argument(() => ref0).Max(refMin1));

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(ref0), () => Guard.Argument(() => ref0).Max(refMin1, (a, b) =>
                {
                    Assert.Equal(ref0, a);
                    Assert.Equal(refMin1, b);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Max (struct).
            valNullArg.Max(valMin1).Max(val0).Max(val1);

            val0Arg.Max(val0).Max(val1);
            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).Max(valMin1));

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).Max(valMin1, (a, b) =>
                {
                    Assert.Equal(val0, a);
                    Assert.Equal(valMin1, b);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // In range (class).
            refNullArg
                .InRange(refMin1, ref0)
                .InRange(ref0, ref0)
                .InRange(ref0, ref1)
                .InRange(refMin1, ref1);

            ref0Arg
                .InRange(refMin1, ref0)
                .InRange(ref0, ref0)
                .InRange(ref0, ref1)
                .InRange(refMin1, ref1);

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(ref0), () => Guard.Argument(() => ref0).InRange(refMin2, refMin1));

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(ref0), () => Guard.Argument(() => ref0).InRange(refMin2, refMin1, (a, min, max) =>
                {
                    Assert.Equal(ref0, a);
                    Assert.Equal(refMin2, min);
                    Assert.Equal(refMin1, max);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentOutOfRangeException>(
               nameof(ref0), () => Guard.Argument(() => ref0).InRange(ref1, ref2));

            ex = Assert.Throws<ArgumentOutOfRangeException>(
               nameof(ref0), () => Guard.Argument(() => ref0).InRange(ref1, ref2, (a, min, max) =>
               {
                   Assert.Equal(ref0, a);
                   Assert.Equal(ref1, min);
                   Assert.Equal(ref2, max);
                   return message;
               }));

            Assert.StartsWith(message, ex.Message);

            // In range (struct).
            valNullArg
                .InRange(valMin1, val0)
                .InRange(val0, val0)
                .InRange(val0, val1)
                .InRange(valMin1, val1);

            val0Arg
                .InRange(valMin1, val0)
                .InRange(val0, val0)
                .InRange(val0, val1)
                .InRange(valMin1, val1);

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).InRange(valMin2, valMin1));

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).InRange(valMin2, valMin1, (a, min, max) =>
                {
                    Assert.Equal(val0, a);
                    Assert.Equal(valMin2, min);
                    Assert.Equal(valMin1, max);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentOutOfRangeException>(
               nameof(val0), () => Guard.Argument(() => val0).InRange(val1, val2));

            ex = Assert.Throws<ArgumentOutOfRangeException>(
               nameof(val0), () => Guard.Argument(() => val0).InRange(val1, val2, (a, min, max) =>
               {
                   Assert.Equal(val0, a);
                   Assert.Equal(val1, min);
                   Assert.Equal(val2, max);
                   return message;
               }));

            Assert.StartsWith(message, ex.Message);

            // Zero (struct).
            valNullArg.Zero();
            val0Arg.Zero();

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val1), () => Guard.Argument(() => val1).Zero());

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val1), () => Guard.Argument(() => val1).Zero(a =>
                {
                    Assert.Equal(val1, a);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Zero (nullable struct).
            nil0Arg.Zero();

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(nil1), () => Guard.Argument(() => nil1).Zero());

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(nil1), () => Guard.Argument(() => nil1).Zero(a =>
                {
                    Assert.Equal(nil1, a);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Not zero (struct).
            valNullArg.NotZero();
            val1Arg.NotZero();

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).NotZero());

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).NotZero(a =>
                {
                    Assert.Equal(val0, a);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Zero (nullable struct).
            nil1Arg.NotZero();

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(nil0), () => Guard.Argument(() => nil0).NotZero());

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(nil0), () => Guard.Argument(() => nil0).NotZero(a =>
                {
                    Assert.Equal(nil0, a);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Positive (struct).
            valNullArg.Positive();
            val1Arg.Positive();

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).Positive());

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).Positive(a =>
                {
                    Assert.Equal(val0, a);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Positive (nullable struct).
            nil1Arg.Positive();

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(nil0), () => Guard.Argument(() => nil0).Positive());

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(nil0), () => Guard.Argument(() => nil0).Positive(a =>
                {
                    Assert.Equal(nil0, a);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Negative (struct).
            valNullArg.Negative();
            valMin1Arg.Negative();

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).Negative());

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(val0), () => Guard.Argument(() => val0).Negative(a =>
                {
                    Assert.Equal(val0, a);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Negative (nullable struct).
            nilMin1Arg.Negative();

            Assert.Throws<ArgumentOutOfRangeException>(
                nameof(nil0), () => Guard.Argument(() => nil0).Negative());

            ex = Assert.Throws<ArgumentOutOfRangeException>(
                nameof(nil0), () => Guard.Argument(() => nil0).Negative(a =>
                {
                    Assert.Equal(nil0, a);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);
        }
    }
}
