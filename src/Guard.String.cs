namespace Dawn
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    /// <content>Provides preconditions for <see cref="string" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>Requires the argument to have an empty string value.</summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and contains one or
        ///     more characters.
        /// </exception>
        public static ref readonly ArgumentInfo<string> Empty(
            in this ArgumentInfo<string> argument, Func<string, string> message)
        {
            if (argument.Value?.Length > 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.StringEmpty(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have a non-empty string value.</summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and does not contain
        ///     any characters.
        /// </exception>
        public static ref readonly ArgumentInfo<string> NotEmpty(
            in this ArgumentInfo<string> argument, string message = null)
        {
            if (argument.Value?.Length == 0)
            {
                var m = message ?? Messages.StringNotEmpty(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that consists only of
        ///     white-space characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and contains one or more
        ///     characters that are not white-space.
        /// </exception>
        public static ref readonly ArgumentInfo<string> WhiteSpace(
            in this ArgumentInfo<string> argument, Func<string, string> message = null)
        {
            if (argument.Value != null && !string.IsNullOrWhiteSpace(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.StringWhiteSpace(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that does not consist only of
        ///     white-space characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>
        ///     and contains only of white-space characters.
        /// </exception>
        public static ref readonly ArgumentInfo<string> NotWhiteSpace(
            in this ArgumentInfo<string> argument, Func<string, string> message = null)
        {
            if (argument.Value != null && string.IsNullOrWhiteSpace(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.StringNotWhiteSpace(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that contains at least the specified
        ///     number of characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="minLength">
        ///     The minimum number of characters allowed in the argument value.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and contains less than the
        ///     specified number of characters.
        /// </exception>
        public static ref readonly ArgumentInfo<string> MinLength(
            in this ArgumentInfo<string> argument,
            int minLength,
            Func<string, int, string>
            message = null)
        {
            if (argument.Value?.Length < minLength)
            {
                var m = message?.Invoke(argument.Value, minLength) ?? Messages.StringMinLength(argument, minLength);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that contains no more than the
        ///     specified number of characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="maxLength">
        ///     The maximum number of characters allowed in the argument value.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and contains more than the
        ///     specified number of characters.
        /// </exception>
        public static ref readonly ArgumentInfo<string> MaxLength(
            in this ArgumentInfo<string> argument,
            int maxLength,
            Func<string, int, string> message = null)
        {
            if (argument.Value?.Length > maxLength)
            {
                var m = message?.Invoke(argument.Value, maxLength) ?? Messages.StringMaxLength(argument, maxLength);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value whose length is between the specified minimum
        ///     and maximum values.
        /// </summary>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="minLength">
        ///     The minimum number of characters allowed in the argument value.
        /// </param>
        /// <param name="maxLength">
        ///     The maximum number of characters allowed in the argument value.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and contains either less than
        ///     <paramref name="minLength" /> or more than <paramref name="maxLength" /> number
        ///     of characters.
        /// </exception>
        public static ref readonly ArgumentInfo<string> LengthInRange(
            in this ArgumentInfo<string> argument,
            int minLength,
            int maxLength,
            Func<string, int, int, string> message = null)
        {
            if (argument.HasValue())
                if (argument.Value.Length < minLength || argument.Value.Length > maxLength)
                {
                    var m = message?.Invoke(argument.Value, minLength, maxLength)
                        ?? Messages.StringLengthInRange(argument, minLength, maxLength);

                    throw new ArgumentException(m, argument.Name);
                }

            return ref argument;
        }

        /// <summary>
        ///     Requires the string argument to have a value that is equal to another string when
        ///     compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="other">The string to compare the argument value to.</param>
        /// <param name="comparison">
        ///     The rules that specify how the strings will be compared.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not equal to <paramref name="other" /> by the
        ///     comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        public static ref readonly ArgumentInfo<string> Equal(
            in this ArgumentInfo<string> argument,
            string other,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue() && !StringEqualityComparer(comparison).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.Equal(argument, other);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the string argument to have a value that is different than another string
        ///     when compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="other">The string to compare the argument value to.</param>
        /// <param name="comparison">
        ///     The rules that specify how the strings will be compared.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is equal to <paramref name="other" /> by the
        ///     comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        public static ref readonly ArgumentInfo<string> NotEqual(
            in this ArgumentInfo<string> argument,
            string other,
            StringComparison comparison,
            Func<string, string> message = null)
        {
            if (argument.HasValue() && StringEqualityComparer(comparison).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotEqual(argument, other);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the beginning of the string argument to match with the specified string.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the beginning of the argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value does not start with <paramref name="value" />
        ///     when a case-sensitive and culture-sensitive comparison is performed.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<string> StartsWith(
            in this ArgumentInfo<string> argument,
            string value,
            Func<string, string, string> message = null)
            => ref argument.StartsWith(value, StringComparison.CurrentCulture, message);

        /// <summary>
        ///     Requires the beginning of the string argument to match with the specified string
        ///     when compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the beginning of the argument.</param>
        /// <param name="comparison">
        ///     The rules that specify how the strings will be compared.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value does not start with <paramref name="value" />
        ///     when compared by the comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        public static ref readonly ArgumentInfo<string> StartsWith(
            in this ArgumentInfo<string> argument,
            string value,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue() && value != null && !argument.Value.StartsWith(value, comparison))
            {
                var m = message?.Invoke(argument.Value, value) ?? Messages.StringStartsWith(argument, value);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the beginning of the string argument to be different than the
        ///     specified string.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the beginning of the argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value starts with <paramref name="value" /> when a
        ///     case-sensitive and culture-sensitive comparison is performed.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<string> DoesNotStartWith(
            in this ArgumentInfo<string> argument,
            string value,
            Func<string, string, string> message = null)
            => ref argument.DoesNotStartWith(value, StringComparison.CurrentCulture, message);

        /// <summary>
        ///     Requires the beginning of the string argument to be different than the specified
        ///     string when compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the beginning of the argument.</param>
        /// <param name="comparison">
        ///     The rules that specify how the strings will be compared.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value starts with <paramref name="value" /> when
        ///     compared by the comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        public static ref readonly ArgumentInfo<string> DoesNotStartWith(
            in this ArgumentInfo<string> argument,
            string value,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue() && value != null && argument.Value.StartsWith(value, comparison))
            {
                var m = message?.Invoke(argument.Value, value) ?? Messages.StringDoesNotStartWith(argument, value);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the end of the string argument to match with the specified string.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the end of the argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value does not end with <paramref name="value" /> when
        ///     a case-sensitive and culture-sensitive comparison is performed.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<string> EndsWith(
            in this ArgumentInfo<string> argument,
            string value,
            Func<string, string, string> message = null)
            => ref argument.EndsWith(value, StringComparison.CurrentCulture, message);

        /// <summary>
        ///     Requires the end of the string argument to match with the specified string when
        ///     compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the end of the argument.</param>
        /// <param name="comparison">
        ///     The rules that specify how the strings will be compared.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value does not end with <paramref name="value" /> when
        ///     compared by the comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        public static ref readonly ArgumentInfo<string> EndsWith(
            in this ArgumentInfo<string> argument,
            string value,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue() && value != null && !argument.Value.EndsWith(value, comparison))
            {
                var m = message?.Invoke(argument.Value, value) ?? Messages.StringEndsWith(argument, value);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the end of the string argument to be different than the specified string.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the end of the argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value ends with <paramref name="value" /> when a
        ///     case-sensitive and culture-sensitive comparison is performed.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<string> DoesNotEndWith(
            in this ArgumentInfo<string> argument,
            string value,
            Func<string, string, string> message = null)
            => ref argument.DoesNotEndWith(value, StringComparison.CurrentCulture, message);

        /// <summary>
        ///     Requires the end of the string argument to be different than the specified string
        ///     when compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the end of the argument.</param>
        /// <param name="comparison">
        ///     The rules that specify how the strings will be compared.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value ends with <paramref name="value" /> when compared
        ///     by the comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        public static ref readonly ArgumentInfo<string> DoesNotEndWith(
            in this ArgumentInfo<string> argument,
            string value,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue() && value != null && argument.Value.EndsWith(value, comparison))
            {
                var m = message?.Invoke(argument.Value, value) ?? Messages.StringDoesNotEndWith(argument, value);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the specified regular expression to find a match in the string argument.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="pattern">The regular expression pattern to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied. The boolean argument indicates whether the exception
        ///     is caused because of a time-out.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="pattern" /> cannot be parsed as a regular expression, or the
        ///     resulting expression could not find a match in <paramref name="argument" />'s value.
        /// </exception>
        public static ref readonly ArgumentInfo<string> Matches(
            in this ArgumentInfo<string> argument,
            string pattern,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue() && pattern != null)
            {
                bool matches;
                try
                {
                    matches = Regex.IsMatch(argument.Value, pattern);
                }
                catch (ArgumentException ex)
                {
                    // ParamName of the ArgumentException thrown by IsMatch is null.
                    throw new ArgumentException(ex.Message, nameof(pattern), ex);
                }

                if (!matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringMatches(argument, pattern);

                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the specified regular expression to find a match in the string argument.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="pattern">The regular expression pattern to use.</param>
        /// <param name="matchTimeout">
        ///     A time-out interval, or <see cref="Regex.InfiniteMatchTimeout" /> to indicate that
        ///     the method should not time out.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied. The boolean argument indicates whether the exception
        ///     is caused because of a time-out.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="pattern" /> cannot be parsed as a regular expression, the resulting
        ///     expression timed out, or it could not find a match in <paramref name="argument" />'s
        ///     value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="matchTimeout" /> is negative, zero, or greater than approximately
        ///     24 days.
        /// </exception>
        public static ref readonly ArgumentInfo<string> Matches(
            in this ArgumentInfo<string> argument,
            string pattern,
            TimeSpan matchTimeout,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue() && pattern != null)
            {
                bool matches;
                try
                {
                    matches = Regex.IsMatch(argument.Value, pattern, RegexOptions.None, matchTimeout);
                }
                catch (ArgumentException ex)
                {
                    // ParamName of the ArgumentException thrown by IsMatch is null.
                    throw new ArgumentException(ex.Message, nameof(pattern), ex);
                }
                catch (RegexMatchTimeoutException ex)
                {
                    var m = message?.Invoke(argument.Value, true)
                        ?? Messages.StringMatchesTimeout(argument, pattern, matchTimeout);

                    throw new ArgumentException(m, argument.Name, ex);
                }

                if (!matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringMatches(argument, pattern);

                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the specified regular expression to find a match in the string argument.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="regex">The regular expression to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied. The boolean argument indicates whether the exception
        ///     is caused because of a time-out.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="regex" /> could not find a match in <paramref name="argument" />'s
        ///     value or it timed out before the evaluation is completed.
        /// </exception>
        public static ref readonly ArgumentInfo<string> Matches(
            in this ArgumentInfo<string> argument,
            Regex regex,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue() && regex != null)
            {
                bool matches;
                try
                {
                    matches = regex.IsMatch(argument.Value);
                }
                catch (RegexMatchTimeoutException ex)
                {
                    var m = message?.Invoke(argument.Value, true)
                        ?? Messages.StringMatchesTimeout(argument, regex.ToString(), ex.MatchTimeout);

                    throw new ArgumentException(m, argument.Name, ex);
                }

                if (!matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringMatches(argument, regex.ToString());

                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the string argument to not contain a match that can be found by the
        ///     specified regular expression.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="pattern">The regular expression pattern to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied. The boolean argument indicates whether the exception
        ///     is caused because of a time-out.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="pattern" /> cannot be parsed as a regular expression, or the
        ///     resulting expression found a match in <paramref name="argument" />'s value.
        /// </exception>
        public static ref readonly ArgumentInfo<string> DoesNotMatch(
            in this ArgumentInfo<string> argument,
            string pattern,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue() && pattern != null)
            {
                bool matches;
                try
                {
                    matches = Regex.IsMatch(argument.Value, pattern);
                }
                catch (ArgumentException ex)
                {
                    // ParamName of the ArgumentException thrown by IsMatch is null.
                    throw new ArgumentException(ex.Message, nameof(pattern), ex);
                }

                if (matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringDoesNotMatch(argument, pattern);

                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the string argument to not contain a match that can be found by the
        ///     specified regular expression.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="pattern">The regular expression pattern to use.</param>
        /// <param name="matchTimeout">
        ///     A time-out interval, or <see cref="Regex.InfiniteMatchTimeout" /> to indicate that
        ///     the method should not time out.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied. The boolean argument indicates whether the exception
        ///     is caused because of a time-out.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="pattern" /> cannot be parsed as a regular expression, the resulting
        ///     expression timed out, or it found a match in <paramref name="argument" />'s value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="matchTimeout" /> is negative, zero, or greater than approximately
        ///     24 days.
        /// </exception>
        public static ref readonly ArgumentInfo<string> DoesNotMatch(
            in this ArgumentInfo<string> argument,
            string pattern,
            TimeSpan matchTimeout,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue() && pattern != null)
            {
                bool matches;
                try
                {
                    matches = Regex.IsMatch(argument.Value, pattern, RegexOptions.None, matchTimeout);
                }
                catch (ArgumentException ex)
                {
                    // ParamName of the ArgumentException thrown by IsMatch is null.
                    throw new ArgumentException(ex.Message, nameof(pattern), ex);
                }
                catch (RegexMatchTimeoutException ex)
                {
                    var m = message?.Invoke(argument.Value, true)
                        ?? Messages.StringDoesNotMatchTimeout(argument, pattern, matchTimeout);

                    throw new ArgumentException(m, argument.Name, ex);
                }

                if (matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringDoesNotMatch(argument, pattern);

                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the string argument to not contain a match that can be found by the
        ///     specified regular expression.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="regex">The regular expression to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied. The boolean argument indicates whether the exception
        ///     is caused because of a time-out.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="regex" /> found a match in <paramref name="argument" />'s value or
        ///     it timed out before the evaluation is completed.
        /// </exception>
        public static ref readonly ArgumentInfo<string> DoesNotMatch(
            in this ArgumentInfo<string> argument,
            Regex regex,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue() && regex != null)
            {
                bool matches;
                try
                {
                    matches = regex.IsMatch(argument.Value);
                }
                catch (RegexMatchTimeoutException ex)
                {
                    var m = message?.Invoke(argument.Value, true)
                        ?? Messages.StringDoesNotMatchTimeout(argument, regex.ToString(), ex.MatchTimeout);

                    throw new ArgumentException(m, argument.Name, ex);
                }

                if (matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringDoesNotMatch(argument, regex.ToString());

                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Returns the string comparer that is most
        ///     relevant to the specified enumeration value.
        /// </summary>
        /// <param name="comparison">
        ///     An enumeration value that specifies how to compare two strings.
        /// </param>
        /// <returns>A string equality comparer.</returns>
        private static IEqualityComparer<string> StringEqualityComparer(StringComparison comparison)
        {
            switch (comparison)
            {
                case StringComparison.CurrentCulture:
                    return StringComparer.CurrentCulture;

                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase;

#if !NETSTANDARD1_0
                case StringComparison.InvariantCulture:
                    return StringComparer.InvariantCulture;

                case StringComparison.InvariantCultureIgnoreCase:
                    return StringComparer.InvariantCultureIgnoreCase;
#endif

                case StringComparison.Ordinal:
                    return StringComparer.Ordinal;

                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;

                default:
                    return EqualityComparer<string>.Default;
            }
        }
    }
}
