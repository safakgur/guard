namespace Dawn.Tests
{
    using System;
    using System.Linq;
    using Xunit;

    public abstract class BaseTests
    {
#if NETCOREAPP1_0
        protected const string T = "NS1: "; // Targeting .NET Standard 1.0.
#elif NETCOREAPP2_0
        protected const string T = "NS2: "; // Targeting .NET Standard 2.0.
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

        protected static void ThrowsArgumentException<T>(
            Guard.ArgumentInfo<T> argument,
            Action<Guard.ArgumentInfo<T>> testWithoutMessage,
            Action<Guard.ArgumentInfo<T>, string> testWithMessage)
        {
            Assert.Throws<ArgumentException>(argument.Name, () => testWithoutMessage(argument));

            var message = RandomMessage;
            var ex = Assert.Throws<ArgumentException>(argument.Name, () => testWithMessage(argument, message));
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
