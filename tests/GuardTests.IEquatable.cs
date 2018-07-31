namespace Dawn.Tests
{
    using System;
    using System.Collections.Generic;
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

            // Equals.
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

            // Equals (custom equality comparer).
            var comparer = new TestEqualityComparer<string>();
            nullRefArg.Equal(a2, comparer);
            CheckAndResetComparer(false, false);

            aArg.Equal(a2, comparer);
            CheckAndResetComparer(true, false);

            Assert.Throws<ArgumentException>(
                nameof(a), () => Guard.Argument(() => a).Equal(b, comparer));

            CheckAndResetComparer(true, false);

            ex = Assert.Throws<ArgumentException>(
                nameof(a), () => Guard.Argument(() => a).Equal(b, comparer, (s1, s2) =>
                {
                    Assert.Equal(a, s1);
                    Assert.Equal(b, s2);
                    return message;
                }));

            CheckAndResetComparer(true, false);
            Assert.StartsWith(message, ex.Message);

            // Not equals.
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

            // Not equals (custom equality comparer).
            nullRefArg.NotEqual(b, comparer);
            CheckAndResetComparer(false, false);

            aArg.NotEqual(b, comparer);
            CheckAndResetComparer(true, false);

            Assert.Throws<ArgumentException>(
                nameof(a), () => Guard.Argument(() => a).NotEqual(a2, comparer));

            CheckAndResetComparer(true, false);

            ex = Assert.Throws<ArgumentException>(
                nameof(a), () => Guard.Argument(() => a).NotEqual(a2, comparer, s =>
                {
                    Assert.Equal(a, s);
                    return message;
                }));

            CheckAndResetComparer(true, false);
            Assert.StartsWith(message, ex.Message);

            void CheckAndResetComparer(bool equalsCalled, bool getHashCodeCalled)
            {
                Assert.Equal(comparer.EqualsCalled, equalsCalled);
                Assert.Equal(comparer.GetHashCodeCalled, getHashCodeCalled);

                comparer.Reset();
                Assert.False(comparer.EqualsCalled);
                Assert.False(comparer.GetHashCodeCalled);
            }
        }

        private sealed class TestEqualityComparer<T> : IEqualityComparer<T>
        {
            private static readonly IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

            public bool EqualsCalled { get; private set; }

            public bool GetHashCodeCalled { get; private set; }

            public bool Equals(T x, T y)
            {
                this.EqualsCalled = true;
                return comparer.Equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                this.GetHashCodeCalled = true;
                return comparer.GetHashCode(obj);
            }

            public void Reset()
            {
                this.EqualsCalled = false;
                this.GetHashCodeCalled = false;
            }
        }
    }
}
