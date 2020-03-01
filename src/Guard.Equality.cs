#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="IEquatable{T}" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>Requires the argument to have the default value of type <typeparamref name="T" />.</summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> does not have the default value of type <typeparamref name="T" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "gd")]
        public static ref readonly ArgumentInfo<T> Default<T>(
            in this ArgumentInfo<T> argument, Func<T, string>? message = null)
            where T : struct
        {
            if (!EqualityComparer<T>.Default.Equals(argument.Value, default))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Default(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is either the default value of
        ///     type <typeparamref name="T" /> or <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither the default value of type
        ///     <typeparamref name="T" /> nor <c>null</c>.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "gd")]
        public static ref readonly ArgumentInfo<T?> Default<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string>? message = null)
            where T : struct
        {
            if (argument.HasValue())
            {
                var value = argument.GetValueOrDefault();
                if (!EqualityComparer<T>.Default.Equals(value, default))
                {
                    var m = message?.Invoke(value) ?? Messages.Default(argument);
                    throw Fail(new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is not the default value of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> have the default value of type <typeparamref name="T" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "gnd")]
        public static ref readonly ArgumentInfo<T> NotDefault<T>(
            in this ArgumentInfo<T> argument, string? message = null)
            where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(argument.Value, default))
            {
                var m = message ?? Messages.NotDefault(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the nullable argument to have a value that is not the default value of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> have the default value of type <typeparamref name="T" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "gnd")]
        public static ref readonly ArgumentInfo<T?> NotDefault<T>(
            in this ArgumentInfo<T?> argument, string? message = null)
            where T : struct
        {
            if (argument.HasValue())
            {
                var value = argument.GetValueOrDefault();
                if (EqualityComparer<T>.Default.Equals(value, default))
                {
                    var m = message ?? Messages.NotDefault(argument);
                    throw Fail(new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have the specified value.</summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is different than <paramref name="other" />.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "geq")]
        public static ref readonly ArgumentInfo<T> Equal<T>(
            in this ArgumentInfo<T> argument, in T other, Func<T, T, string>? message = null)
            => ref argument.Equal(other, null!, message);

        /// <summary>Requires the argument to have the specified value.</summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is different than <paramref name="other" /> by the
        ///     comparison made by <paramref name="comparer" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "geqc")]
        public static ref readonly ArgumentInfo<T> Equal<T>(
            in this ArgumentInfo<T> argument,
            in T other,
            IEqualityComparer<T> comparer,
            Func<T, T, string>? message = null)
        {
            if (argument.HasValue() && !(comparer ?? EqualityComparer<T>.Default).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.Equal(argument, other);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is different than the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is equal to <paramref name="other" />.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "gneq")]
        public static ref readonly ArgumentInfo<T> NotEqual<T>(
            in this ArgumentInfo<T> argument, in T other, Func<T, string>? message = null)
            => ref argument.NotEqual(other, null!, message);

        /// <summary>
        ///     Requires the argument to have a value that is different than the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the equatable argument.</typeparam>
        /// <param name="argument">The equatable argument.</param>
        /// <param name="other">The value to compare the argument value to.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is equal to <paramref name="other" /> by the
        ///     comparison made by <paramref name="comparer" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "gneqc")]
        public static ref readonly ArgumentInfo<T> NotEqual<T>(
            in this ArgumentInfo<T> argument,
            in T other,
            IEqualityComparer<T> comparer,
            Func<T, string>? message = null)
        {
            if (argument.HasValue() && (comparer ?? EqualityComparer<T>.Default).Equals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.NotEqual(argument, other);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have the same reference as the specified object.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="other">The object to compare the argument's reference to.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value has a different reference than different than <paramref name="other" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "gs")]
        public static ref readonly ArgumentInfo<T> Same<T>(
            in this ArgumentInfo<T> argument, object other, Func<T, object, string>? message = null)
            where T : class
        {
            if (argument.HasValue() && !ReferenceEquals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.Same(argument, other);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a different reference than the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="other">The object to compare the argument's reference to.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value has the same reference as <paramref name="other" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Equality", "gns")]
        public static ref readonly ArgumentInfo<T> NotSame<T>(
            in this ArgumentInfo<T> argument, object other, Func<T, object, string>? message = null)
            where T : class
        {
            if (argument.HasValue() && ReferenceEquals(argument.Value, other))
            {
                var m = message?.Invoke(argument.Value, other) ?? Messages.NotSame(argument, other);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }
    }
}
