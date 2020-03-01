#nullable enable

using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="DateTime" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the date-time argument to have its <see cref="DateTime.Kind" /> specified,
        ///     i.e. not to have it as <see cref="DateTimeKind.Unspecified" />.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" />'s <see cref="DateTime.Kind" /> property is
        ///     <see cref="DateTimeKind.Unspecified" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("DateTime", "gtks")]
        public static ref readonly ArgumentInfo<DateTime> KindSpecified(
            in this ArgumentInfo<DateTime> argument, Func<DateTime, string>? message = null)
        {
            if (argument.Value.Kind == DateTimeKind.Unspecified)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.KindSpecified(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the date-time argument to either be <c>null</c> or have its
        ///     <see cref="DateTime.Kind" /> specified, i.e. not to have it as
        ///     <see cref="DateTimeKind.Unspecified" />.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" />'s <see cref="DateTime.Kind" /> property is
        ///     <see cref="DateTimeKind.Unspecified" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("DateTime", "gtks")]
        public static ref readonly ArgumentInfo<DateTime?> KindSpecified(
            in this ArgumentInfo<DateTime?> argument, Func<DateTime, string>? message = null)
        {
            if (argument.TryGetValue(out var value) && value.Kind == DateTimeKind.Unspecified)
            {
                var m = message?.Invoke(value) ?? Messages.KindSpecified(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the date-time argument not to have its <see cref="DateTime.Kind" />
        ///     specified, i.e. to have it as <see cref="DateTimeKind.Unspecified" />.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" />'s <see cref="DateTime.Kind" /> property is not
        ///     <see cref="DateTimeKind.Unspecified" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("DateTime", "gtku")]
        public static ref readonly ArgumentInfo<DateTime> KindUnspecified(
            in this ArgumentInfo<DateTime> argument, Func<DateTime, string>? message = null)
        {
            if (argument.Value.Kind != DateTimeKind.Unspecified)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.KindUnspecified(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the date-time argument either to be <c>null</c> or not to have its
        ///     <see cref="DateTime.Kind" /> specified, i.e. to have it as
        ///     <see cref="DateTimeKind.Unspecified" />.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" />'s <see cref="DateTime.Kind" /> property is not
        ///     <see cref="DateTimeKind.Unspecified" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("DateTime", "gtku")]
        public static ref readonly ArgumentInfo<DateTime?> KindUnspecified(
            in this ArgumentInfo<DateTime?> argument, Func<DateTime, string>? message = null)
        {
            if (argument.TryGetValue(out var value) && value.Kind != DateTimeKind.Unspecified)
            {
                var m = message?.Invoke(value) ?? Messages.KindUnspecified(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }
    }
}
