using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="System.Enum" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>Exposes the enum preconditions.</summary>
        /// <typeparam name="T">Type of the enum argument.</typeparam>
        /// <param name="argument">The enum argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     argument type is not an enum.
        /// </param>
        /// <returns>A new <see cref="EnumArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="T" /> is not an enum.</exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [Obsolete("Use the enum preconditions directly, e.g. `arg.Defined()` instead of `arg.Enum().Defined()`.")]
        public static EnumArgumentInfo<T> Enum<T>(
            in this ArgumentInfo<T> argument, Func<T, string> message = null)
            where T : struct, IComparable, IFormattable
        {
            if (!typeof(T).IsEnum())
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Enum(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return new EnumArgumentInfo<T>(argument);
        }

        /// <summary>Exposes the nullable enum preconditions.</summary>
        /// <typeparam name="T">Type of the enum argument.</typeparam>
        /// <param name="argument">The enum argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     argument type is not an enum.
        /// </param>
        /// <returns>A new <see cref="NullableEnumArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="T" /> is not an enum.</exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [Obsolete("Use the enum preconditions directly, e.g. `arg.Defined()` instead of `arg.Enum().Defined()`.")]
        public static NullableEnumArgumentInfo<T> Enum<T>(
            in this ArgumentInfo<T?> argument, Func<T?, string> message = null)
            where T : struct, IComparable, IFormattable
        {
            if (!typeof(T).IsEnum())
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Enum(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return new NullableEnumArgumentInfo<T>(argument);
        }

        /// <summary>Represents a method argument with an enumeration value.</summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        [DebuggerStepThrough]
        [Obsolete("Use the enum preconditions directly, e.g. `arg.Defined()` instead of `arg.Enum().Defined()`.")]
        public readonly struct EnumArgumentInfo<T>
            where T : struct, IComparable, IFormattable
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="EnumArgumentInfo{T}" /> struct.
            /// </summary>
            /// <param name="argument">The original argument.</param>
            internal EnumArgumentInfo(ArgumentInfo<T> argument)
                => Argument = argument;

            /// <summary>Gets the original argument.</summary>
            public ArgumentInfo<T> Argument { get; }

            /// <summary>Gets the value of an enum argument.</summary>
            /// <param name="argument">The argument whose value to return.</param>
            /// <returns>The value of <see cref="Argument" />.</returns>
            public static implicit operator T(EnumArgumentInfo<T> argument)
                => argument.Argument.Value;

            /// <summary>
            ///     Requires the enum argument to be a defined member of the enum type <typeparamref name="T" />.
            /// </summary>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is not a defined member of the enum type <typeparamref name="T" />.
            /// </exception>
            /// <returns>The current instance.</returns>
            [AssertionMethod]
            [DebuggerStepThrough]
            public EnumArgumentInfo<T> Defined(Func<T, string> message = null)
            {
                if (!EnumInfo<T>.Values.Contains(Argument.Value))
                {
                    var m = message?.Invoke(Argument.Value) ?? Messages.EnumDefined(Argument);
                    throw new ArgumentException(m, Argument.Name);
                }

                return this;
            }

            /// <summary>Requires the enum argument to have none of its bits set.</summary>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value has one or more of its bits set.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public EnumArgumentInfo<T> None(Func<T, string> message = null)
            {
                if (!EqualityComparer<T>.Default.Equals(Argument.Value, default))
                {
                    var m = message?.Invoke(Argument.Value) ?? Messages.EnumNone(Argument);
                    throw new ArgumentException(m, Argument.Name);
                }

                return this;
            }

            /// <summary>Requires the enum argument to have at least one of its bits set.</summary>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value has none of its bits set.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public EnumArgumentInfo<T> NotNone(Func<T, string> message = null)
            {
                if (EqualityComparer<T>.Default.Equals(Argument.Value, default))
                {
                    var m = message?.Invoke(Argument.Value) ?? Messages.EnumNotNone(Argument);
                    throw new ArgumentException(m, Argument.Name);
                }

                return this;
            }

            /// <summary>Requires the enum argument to have the specified value.</summary>
            /// <param name="other">The value to compare the argument value to.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is different than <paramref name="other" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public EnumArgumentInfo<T> Equal(T other, Func<T, T, string> message = null)
            {
                if (!EqualityComparer<T>.Default.Equals(Argument.Value, other))
                {
                    var m = message?.Invoke(Argument.Value, other) ?? Messages.Equal(Argument, other);
                    throw new ArgumentException(m, Argument.Name);
                }

                return this;
            }

            /// <summary>
            ///     Requires the enum argument to have a value is different than the specified value.
            /// </summary>
            /// <param name="other">The value to compare the argument value to.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is equal to <paramref name="other" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public EnumArgumentInfo<T> NotEqual(T other, Func<T, string> message = null)
            {
                if (EqualityComparer<T>.Default.Equals(Argument.Value, other))
                {
                    var m = message?.Invoke(Argument.Value) ?? Messages.NotEqual(Argument, other);
                    throw new ArgumentException(m, Argument.Name);
                }

                return this;
            }

            /// <summary>Requires the enum argument to have the specified flag bits set.</summary>
            /// <param name="flag">The flags to check.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> does not have the bits specified in
            ///     <paramref name="flag" /> set.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public EnumArgumentInfo<T> HasFlag(T flag, Func<T, T, string> message = null)
            {
                if (!EnumInfo<T>.HasFlag(Argument.Value, flag))
                {
                    var m = message?.Invoke(Argument.Value, flag) ?? Messages.EnumHasFlag(Argument, flag);
                    throw new ArgumentException(m, Argument.Name);
                }

                return this;
            }

            /// <summary>Requires the enum argument to have the specified flag bits unset.</summary>
            /// <param name="flag">The flags to check.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> have the bits specified in <paramref name="flag" /> set.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public EnumArgumentInfo<T> DoesNotHaveFlag(T flag, Func<T, T, string> message = null)
            {
                if (EnumInfo<T>.HasFlag(Argument.Value, flag))
                {
                    var m = message?.Invoke(Argument.Value, flag) ?? Messages.EnumDoesNotHaveFlag(Argument, flag);
                    throw new ArgumentException(m, Argument.Name);
                }

                return this;
            }
        }

        /// <summary>Represents a method argument with a nullable enumeration value.</summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        [Obsolete("Use the enum preconditions directly, e.g. `arg.Defined()` instead of `arg.Enum().Defined()`.")]
        public readonly struct NullableEnumArgumentInfo<T>
            where T : struct, IComparable, IFormattable
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="NullableEnumArgumentInfo{T}" /> struct.
            /// </summary>
            /// <param name="argument">The original argument.</param>
            internal NullableEnumArgumentInfo(ArgumentInfo<T?> argument)
                => Argument = argument;

            /// <summary>Gets the original argument.</summary>
            public ArgumentInfo<T?> Argument { get; }

            /// <summary>Gets the value of a nullable enum argument.</summary>
            /// <param name="argument">The argument whose value to return.</param>
            /// <returns>The value of <see cref="Argument" />.</returns>
            public static implicit operator T?(NullableEnumArgumentInfo<T> argument)
                => argument.Argument.Value;

            /// <summary>Requires the nullable enum argument to be <c>null</c>.</summary>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns><see cref="Argument" />.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is not <c>null</c>.
            /// </exception>
            public ArgumentInfo<T?> Null(Func<T, string> message = null)
            {
                if (Argument.HasValue())
                {
                    var m = message?.Invoke(Argument.Value.Value) ?? Messages.Null(Argument);
                    throw new ArgumentException(m, Argument.Name);
                }

                return Argument;
            }

            /// <summary>Requires the nullable enum argument to be not <c>null</c>.</summary>
            /// <param name="message">
            ///     The message of the exception that will be thrown if the precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentNullException">
            ///     <see cref="Argument" /> value is <c>null</c> and the argument is not modified
            ///     since it is initialized.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is <c>null</c> and the argument is modified after
            ///     its initialization.
            /// </exception>
            public EnumArgumentInfo<T> NotNull(string message = null)
#pragma warning disable CS0618 // Type or member is obsolete
                => Argument.NotNull(message).Enum();

#pragma warning restore CS0618 // Type or member is obsolete

            /// <summary>
            ///     Requires the nullable enum argument to be either a defined member of the enum
            ///     type <typeparamref name="T" /> or <c>null</c>.
            /// </summary>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is not <c>null</c> and is not a defined member of
            ///     the enum type <typeparamref name="T" />.
            /// </exception>
            /// <returns>The current instance.</returns>
            [AssertionMethod]
            [DebuggerStepThrough]
            public NullableEnumArgumentInfo<T> Defined(Func<T, string> message = null)
            {
                if (NotNull(out var a))
                    a.Defined(message);

                return this;
            }

            /// <summary>
            ///     Requires the nullable enum argument to either have none of its bits set or be <c>null</c>.
            /// </summary>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is not <c>null</c> and has one or more of its bits set.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public NullableEnumArgumentInfo<T> None(Func<T, string> message = null)
            {
                if (NotNull(out var a))
                    a.None(message);

                return this;
            }

            /// <summary>Requires the enum argument to have at least one of its bits set.</summary>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is not <c>null</c> and has none of its bits set.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public NullableEnumArgumentInfo<T> NotNone(Func<T, string> message = null)
            {
                if (NotNull(out var a))
                    a.NotNone(message);

                return this;
            }

            /// <summary>
            ///     Requires the nullable enum argument to either have the specified value or be <c>null</c>.
            /// </summary>
            /// <param name="other">The value to compare the argument value to.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is not <c>null</c> and is different than <paramref name="other" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public NullableEnumArgumentInfo<T> Equal(T other, Func<T, T, string> message = null)
            {
                if (NotNull(out var a))
                    a.Equal(other, message);

                return this;
            }

            /// <summary>
            ///     Requires the nullable enum argument to have a value that is either different than
            ///     the specified value or <c>null</c>.
            /// </summary>
            /// <param name="other">The value to compare the argument value to.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> value is not <c>null</c> and is equal to <paramref name="other" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public NullableEnumArgumentInfo<T> NotEqual(T other, Func<T, string> message = null)
            {
                if (NotNull(out var a))
                    a.NotEqual(other, message);

                return this;
            }

            /// <summary>
            ///     Requires the nullable enum argument to either have the specified flag bits set or
            ///     be <c>null</c>.
            /// </summary>
            /// <param name="flag">The flags to check.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> is not <c>null</c> and does not have the bits specified
            ///     in <paramref name="flag" /> set.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public NullableEnumArgumentInfo<T> HasFlag(T flag, Func<T, T, string> message = null)
            {
                if (NotNull(out var a))
                    a.HasFlag(flag, message);

                return this;
            }

            /// <summary>
            ///     Requires the nullable enum argument to either have the specified flag bits unset
            ///     or be <c>null</c>.
            /// </summary>
            /// <param name="flag">The flags to check.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current instance.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Argument" /> is not <c>null</c> and have the bits specified in
            ///     <paramref name="flag" /> set.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            public NullableEnumArgumentInfo<T> DoesNotHaveFlag(T flag, Func<T, T, string> message = null)
            {
                if (NotNull(out var a))
                    a.DoesNotHaveFlag(flag, message);

                return this;
            }

            /// <summary>
            ///     Initializes an <see cref="EnumArgumentInfo{T}" /> if the <see cref="Argument" />
            ///     is not <c>null</c>. A return value indicates whether the new argument is created.
            /// </summary>
            /// <param name="result">
            ///     The new enum argument, if the <see cref="Argument" /> is not <c>null</c>;
            ///     otherwise, the uninitialized argument.
            /// </param>
            /// <returns>
            ///     <c>true</c>, if the <see cref="Argument" /> is not <c>null</c>; otherwise, <c>false</c>.
            /// </returns>
            private bool NotNull(out EnumArgumentInfo<T> result)
            {
                if (Argument.Value.HasValue)
                {
                    result = new EnumArgumentInfo<T>(
                        new ArgumentInfo<T>(
                            Argument.Value.Value,
                            Argument.Name,
                            Argument.Modified,
                            Argument.Secure));

                    return true;
                }

                result = default;
                return false;
            }
        }
    }
}
