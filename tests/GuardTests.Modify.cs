namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = T + "Guard supports modifications.")]
        public void GuardSupportsModifications()
        {
            // Modify with value.
            var untrimmed = " 1 ";
            var untrimmedArg = Guard.Argument(() => untrimmed);
            Assert.False(untrimmedArg.Modified);

            var trimmed = untrimmed.Trim();
            var trimmedArg = untrimmedArg.Modify(trimmed);
            Assert.Equal(untrimmedArg.Name, trimmedArg.Name);
            Assert.Equal(trimmed, trimmedArg.Value);
            Assert.True(trimmedArg.Modified);

            // Modify with converter.
            var integer = int.Parse(trimmed);
            var integerArg = trimmedArg.Modify(s => int.Parse(s));
            Assert.Equal(trimmedArg.Name, integerArg.Name);
            Assert.Equal(integer, integerArg.Value);
            Assert.True(integerArg.Modified);

            // Wrap.
            integerArg = trimmedArg.Wrap(s => int.Parse(s));
            Assert.Equal(trimmedArg.Name, integerArg.Name);
            Assert.Equal(integer, integerArg.Value);
            Assert.True(integerArg.Modified);

            var str = "A";
            Assert.Throws<FormatException>(
                () => Guard.Argument(() => str).Modify(s => int.Parse(s)));

            Assert.Throws<ArgumentException>(
                nameof(str), () => Guard.Argument(() => str).Wrap(s => int.Parse(s)));

            var message = RandomMessage;
            var ex = Assert.Throws<ArgumentException>(
                nameof(str), () => Guard.Argument(() => str).Wrap(s => int.Parse(s), s =>
                {
                    Assert.Same(str, s);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);
        }

#if !NETCOREAPP1_0
        [Fact(DisplayName = T + "Guard supports cloning.")]
        public void GuardSupportsCloning()
        {
            var cloneable = new TestCloneable();
            Assert.False(cloneable.IsCloned);

            var cloneableArg = Guard.Argument(() => cloneable);
            Assert.False(cloneableArg.Modified);

            var clonedArg = cloneableArg.Clone();
            Assert.Equal(cloneableArg.Name, clonedArg.Name);
            Assert.True(clonedArg.Value.IsCloned);
            Assert.Equal(cloneableArg.Modified, clonedArg.Modified);

            // Clone with modification.
            var modifiedCloneableArg = cloneableArg.Modify(c => new TestCloneable());
            Assert.True(modifiedCloneableArg.Modified);

            var modifedClonedArg = modifiedCloneableArg.Clone();
            Assert.Equal(modifiedCloneableArg.Name, modifedClonedArg.Name);
            Assert.True(modifedClonedArg.Value.IsCloned);
            Assert.Equal(modifiedCloneableArg.Modified, modifedClonedArg.Modified);
        }

        private sealed class TestCloneable : ICloneable
        {
            public TestCloneable()
            {
            }

            private TestCloneable(bool cloned)
            {
                this.IsCloned = true;
            }

            public bool IsCloned { get; }

            public object Clone() => new TestCloneable(true);
        }
#endif
    }
}
