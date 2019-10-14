namespace Dawn.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public abstract class BaseTests
    {
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        protected static int RandomNumber => RandomUtils.Current.Next();

        protected static bool RandomBoolean => RandomUtils.Current.NextDouble() >= .5;

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

        protected static void Test<T>(
            T? value,
            string name,
            Action<Guard.ArgumentInfo<T?>> nullableBody,
            Action<Guard.ArgumentInfo<T>> nonNullableBody,
            bool testModified = true,
            bool testSecure = true)
            where T : struct
        {
            Test(value, name, nullableBody, testModified, testSecure);
            if (value.HasValue)
                Test(value.Value, name, nonNullableBody, testModified, testSecure);
        }

        protected static void Test<T>(
            T value,
            string name,
            Action<Guard.ArgumentInfo<T>> body,
            bool testModified = true,
            bool testSecure = true)
        {
            body(new Guard.ArgumentInfo<T>(value, name, false, false));
            if (testModified)
                body(new Guard.ArgumentInfo<T>(value, name, true, false));

            if (testSecure)
                body(new Guard.ArgumentInfo<T>(value, name, false, true));

            if (testModified && testSecure)
                body(new Guard.ArgumentInfo<T>(value, name, true, true));
        }

        protected static ArgumentNullException[] ThrowsArgumentNullException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false,
            bool doNotTestScoping = false)
            => ThrowsArgumentNullException(argument, testWithoutMessage, null, testWithMessage, allowMessageMismatch, doNotTestScoping);

        protected static ArgumentNullException[] ThrowsArgumentNullException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Func<string, bool> testGeneratedMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false,
            bool doNotTestScoping = false)
            => ThrowsArgumentException<T, ArgumentNullException>(
                argument, testWithoutMessage, testGeneratedMessage, testWithMessage, allowMessageMismatch, doNotTestScoping);

        protected static ArgumentOutOfRangeException[] ThrowsArgumentOutOfRangeException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false,
            bool doNotTestScoping = false)
            => ThrowsArgumentOutOfRangeException(argument, testWithoutMessage, null, testWithMessage, allowMessageMismatch, doNotTestScoping);

        protected static ArgumentOutOfRangeException[] ThrowsArgumentOutOfRangeException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Func<string, bool> testGeneratedMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false,
            bool doNotTestScoping = false)
            => ThrowsArgumentException<T, ArgumentOutOfRangeException>(
                argument, testWithoutMessage, testGeneratedMessage, testWithMessage, allowMessageMismatch, doNotTestScoping);

        protected static ArgumentException[] ThrowsArgumentException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false,
            bool doNotTestScoping = false)
            => ThrowsArgumentException(argument, testWithoutMessage, null, testWithMessage, allowMessageMismatch, doNotTestScoping);

        protected static ArgumentException[] ThrowsArgumentException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Func<string, bool> testGeneratedMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage,
            bool allowMessageMismatch = false,
            bool doNotTestScoping = false)
            => ThrowsArgumentException<T, ArgumentException>(
                argument, testWithoutMessage, testGeneratedMessage, testWithMessage, allowMessageMismatch, doNotTestScoping);

        protected static TException[] ThrowsArgumentException<TArgument, TException>(
            Guard.ArgumentInfo<TArgument> argument,
            Action<Guard.ArgumentInfo<TArgument>> testWithoutMessage,
            Func<string, bool> testGeneratedMessage,
            Action<Guard.ArgumentInfo<TArgument>, string> testWithMessage,
            bool allowMessageMismatch = false,
            bool doNotTestScoping = false)
            where TException : ArgumentException
        {
            using var scope = new Scope(doNotTestScoping);
            var isBase = typeof(TException) == typeof(ArgumentException);
            if (!isBase && argument.Modified)
            {
                var result = ThrowsArgumentException<TArgument, ArgumentException>(
                    argument, testWithoutMessage, testGeneratedMessage, testWithMessage, allowMessageMismatch);

                return new[] { result[0] as TException, result[1] as TException };
            }

            var exWithoutMessage = Assert.Throws<TException>(
                argument.Name,
                () => testWithoutMessage(argument));

            scope.CheckException(exWithoutMessage);

            if (exWithoutMessage is ArgumentOutOfRangeException rangeExWithoutMessage)
                Assert.Equal(argument.Secure, rangeExWithoutMessage.ActualValue == null);

            if (testGeneratedMessage != null)
                Assert.True(testGeneratedMessage(exWithoutMessage.Message));

            var message = RandomMessage;
            var exWithMessage = Assert.Throws<TException>(
                argument.Name,
                () => testWithMessage(argument, message));

            scope.CheckException(exWithMessage);

            if (exWithMessage is ArgumentOutOfRangeException rangeExWithMessage)
                Assert.Equal(argument.Secure, rangeExWithMessage.ActualValue == null);

            if (!allowMessageMismatch)
                Assert.StartsWith(message, exWithMessage.Message);

            if (!isBase)
            {
                var modified = argument.Modify(argument.Value);
                ThrowsArgumentException(
                    modified, testWithoutMessage, testGeneratedMessage, testWithMessage, allowMessageMismatch);
            }

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

        private sealed class Scope : IDisposable
        {
            private readonly IDisposable scope;

#pragma warning disable IDE0044
            private Exception lastException;
#pragma warning restore IDE0044

            public Scope(bool doNotTestScoping = false)
            {
                if (RandomBoolean && !doNotTestScoping)
                    this.scope = Guard.BeginScope((ex, stackTrace) => this.lastException = ex);
            }

            public void CheckException(Exception exception)
            {
                if (this.scope != null && exception != null)
                    Assert.Same(this.lastException, exception);
            }

            public void Dispose() => this.scope?.Dispose();
        }
    }
}
