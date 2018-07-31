namespace Dawn
{
    using System;

    /// <content>Provides preconditions for <see cref="double" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     "not a number" (<see cref="double.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="double.NaN" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double> NaN(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (!double.IsNaN(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NaN(argument);
                throw new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     <c>null</c> or "not a number" (<see cref="double.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.NaN" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double?> NaN(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.NotNull(out var a) && !double.IsNaN(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.NaN(a);
                throw new ArgumentOutOfRangeException(a.Name, a.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     "not a number" (<see cref="double.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.NaN" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double> NotNaN(
            in this ArgumentInfo<double> argument, string message = null)
        {
            if (double.IsNaN(argument.Value))
            {
                var m = message ?? Messages.NotNaN(argument);
                throw new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     "not a number" (<see cref="double.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.NaN" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double?> NotNaN(
            in this ArgumentInfo<double?> argument, string message = null)
        {
            if (argument.NotNull(out var a) && double.IsNaN(a.Value))
            {
                var m = message ?? Messages.NotNaN(a);
                throw new ArgumentOutOfRangeException(a.Name, a.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     positive infinity (<see cref="double.PositiveInfinity" />) or negative infinity
        ///     (<see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <see cref="double.PositiveInfinity" />
        ///     nor <see cref="double.NegativeInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double> Infinity(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (!double.IsInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Infinity(argument);
                throw new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     <c>null</c>, positive infinity (<see cref="double.PositiveInfinity" />) or negative
        ///     infinity (<see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>, not
        ///     <see cref="double.PositiveInfinity" /> and not <see cref="double.NegativeInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double?> Infinity(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.NotNull(out var a) && !double.IsInfinity(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.Infinity(a);
                throw new ArgumentOutOfRangeException(a.Name, a.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     neither positive infinity (<see cref="double.PositiveInfinity" />) nor negative
        ///     infinity (<see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double> NotInfinity(
            in this ArgumentInfo<double> argument, string message = null)
        {
            if (double.IsInfinity(argument.Value))
            {
                var m = message ?? Messages.NotInfinity(argument);
                throw new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     neither positive infinity (<see cref="double.PositiveInfinity" />) nor negative
        ///     infinity (<see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double?> NotInfinity(
            in this ArgumentInfo<double?> argument, string message = null)
        {
            if (argument.NotNull(out var a) && double.IsInfinity(a.Value))
            {
                var m = message ?? Messages.NotInfinity(a);
                throw new ArgumentOutOfRangeException(a.Name, a.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     positive infinity (<see cref="double.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="double.PositiveInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double> PositiveInfinity(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (!double.IsPositiveInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.PositiveInfinity(argument);
                throw new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     <c>null</c> or positive infinity (<see cref="double.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.PositiveInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double?> PositiveInfinity(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.NotNull(out var a) && !double.IsPositiveInfinity(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.PositiveInfinity(a);
                throw new ArgumentOutOfRangeException(a.Name, a.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     positive infinity (<see cref="double.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.PositiveInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double> NotPositiveInfinity(
            in this ArgumentInfo<double> argument, string message = null)
        {
            if (double.IsPositiveInfinity(argument.Value))
            {
                var m = message ?? Messages.NotPositiveInfinity(argument);
                throw new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     positive infinity (<see cref="double.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.PositiveInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double?> NotPositiveInfinity(
            in this ArgumentInfo<double?> argument, string message = null)
        {
            if (argument.NotNull(out var a) && double.IsPositiveInfinity(a.Value))
            {
                var m = message ?? Messages.NotPositiveInfinity(a);
                throw new ArgumentOutOfRangeException(a.Name, a.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     negative infinity (<see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="double.NegativeInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double> NegativeInfinity(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (!double.IsNegativeInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NegativeInfinity(argument);
                throw new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     <c>null</c> or negative infinity (<see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.NegativeInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double?> NegativeInfinity(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.NotNull(out var a) && !double.IsNegativeInfinity(a.Value))
            {
                var m = message?.Invoke(a.Value) ?? Messages.NegativeInfinity(a);
                throw new ArgumentOutOfRangeException(a.Name, a.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     negative infinity (<see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.NegativeInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double> NotNegativeInfinity(
            in this ArgumentInfo<double> argument, string message = null)
        {
            if (double.IsNegativeInfinity(argument.Value))
            {
                var m = message ?? Messages.NotNegativeInfinity(argument);
                throw new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     negative infinity (<see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.NegativeInfinity" />.
        /// </exception>
        public static ref readonly ArgumentInfo<double?> NotNegativeInfinity(
            in this ArgumentInfo<double?> argument, string message = null)
        {
            if (argument.NotNull(out var a) && double.IsNegativeInfinity(a.Value))
            {
                var m = message ?? Messages.NotNegativeInfinity(a);
                throw new ArgumentOutOfRangeException(a.Name, a.Value, m);
            }

            return ref argument;
        }
    }
}
