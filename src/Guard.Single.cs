#nullable enable

using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="float" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is "not a
        ///     number" (<see cref="float.NaN" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="float.NaN" />, and the argument
        ///     is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="float.NaN" />, and the argument
        ///     is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gnan")]
        public static ref readonly ArgumentInfo<float> NaN(
            in this ArgumentInfo<float> argument, Func<float, string>? message = null)
        {
            if (!float.IsNaN(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NaN(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
        ///     <c>null</c> or "not a number" ( <see cref="float.NaN" />).
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
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gnan")]
        public static ref readonly ArgumentInfo<float?> NaN(
            in this ArgumentInfo<float?> argument, Func<float?, string>? message = null)
        {
            if (argument.TryGetValue(out var value) && !float.IsNaN(value))
            {
                var m = message?.Invoke(value) ?? Messages.NaN(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     "not a number" ( <see cref="float.NaN" />).
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
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gnnan")]
        public static ref readonly ArgumentInfo<float> NotNaN(
            in this ArgumentInfo<float> argument, string? message = null)
        {
            if (float.IsNaN(argument.Value))
            {
                var m = message ?? Messages.NotNaN(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     "not a number" ( <see cref="float.NaN" />).
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
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gnnan")]
        public static ref readonly ArgumentInfo<float?> NotNaN(
            in this ArgumentInfo<float?> argument, string? message = null)
        {
            if (argument.TryGetValue(out var value) && float.IsNaN(value))
            {
                var m = message ?? Messages.NotNaN(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
        ///     positive infinity ( <see cref="float.PositiveInfinity" />) or negative infinity ( <see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <see cref="float.PositiveInfinity" />
        ///     nor <see cref="float.NegativeInfinity" />, and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <see cref="float.PositiveInfinity" />
        ///     nor <see cref="float.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "ginf")]
        public static ref readonly ArgumentInfo<float> Infinity(
            in this ArgumentInfo<float> argument, Func<float, string>? message = null)
        {
            if (!float.IsInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Infinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     <c>null</c>, positive infinity ( <see cref="float.PositiveInfinity" />) or negative
        ///     infinity ( <see cref="float.NegativeInfinity" />).
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
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "ginf")]
        public static ref readonly ArgumentInfo<float?> Infinity(
            in this ArgumentInfo<float?> argument, Func<float?, string>? message = null)
        {
            if (argument.TryGetValue(out var value) && !float.IsInfinity(value))
            {
                var m = message?.Invoke(value) ?? Messages.Infinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is neither
        ///     positive infinity ( <see cref="float.PositiveInfinity" />) nor negative infinity ( <see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" /> or
        ///     <see cref="float.NegativeInfinity" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" /> or
        ///     <see cref="float.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gninf")]
        public static ref readonly ArgumentInfo<float> NotInfinity(
            in this ArgumentInfo<float> argument, Func<float, string>? message = null)
        {
            if (float.IsInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is neither
        ///     positive infinity ( <see cref="float.PositiveInfinity" />) nor negative infinity ( <see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" /> or
        ///     <see cref="float.NegativeInfinity" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is either <see cref="float.PositiveInfinity" /> or
        ///     <see cref="float.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gninf")]
        public static ref readonly ArgumentInfo<float?> NotInfinity(
            in this ArgumentInfo<float?> argument, Func<float?, string>? message = null)
        {
            if (argument.TryGetValue(out var value) && float.IsInfinity(value))
            {
                var m = message?.Invoke(value) ?? Messages.NotInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     positive infinity ( <see cref="float.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="float.PositiveInfinity" />, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="float.PositiveInfinity" />, and
        ///     the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gposinf")]
        public static ref readonly ArgumentInfo<float> PositiveInfinity(
            in this ArgumentInfo<float> argument, Func<float, string>? message = null)
        {
            if (!float.IsPositiveInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.PositiveInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
        ///     <c>null</c> or positive infinity ( <see cref="float.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.PositiveInfinity" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.PositiveInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gposinf")]
        public static ref readonly ArgumentInfo<float?> PositiveInfinity(
            in this ArgumentInfo<float?> argument, Func<float?, string>? message = null)
        {
            if (argument.TryGetValue(out var value) && !float.IsPositiveInfinity(value))
            {
                var m = message?.Invoke(value) ?? Messages.PositiveInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     positive infinity ( <see cref="float.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.PositiveInfinity" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.PositiveInfinity" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gnposinf")]
        public static ref readonly ArgumentInfo<float> NotPositiveInfinity(
            in this ArgumentInfo<float> argument, string? message = null)
        {
            if (float.IsPositiveInfinity(argument.Value))
            {
                var m = message ?? Messages.NotPositiveInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     positive infinity ( <see cref="float.PositiveInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.PositiveInfinity" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.PositiveInfinity" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gnposinf")]
        public static ref readonly ArgumentInfo<float?> NotPositiveInfinity(
            in this ArgumentInfo<float?> argument, string? message = null)
        {
            if (argument.TryGetValue(out var value) && float.IsPositiveInfinity(value))
            {
                var m = message ?? Messages.NotPositiveInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is
        ///     negative infinity ( <see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <see cref="float.NegativeInfinity" />, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <see cref="float.NegativeInfinity" />, and
        ///     the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gneginf")]
        public static ref readonly ArgumentInfo<float> NegativeInfinity(
            in this ArgumentInfo<float> argument, Func<float, string>? message = null)
        {
            if (!float.IsNegativeInfinity(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NegativeInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
        ///     <c>null</c> or negative infinity ( <see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.NegativeInfinity" />, and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor
        ///     <see cref="float.NegativeInfinity" />, and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gneginf")]
        public static ref readonly ArgumentInfo<float?> NegativeInfinity(
            in this ArgumentInfo<float?> argument, Func<float?, string>? message = null)
        {
            if (argument.TryGetValue(out var value) && !float.IsNegativeInfinity(value))
            {
                var m = message?.Invoke(value) ?? Messages.NegativeInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     negative infinity ( <see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.NegativeInfinity" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.NegativeInfinity" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gnneginf")]
        public static ref readonly ArgumentInfo<float> NotNegativeInfinity(
            in this ArgumentInfo<float> argument, string? message = null)
        {
            if (float.IsNegativeInfinity(argument.Value))
            {
                var m = message ?? Messages.NotNegativeInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
        ///     negative infinity ( <see cref="float.NegativeInfinity" />).
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is <see cref="float.NegativeInfinity" />, and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <see cref="float.NegativeInfinity" />, and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Single", "gnneginf")]
        public static ref readonly ArgumentInfo<float?> NotNegativeInfinity(
            in this ArgumentInfo<float?> argument, string? message = null)
        {
            if (argument.TryGetValue(out var value) && float.IsNegativeInfinity(value))
            {
                var m = message ?? Messages.NotNegativeInfinity(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is within
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
        [GuardFunction("Single", "geqd")]
        public static ref readonly ArgumentInfo<float> Equal(
            in this ArgumentInfo<float> argument,
            float other,
            float delta,
            Func<float, float, string>? message = null)
        {
            var diff = Math.Abs(argument.Value - other);
            if (diff > delta)
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.Equal(argument, other, delta);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is either
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
        [GuardFunction("Single", "geqd")]
        public static ref readonly ArgumentInfo<float?> Equal(
            in this ArgumentInfo<float?> argument,
            float other,
            float delta,
            Func<float, float, string>? message = null)
        {
            if (argument.TryGetValue(out var value))
            {
                var diff = Math.Abs(value - other);
                if (diff > delta)
                {
                    var m = message?.Invoke(value, other) ?? Messages.Equal(argument, other, delta);
                    throw Fail(!argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that is not
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
        [GuardFunction("Single", "gneqd")]
        public static ref readonly ArgumentInfo<float> NotEqual(
            in this ArgumentInfo<float> argument,
            float other,
            float delta,
            Func<float, float, string>? message = null)
        {
            var diff = Math.Abs(argument.Value - other);
            if (diff <= delta)
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.NotEqual(argument, other, delta);
                throw Fail(!argument.Modified
                    ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the single-precision floating-point argument to have a value that either is
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
        [GuardFunction("Single", "gneqd")]
        public static ref readonly ArgumentInfo<float?> NotEqual(
            in this ArgumentInfo<float?> argument,
            float other,
            float delta,
            Func<float, float, string>? message = null)
        {
            if (argument.TryGetValue(out var value))
            {
                var diff = Math.Abs(value - other);
                if (diff <= delta)
                {
                    var m = message?.Invoke(value, other) ?? Messages.NotEqual(argument, other, delta);
                    throw Fail(!argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }
    }
}
