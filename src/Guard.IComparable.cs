#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="IComparable" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the argument to have a value that is equal to or greater than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="minValue">The minimum value that the argument is allowed to have.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is less than <paramref name="minValue" /> and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is less than <paramref name="minValue" /> and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gmin")]
        public static ref readonly ArgumentInfo<T> Min<T>(
            in this ArgumentInfo<T> argument, in T minValue, Func<T, T, string>? message = null)
            where T : IComparable<T>?
        {
            if (argument.HasValue && Comparer<T>.Default.Compare(argument.Value, minValue) < 0)
            {
                var m = message?.Invoke(argument.Value, minValue) ?? Messages.Min(argument, minValue);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is <c>null</c>, equal to the
        ///     specified value, or greater than the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="minValue">The minimum value that the argument is allowed to have.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is less than <paramref name="minValue" /> and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is less than <paramref name="minValue" /> and the
        ///     argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gmin")]
        public static ref readonly ArgumentInfo<T?> Min<T>(
            in this ArgumentInfo<T?> argument, in T minValue, Func<T, T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, minValue) < 0)
            {
                var m = message?.Invoke(value, minValue) ?? Messages.Min(argument, minValue);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is greater than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="other">The value that the argument must be greater than.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is less than or equal to
        ///     <paramref name="other" /> and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is less than or equal to
        ///     <paramref name="other" /> and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "ggt")]
        public static ref readonly ArgumentInfo<T> GreaterThan<T>(
            in this ArgumentInfo<T> argument, in T other, Func<T, T, string>? message = null)
            where T : IComparable<T>?
        {
            if (argument.HasValue && Comparer<T>.Default.Compare(argument.Value, other) <= 0)
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.GreaterThan(argument, other);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is <c>null</c> or greater than
        ///     the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="other">The value that the argument must be greater than.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is less than or equal to
        ///     <paramref name="other" /> and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is less than or equal to
        ///     <paramref name="other" /> and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "ggt")]
        public static ref readonly ArgumentInfo<T?> GreaterThan<T>(
            in this ArgumentInfo<T?> argument, in T other, Func<T, T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, other) <= 0)
            {
                var m = message?.Invoke(value, other) ?? Messages.GreaterThan(argument, other);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is equal to or lower than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="maxValue">The maximum value that the argument is allowed to have.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is greater than <paramref name="maxValue" /> and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is greater than <paramref name="maxValue" /> and
        ///     the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gmax")]
        public static ref readonly ArgumentInfo<T> Max<T>(
            in this ArgumentInfo<T> argument, in T maxValue, Func<T, T, string>? message = null)
            where T : IComparable<T>?
        {
            if (argument.HasValue && Comparer<T>.Default.Compare(argument.Value, maxValue) > 0)
            {
                var m = message?.Invoke(argument.Value, maxValue) ?? Messages.Max(argument, maxValue);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is <c>null</c>, equal to the
        ///     specified value, or lower than the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="maxValue">The maximum value that the argument is allowed to have.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is greater than <paramref name="maxValue" /> and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is greater than <paramref name="maxValue" /> and
        ///     the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gmax")]
        public static ref readonly ArgumentInfo<T?> Max<T>(
            in this ArgumentInfo<T?> argument, in T maxValue, Func<T, T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, maxValue) > 0)
            {
                var m = message?.Invoke(value, maxValue) ?? Messages.Max(argument, maxValue);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="other">The value that the argument must be less than.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is greater than or equal to
        ///     <paramref name="other" /> and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is greater than or equal to
        ///     <paramref name="other" /> and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "glt")]
        public static ref readonly ArgumentInfo<T> LessThan<T>(
            in this ArgumentInfo<T> argument, in T other, Func<T, T, string>? message = null)
            where T : IComparable<T>?
        {
            if (argument.HasValue && Comparer<T>.Default.Compare(argument.Value, other) >= 0)
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.LessThan(argument, other);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is <c>null</c> or less than
        ///     the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="other">The value that the argument must be less than.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is greater than or equal to
        ///     <paramref name="other" /> and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is greater than or equal to
        ///     <paramref name="other" /> and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "glt")]
        public static ref readonly ArgumentInfo<T?> LessThan<T>(
            in this ArgumentInfo<T?> argument, in T other, Func<T, T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, other) >= 0)
            {
                var m = message?.Invoke(value, other) ?? Messages.GreaterThan(argument, other);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is between the specified minimum and
        ///     maximum values.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="minValue">The minimum value that the argument is allowed to have.</param>
        /// <param name="maxValue">The maximum value that the argument is allowed to have.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not between <paramref name="minValue" /> and
        ///     <paramref name="maxValue" />. And the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not between <paramref name="minValue" /> and
        ///     <paramref name="maxValue" />. And the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gr")]
        public static ref readonly ArgumentInfo<T> InRange<T>(
            in this ArgumentInfo<T> argument, in T minValue, in T maxValue, Func<T, T, T, string>? message = null)
            where T : IComparable<T>?
        {
            if (argument.HasValue)
            {
                var comparer = Comparer<T>.Default;
                if (comparer.Compare(argument.Value, minValue) < 0 ||
                    comparer.Compare(argument.Value, maxValue) > 0)
                {
                    var m = message?.Invoke(argument.Value, minValue, maxValue) ?? Messages.InRange(argument, minValue, maxValue);
                    throw Fail(!argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                        : new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is either between the specified
        ///     minimum and maximum values or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="minValue">The minimum value that the argument is allowed to have.</param>
        /// <param name="maxValue">The maximum value that the argument is allowed to have.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not <c>null</c> and is not between
        ///     <paramref name="minValue" /> and <paramref name="maxValue" />. And the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and is not between
        ///     <paramref name="minValue" /> and <paramref name="maxValue" />. And the argument is
        ///     modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gr")]
        public static ref readonly ArgumentInfo<T?> InRange<T>(
            in this ArgumentInfo<T?> argument, in T minValue, in T maxValue, Func<T, T, T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value))
            {
                var comparer = Comparer<T>.Default;
                if (comparer.Compare(value, minValue) < 0 || comparer.Compare(value, maxValue) > 0)
                {
                    var m = message?.Invoke(value, minValue, maxValue) ?? Messages.InRange(argument, minValue, maxValue);
                    throw Fail(!argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                        : new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to be zero.</summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not zero and the argument is not modified since
        ///     it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not zero and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gz")]
        public static ref readonly ArgumentInfo<T> Zero<T>(
            in this ArgumentInfo<T> argument, Func<T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) != 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Zero(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is either zero or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not zero and the argument is not modified since
        ///     it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not zero and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gz")]
        public static ref readonly ArgumentInfo<T?> Zero<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, default) != 0)
            {
                var m = message?.Invoke(value) ?? Messages.Zero(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have a value that is not zero.</summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero and the argument is modified after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gnz")]
        public static ref readonly ArgumentInfo<T> NotZero<T>(
            in this ArgumentInfo<T> argument, string? message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) == 0)
            {
                var m = message ?? Messages.NotZero(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that either is not zero or is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero and the argument is not modified since it
        ///     is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero and the argument is modified after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gnz")]
        public static ref readonly ArgumentInfo<T?> NotZero<T>(
            in this ArgumentInfo<T?> argument, string? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, default) == 0)
            {
                var m = message ?? Messages.NotZero(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have a value that is greater than zero.</summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero or less, and the argument is not modified
        ///     since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero or less, and the argument is modified
        ///     after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gpos")]
        public static ref readonly ArgumentInfo<T> Positive<T>(
            in this ArgumentInfo<T> argument, Func<T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) <= 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Positive(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is either greater than zero or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero or less, and the argument is not modified
        ///     since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero or less, and the argument is modified
        ///     after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gpos")]
        public static ref readonly ArgumentInfo<T?> Positive<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, default) <= 0)
            {
                var m = message?.Invoke(value) ?? Messages.Positive(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have a value that is not greater than zero.</summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is greater than zero, and the argument is not
        ///     modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is greater than zero, and the argument is modified
        ///     after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gnpos")]
        public static ref readonly ArgumentInfo<T> NotPositive<T>(
            in this ArgumentInfo<T> argument, Func<T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) > 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotPositive(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is not greater than zero.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is greater than zero, and the argument is not
        ///     modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is greater than zero, and the argument is modified
        ///     after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gnpos")]
        public static ref readonly ArgumentInfo<T?> NotPositive<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, default) > 0)
            {
                var m = message?.Invoke(value) ?? Messages.NotPositive(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have a value that is less than zero.</summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero or greater, and the argument is not
        ///     modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero or greater, and the argument is modified
        ///     after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gneg")]
        public static ref readonly ArgumentInfo<T> Negative<T>(
            in this ArgumentInfo<T> argument, Func<T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) >= 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Negative(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is either less than zero or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero or greater, and the argument is not
        ///     modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero or greater, and the argument is modified
        ///     after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gneg")]
        public static ref readonly ArgumentInfo<T?> Negative<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, default) >= 0)
            {
                var m = message?.Invoke(value) ?? Messages.Negative(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have a value that is not less than zero.</summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is less than zero, and the argument is not
        ///     modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is less than zero, and the argument is modified
        ///     after its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gnneg")]
        public static ref readonly ArgumentInfo<T> NotNegative<T>(
            in this ArgumentInfo<T> argument, Func<T, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) < 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotNegative(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is not less than zero.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is less than zero, and the argument is not
        ///     modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is less than zero, and the argument is modified
        ///     after its initialization.
        /// </exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Comparison", "gnneg")]
        public static ref readonly ArgumentInfo<T?> NotNegative<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string>? message = null)
            where T : struct, IComparable<T>
        {
            if (argument.TryGetValue(out var value) && Comparer<T>.Default.Compare(value, default) < 0)
            {
                var m = message?.Invoke(value) ?? Messages.NotNegative(argument);
                throw Fail(!argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : value as object, m)
                     : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }
    }
}
