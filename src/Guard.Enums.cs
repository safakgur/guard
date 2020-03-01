#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="System.Enum" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the enum argument to have a value that is a defined member of the enum type <typeparamref name="T" />.
        /// </summary>
        /// <param name="argument">The enum argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not a defined member of the enum type <typeparamref name="T" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Enum", "gdef")]
        public static ref readonly ArgumentInfo<T> Defined<T>(
            this in ArgumentInfo<T> argument, Func<T, string>? message = null)
            where T : struct, System.Enum
        {
            if (!EnumInfo<T>.Values.Contains(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.EnumDefined(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the enum argument to have a value that is either a defined member of the
        ///     enum type <typeparamref name="T" /> or <c>null</c>.
        /// </summary>
        /// <param name="argument">The enum argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor a defined member of the
        ///     enum type <typeparamref name="T" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Enum", "gdef")]
        public static ref readonly ArgumentInfo<T?> Defined<T>(
            this in ArgumentInfo<T?> argument, Func<T?, string>? message = null)
            where T : struct, System.Enum
        {
            if (argument.TryGetValue(out var value) && !EnumInfo<T>.Values.Contains(value))
            {
                var m = message?.Invoke(value) ?? Messages.EnumDefined(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the enum argument to have a value that has the specified flag bits set.
        /// </summary>
        /// <param name="argument">The enum argument.</param>
        /// <param name="flag">The flags to check.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value does not have the bits specified in
        ///     <paramref name="flag" /> set.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Enum", "ghf")]
        public static ref readonly ArgumentInfo<T> HasFlag<T>(
            this in ArgumentInfo<T> argument, T flag, Func<T, T, string>? message = null)
            where T : struct, System.Enum
        {
            if (!EnumInfo<T>.HasFlag(argument.Value, flag))
            {
                var m = message?.Invoke(argument.Value, flag) ?? Messages.EnumHasFlag(argument, flag);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the enum argument to have a value that either has the specified flag bits
        ///     set or is <c>null</c>.
        /// </summary>
        /// <param name="argument">The enum argument.</param>
        /// <param name="flag">The flags to check.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and does not have the bits
        ///     specified in <paramref name="flag" /> set.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Enum", "ghf")]
        public static ref readonly ArgumentInfo<T?> HasFlag<T>(
            this in ArgumentInfo<T?> argument, T flag, Func<T, T, string>? message = null)
            where T : struct, System.Enum
        {
            if (argument.TryGetValue(out var value) && !EnumInfo<T>.HasFlag(value, flag))
            {
                var m = message?.Invoke(value, flag) ?? Messages.EnumHasFlag(argument, flag);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the enum argument to have a value that has the specified flag bits unset.
        /// </summary>
        /// <param name="argument">The enum argument.</param>
        /// <param name="flag">The flags to check.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value has one or more of the bits specified in
        ///     <paramref name="flag" /> set.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Enum", "gnhf")]
        public static ref readonly ArgumentInfo<T> DoesNotHaveFlag<T>(
            this in ArgumentInfo<T> argument, T flag, Func<T, T, string>? message = null)
            where T : struct, System.Enum
        {
            if (EnumInfo<T>.HasFlag(argument.Value, flag))
            {
                var m = message?.Invoke(argument.Value, flag) ?? Messages.EnumDoesNotHaveFlag(argument, flag);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the enum argument to have a value that either has the specified flag bits
        ///     unset or is <c>null</c>.
        /// </summary>
        /// <param name="argument">The enum argument.</param>
        /// <param name="flag">The flags to check.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and has one or more of the bits
        ///     specified in <paramref name="flag" /> set.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Enum", "gnhf")]
        public static ref readonly ArgumentInfo<T?> DoesNotHaveFlag<T>(
            this in ArgumentInfo<T?> argument, T flag, Func<T, T, string>? message = null)
            where T : struct, System.Enum
        {
            if (argument.TryGetValue(out var value) && EnumInfo<T>.HasFlag(value, flag))
            {
                var m = message?.Invoke(value, flag) ?? Messages.EnumDoesNotHaveFlag(argument, flag);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Provides a compiled flag comparer and cached values of the enum type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        private static class EnumInfo<T>
            where T : struct
        {
            /// <summary>Checks whether an enum value has the specified flag bits set.</summary>
            public static readonly Func<T, T, bool> HasFlag = InitHasFlag();

            /// <summary>Contains all the enum values defined for type <typeparamref name="T" /></summary>
            public static readonly HashSet<T> Values
                = new HashSet<T>((System.Enum.GetValues(typeof(T)) as IEnumerable<T>)!);

            /// <summary>Initializes <see cref="HasFlag" />.</summary>
            /// <returns>
            ///     A function that checks whether an enum value has the specified flag bits set.
            /// </returns>
            private static Func<T, T, bool> InitHasFlag()
            {
                var enumType = typeof(T);
                var valueType = System.Enum.GetUnderlyingType(enumType);

                var left = Expression.Parameter(enumType, "left");
                var leftValue = Expression.Convert(left, valueType);

                var right = Expression.Parameter(enumType, "right");
                var rightValue = Expression.Convert(right, valueType);

                var and = Expression.And(leftValue, rightValue);
                var equal = Expression.Equal(and, rightValue);
                var lambda = Expression.Lambda<Func<T, T, bool>>(equal, left, right);
                return lambda.Compile();
            }
        }
    }
}
