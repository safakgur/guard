#nullable enable

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Nullability preconditions.</content>
    public static partial class Guard
    {
        /// <summary>Requires the argument to be <c>null</c>.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException"><paramref name="argument" /> is not <c>null</c>.</exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Null", "gn")]
        public static ref readonly ArgumentInfo<T?> Null<T>(
            in this ArgumentInfo<T?> argument, Func<T, string>? message = null)
            where T : class
        {
            if (argument.HasValue())
            {
                var m = message?.Invoke(argument.Value!) ?? Messages.Null(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the nullable argument to be <c>null</c>.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException"><paramref name="argument" /> is not <c>null</c>.</exception>
        /// <remarks>
        ///     The argument value that is passed to <paramref name="message" /> cannot be
        ///     <c>null</c>, but it is defined as nullable anyway. This is because passing a lambda
        ///     would cause the calls to be ambiguous between this method and its overload when the
        ///     message delegate accepts a non-nullable argument.
        /// </remarks>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Null", "gn")]
        public static ref readonly ArgumentInfo<T?> Null<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string>? message = null)
            where T : struct
        {
            if (argument.HasValue())
            {
                Debug.Assert(argument.Value.HasValue, "argument.HasValue");
                var m = message?.Invoke(argument.GetValueOrDefault()) ?? Messages.Null(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument not to be <c>null</c>.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="argument" /> value is <c>null</c> and the argument is not modified
        ///     since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <c>null</c> and the argument is modified after
        ///     its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Null", "gnn")]
        public static ref readonly ArgumentInfo<T> NotNull<T>(
            in this ArgumentInfo<T> argument, string? message = null)
            where T : class
        {
            if (!argument.HasValue())
            {
                var m = message ?? Messages.NotNull(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentNullException(argument.Name, m)
                    : new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the nullable argument not to be <c>null</c>.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="argument" /> value is <c>null</c> and the argument is not modified
        ///     since it is initialized.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is <c>null</c> and the argument is modified after
        ///     its initialization.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Null", "gnn")]
        public static ArgumentInfo<T> NotNull<T>(
            in this ArgumentInfo<T?> argument, string? message = null)
            where T : struct
        {
            if (!argument.HasValue())
            {
                var m = message ?? Messages.NotNull(argument);
                throw Fail(!argument.Modified
                    ? new ArgumentNullException(argument.Name, m)
                    : new ArgumentException(m, argument.Name));
            }

            return new ArgumentInfo<T>(
                argument.GetValueOrDefault(), argument.Name, argument.Modified, argument.Secure);
        }

        /// <summary>Requires at least one of the specified arguments not to be <c>null</c>.</summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <param name="argument1">The first argument.</param>
        /// <param name="argument2">The second argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     None of the specified arguments have value.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Null")]
        public static void NotAllNull<T1, T2>(
            in ArgumentInfo<T1> argument1, in ArgumentInfo<T2> argument2, string? message = null)
        {
            if (!argument1.HasValue() && !argument2.HasValue())
            {
                var m = message ?? Messages.NotAllNull(argument1.Name, argument2.Name);
                throw Fail(new ArgumentNullException($"{argument1.Name}, {argument2.Name}", m));
            }
        }

        /// <summary>Requires at least one of the specified arguments not to be <c>null</c>.</summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <param name="argument1">The first argument.</param>
        /// <param name="argument2">The second argument.</param>
        /// <param name="argument3">The third argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     None of the specified arguments have value.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Null")]
        public static void NotAllNull<T1, T2, T3>(
            in ArgumentInfo<T1> argument1,
            in ArgumentInfo<T2> argument2,
            in ArgumentInfo<T3> argument3,
            string? message = null)
        {
            if (!argument1.HasValue() && !argument2.HasValue() && !argument3.HasValue())
            {
                var m = message ?? Messages.NotAllNull(argument1.Name, argument2.Name, argument3.Name);
                throw Fail(new ArgumentNullException($"{argument1.Name}, {argument2.Name}, {argument3.Name}", m));
            }
        }

        /// <summary>
        ///     Retrieves the value of a nullable argument, or the default value of <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <returns>
        ///     The inner value of the nullable argument's value, if
        ///     <see cref="ArgumentInfo{T}.HasValue" /> returns <c>true</c>; otherwise, the default
        ///     value of <typeparamref name="T" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NonGuard]
        public static T GetValueOrDefault<T>(in this ArgumentInfo<T?> argument)
            where T : struct
            => argument.Value.GetValueOrDefault();
    }
}
