namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed class ModifyTests : BaseTests
    {
        [Fact(DisplayName = T + "Modify: Modify using value")]
        public void ModifyUsingValue()
        {
            var untrimmed = " 1 ";
            var untrimmedArg = Guard.Argument(() => untrimmed);
            Assert.False(untrimmedArg.Modified);

            var trimmed = untrimmed.Trim();
            var trimmedArg = untrimmedArg.Modify(trimmed);
            Assert.Equal(untrimmedArg.Name, trimmedArg.Name);
            Assert.Equal(trimmed, trimmedArg.Value);
        }

        [Fact(DisplayName = T + "Modify: Modify using converter")]
        public void ModifyUsingConverter()
        {
            var stringValue = 1.ToString();
            var stringArg = Guard.Argument(() => stringValue);
            var integerArg = stringArg.Modify(s => int.Parse(s));

            Assert.Equal(stringArg.Name, integerArg.Name);
            Assert.Equal(1, integerArg.Value);
            Assert.True(integerArg.Modified);

            var exception = new Exception(RandomMessage);
            Assert.Same(exception, Assert.Throws<Exception>(()
                => stringArg.Modify<string, int>(s => { throw exception; })));
        }

        [Fact(DisplayName = T + "Modify: Wrap factory")]
        public void Wrap()
        {
            var stringValue = 1.ToString();
            var stringArg = Guard.Argument(() => stringValue);
            var integerArg = stringArg.Wrap(s => int.Parse(s));

            Assert.Equal(stringArg.Name, integerArg.Name);
            Assert.Equal(1, integerArg.Value);
            Assert.True(integerArg.Modified);

            var exception = new Exception(RandomMessage);
            Assert.DoesNotContain(exception, ThrowsArgumentException(
                stringArg,
                arg => arg.Wrap<string, int>(s => { throw exception; }),
                (arg, message) => arg.Wrap<string, int>(s => { throw exception; }, s =>
                {
                    Assert.Same(stringArg.Value, s);
                    return message;
                })));
        }

#if !NETCOREAPP1_0
        [Fact(DisplayName = T + "Modify: Clone")]
        public void GuardSupportsCloning()
        {
            var cloneable = new TestCloneable();
            Assert.False(cloneable.IsClone);

            var cloneableArg = Guard.Argument(() => cloneable);
            Assert.False(cloneableArg.Modified);

            var cloneArg = cloneableArg.Clone();
            Assert.Equal(cloneableArg.Name, cloneArg.Name);
            Assert.True(cloneArg.Value.IsClone);
            Assert.Equal(cloneableArg.Modified, cloneArg.Modified);

            var modifiedCloneableArg = cloneableArg.Modify(c => new TestCloneable());
            Assert.True(modifiedCloneableArg.Modified);

            var modifiedCloneArg = modifiedCloneableArg.Clone();
            Assert.Equal(modifiedCloneableArg.Name, modifiedCloneArg.Name);
            Assert.True(modifiedCloneArg.Value.IsClone);
            Assert.Equal(modifiedCloneableArg.Modified, modifiedCloneArg.Modified);
        }

        private sealed class TestCloneable : ICloneable
        {
            public TestCloneable()
            {
            }

            private TestCloneable(bool isClone) => this.IsClone = true;

            public bool IsClone { get; }

            public object Clone() => new TestCloneable(true);
        }
#endif
    }
}
