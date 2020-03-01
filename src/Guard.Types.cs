#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides type preconditions.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires the argument to have a value that is an instance of the specified generic type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type that the argument's value should be an instance of.
        /// </typeparam>
        /// <param name="argument">The object argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not an instance of type <typeparamref name="T" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Type")]
        public static ArgumentInfo<T> Type<T>(
            in this ArgumentInfo<object> argument, Func<object, string>? message = null)
        {
            if (!TypeInfo<T>.CanBeInitializedFrom(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Type(argument, typeof(T));
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return new ArgumentInfo<T>(
                (T)argument.Value, argument.Name, argument.Modified, argument.Secure);
        }

        /// <summary>
        ///     Requires the argument to have a value that is not an instance of the specified
        ///     generic type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type that the argument's value should not be an instance of.
        /// </typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is an instance of type <typeparamref name="T" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Type")]
        public static ref readonly ArgumentInfo<object> NotType<T>(
            in this ArgumentInfo<object> argument, Func<T, string>? message = null)
        {
            if (argument!.TryGetValue(out var value) && TypeInfo<T>.CanBeInitializedFrom(value))
            {
                var m = message?.Invoke((T)value) ?? Messages.NotType(argument, typeof(T));
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is an instance of the specified type.
        /// </summary>
        /// <param name="argument">The object argument.</param>
        /// <param name="type">The type that the argument's value should be an instance of.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not an instance of the type represented by <paramref name="type" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Type")]
        public static ref readonly ArgumentInfo<object> Type(
            in this ArgumentInfo<object> argument, Type type, Func<object, Type, string>? message = null)
        {
            if (argument!.TryGetValue(out var value) && !TypeInfo.CanBeConvertedTo(value, type))
            {
                var m = message?.Invoke(value, type) ?? Messages.Type(argument, type);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a value that is not an instance of the specified type.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="type">
        ///     The type that the argument's value should not be an instance of.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is an instance of the type represented by <paramref name="type" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Type")]
        public static ref readonly ArgumentInfo<object> NotType(
            in this ArgumentInfo<object> argument, Type type, Func<object, Type, string>? message = null)
        {
            if (argument!.TryGetValue(out var value) && TypeInfo.CanBeConvertedTo(value, type))
            {
                var m = message?.Invoke(value, type) ?? Messages.NotType(argument, type);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <content>Contains the compatibility preconditions.</content>
        public partial struct ArgumentInfo<T>
        {
            /// <summary>
            ///     Requires the argument to have a value that can be assigned to an instance of the
            ///     specified type.
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should be assignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> cannot be assigned to type <typeparamref name="TTarget" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            [GuardFunction("Type", "gcomp")]
            public ArgumentInfo<T> Compatible<TTarget>(Func<T, string>? message = null)
            {
                if (!HasValue || Value is TTarget value)
                    return this;

                var m = message?.Invoke(Value) ?? Messages.Compatible<T, TTarget>(this);
                throw Fail(new ArgumentException(m, Name));
            }

            /// <summary>
            ///     Requires the argument to have a value that cannot be assigned to an instance of
            ///     the specified type.
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should not be assignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> can be assigned to type <typeparamref name="TTarget" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            [GuardFunction("Type", "gncomp")]
            public ArgumentInfo<T> NotCompatible<TTarget>(Func<TTarget, string>? message = null)
            {
                if (HasValue && Value is TTarget value)
                {
                    var m = message?.Invoke(value) ?? Messages.NotCompatible<T, TTarget>(this);
                    throw Fail(new ArgumentException(m, Name));
                }

                return this;
            }

            /// <summary>
            ///     <para>
            ///         Requires the argument to have a value that can be assigned to an instance of
            ///         the specified type.
            ///     </para>
            ///     <para>The return value will be a new argument of type <typeparamref name="TTarget" />.</para>
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should be assignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>A new <see cref="ArgumentInfo{TTarget}" />.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> cannot be assigned to type <typeparamref name="TTarget" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            [GuardFunction("Type", "gcast")]
            public ArgumentInfo<TTarget> Cast<TTarget>(Func<T, string>? message = null)
            {
                if (Value is TTarget value)
                    return new ArgumentInfo<TTarget>(value, this.Name, this.Modified, this.Secure);

                var m = message?.Invoke(Value) ?? Messages.Compatible<T, TTarget>(this);
                throw Fail(new ArgumentException(m, Name));
            }
        }

        /// <summary>Provides cached utilities for <typeparamref name="T" />.</summary>
        /// <typeparam name="T">The type.</typeparam>
        private static class TypeInfo<T>
        {
            /// <summary>
            ///     A function that determines whether the specified object can be converted to type <typeparamref name="T" />.
            /// </summary>
            public static readonly Func<object, bool> CanBeInitializedFrom = InitCanBeInitializedFrom();

            /// <summary>Initializes <see cref="CanBeInitializedFrom" />.</summary>
            /// <returns>
            ///     A function that determines whether the specified object can be converted to type <typeparamref name="T" />.
            /// </returns>
            private static Func<object, bool> InitCanBeInitializedFrom()
            {
                var targetType = typeof(T);
                var isValueType = IsValueType(targetType);
                var isNullable = !isValueType || targetType.IsGenericType(typeof(Nullable<>));

                Type? type = targetType;
                var resultChain = new HashSet<Type>();
                do
                {
                    resultChain.Add(type);
                    type = type.GetBaseType();
                }
                while (type != null);
                if (isValueType && isNullable)
                    resultChain.Add(targetType.GetGenericArguments()[0]);

                resultChain.TrimExcess();
                return obj => obj is null ? isNullable : resultChain.Contains(obj.GetType());
            }
        }

        /// <summary>Provides non-generic, cached utilities for specified types.</summary>
        private static class TypeInfo
        {
            /// <summary>The locker that synchronizes access to <see cref="CanBeConvertedToDict" />.</summary>
            private static readonly ReaderWriterLockSlim Locker
                = new ReaderWriterLockSlim();

            /// <summary>
            ///     The functions that determine whether a specified object can be converted to the
            ///     type that the function is mapped to.
            /// </summary>
            private static readonly Dictionary<Type, Func<object, bool>> CanBeConvertedToDict
                = new Dictionary<Type, Func<object, bool>>();

            /// <summary>
            ///     Determines whether an object can be converted to an instance of the specified type.
            /// </summary>
            /// <param name="obj">The object to check.</param>
            /// <param name="targetType">The type to check.</param>
            /// <returns>
            ///     <c>true</c>, if <paramref name="obj" /> can be converted to an instance of <paramref name="targetType" />.
            /// </returns>
            /// <remarks>Calls <see cref="TypeInfo{T}.CanBeInitializedFrom" />.</remarks>
            public static bool CanBeConvertedTo(object obj, Type targetType)
            {
                Func<object, bool>? func;

                Locker.EnterUpgradeableReadLock();
                try
                {
                    if (!CanBeConvertedToDict.TryGetValue(targetType, out func))
                    {
                        var t = typeof(TypeInfo<>).MakeGenericType(targetType);
                        var f = Expression.Field(null, t.GetField(nameof(TypeInfo<object>.CanBeInitializedFrom)));
                        var o = Expression.Parameter(typeof(object), "obj");
                        var c = Expression.Invoke(f, o);
                        var l = Expression.Lambda<Func<object, bool>>(c, o);
                        func = l.Compile();

                        Locker.EnterWriteLock();
                        try
                        {
                            CanBeConvertedToDict[targetType] = func;
                        }
                        finally
                        {
                            Locker.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    Locker.ExitUpgradeableReadLock();
                }

                return func(obj);
            }
        }
    }
}
