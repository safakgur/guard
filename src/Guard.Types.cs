namespace Dawn
{
    using System;

    /// <content>Provides type preconditions.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the argument to have a value that is
        ///     an instance of the specified generic type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type that the argument's value should be an instance of.
        /// </typeparam>
        /// <param name="argument">The object argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not an
        ///     instance of type <typeparamref name="T" />.
        /// </exception>
        public static ArgumentInfo<T> Type<T>(
            in this ArgumentInfo<object> argument, Func<object, string> message = null)
        {
            if (argument.HasValue() && argument.Value.GetType() != typeof(T))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Type(argument, typeof(T));
                throw new ArgumentException(m, argument.Name);
            }

            return new ArgumentInfo<T>((T)argument.Value, argument.Name, argument.Modified);
        }

        /// <summary>
        ///     Requires the argument to have a value that is
        ///     not an instance of the specified generic type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type that the argument's value should not be an instance of.
        /// </typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is an
        ///     instance of type <typeparamref name="T" />.
        /// </exception>
        public static ArgumentInfo<object> NotType<T>(
            in this ArgumentInfo<object> argument, Func<T, string> message = null)
        {
            if (argument.HasValue() && argument.Value.GetType() == typeof(T))
            {
                var m = message?.Invoke((T)argument.Value) ?? Messages.NotType(argument, typeof(T));
                throw new ArgumentException(m, argument.Name);
            }

            return argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that
        ///     is an instance of the specified type.
        /// </summary>
        /// <param name="argument">The object argument.</param>
        /// <param name="type">
        ///     The type that the argument's value should be an instance of.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not an instance
        ///     of the type represented by <paramref name="type" />.
        /// </exception>
        public static ArgumentInfo<object> Type(
            in this ArgumentInfo<object> argument, Type type, Func<object, Type, string> message = null)
        {
            if (argument.HasValue() && argument.Value.GetType() != type && type != null)
            {
                var m = message?.Invoke(argument.Value, type) ?? Messages.Type(argument, type);
                throw new ArgumentException(m, argument.Name);
            }

            return new ArgumentInfo<object>(argument.Value, argument.Name, argument.Modified);
        }

        /// <summary>
        ///     Requires the argument to have a value that
        ///     is not an instance of the specified type.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="type">
        ///     The type that the argument's value should not be an instance of.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is an instance
        ///     of the type represented by <paramref name="type" />.
        /// </exception>
        public static ArgumentInfo<object> NotType(
            in this ArgumentInfo<object> argument, Type type, Func<object, Type, string> message = null)
        {
            if (argument.HasValue() && argument.Value.GetType() == type)
            {
                var m = message?.Invoke(argument.Value, type) ?? Messages.NotType(argument, type);
                throw new ArgumentException(m, argument.Name);
            }

            return argument;
        }
    }
}
