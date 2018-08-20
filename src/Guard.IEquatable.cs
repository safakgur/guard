namespace Dawn
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <content>Provides preconditions for <see cref="IEquatable{T}" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the argument to have the default
        ///     value of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> does not have the
        ///     default value of type <typeparamref name="T" />.
        /// </exception>
        public static ref readonly ArgumentInfo<T> Default<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message = null)
            where T : struct
        {
            if (!EqualityComparer<T>.Default.Equals(argument.Value, default))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Default(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is either the
        ///     default value of type <typeparamref name="T" /> or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither the default value
        ///     of type <typeparamref name="T" /> nor <c>null</c>.
        /// </exception>
        public static ref readonly ArgumentInfo<T?> Default<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string> message = null)
            where T : struct
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (!EqualityComparer<T>.Default.Equals(value, default))
                {
                    var m = message?.Invoke(value) ?? Messages.Default(argument);
                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is not
        ///     the default value of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> have the default
        ///     value of type <typeparamref name="T" />.
        /// </exception>
        [Obsolete("Use the NotDefault overload that accepts the message as a string.")]
        public static ref readonly ArgumentInfo<T> NotDefault<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message)
            where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(argument.Value, default))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotDefault(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is not
        ///     the default value of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown
        ///     if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> have the default
        ///     value of type <typeparamref name="T" />.
        /// </exception>
        public static ref readonly ArgumentInfo<T> NotDefault<T>(
            in this ArgumentInfo<T> argument, string message = null)
            where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(argument.Value, default))
            {
                var m = message ?? Messages.NotDefault(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is not
        ///     the default value of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown
        ///     if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> have the default
        ///     value of type <typeparamref name="T" />.
        /// </exception>
        public static ref readonly ArgumentInfo<T?> NotDefault<T>(
            in this ArgumentInfo<T?> argument, string message = null)
            where T : struct
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (EqualityComparer<T>.Default.Equals(value, default))
                {
                    var m = message ?? Messages.NotDefault(argument);
                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have the specified value.</summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is
        ///     different than <paramref name="other" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<T> Equal<T>(
            in this ArgumentInfo<T> argument, in T other, Func<T, T, string> message = null)
            => ref argument.Equal(other, null, message);

        /// <summary>Requires the argument to have the specified value.</summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is different than
        ///     <paramref name="other" /> by the comparison made by
        ///     <paramref name="comparer" />.
        /// </exception>
        public static ref readonly ArgumentInfo<T> Equal<T>(
            in this ArgumentInfo<T> argument,
            in T other,
            IEqualityComparer<T> comparer,
            Func<T, T, string> message = null)
        {
            if (argument.HasValue() && !(comparer ?? EqualityComparer<T>.Default).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.Equal(argument, other);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that
        ///     is different than the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref = "ArgumentException" >
        ///     <paramref name="argument" /> value is
        ///     equal to <paramref name="other" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<T> NotEqual<T>(
            in this ArgumentInfo<T> argument, in T other, Func<T, string> message = null)
            => ref argument.NotEqual(other, null, message);

        /// <summary>
        ///     Requires the argument to have a value that
        ///     is different than the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref = "ArgumentException" >
        ///     <paramref name="argument" /> value is equal to
        ///     <paramref name="other" /> by the comparison made by
        ///     <paramref name="comparer" />.
        /// </exception>
        public static ref readonly ArgumentInfo<T> NotEqual<T>(
            in this ArgumentInfo<T> argument,
            in T other,
            IEqualityComparer<T> comparer,
            Func<T, string> message = null)
        {
            if (argument.HasValue() && (comparer ?? EqualityComparer<T>.Default).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotEqual(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }
    }
}
