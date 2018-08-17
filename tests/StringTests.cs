namespace Dawn.Tests
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class StringTests : BaseTests
    {
        private static readonly TimeSpan matchTimeout = TimeSpan.FromMilliseconds(10);

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
        [InlineData(null, null, null, StringComparison.Ordinal)]
        [InlineData("A", "A", "a", StringComparison.Ordinal)]
        [InlineData("A", "a", "B", StringComparison.OrdinalIgnoreCase)]
        public void EqualWithComparison(string value, string equal, string unequal, StringComparison comparison)
        {
            var valueArg = Guard.Argument(() => value)
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
                (arg, message) => arg.Equal(unequal, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(unequal, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotEqual(equal, comparison),
                (arg, message) => arg.NotEqual(equal, comparison, v =>
                {
                    Assert.Same(value, v);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: StartsWith/DoesNotStartWith w/o comparison")]
        [InlineData(null, null, null)]
        [InlineData("ABC", "AB", "B")]
        public void StartsWithWithoutComparison(string value, string head, string nonHead)
        {
            var valueArg = Guard.Argument(() => value).StartsWith(head).DoesNotStartWith(nonHead);
            if (value == null)
            {
                valueArg.StartsWith(nonHead).DoesNotStartWith(head);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.StartsWith(nonHead),
                (arg, message) => arg.StartsWith(nonHead, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(nonHead, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.DoesNotStartWith(head),
                (arg, message) => arg.DoesNotStartWith(head, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(head, other);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: StartsWith/DoesNotStartWith w/ comparison")]
        [InlineData(null, null, null, StringComparison.Ordinal)]
        [InlineData("ABC", "AB", "ab", StringComparison.Ordinal)]
        [InlineData("ABC", "ab", "b", StringComparison.OrdinalIgnoreCase)]
        public void StartsWithWithComparison(string value, string head, string nonHead, StringComparison comparison)
        {
            var valueArg = Guard.Argument(() => value)
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
                (arg, message) => arg.StartsWith(nonHead, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(nonHead, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.DoesNotStartWith(head, comparison),
                (arg, message) => arg.DoesNotStartWith(head, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(head, other);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: EndsWith/DoesNotEndWith w/o comparison")]
        [InlineData(null, null, null)]
        [InlineData("ABC", "BC", "B")]
        public void EndsWithWithoutComparison(string value, string tail, string nonTail)
        {
            var valueArg = Guard.Argument(() => value).EndsWith(tail).DoesNotEndWith(nonTail);
            if (value == null)
            {
                valueArg.EndsWith(nonTail).DoesNotEndWith(tail);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.EndsWith(nonTail),
                (arg, message) => arg.EndsWith(nonTail, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(nonTail, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.DoesNotEndWith(tail),
                (arg, message) => arg.DoesNotEndWith(tail, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(tail, other);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: EndsWith/DoesNotEndWith w/ comparison")]
        [InlineData(null, null, null, StringComparison.Ordinal)]
        [InlineData("ABC", "BC", "bc", StringComparison.Ordinal)]
        [InlineData("ABC", "bc", "b", StringComparison.OrdinalIgnoreCase)]
        public void EndsWithWithComparison(string value, string tail, string nonTail, StringComparison comparison)
        {
            var valueArg = Guard.Argument(() => value)
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
                (arg, message) => arg.EndsWith(nonTail, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(nonTail, other);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.DoesNotEndWith(tail, comparison),
                (arg, message) => arg.DoesNotEndWith(tail, comparison, (v, other) =>
                {
                    Assert.Same(value, v);
                    Assert.Same(tail, other);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "String: Matches/DoesNotMatch")]
        [InlineData(null, null, null, null, null)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "0123456789", "ABC", "[", "([A-Z]+)*!")]
        public void Matches(
            string withMatch,
            string withoutMatch,
            string validPattern,
            string invalidPattern,
            string timeoutPattern)
        {
            var validRegexWithoutTimeout = validPattern is null ? null : new Regex(validPattern);
            var validRegexWithTimeout = validPattern is null ? null : new Regex(validPattern, RegexOptions.None, matchTimeout);
            var timeoutRegex = timeoutPattern is null ? null : new Regex(timeoutPattern, RegexOptions.None, matchTimeout);

            var withMatchArg = Guard.Argument(() => withMatch)
                .Matches(validPattern)
                .Matches(validPattern, matchTimeout)
                .Matches(validRegexWithoutTimeout)
                .Matches(validRegexWithTimeout);

            var withoutMatchArg = Guard.Argument(() => withoutMatch)
                .DoesNotMatch(validPattern)
                .DoesNotMatch(validPattern, matchTimeout)
                .DoesNotMatch(validRegexWithoutTimeout)
                .DoesNotMatch(validRegexWithTimeout);

            if (withMatch is null)
            {
                withMatchArg
                    .Matches(invalidPattern)
                    .Matches(invalidPattern, matchTimeout)
                    .Matches(timeoutPattern)
                    .Matches(timeoutPattern, matchTimeout)
                    .DoesNotMatch(invalidPattern)
                    .DoesNotMatch(invalidPattern, matchTimeout)
                    .DoesNotMatch(timeoutPattern)
                    .DoesNotMatch(timeoutPattern, matchTimeout)
                    .DoesNotMatch(validPattern)
                    .DoesNotMatch(validPattern, matchTimeout)
                    .DoesNotMatch(validRegexWithoutTimeout)
                    .DoesNotMatch(validRegexWithTimeout)
                    .Matches(timeoutRegex)
                    .DoesNotMatch(timeoutRegex);

                withoutMatchArg
                    .Matches(invalidPattern)
                    .Matches(invalidPattern, matchTimeout)
                    .Matches(timeoutPattern)
                    .Matches(timeoutPattern, matchTimeout)
                    .DoesNotMatch(invalidPattern)
                    .DoesNotMatch(invalidPattern, matchTimeout)
                    .DoesNotMatch(timeoutPattern)
                    .DoesNotMatch(timeoutPattern, matchTimeout)
                    .Matches(validPattern)
                    .Matches(validPattern, matchTimeout)
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
                        arg => arg.Matches(timeoutPattern, matchTimeout),
                        (arg, message) => arg.Matches(timeoutPattern, matchTimeout, (s, t) =>
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
                        arg => arg.DoesNotMatch(timeoutPattern, matchTimeout),
                        (arg, message) => arg.DoesNotMatch(timeoutPattern, matchTimeout, (s, t) =>
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
                        (arg, message) => arg.DoesNotMatch(timeoutRegex, (s, t) =>
                        {
                            Assert.Same(withMatch, s);
                            Assert.True(t);
                            return message;
                        }),
                        true);
                })
            };

            // Matches - invalid pattern
            ThrowsArgumentException(
                Guard.Argument(withMatch, "pattern"),
                arg => arg.Matches(invalidPattern),
                (arg, message) => arg.Matches(invalidPattern, (s, t) =>
                {
                    Assert.Same(withMatch, s);
                    Assert.False(t);
                    return message;
                }),
                true);

            // Matches - valid pattern w/o timeout
            ThrowsArgumentException(
                withoutMatchArg,
                arg => arg.Matches(validPattern),
                (arg, message) => arg.Matches(validPattern, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Matches - valid pattern w/ timeout
            ThrowsArgumentException(
                withoutMatchArg,
                arg => arg.Matches(validPattern, matchTimeout),
                (arg, message) => arg.Matches(validPattern, matchTimeout, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Matches - valid expression w/o timeout
            ThrowsArgumentException(
                withoutMatchArg,
                arg => arg.Matches(validRegexWithoutTimeout),
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
                (arg, message) => arg.Matches(validRegexWithTimeout, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Does not match - invalid pattern
            ThrowsArgumentException(
                Guard.Argument(withoutMatch, "pattern"),
                arg => arg.DoesNotMatch(invalidPattern),
                (arg, message) => arg.DoesNotMatch(invalidPattern, (s, t) =>
                {
                    Assert.Same(withoutMatch, s);
                    Assert.False(t);
                    return message;
                }),
                true);

            // Does not match - valid pattern w/o timeout
            ThrowsArgumentException(
                withMatchArg,
                arg => arg.DoesNotMatch(validPattern),
                (arg, message) => arg.DoesNotMatch(validPattern, (s, t) =>
                {
                    Assert.Same(withMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Does not match - valid pattern w/ timeout
            ThrowsArgumentException(
                withMatchArg,
                arg => arg.DoesNotMatch(validPattern, matchTimeout),
                (arg, message) => arg.DoesNotMatch(validPattern, matchTimeout, (s, t) =>
                {
                    Assert.Same(withMatch, s);
                    Assert.False(t);
                    return message;
                }));

            // Does not match - valid expression w/o timeout
            ThrowsArgumentException(
                withMatchArg,
                arg => arg.DoesNotMatch(validRegexWithoutTimeout),
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
