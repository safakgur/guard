namespace Dawn
{
    using System;
    using System.Diagnostics;
    using JetBrains.Annotations;

    /// <content>Provides preconditions for <see cref="double" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is "not a
        ///     number" ( <see cref="double.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="double.NaN" />, and the argument
        ///     is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="double.NaN" />, and the argument
        ///     is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> NaN(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (!double.IsNaN(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NaN(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     <c>null</c> or "not a number" ( <see cref="double.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.NaN" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.NaN" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> NaN(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (!double.IsNaN(value))
                {
                    var m = message?.Invoke(value) ?? Messages.NaN(argument);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     "not a number" ( <see cref="double.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="double.NaN" />, and the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.NaN" />, and the argument is
        ///     modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> NotNaN(
            in this ArgumentInfo<double> argument, string message = null)
        {
            if (double.IsNaN(argument.Value))
            {
                var m = message ?? Messages.NotNaN(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     "not a number" ( <see cref="double.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="double.NaN" />, and the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.NaN" />, and the argument is
        ///     modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> NotNaN(
            in this ArgumentInfo<double?> argument, string message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (double.IsNaN(value))
                {
                    var m = message ?? Messages.NotNaN(argument);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     positive infinity ( <see cref="double.PositiveInfinity" />) or negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <see cref="double.PositiveInfinity" />
        ///     nor <see cref="double.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <see cref="double.PositiveInfinity" />
        ///     nor <see cref="double.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> Infinity(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (!double.IsInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Infinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     <c>null</c>, positive infinity ( <see cref="double.PositiveInfinity" />) or negative
        ///     infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <c>null</c>, not
        ///     <see cref="double.PositiveInfinity" /> and not
        ///     <see cref="double.NegativeInfinity" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>, not
        ///     <see cref="double.PositiveInfinity" /> and not
        ///     <see cref="double.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> Infinity(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (!double.IsInfinity(value))
                {
                    var m = message?.Invoke(value) ?? Messages.Infinity(argument);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is neither
        ///     positive infinity ( <see cref="double.PositiveInfinity" />) nor negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [Obsolete("Use the NotInfinity overload that accepts the message as a `Func<double, string>`.")]
        public static ref readonly ArgumentInfo<double> NotInfinity(
            in this ArgumentInfo<double> argument, string message)
        {
            if (double.IsInfinity(argument.Value))
            {
                var m = message ?? Messages.NotInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is neither
        ///     positive infinity ( <see cref="double.PositiveInfinity" />) nor negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> NotInfinity(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (double.IsInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is neither
        ///     positive infinity ( <see cref="double.PositiveInfinity" />) nor negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [Obsolete("Use the NotInfinity overload that accepts the message as a `Func<double?, string>`.")]
        public static ref readonly ArgumentInfo<double?> NotInfinity(
            in this ArgumentInfo<double?> argument, string message)
        {
            if (argument.NotNull(out var a) && double.IsInfinity(a.Value))
            {
                var m = message ?? Messages.NotInfinity(a);
                throw !a.Modified
                    ? new ArgumentOutOfRangeException(a.Name, argument.Secure ? null : a.Value as object, m)
                    : new ArgumentException(m, a.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is neither
        ///     positive infinity ( <see cref="double.PositiveInfinity" />) nor negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="double.PositiveInfinity" />
        ///     or <see cref="double.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> NotInfinity(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (double.IsInfinity(value))
                {
                    var m = message?.Invoke(value) ?? Messages.NotInfinity(argument);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     positive infinity ( <see cref="double.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="double.PositiveInfinity" />, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="double.PositiveInfinity" />, and
        ///     the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> PositiveInfinity(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (!double.IsPositiveInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.PositiveInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     <c>null</c> or positive infinity ( <see cref="double.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.PositiveInfinity" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.PositiveInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> PositiveInfinity(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (!double.IsPositiveInfinity(value))
                {
                    var m = message?.Invoke(value) ?? Messages.PositiveInfinity(argument);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     positive infinity ( <see cref="double.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="double.PositiveInfinity" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.PositiveInfinity" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> NotPositiveInfinity(
            in this ArgumentInfo<double> argument, string message = null)
        {
            if (double.IsPositiveInfinity(argument.Value))
            {
                var m = message ?? Messages.NotPositiveInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     positive infinity ( <see cref="double.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="double.PositiveInfinity" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.PositiveInfinity" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> NotPositiveInfinity(
            in this ArgumentInfo<double?> argument, string message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (double.IsPositiveInfinity(value))
                {
                    var m = message ?? Messages.NotPositiveInfinity(argument);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is
        ///     negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="double.NegativeInfinity" />, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="double.NegativeInfinity" />, and
        ///     the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> NegativeInfinity(
            in this ArgumentInfo<double> argument, Func<double, string> message = null)
        {
            if (!double.IsNegativeInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NegativeInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     <c>null</c> or negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.NegativeInfinity" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="double.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> NegativeInfinity(
            in this ArgumentInfo<double?> argument, Func<double?, string> message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (!double.IsNegativeInfinity(value))
                {
                    var m = message?.Invoke(value) ?? Messages.NegativeInfinity(argument);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="double.NegativeInfinity" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.NegativeInfinity" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> NotNegativeInfinity(
            in this ArgumentInfo<double> argument, string message = null)
        {
            if (double.IsNegativeInfinity(argument.Value))
            {
                var m = message ?? Messages.NotNegativeInfinity(argument);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     negative infinity ( <see cref="double.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="double.NegativeInfinity" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="double.NegativeInfinity" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> NotNegativeInfinity(
            in this ArgumentInfo<double?> argument, string message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                if (double.IsNegativeInfinity(value))
                {
                    var m = message ?? Messages.NotNegativeInfinity(argument);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is within
        ///     the specified accuracy of the specified value.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="delta">The required accuracy.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is different from <paramref name="other" /> by
        ///     more than <paramref name="delta" />
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> Equal(
            in this ArgumentInfo<double> argument,
            double other,
            double delta,
            Func<double, double, string> message = null)
        {
            var diff = Math.Abs(argument.Value - other);
            if (diff > delta)
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.Equal(argument, other, delta);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is either
        ///     <c>null</c>, or within the specified accuracy of the specified value.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="delta">The required accuracy.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is different from <paramref name="other" /> by
        ///     more than <paramref name="delta" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> Equal(
            in this ArgumentInfo<double?> argument,
            double other,
            double delta,
            Func<double, double, string> message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                var diff = Math.Abs(value - other);
                if (diff > delta)
                {
                    var m = message?.Invoke(value, other) ?? Messages.Equal(argument, other, delta);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that is not
        ///     within the specified accuracy of the specified value.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="delta">The required inaccuracy.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is equal to <paramref name="other" /> or different
        ///     from it by less than <paramref name="delta" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double> NotEqual(
            in this ArgumentInfo<double> argument,
            double other,
            double delta,
            Func<double, double, string> message = null)
        {
            var diff = Math.Abs(argument.Value - other);
            if (diff <= delta)
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.NotEqual(argument, other, delta);
                throw !argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the double-precision floating-point argument to have a value that either is
        ///     <c>null</c> or is not within the specified accuracy of the specified value.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="delta">The required inaccuracy.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is equal to <paramref name="other" /> or different
        ///     from it by less than <paramref name="delta" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        public static ref readonly ArgumentInfo<double?> NotEqual(
            in this ArgumentInfo<double?> argument,
            double other,
            double delta,
            Func<double, double, string> message = null)
        {
            if (argument.HasValue())
            {
                var value = argument.Value.Value;
                var diff = Math.Abs(value - other);
                if (diff <= delta)
                {
                    var m = message?.Invoke(value, other) ?? Messages.NotEqual(argument, other, delta);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }
    }
}
