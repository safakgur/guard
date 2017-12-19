namespace Dawn
{
    using System;
    using System.Collections.Generic;

    /// <content>Provides preconditions for <see cref="IComparable" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the argument to have a value that is
        ///     equal to or greater than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="minValue">
        ///     The minimum value that the argument is allowed to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is less than
        ///     <paramref name="minValue" /> and the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is less than
        ///     <paramref name="minValue" /> and the argument
        ///     is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T> Min<T>(
            in this ArgumentInfo<T> argument, in T minValue, Func<T, T, string> message = null)
            where T : IComparable<T>
        {
            if (argument.HasValue() && Comparer<T>.Default.Compare(argument.Value, minValue) < 0)
            {
                var m = message?.Invoke(argument.Value, minValue) ?? Messages.Min(argument, minValue);
                throw !argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                     : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is
        ///     <c>null</c>, equal to the specified value, or greater
        ///     than the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="minValue">
        ///     The minimum value that the argument is allowed to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is less than
        ///     <paramref name="minValue" /> and the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is less than
        ///     <paramref name="minValue" /> and the argument
        ///     is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T?> Min<T>(
            in this ArgumentInfo<T?> argument, in T minValue, Func<T, T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (argument.NotNull(out var a))
                a.Min(minValue, message);

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is
        ///     equal to or lower than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="maxValue">
        ///     The maximum value that the argument is allowed to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is greater than
        ///     <paramref name="maxValue" /> and the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is greater than
        ///     <paramref name="maxValue" /> and the argument
        ///     is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T> Max<T>(
            in this ArgumentInfo<T> argument, in T maxValue, Func<T, T, string> message = null)
            where T : IComparable<T>
        {
            if (argument.HasValue() && Comparer<T>.Default.Compare(argument.Value, maxValue) > 0)
            {
                var m = message?.Invoke(argument.Value, maxValue) ?? Messages.Max(argument, maxValue);
                throw !argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                     : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is
        ///     <c>null</c>, equal to the specified value, or lower
        ///     than the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="maxValue">
        ///     The maximum value that the argument is allowed to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is greater than
        ///     <paramref name="maxValue" /> and the argument is
        ///     not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is greater than
        ///     <paramref name="maxValue" /> and the argument
        ///     is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T?> Max<T>(
            in this ArgumentInfo<T?> argument, in T maxValue, Func<T, T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (argument.NotNull(out var a))
                a.Max(maxValue, message);

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is between
        ///     the specified minimum and maximum values.
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
        public static ref readonly ArgumentInfo<T> InRange<T>(
            in this ArgumentInfo<T> argument, in T minValue, in T maxValue, Func<T, T, T, string> message = null)
            where T : IComparable<T>
        {
            if (argument.HasValue())
            {
                var comparer = Comparer<T>.Default;
                if (comparer.Compare(argument.Value, minValue) < 0 ||
                    comparer.Compare(argument.Value, maxValue) > 0)
                {
                    var m = message?.Invoke(argument.Value, minValue, maxValue) ?? Messages.InRange(argument, minValue, maxValue);
                    throw !argument.Modified
                        ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                        : new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is either
        ///     between the specified minimum and maximum values or <c>null</c>.
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
        ///     <paramref name="argument" /> value is not <c>null</c> and is not
        ///     between <paramref name="minValue"/> and <paramref name="maxValue"/>.
        ///     And the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and is not
        ///     between <paramref name="minValue"/> and <paramref name="maxValue"/>.
        ///     And the argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T?> InRange<T>(
            in this ArgumentInfo<T?> argument, in T minValue, in T maxValue, Func<T, T, T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (argument.NotNull(out var a))
                a.InRange(minValue, maxValue, message);

            return ref argument;
        }

        /// <summary>Requires the argument value to be zero.</summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not zero and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not zero and
        ///     the argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T> Zero<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) != 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Zero(argument);
                throw !argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                     : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a
        ///     value that is either zero or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is not zero and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not zero and
        ///     the argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T?> Zero<T>(
            in this ArgumentInfo<T?> argument, Func<T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (argument.NotNull(out var a))
                a.Zero(message);

            return ref argument;
        }

        /// <summary>Requires the argument to have a value that is not zero.</summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero and the
        ///     argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T> NotZero<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) == 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotZero(argument);
                throw !argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                     : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value
        ///     that either is not zero or is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero and the
        ///     argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero and the
        ///     argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T?> NotZero<T>(
            in this ArgumentInfo<T?> argument, Func<T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (argument.NotNull(out var a))
                a.NotZero(message);

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is greater than zero.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero or less, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero or less, and
        ///     the argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T> Positive<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) <= 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Positive(argument);
                throw !argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                     : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that
        ///     is either greater than zero or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero or less, and
        ///     the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero or less, and
        ///     the argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T?> Positive<T>(
            in this ArgumentInfo<T?> argument, Func<T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (argument.NotNull(out var a))
                a.Positive(message);

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is less than zero.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero or greater,
        ///     and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero or greater,
        ///     and the argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T> Negative<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(argument.Value, default) >= 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Negative(argument);
                throw !argument.Modified
                     ? new ArgumentOutOfRangeException(argument.Name, argument.Value, m)
                     : new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value
        ///     that is either less than zero or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the comparable argument.</typeparam>
        /// <param name="argument">The comparable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="argument" /> value is zero or greater,
        ///     and the argument is not modified since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is zero or greater,
        ///     and the argument is modified after its initialization
        /// </exception>
        public static ref readonly ArgumentInfo<T?> Negative<T>(
            in this ArgumentInfo<T?> argument, Func<T, string> message = null)
            where T : struct, IComparable<T>
        {
            if (argument.NotNull(out var a))
                a.Negative(message);

            return ref argument;
        }
    }
}
