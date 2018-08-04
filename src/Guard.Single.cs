namespace Dawn
{
    using System;

    /// <content>Provides preconditions for <see cref="float" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     "not a number" (<see cref="float.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="float.NaN" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="float.NaN" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float> NaN(
            in this ArgumentInfo<float> argument, Func<float, string> message = null)
        {
            if (!float.IsNaN(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NaN(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
        ///     <c>null</c> or "not a number" (<see cref="float.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.NaN" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.NaN" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" />
        ///     cannot be <c>null</c>, but it is defined as nullable anyway.
        ///     This is because passing a lambda would cause the calls
        ///     to be ambiguous between this method and its overload
        ///     when the message delegate accepts a non-nullable argument.
        /// </remarks>
        public static ref readonly ArgumentInfo<float?> NaN(
            in this ArgumentInfo<float?> argument, Func<float?, string> message = null)
        {
            if (argument.NotNull(out var a) && !float.IsNaN(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.NaN(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     "not a number" (<see cref="float.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.NaN" />, and the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.NaN" />, and the argument is
        ///     modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float> NotNaN(
            in this ArgumentInfo<float> argument, string message = null)
        {
            if (float.IsNaN(argument.Value))
            {
                var m = message ?? Messages.NotNaN(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     "not a number" (<see cref="float.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.NaN" />, and the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.NaN" />, and the argument is
        ///     modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float?> NotNaN(
            in this ArgumentInfo<float?> argument, string message = null)
        {
            if (argument.NotNull(out var a) && float.IsNaN(a.Value))
            {
                var m = message ?? Messages.NotNaN(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
        ///     positive infinity (<see cref="float.PositiveInfinity" />) or negative infinity
        ///     (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <see cref="float.PositiveInfinity" />
        ///     nor <see cref="float.NegativeInfinity" />, and the argument is not modified since
        ///     it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <see cref="float.PositiveInfinity" />
        ///     nor <see cref="float.NegativeInfinity" />, and the argument is modified after its
        ///     initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float> Infinity(
            in this ArgumentInfo<float> argument, Func<float, string> message = null)
        {
            if (!float.IsInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Infinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     <c>null</c>, positive infinity (<see cref="float.PositiveInfinity" />) or negative
        ///     infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <c>null</c>, not
        ///     <see cref="float.PositiveInfinity" /> and not <see cref="float.NegativeInfinity" />,
        ///     and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>, not
        ///     <see cref="float.PositiveInfinity" /> and not <see cref="float.NegativeInfinity" />,
        ///     and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" />
        ///     cannot be <c>null</c>, but it is defined as nullable anyway.
        ///     This is because passing a lambda would cause the calls
        ///     to be ambiguous between this method and its overload
        ///     when the message delegate accepts a non-nullable argument.
        /// </remarks>
        public static ref readonly ArgumentInfo<float?> Infinity(
            in this ArgumentInfo<float?> argument, Func<float?, string> message = null)
        {
            if (argument.NotNull(out var a) && !float.IsInfinity(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.Infinity(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     neither positive infinity (<see cref="float.PositiveInfinity" />) nor negative
        ///     infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" />
        ///     or <see cref="float.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" />
        ///     or <see cref="float.NegativeInfinity" />, and the argument is modified after its
        ///     initialization.
        /// </exception>
        [Obsolete("Use the NotInfinity overload that accepts the message as a `Func<float, string>`.")]
        public static ref readonly ArgumentInfo<float> NotInfinity(
            in this ArgumentInfo<float> argument, string message)
        {
            if (float.IsInfinity(argument.Value))
            {
                var m = message ?? Messages.NotInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     neither positive infinity (<see cref="float.PositiveInfinity" />) nor negative
        ///     infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" />
        ///     or <see cref="float.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" />
        ///     or <see cref="float.NegativeInfinity" />, and the argument is modified after its
        ///     initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float> NotInfinity(
            in this ArgumentInfo<float> argument, Func<float, string> message = null)
        {
            if (float.IsInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     neither positive infinity (<see cref="float.PositiveInfinity" />) nor negative
        ///     infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" />
        ///     or <see cref="float.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" />
        ///     or <see cref="float.NegativeInfinity" />, and the argument is modified after its
        ///     initialization.
        /// </exception>
        [Obsolete("Use the NotInfinity overload that accepts the message as a `Func<float?, string>`.")]
        public static ref readonly ArgumentInfo<float?> NotInfinity(
            in this ArgumentInfo<float?> argument, string message)
        {
            if (argument.NotNull(out var a) && float.IsInfinity(a.Value))
            {
                var m = message ?? Messages.NotInfinity(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     neither positive infinity (<see cref="float.PositiveInfinity" />) nor negative
        ///     infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" />
        ///     or <see cref="float.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" />
        ///     or <see cref="float.NegativeInfinity" />, and the argument is modified after its
        ///     initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" />
        ///     cannot be <c>null</c>, but it is defined as nullable anyway.
        ///     This is because passing a lambda would cause the calls
        ///     to be ambiguous between this method and its overload
        ///     when the message delegate accepts a non-nullable argument.
        /// </remarks>
        public static ref readonly ArgumentInfo<float?> NotInfinity(
            in this ArgumentInfo<float?> argument, Func<float?, string> message = null)
        {
            if (argument.NotNull(out var a) && float.IsInfinity(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.NotInfinity(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     positive infinity (<see cref="float.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="float.PositiveInfinity" />,
        ///     and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="float.PositiveInfinity" />,
        ///     and the argument is modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float> PositiveInfinity(
            in this ArgumentInfo<float> argument, Func<float, string> message = null)
        {
            if (!float.IsPositiveInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.PositiveInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
        ///     <c>null</c> or positive infinity (<see cref="float.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.PositiveInfinity" />, and the argument is not modified
        ///     since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.PositiveInfinity" />, and the argument is modified
        ///     after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" />
        ///     cannot be <c>null</c>, but it is defined as nullable anyway.
        ///     This is because passing a lambda would cause the calls
        ///     to be ambiguous between this method and its overload
        ///     when the message delegate accepts a non-nullable argument.
        /// </remarks>
        public static ref readonly ArgumentInfo<float?> PositiveInfinity(
            in this ArgumentInfo<float?> argument, Func<float?, string> message = null)
        {
            if (argument.NotNull(out var a) && !float.IsPositiveInfinity(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.PositiveInfinity(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     positive infinity (<see cref="float.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.PositiveInfinity" />, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.PositiveInfinity" />, and
        ///     the argument is modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float> NotPositiveInfinity(
            in this ArgumentInfo<float> argument, string message = null)
        {
            if (float.IsPositiveInfinity(argument.Value))
            {
                var m = message ?? Messages.NotPositiveInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     positive infinity (<see cref="float.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.PositiveInfinity" />, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.PositiveInfinity" />, and
        ///     the argument is modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float?> NotPositiveInfinity(
            in this ArgumentInfo<float?> argument, string message = null)
        {
            if (argument.NotNull(out var a) && float.IsPositiveInfinity(a.Value))
            {
                var m = message ?? Messages.NotPositiveInfinity(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     negative infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="float.NegativeInfinity" />,
        ///     and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="float.NegativeInfinity" />,
        ///     and the argument is modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float> NegativeInfinity(
            in this ArgumentInfo<float> argument, Func<float, string> message = null)
        {
            if (!float.IsNegativeInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NegativeInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
        ///     <c>null</c> or negative infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.NegativeInfinity" />, and the argument is not
        ///     modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.NegativeInfinity" />, and the argument is
        ///     modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" />
        ///     cannot be <c>null</c>, but it is defined as nullable anyway.
        ///     This is because passing a lambda would cause the calls
        ///     to be ambiguous between this method and its overload
        ///     when the message delegate accepts a non-nullable argument.
        /// </remarks>
        public static ref readonly ArgumentInfo<float?> NegativeInfinity(
            in this ArgumentInfo<float?> argument, Func<float?, string> message = null)
        {
            if (argument.NotNull(out var a) && !float.IsNegativeInfinity(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.NegativeInfinity(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     negative infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.NegativeInfinity" />, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.NegativeInfinity" />, and
        ///     the argument is modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float> NotNegativeInfinity(
            in this ArgumentInfo<float> argument, string message = null)
        {
            if (float.IsNegativeInfinity(argument.Value))
            {
                var m = message ?? Messages.NotNegativeInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     negative infinity (<see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.NegativeInfinity" />, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.NegativeInfinity" />, and
        ///     the argument is modified after its initialization.
        /// </exception>
        public static ref readonly ArgumentInfo<float?> NotNegativeInfinity(
            in this ArgumentInfo<float?> argument, string message = null)
        {
            if (argument.NotNull(out var a) && float.IsNegativeInfinity(a.Value))
            {
                var m = message ?? Messages.NotNegativeInfinity(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, a.Value, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }
    }
}
