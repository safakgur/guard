namespace Dawn.Tests
{
    using System;
    using System.Linq;

    public sealed partial class GuardTests
    {
        #region Fields

        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        #endregion Fields

        #region Properties

        private static int RandomNumber => RandomUtils.Current.Next();

        private static StringComparison RandomStringComparison
            => (StringComparison)RandomUtils.Current.Next(1, 6);

        private static string RandomMessage
        {
            get
            {
                return string.Join(string.Empty, Enumerable
                    .Range(0, RandomUtils.Current.Next(5, 21))
                    .Select(i => Alphabet[RandomUtils.Current.Next(Alphabet.Length)]));
            }
        }

        #endregion Properties

        #region Classes

        private static class RandomUtils
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

        #endregion Classes
    }
}
