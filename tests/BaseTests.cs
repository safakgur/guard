namespace Dawn.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public abstract class BaseTests
    {
#if NETCOREAPP1_0
        protected const string T = "NS1 "; // Targeting .NET Standard 1.0.
#elif NETCOREAPP2_0
        protected const string T = "NS2 "; // Targeting .NET Standard 2.0.
#endif

        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        protected static int RandomNumber => RandomUtils.Current.Next();

        protected static string RandomMessage
        {
            get
            {
                return string.Join(string.Empty, Enumerable
                    .Range(0, RandomUtils.Current.Next(5, 21))
                    .Select(i => Alphabet[RandomUtils.Current.Next(Alphabet.Length)]));
            }
        }

        protected interface ITestEnumerable<T> : IEnumerable<T>
        {
            IEnumerable<T> Items { get; }

            bool Enumerated { get; }

            int EnumerationCount { get; }

            void Reset();
        }

        protected interface ITestEnumerableWithCount<T> : ITestEnumerable<T>, IReadOnlyCollection<T>
        {
            bool CountCalled { get; }
        }

        protected interface ITestEnumerableWithContains<T> : ITestEnumerable<T>
        {
            bool Contains(T item);

            bool ContainsCalled { get; }
        }

        protected static ArgumentNullException[] ThrowsArgumentNullException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false)
            => ThrowsArgumentNullException(argument, testWithoutMessage, null, testWithMessage, allowMessageMismatch);

        protected static ArgumentNullException[] ThrowsArgumentNullException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Func<string, bool> testGeneratedMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false)
        {
            var exWithoutMessage = Assert.Throws<ArgumentNullException>(
                argument.Name,
                () => testWithoutMessage(argument));

            if (testGeneratedMessage != null)
                Assert.True(testGeneratedMessage(exWithoutMessage.Message));

            var message = RandomMessage;
            var exWithMessage = Assert.Throws<ArgumentNullException>(
                argument.Name,
                () => testWithMessage(argument, message));

            if (!allowMessageMismatch)
                Assert.StartsWith(message, exWithMessage.Message);

            var modified = argument.Modify(argument.Value);
            ThrowsArgumentException(
                modified, testWithoutMessage, testGeneratedMessage, testWithMessage, allowMessageMismatch);

            return new[] { exWithoutMessage, exWithMessage };
        }

        protected static ArgumentOutOfRangeException[] ThrowsArgumentOutOfRangeException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false)
            => ThrowsArgumentOutOfRangeException(argument, testWithoutMessage, null, testWithMessage, allowMessageMismatch);

        protected static ArgumentOutOfRangeException[] ThrowsArgumentOutOfRangeException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Func<string, bool> testGeneratedMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false)
        {
            var exWithoutMessage = Assert.Throws<ArgumentOutOfRangeException>(
                argument.Name,
                () => testWithoutMessage(argument));

            Assert.Equal(argument.Secure, exWithoutMessage.ActualValue == null);

            if (testGeneratedMessage != null)
                Assert.True(testGeneratedMessage(exWithoutMessage.Message));

            var message = RandomMessage;
            var exWithMessage = Assert.Throws<ArgumentOutOfRangeException>(
                argument.Name,
                () => testWithMessage(argument, message));

            Assert.Equal(argument.Secure, exWithMessage.ActualValue == null);

            if (!allowMessageMismatch)
                Assert.StartsWith(message, exWithMessage.Message);

            var modified = argument.Modify(argument.Value);
            ThrowsArgumentException(
                modified, testWithoutMessage, testGeneratedMessage, testWithMessage, allowMessageMismatch);

            return new[] { exWithoutMessage, exWithMessage };
        }

        protected static ArgumentException[] ThrowsArgumentException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false)
            => ThrowsArgumentException(argument, testWithoutMessage, null, testWithMessage, allowMessageMismatch);

        protected static ArgumentException[] ThrowsArgumentException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Func<string, bool> testGeneratedMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false)
        {
            var exWithoutMessage = Assert.Throws<ArgumentException>(
                argument.Name,
                () => testWithoutMessage(argument));

            if (testGeneratedMessage != null)
                Assert.True(testGeneratedMessage(exWithoutMessage.Message));

            var message = RandomMessage;
            var exWithMessage = Assert.Throws<ArgumentException>(
                argument.Name,
                () => testWithMessage(argument, message));

            if (!allowMessageMismatch)
                Assert.StartsWith(message, exWithMessage.Message);

            return new[] { exWithoutMessage, exWithMessage };
        }

        protected static void ThrowsException<T, TException>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false)
            where TException : Exception
        {
            var ex = Assert.Throws<TException>(() => testWithoutMessage(argument));
            if (ex is ArgumentException argEx)
                Assert.Same(argument.Name, argEx.ParamName);

            var message = RandomMessage;
            ex = Assert.Throws<TException>(() => testWithMessage(argument, message));
            argEx = ex as ArgumentException;
            if (argEx != null)
                Assert.Same(argument.Name, argEx.ParamName);

            if (!allowMessageMismatch)
                Assert.StartsWith(message, ex.Message);
        }

        protected static void CheckAndReset<T>(
            ITestEnumerable<T> enumerable,
            bool? countCalled = null,
            bool? containsCalled = null,
            int? enumerationCount = null,
            bool? enumerated = null,
            bool? forceEnumerated = null)
        {
            if (enumerable is null)
                return;

            var withCount = enumerable as ITestEnumerableWithCount<T>;
            if (withCount != null && countCalled.HasValue)
            {
                Assert.Equal(countCalled, withCount.CountCalled);
                Assert.Equal(forceEnumerated ?? !countCalled, enumerable.Enumerated);

                if (countCalled.Value && forceEnumerated != true)
                    Assert.Equal(0, enumerable.EnumerationCount);
            }

            var withContains = enumerable as ITestEnumerableWithContains<T>;
            if (withContains != null && containsCalled.HasValue)
            {
                Assert.Equal(containsCalled, withContains.ContainsCalled);
                Assert.Equal(forceEnumerated ?? !containsCalled, enumerable.Enumerated);

                if (containsCalled.Value && forceEnumerated != true)
                    Assert.Equal(0, enumerable.EnumerationCount);
            }

            if (withCount is null && withContains is null)
            {
                enumerated = forceEnumerated ?? enumerated;

                if (!enumerated.HasValue && enumerationCount.HasValue)
                    enumerated = enumerationCount > 0;

                if (enumerated.HasValue)
                    Assert.Equal(enumerated, enumerable.Enumerated);

                if (enumerationCount.HasValue)
                    Assert.Equal(enumerationCount, enumerable.EnumerationCount);
            }

            enumerable.Reset();
            Assert.False(enumerable.Enumerated);
            Assert.Equal(0, enumerable.EnumerationCount);

            if (withCount != null)
                Assert.False(withCount.CountCalled);

            if (withContains != null)
                Assert.False(withContains.ContainsCalled);
        }

        protected static class RandomUtils
        {
            private static readonly Random Seeder = new Random();

            [ThreadStatic]
            private static Random current;

            public static Random Current
            {
                get
                {
                    if (current == null)
                    {
                        int seed;
                        lock (Seeder)
                            seed = Seeder.Next();

                        current = new Random(seed);
                    }

                    return current;
                }
            }
        }
    }
}
