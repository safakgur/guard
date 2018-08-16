namespace Dawn
{
    using System;

    /// <content>Nullness preconditions.</content>
    public static partial class Guard
    {
        /// <summary>Requires the argument to be <c>null</c>.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> is not <c>null</c>.
        /// </exception>
        public static ref readonly ArgumentInfo<T> Null<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message = null)
            where T : class
        {
            if (argument.HasValue())
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Null(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>Requires the nullable argument to be <c>null</c>.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> is not <c>null</c>.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" />
        ///     cannot be <c>null</c>, but it is defined as nullable anyway.
        ///     This is because passing a lambda would cause the calls
        ///     to be ambiguous between this method and its overload
        ///     when the message delegate accepts a non-nullable argument.
        /// </remarks>
        public static ref readonly ArgumentInfo<T?> Null<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string> message = null)
            where T : struct
        {
            if (argument.HasValue())
            {
                var m = message?.Invoke(argument.Value.Value) ?? Messages.Null(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>Requires the argument to not be <c>null</c>.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="argument" /> value is <c>null</c> and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <c>null</c> and
        ///     the argument is modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<T> NotNull<T>(
            in this ArgumentInfo<T> argument, string message = null)
            where T : class
        {
            if (!argument.HasValue())
            {
                var m = message ?? Messages.NotNull(argument);
                throw !argument.Modified
                    ? new ArgumentNullException(argument.Name, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>Requires the nullable argument to not be <c>null</c>.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="argument" /> value is <c>null</c> and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <c>null</c> and
        ///     the argument is modified after its initialization.
        /// </exception>
        public static ArgumentInfo<T> NotNull<T>(
            in this ArgumentInfo<T?> argument, string message = null)
            where T : struct
        {
            if (!argument.HasValue())
            {
                var m = message ?? Messages.NotNull(argument);
                throw !argument.Modified
                    ? new ArgumentNullException(argument.Name, m)
                    : new ArgumentException(m, argument.Name);
            }

            return new ArgumentInfo<T>(argument.Value.Value, argument.Name, argument.Modified);
        }

        /// <summary>
        ///     Initializes a new <see cref="ArgumentInfo{T}" />
        ///     if the argument value is not <c>null</c>.
        ///     A return value indicates whether the new argument is created.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="result">
        ///     The new argument, if <paramref name="argument" /> is
        ///     not <c>null</c>; otherwise, the uninitialized argument.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if the <paramref name="argument" />
        ///     is not <c>null</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool NotNull<T>(
            in this ArgumentInfo<T?> argument, out ArgumentInfo<T> result)
            where T : struct
        {
            if (argument.HasValue())
            {
                result = new ArgumentInfo<T>(argument.Value.Value, argument.Name, argument.Modified);
                return true;
            }

            result = default;
            return false;
        }
    }
}
