namespace Dawn.Tests
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class StringTests : BaseTests
    {
        private static readonly TimeSpan MatchTimeout = TimeSpan.FromMilliseconds(10);

        [Theory(DisplayName = T + "String: Empty/NotEmpty")]
        [InlineData(null, null)]
        [InlineData("", "A")]
        public void Empty(string empty, string nonEmpty)
        {
            var emptyArg = Guard.Argument(() => empty).Empty();
            var nonEmptyArg = Guard.Argument(() => nonEmpty).NotEmpty();

            if (empty is null)
            {
                emptyArg.NotEmpty();
                nonEmptyArg.Empty();
                return;
            }

            ThrowsArgumentException(
                nonEmptyArg,
                arg => arg.Empty(),
                (arg, message) => arg.Empty(v =>
                {
                    Assert.Same(nonEmpty, v);
                    return message;
                }));

            ThrowsArgumentException(
                emptyArg,
                arg => arg.NotEmpty(),
                (arg, message) => arg.NotEmpty(v =>
                {
                    Assert.Same(empty, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: WhiteSpace/NotWhiteSpace")]
        [InlineData(null, null)]
        [InlineData("", "A")]
        [InlineData(" ", "A")]
        public void WhiteSpace(string ws, string nonWs)
        {
            var wsArg = Guard.Argument(() => ws).WhiteSpace();
            var nonWsArg = Guard.Argument(() => nonWs).NotWhiteSpace();

            if (ws is null)
            {
                wsArg.NotWhiteSpace();
                nonWsArg.WhiteSpace();
                return;
            }

            ThrowsArgumentException(
                nonWsArg,
                arg => arg.WhiteSpace(),
                (arg, message) => arg.WhiteSpace(v =>
                {
                    Assert.Same(nonWs, v);
                    return message;
                }));

            ThrowsArgumentException(
                wsArg,
                arg => arg.NotWhiteSpace(),
                (arg, message) => arg.NotWhiteSpace(v =>
                {
                    Assert.Same(ws, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: Length/NotLength")]
        [InlineData(null, -1, 0)]
        [InlineData("A", 1, 2)]
        public void Length(string value, int length, int nonLength)
        {
            var valueArg = Guard.Argument(() => value).Length(length).NotLength(nonLength);

            if (value is null)
            {
                valueArg.Length(nonLength).NotLength(length);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.Length(nonLength),
                (arg, message) => arg.Length(nonLength, (v, l) =>
                {
                    Assert.Same(value, v);
                    Assert.Equal(nonLength, l);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotLength(length),
                (arg, message) => arg.NotLength(length, (v, l) =>
                {
                    Assert.Same(value, v);
                    Assert.Equal(length, l);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: MinLength")]
        [InlineData(null, 3, 4)]
        [InlineData("", 0, 1)]
        [InlineData("ABC", 3, 4)]
        [InlineData("DEF", 2, 5)]
        public void MinLength(string value, int lengthOrLess, int moreThanLength)
        {
            var valueArg = Guard.Argument(() => value).MinLength(lengthOrLess);

            if (value is null)
            {
                valueArg.MinLength(moreThanLength);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.MinLength(moreThanLength),
                (arg, message) => arg.MinLength(moreThanLength, (v, m) =>
                {
                    Assert.Same(value, v);
                    Assert.Equal(moreThanLength, m);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: MaxLength")]
        [InlineData(null, 3, 2)]
        [InlineData("", 0, -1)]
        [InlineData("ABC", 3, 2)]
        [InlineData("DEF", 4, 1)]
        public void MaxLength(string value, int lengthOrMore, int lessThanLength)
        {
            var valueArg = Guard.Argument(() => value).MaxLength(lengthOrMore);

            if (value is null)
            {
                valueArg.MaxLength(lessThanLength);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.MaxLength(lessThanLength),
                (arg, message) => arg.MaxLength(lessThanLength, (v, m) =>
                {
                    Assert.Equal(value, v);
                    Assert.Equal(lessThanLength, m);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: LengthInRange")]
        [InlineData(null, 2, 4)]
        [InlineData("", -1, 1)]
        [InlineData("ABC", 2, 4)]
        [InlineData("DEF", 1, 5)]
        public void LengthInRange(string value, int lessThanLength, int moreThanLength)
        {
            var length = value?.Length ?? RandomNumber;
            var valueArg = Guard.Argument(() => value)
                .LengthInRange(lessThanLength, length)
                .LengthInRange(length, length)
                .LengthInRange(length, moreThanLength)
                .LengthInRange(lessThanLength, moreThanLength);

            if (value is null)
            {
                for (var i = 0; i < 2; i++)
                {
                    var limit = i == 0 ? lessThanLength : moreThanLength;
                    valueArg.LengthInRange(limit, limit);
                }

                return;
            }

            for (var i = 0; i < 2; i++)
            {
                var limit = i == 0 ? lessThanLength : moreThanLength;
                ThrowsArgumentException(
                    valueArg,
                    arg => arg.LengthInRange(limit, limit),
                    (arg, message) => arg.LengthInRange(limit, limit, (v, min, max) =>
                    {
                        Assert.Same(value, v);
                        Assert.Equal(limit, min);
                        Assert.Equal(limit, max);
                        return message;
                    }));
            }
        }

        [Theory(DisplayName = T + "String: Equal/NotEqual w/ comparison")]
        [InlineData(null, null, null, StringComparison.Ordinal, false)]
        [InlineData("AB", "AB", "ab", StringComparison.Ordinal, false)]
        [InlineData("AB", "AB", "ab", StringComparison.Ordinal, true)]
        [InlineData("AB", "ab", "BC", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("AB", "ab", "BC", StringComparison.OrdinalIgnoreCase, true)]
        public void EqualWithComparison(
            string value, string equal, string unequal, StringComparison comparison, bool secure)
        {
            var valueArg = Guard.Argument(() => value, secure)
                .Equal(equal, comparison)
                .NotEqual(unequal, comparison);

            if (value == null)
            {
                valueArg
                    .Equal(unequal, comparison)
                    .NotEqual(equal, comparison);

                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.Equal(unequal, comparison),
                m => secure != m.Contains(unequal),
                (arg, message) => arg.Equal(unequal, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(unequal, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotEqual(equal, comparison),
                m => secure != m.Contains(equal),
                (arg, message) => arg.NotEqual(equal, comparison, v =>
                {
                    Assert.Same(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: StartsWith/DoesNotStartWith w/o comparison")]
        [InlineData(null, null, null, false)]
        [InlineData("ABC", "AB", "B", false)]
        [InlineData("ABC", "AB", "B", true)]
        public void StartsWithWithoutComparison(
            string value, string head, string nonHead, bool secure)
        {
            var valueArg = Guard.Argument(() => value, secure)
                .StartsWith(head)
                .DoesNotStartWith(nonHead);

            if (value == null)
            {
                valueArg
                    .StartsWith(nonHead)
                    .DoesNotStartWith(head);

                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.StartsWith(nonHead),
                m => secure != m.Contains(nonHead),
                (arg, message) => arg.StartsWith(nonHead, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(nonHead, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.DoesNotStartWith(head),
                m => secure != m.Contains(head),
                (arg, message) => arg.DoesNotStartWith(head, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(head, other);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: StartsWith/DoesNotStartWith w/ comparison")]
        [InlineData(null, null, null, StringComparison.Ordinal, false)]
        [InlineData("ABC", "AB", "ab", StringComparison.Ordinal, false)]
        [InlineData("ABC", "AB", "ab", StringComparison.Ordinal, true)]
        [InlineData("ABC", "ab", "b", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("ABC", "ab", "b", StringComparison.OrdinalIgnoreCase, true)]
        public void StartsWithWithComparison(
            string value, string head, string nonHead, StringComparison comparison, bool secure)
        {
            var valueArg = Guard.Argument(() => value, secure)
                .StartsWith(head, comparison)
                .DoesNotStartWith(nonHead, comparison);

            if (value == null)
            {
                valueArg
                    .StartsWith(nonHead, comparison)
                    .DoesNotStartWith(head, comparison);

                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.StartsWith(nonHead, comparison),
                m => secure != m.Contains(nonHead),
                (arg, message) => arg.StartsWith(nonHead, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(nonHead, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.DoesNotStartWith(head, comparison),
                m => secure != m.Contains(head),
                (arg, message) => arg.DoesNotStartWith(head, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(head, other);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: EndsWith/DoesNotEndWith w/o comparison")]
        [InlineData(null, null, null, false)]
        [InlineData("ABC", "BC", "B", false)]
        [InlineData("ABC", "BC", "B", true)]
        public void EndsWithWithoutComparison(
            string value, string tail, string nonTail, bool secure)
        {
            var valueArg = Guard.Argument(() => value, secure)
                .EndsWith(tail)
                .DoesNotEndWith(nonTail);

            if (value == null)
            {
                valueArg.EndsWith(nonTail).DoesNotEndWith(tail);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.EndsWith(nonTail),
                m => secure != m.Contains(nonTail),
                (arg, message) => arg.EndsWith(nonTail, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(nonTail, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.DoesNotEndWith(tail),
                m => secure != m.Contains(tail),
                (arg, message) => arg.DoesNotEndWith(tail, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(tail, other);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: EndsWith/DoesNotEndWith w/ comparison")]
        [InlineData(null, null, null, StringComparison.Ordinal, false)]
        [InlineData("ABC", "BC", "bc", StringComparison.Ordinal, false)]
        [InlineData("ABC", "BC", "bc", StringComparison.Ordinal, true)]
        [InlineData("ABC", "bc", "b", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("ABC", "bc", "b", StringComparison.OrdinalIgnoreCase, true)]
        public void EndsWithWithComparison(
            string value, string tail, string nonTail, StringComparison comparison, bool secure)
        {
            var valueArg = Guard.Argument(() => value, secure)
                .EndsWith(tail, comparison)
                .DoesNotEndWith(nonTail, comparison);

            if (value == null)
            {
                valueArg
                    .EndsWith(nonTail, comparison)
                    .DoesNotEndWith(tail, comparison);

                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.EndsWith(nonTail, comparison),
                m => secure != m.Contains(nonTail),
                (arg, message) => arg.EndsWith(nonTail, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(nonTail, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.DoesNotEndWith(tail, comparison),
                m => secure != m.Contains(tail),
                (arg, message) => arg.DoesNotEndWith(tail, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(tail, other);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: Matches/DoesNotMatch")]
        [InlineData(null, null, null, null, null, false)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "0123456789", "ABC", "[", "([A-Z]+)*!", false)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "0123456789", "ABC", "[", "([A-Z]+)*!", true)]
        public void Matches(
            string withMatch,
            string withoutMatch,
            string validPattern,
            string invalidPattern,
            string timeoutPattern,
            bool secure)
        {
            var validRegexWithoutTimeout = validPattern is null ? null : new Regex(validPattern);
            var validRegexWithTimeout = validPattern is null ? null : new Regex(validPattern, RegexOptions.None, MatchTimeout);
            var timeoutRegex = timeoutPattern is null ? null : new Regex(timeoutPattern, RegexOptions.None, MatchTimeout);

            var withMatchArg = Guard.Argument(() => withMatch, secure)
                .Matches(validPattern)
                .Matches(validPattern, MatchTimeout)
                .Matches(validRegexWithoutTimeout)
                .Matches(validRegexWithTimeout);

            var withoutMatchArg = Guard.Argument(() => withoutMatch, secure)
                .DoesNotMatch(validPattern)
                .DoesNotMatch(validPattern, MatchTimeout)
                .DoesNotMatch(validRegexWithoutTimeout)
                .DoesNotMatch(validRegexWithTimeout);

            if (withMatch is null)
            {
                withMatchArg
                    .Matches(invalidPattern)
                    .Matches(invalidPattern, MatchTimeout)
                    .Matches(timeoutPattern)
                    .Matches(timeoutPattern, MatchTimeout)
                    .DoesNotMatch(invalidPattern)
                    .DoesNotMatch(invalidPattern, MatchTimeout)
                    .DoesNotMatch(timeoutPattern)
                    .DoesNotMatch(timeoutPattern, MatchTimeout)
                    .DoesNotMatch(validPattern)
                    .DoesNotMatch(validPattern, MatchTimeout)
                    .DoesNotMatch(validRegexWithoutTimeout)
                    .DoesNotMatch(validRegexWithTimeout)
                    .Matches(timeoutRegex)
                    .DoesNotMatch(timeoutRegex);

                withoutMatchArg
                    .Matches(invalidPattern)
                    .Matches(invalidPattern, MatchTimeout)
                    .Matches(timeoutPattern)
                    .Matches(timeoutPattern, MatchTimeout)
                    .DoesNotMatch(invalidPattern)
                    .DoesNotMatch(invalidPattern, MatchTimeout)
                    .DoesNotMatch(timeoutPattern)
                    .DoesNotMatch(timeoutPattern, MatchTimeout)
                    .Matches(validPattern)
                    .Matches(validPattern, MatchTimeout)
                    .Matches(validRegexWithoutTimeout)
                    .Matches(validRegexWithTimeout)
                    .Matches(timeoutRegex)
                    .DoesNotMatch(timeoutRegex);

                return;
            }

            var timeoutTasks = new[]
            {
                // Matches - timeout pattern
                Task.Run(() =>
                {
                    ThrowsArgumentException(
                        withMatchArg,
                        arg => arg.Matches(timeoutPattern, MatchTimeout),
                        m => secure != m.Contains(timeoutPattern),
                        (arg, message) => arg.Matches(timeoutPattern, MatchTimeout, (s, t) =>
                        {
                            Assert.Same(withMatch, s);
                            Assert.True(t);
                            return message;
                        }),
                        true);
                }),

                // Matches - timeout expression
                Task.Run(() =>
                {
                    ThrowsArgumentException(
                        withMatchArg,
                        arg => arg.Matches(timeoutRegex),
                        m => secure != m.Contains(timeoutPattern),
                        (arg, message) => arg.Matches(timeoutRegex, (s, t) =>
                        {
                            Assert.Same(withMatch, s);
                            Assert.True(t);
                            return message;
                        }),
                        true);
                }),

                // Does not match - timeout pattern
                Task.Run(() =>
                {
                    ThrowsArgumentException(
                        withMatchArg,
                        arg => arg.DoesNotMatch(timeoutPattern, MatchTimeout),
                        m => secure != m.Contains(timeoutPattern),
                        (arg, message) => arg.DoesNotMatch(timeoutPattern, MatchTimeout, (s, t) =>
                        {
                            Assert.Same(withMatch, s);
                            Assert.True(t);
                            return message;
                        }),
                        true);
                }),

                // Does not match - timeout expression
                Task.Run(() =>
                {
                    ThrowsArgumentException(
                        withMatchArg,
                        arg => arg.DoesNotMatch(timeoutRegex),
                        m => secure != m.Contains(timeoutPattern),
                        (arg, message) => arg.DoesNotMatch(timeoutRegex, (s, t) =>
                        {
                            Assert.Same(withMatch, s);
                            Assert.True(t);
                            return message;
                        }),
                        true,
                        true);
                })
            };

            // Matches - invalid pattern
            ThrowsArgumentException(
                Guard.Argument(withMatch, "pattern", secure),
                arg => arg.Matches(invalidPattern),
                (arg, message) => arg.Matches(invalidPattern, (s, t) =>
                {
                    Assert.Same(withMatch, s);
                    Assert.False(t);
                    return message;
                }),
                true,
                true);

            // Matches - valid pattern w/o timeout
            ThrowsArgumentException(
                withoutMatchArg,
                arg => arg.Matches(validPattern),
                m => secure != m.Contains(validPattern),
                (arg, message) => arg.Matches(validPattern, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Matches - valid pattern w/ timeout
            ThrowsArgumentException(
                withoutMatchArg,
                arg => arg.Matches(validPattern, MatchTimeout),
                m => secure != m.Contains(validPattern),
                (arg, message) => arg.Matches(validPattern, MatchTimeout, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Matches - valid expression w/o timeout
            ThrowsArgumentException(
                withoutMatchArg,
                arg => arg.Matches(validRegexWithoutTimeout),
                m => secure != m.Contains(validPattern),
                (arg, message) => arg.Matches(validRegexWithoutTimeout, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Matches - valid expression w/ timeout
            ThrowsArgumentException(
                withoutMatchArg,
                arg => arg.Matches(validRegexWithTimeout),
                m => secure != m.Contains(validPattern),
                (arg, message) => arg.Matches(validRegexWithTimeout, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Does not match - invalid pattern
            ThrowsArgumentException(
                Guard.Argument(withoutMatch, "pattern", secure),
                arg => arg.DoesNotMatch(invalidPattern),
                (arg, message) => arg.DoesNotMatch(invalidPattern, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }),
                true,
                true);

            // Does not match - valid pattern w/o timeout
            ThrowsArgumentException(
                withMatchArg,
                arg => arg.DoesNotMatch(validPattern),
                m => secure != m.Contains(validPattern),
                (arg, message) => arg.DoesNotMatch(validPattern, (s, t) =>
                {
                    Assert.Same(withMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Does not match - valid pattern w/ timeout
            ThrowsArgumentException(
                withMatchArg,
                arg => arg.DoesNotMatch(validPattern, MatchTimeout),
                m => secure != m.Contains(validPattern),
                (arg, message) => arg.DoesNotMatch(validPattern, MatchTimeout, (s, t) =>
                {
                    Assert.Same(withMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Does not match - valid expression w/o timeout
            ThrowsArgumentException(
                withMatchArg,
                arg => arg.DoesNotMatch(validRegexWithoutTimeout),
                m => secure != m.Contains(validPattern),
                (arg, message) => arg.DoesNotMatch(validRegexWithoutTimeout, (s, t) =>
                {
                    Assert.Same(withMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Does not match - valid expression w/ timeout
            ThrowsArgumentException(
                withMatchArg,
                arg => arg.DoesNotMatch(validRegexWithTimeout),
                m => secure != m.Contains(validPattern),
                (arg, message) => arg.DoesNotMatch(validRegexWithTimeout, (s, t) =>
                {
                    Assert.Same(withMatch, s);
                    Assert.False(t);
                    return message;
                }));

            Task.WaitAll(timeoutTasks);
        }
    }
}
