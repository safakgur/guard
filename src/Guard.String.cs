namespace Dawn
{
    using System;
    using System.Collections.Generic;

    /// <content>Provides preconditions for <see cref="string" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>Requires the argument to have an empty string value.</summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>
        ///     and contains one or more characters.
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
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>
        ///     and does not contain any characters.
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
        ///     Requires the argument to have a string value that
        ///     consists only of white-space characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and
        ///     contains one or more characters that are not white-space.
        /// </exception>
        public static ref readonly ArgumentInfo<string> WhiteSpace(
            in this ArgumentInfo<string> argument, Func<string, string> message = null)
        {
            if (argument.Value != null && !IsWhiteSpace(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.StringWhiteSpace(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that
        ///     does not consist only of white-space characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>
        ///     and contains only of white-space characters.
        /// </exception>
        public static ref readonly ArgumentInfo<string> NotWhiteSpace(
            in this ArgumentInfo<string> argument, Func<string, string> message = null)
        {
            if (argument.Value != null && IsWhiteSpace(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.StringNotWhiteSpace(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a string value that contains
        ///     at least the specified number of characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="minLength">
        ///     The minimum number of characters allowed in the argument value.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and
        ///     contains less than the minimum number of characters.
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
        ///     Requires the argument to have a string value that contains
        ///     no more than the specified number of characters.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="maxLength">
        ///     The maximum number of characters allowed in the argument value.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and
        ///     contains more than the maximum number of characters.
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
        ///     Requires the argument to have a value whose length
        ///     is between the specified minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="minValue">
        ///     The minimum value that the argument is allowed to have.
        /// </param>
        /// <param name="maxValue">
        ///     The maximum value that the argument is allowed to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not between
        ///     <paramref name="minValue"/> and <paramref name="maxValue"/>.
        ///     And the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not between
        ///     <paramref name="minValue"/> and <paramref name="maxValue"/>.
        ///     And the argument is modified after its initialization
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
        ///     Requires the string argument to have a value that is equal
        ///     to another string when compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="other">The string to compare the argument value to.</param>
        /// <param name="comparison">
        ///     The rules that specify how the strings will be compared.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not equal to
        ///     <paramref name="other" /> by the comparison rules
        ///     specified in <paramref name="comparison" />.
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
        ///     Requires the string argument to have a value that is different
        ///     than another string when compared by the specified rules.
        /// </summary>
        /// <param name="argument">The string argument.</param>
        /// <param name="other">The string to compare the argument value to.</param>
        /// <param name="comparison">
        ///     The rules that specify how the strings will be compared.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is equal to
        ///     <paramref name="other" /> by the comparison rules
        ///     specified in <paramref name="comparison" />.
        /// </exception>
        public static ref readonly ArgumentInfo<string> NotEqual(
            in this ArgumentInfo<string> argument,
            string other,
            StringComparison comparison,
            Func<string, string, string> message = null)
        {
            if (argument.HasValue() && StringEqualityComparer(comparison).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.NotEqual(argument, other);
                throw new ArgumentException(m, argument.Name);
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
