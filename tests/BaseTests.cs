namespace Dawn.Tests
{
    using System;
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

        protected static ArgumentNullException[] ThrowsArgumentNullException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage)
        {
            var exWithoutMessage = Assert.Throws<ArgumentNullException>(
                argument.Name,
                () => testWithoutMessage(argument));

            var message = RandomMessage;
            var exWithMessage = Assert.Throws<ArgumentNullException>(
                argument.Name,
                () => testWithMessage(argument, message));

            Assert.StartsWith(message, exWithMessage.Message);

            var modified = argument.Modify(argument.Value);
            ThrowsArgumentException(modified, testWithoutMessage, testWithMessage);

            return new[] { exWithoutMessage, exWithMessage };
        }

        protected static ArgumentOutOfRangeException[] ThrowsArgumentOutOfRangeException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage)
        {
            var exWithoutMessage = Assert.Throws<ArgumentOutOfRangeException>(
                argument.Name,
                () => testWithoutMessage(argument));

            var message = RandomMessage;
            var exWithMessage = Assert.Throws<ArgumentOutOfRangeException>(
                argument.Name,
                () => testWithMessage(argument, message));

            Assert.StartsWith(message, exWithMessage.Message);

            var modified = argument.Modify(argument.Value);
            ThrowsArgumentException(modified, testWithoutMessage, testWithMessage);

            return new[] { exWithoutMessage, exWithMessage };
        }

        protected static ArgumentException[] ThrowsArgumentException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage)
        {
            var exWithoutMessage = Assert.Throws<ArgumentException>(
                argument.Name,
                () => testWithoutMessage(argument));

            var message = RandomMessage;
            var exWithMessage = Assert.Throws<ArgumentException>(
                argument.Name,
                () => testWithMessage(argument, message));

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

        protected static class RandomUtils
        {
            private static readonly Random seeder = new Random();

            [ThreadStatic]
            private static Random current;

            public static Random Current
            {
                get
                {
                    if (current == null)
                    {
                        int seed;
                        lock (seeder)
                            seed = seeder.Next();

                        current = new Random(seed);
                    }

                    return current;
                }
            }
        }
    }
}
