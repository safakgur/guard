using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Dawn
{
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
        ///     <paramref name="argument" /> value is not <c>null</c> and contains one or more characters.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gem")]
        public static ref readonly ArgumentInfo<string> Empty(
            in this ArgumentInfo<string> argument, Func<string, string> message = null)
        {
            if (argument.Value?.Length > 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.StringEmpty(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have a non-empty string value.</summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and does not contain any characters.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnem")]
        public static ref readonly ArgumentInfo<string> NotEmpty(
            in this ArgumentInfo<string> argument, string message = null)
        {
            if (argument.Value?.Length == 0)
            {
                var m = message ?? Messages.StringNotEmpty(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that consists only of white-space characters.
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
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gw")]
        public static ref readonly ArgumentInfo<string> WhiteSpace(
            in this ArgumentInfo<string> argument, Func<string, string> message = null)
        {
            if (argument.Value != null && !string.IsNullOrWhiteSpace(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.StringWhiteSpace(argument);
                throw Fail(new ArgumentException(m, argument.Name));
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
        ///     <paramref name="argument" /> value is not <c>null</c> and contains only of
        ///     white-space characters.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnw")]
        public static ref readonly ArgumentInfo<string> NotWhiteSpace(
            in this ArgumentInfo<string> argument, Func<string, string> message = null)
        {
            if (argument.Value != null && string.IsNullOrWhiteSpace(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.StringNotWhiteSpace(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that does not consist only of
        ///     white-space characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and contains only of
        ///     white-space characters.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnw")]
        public static ref readonly ArgumentInfo<string> NotWhiteSpace(
            in this ArgumentInfo<string> argument, string message)
        {
            if (argument.Value != null && string.IsNullOrWhiteSpace(argument.Value))
            {
                var m = message ?? Messages.StringNotWhiteSpace(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that consists of specified number of characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="length">
        ///     The exact number of characters that the argument value is required to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and does not have the exact
        ///     number of characters specified in <paramref name="length" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gl")]
        public static ref readonly ArgumentInfo<string> Length(
            in this ArgumentInfo<string> argument, int length, Func<string, int, string> message = null)
        {
            if (argument.HasValue && argument.Value.Length != length)
            {
                var m = message?.Invoke(argument.Value, length) ?? Messages.StringLength(argument, length);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that does not consist of specified
        ///     number of characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="length">
        ///     The exact number of characters that the argument value is required not to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and has the exact number of
        ///     characters specified in <paramref name="length" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnl")]
        public static ref readonly ArgumentInfo<string> NotLength(
            in this ArgumentInfo<string> argument, int length, Func<string, int, string> message = null)
        {
            if (argument.HasValue && argument.Value.Length == length)
            {
                var m = message?.Invoke(argument.Value, length) ?? Messages.StringNotLength(argument, length);
                throw Fail(new ArgumentException(m, argument.Name));
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
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gminl")]
        public static ref readonly ArgumentInfo<string> MinLength(
            in this ArgumentInfo<string> argument,
            int minLength,
            Func<string, int, string>
            message = null)
        {
            if (argument.Value?.Length < minLength)
            {
                var m = message?.Invoke(argument.Value, minLength) ?? Messages.StringMinLength(argument, minLength);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that contains no more than the specified
        ///     number of characters.
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
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gmaxl")]
        public static ref readonly ArgumentInfo<string> MaxLength(
            in this ArgumentInfo<string> argument,
            int maxLength,
            Func<string, int, string> message = null)
        {
            if (argument.Value?.Length > maxLength)
            {
                var m = message?.Invoke(argument.Value, maxLength) ?? Messages.StringMaxLength(argument, maxLength);
                throw Fail(new ArgumentException(m, argument.Name));
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
        ///     <paramref name="minLength" /> or more than <paramref name="maxLength" /> number of characters.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "glr")]
        public static ref readonly ArgumentInfo<string> LengthInRange(
            in this ArgumentInfo<string> argument,
            int minLength,
            int maxLength,
            Func<string, int, int, string> message = null)
        {
            if (argument.HasValue)
                if (argument.Value.Length < minLength || argument.Value.Length > maxLength)
                {
                    var m = message?.Invoke(argument.Value, minLength, maxLength)
                        ?? Messages.StringLengthInRange(argument, minLength, maxLength);

                    throw Fail(new ArgumentException(m, argument.Name));
                }

            return ref argument;
        }

        /// <summary>
        ///     Requires the string argument to have a value that is equal to another string when
        ///     compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="other">The string to compare the argument value to.</param>
        /// <param name="comparison">The rules that specify how the strings will be compared.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not equal to <paramref name="other" /> by the
        ///     comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "geqs")]
        public static ref readonly ArgumentInfo<string> Equal(
            in this ArgumentInfo<string> argument,
            string other,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue && !StringEqualityComparer(comparison).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.Equal(argument, other);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the string argument to have a value that is different than another string
        ///     when compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="other">The string to compare the argument value to.</param>
        /// <param name="comparison">The rules that specify how the strings will be compared.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is equal to <paramref name="other" /> by the
        ///     comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gneqs")]
        public static ref readonly ArgumentInfo<string> NotEqual(
            in this ArgumentInfo<string> argument,
            string other,
            StringComparison comparison,
            Func<string, string> message = null)
        {
            if (argument.HasValue && StringEqualityComparer(comparison).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotEqual(argument, other);
                throw Fail(new ArgumentException(m, argument.Name));
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
        ///     <paramref name="argument" /> value does not start with <paramref name="value" /> when
        ///     a case-sensitive and culture-sensitive comparison is performed.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("String", "gstart")]
        public static ref readonly ArgumentInfo<string> StartsWith(
            in this ArgumentInfo<string> argument,
            string value,
            Func<string, string, string> message = null)
            => ref argument.StartsWith(value, StringComparison.CurrentCulture, message);

        /// <summary>
        ///     Requires the beginning of the string argument to match with the specified string when
        ///     compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="value">The string to search in the beginning of the argument.</param>
        /// <param name="comparison">The rules that specify how the strings will be compared.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value does not start with <paramref name="value" /> when
        ///     compared by the comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gstarts")]
        public static ref readonly ArgumentInfo<string> StartsWith(
            in this ArgumentInfo<string> argument,
            string value,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue && value != null && !argument.Value.StartsWith(value, comparison))
            {
                var m = message?.Invoke(argument.Value, value) ?? Messages.StringStartsWith(argument, value);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the beginning of the string argument to be different than the specified string.
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
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnstart")]
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
        /// <param name="comparison">The rules that specify how the strings will be compared.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value starts with <paramref name="value" /> when
        ///     compared by the comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnstarts")]
        public static ref readonly ArgumentInfo<string> DoesNotStartWith(
            in this ArgumentInfo<string> argument,
            string value,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue && value != null && argument.Value.StartsWith(value, comparison))
            {
                var m = message?.Invoke(argument.Value, value) ?? Messages.StringDoesNotStartWith(argument, value);
                throw Fail(new ArgumentException(m, argument.Name));
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
        ///     <paramref name="argument" /> value does not end with <paramref name="value" /> when a
        ///     case-sensitive and culture-sensitive comparison is performed.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("String", "gend")]
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
        /// <param name="comparison">The rules that specify how the strings will be compared.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value does not end with <paramref name="value" /> when
        ///     compared by the comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gends")]
        public static ref readonly ArgumentInfo<string> EndsWith(
            in this ArgumentInfo<string> argument,
            string value,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue && value != null && !argument.Value.EndsWith(value, comparison))
            {
                var m = message?.Invoke(argument.Value, value) ?? Messages.StringEndsWith(argument, value);
                throw Fail(new ArgumentException(m, argument.Name));
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
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnend")]
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
        /// <param name="comparison">The rules that specify how the strings will be compared.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value ends with <paramref name="value" /> when compared
        ///     by the comparison rules specified in <paramref name="comparison" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnends")]
        public static ref readonly ArgumentInfo<string> DoesNotEndWith(
            in this ArgumentInfo<string> argument,
            string value,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue && value != null && argument.Value.EndsWith(value, comparison))
            {
                var m = message?.Invoke(argument.Value, value) ?? Messages.StringDoesNotEndWith(argument, value);
                throw Fail(new ArgumentException(m, argument.Name));
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
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gm")]
        public static ref readonly ArgumentInfo<string> Matches(
            in this ArgumentInfo<string> argument,
            [RegexPattern] string pattern,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue && pattern != null)
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

                    throw Fail(new ArgumentException(m, argument.Name));
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
        ///     expression timed out, or it could not find a match in <paramref name="argument" />'s value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="matchTimeout" /> is negative, zero, or greater than approximately 24 days.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gmt")]
        public static ref readonly ArgumentInfo<string> Matches(
            in this ArgumentInfo<string> argument,
            [RegexPattern] string pattern,
            TimeSpan matchTimeout,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue && pattern != null)
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

                    throw Fail(new ArgumentException(m, argument.Name, ex));
                }

                if (!matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringMatches(argument, pattern);

                    throw Fail(new ArgumentException(m, argument.Name));
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
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gm")]
        public static ref readonly ArgumentInfo<string> Matches(
            in this ArgumentInfo<string> argument,
            Regex regex,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue && regex != null)
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

                    throw Fail(new ArgumentException(m, argument.Name, ex));
                }

                if (!matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringMatches(argument, regex.ToString());

                    throw Fail(new ArgumentException(m, argument.Name));
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
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnm")]
        public static ref readonly ArgumentInfo<string> DoesNotMatch(
            in this ArgumentInfo<string> argument,
            [RegexPattern] string pattern,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue && pattern != null)
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

                    throw Fail(new ArgumentException(m, argument.Name));
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
        ///     <paramref name="matchTimeout" /> is negative, zero, or greater than approximately 24 days.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnmt")]
        public static ref readonly ArgumentInfo<string> DoesNotMatch(
            in this ArgumentInfo<string> argument,
            [RegexPattern] string pattern,
            TimeSpan matchTimeout,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue && pattern != null)
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

                    throw Fail(new ArgumentException(m, argument.Name, ex));
                }

                if (matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringDoesNotMatch(argument, pattern);

                    throw Fail(new ArgumentException(m, argument.Name));
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
        ///     <paramref name="regex" /> found a match in <paramref name="argument" />'s value or it
        ///     timed out before the evaluation is completed.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("String", "gnm")]
        public static ref readonly ArgumentInfo<string> DoesNotMatch(
            in this ArgumentInfo<string> argument,
            Regex regex,
            Func<string, bool, string> message = null)
        {
            if (argument.HasValue && regex != null)
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

                    throw Fail(new ArgumentException(m, argument.Name, ex));
                }

                if (matches)
                {
                    var m = message?.Invoke(argument.Value, false)
                        ?? Messages.StringDoesNotMatch(argument, regex.ToString());

                    throw Fail(new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Returns the string comparer that is most relevant to the specified enumeration value.
        /// </summary>
        /// <param name="comparison">
        ///     An enumeration value that specifies how to compare two strings.
        /// </param>
        /// <returns>A string equality comparer.</returns>
        private static IEqualityComparer<string> StringEqualityComparer(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.CurrentCulture => StringComparer.CurrentCulture,
                StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
#if !NETSTANDARD1_0
                StringComparison.InvariantCulture => StringComparer.InvariantCulture,
                StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
#endif
                StringComparison.Ordinal => StringComparer.Ordinal,
                StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
                _ => EqualityComparer<string>.Default,
            };
        }
    }
}
