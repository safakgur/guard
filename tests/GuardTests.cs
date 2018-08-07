namespace Dawn.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public sealed partial class GuardTests
    {
        /// <summary>A prefix for the test names identifying the current compilation target.</summary>
#if NETCOREAPP1_0
        const string T = "NS1 "; // Targeting .NET Standard 1.0.
#elif NETCOREAPP2_0
        const string T = "NS2 "; // Targeting .NET Standard 2.0.
#endif

        [Fact(DisplayName = T + "Guard supports compatibility preconditions.")]
        public void GuardSupportsCompatibilityPreconditions()
        {
            var message = RandomMessage;

            var @null = null as Stream;
            var nullArg = Guard.Argument(() => @null);
            using (var memory = new MemoryStream() as Stream)
            {
                // Compatible.
                nullArg
                    .Compatible<object>()
                    .Compatible<MemoryStream>();

                var memoryArg = Guard.Argument(() => memory);
                memoryArg
                    .Compatible<object>()
                    .Compatible<MemoryStream>();

                Assert.Throws<ArgumentException>(
                    nameof(memory), () => Guard.Argument(() => memory).Compatible<string>());

                var ex = Assert.Throws<ArgumentException>(
                    nameof(memory), () => Guard.Argument(() => memory).Compatible<string>(s =>
                    {
                        Assert.Same(memory, s);
                        return message;
                    }));

                Assert.StartsWith(message, ex.Message);

                // Not compatible.
                nullArg
                    .NotCompatible<object>()
                    .NotCompatible<MemoryStream>();

                memoryArg.NotCompatible<string>();

                Assert.Throws<ArgumentException>(
                    nameof(memory), () => Guard.Argument(() => memory).NotCompatible<MemoryStream>());

                ex = Assert.Throws<ArgumentException>(
                    nameof(memory), () => Guard.Argument(() => memory).NotCompatible<MemoryStream>(m =>
                    {
                        MemoryStream temp = m;
                        Assert.Same(memory, m);
                        return message;
                    }));

                Assert.StartsWith(message, ex.Message);

                Assert.Throws<ArgumentException>(
                    nameof(memory), () => Guard.Argument(() => memory).NotCompatible<object>());

                ex = Assert.Throws<ArgumentException>(
                    nameof(memory), () => Guard.Argument(() => memory).NotCompatible<object>(o =>
                    {
                        Assert.Same(memory, o);
                        return message;
                    }));

                Assert.StartsWith(message, ex.Message);

                // Cast.
                Assert.Throws<ArgumentException>(
                    nameof(@null), () => Guard.Argument(() => @null).Cast<object>());

                Assert.Throws<ArgumentException>(
                    nameof(@null), () => Guard.Argument(() => @null).Cast<MemoryStream>());

                object temp1 = memoryArg.Cast<object>();
                MemoryStream temp2 = memoryArg.Cast<MemoryStream>();

                Assert.Throws<ArgumentException>(
                    nameof(memory), () => Guard.Argument(() => memory).Cast<string>());

                ex = Assert.Throws<ArgumentException>(
                    nameof(memory), () => Guard.Argument(() => memory).Cast<string>(s =>
                    {
                        Assert.Same(memory, s);
                        return message;
                    }));

                Assert.StartsWith(message, ex.Message);
            }
        }
    }
}
