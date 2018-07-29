namespace Dawn
{
    using System;
    using System.Collections.Generic;

    /// <content>Provides preconditions for <see cref="IEquatable{T}" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the argument to have default
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
            where T : struct, IEquatable<T>
        {
            if (!argument.Value.Equals(default))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Default(argument);
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
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> have the default
        ///     value of type <typeparamref name="T" />.
        /// </exception>
        public static ref readonly ArgumentInfo<T> NotDefault<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message = null)
            where T : struct, IEquatable<T>
        {
            if (argument.Value.Equals(default))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotDefault(argument);
                throw new ArgumentException(m, argument.Name);
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
        public static ref readonly ArgumentInfo<T> Equal<T>(
            in this ArgumentInfo<T> argument, in T other, Func<T, T, string> message = null)
            where T : IEquatable<T>
        {
            if (argument.HasValue() && !EqualityComparer<T>.Default.Equals(argument.Value, other))
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
        public static ref readonly ArgumentInfo<T> NotEqual<T>(
            in this ArgumentInfo<T> argument, in T other, Func<T, string> message = null)
            where T : IEquatable<T>
        {
            if (argument.HasValue() && EqualityComparer<T>.Default.Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotEqual(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }
    }
}
