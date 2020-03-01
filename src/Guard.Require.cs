#nullable enable

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides generic preconditions.</content>
    public static partial class Guard
    {
        /// <content>Contains the predicate preconditions.</content>
        public readonly partial struct ArgumentInfo<T>
        {
            /// <summary>Requires the argument to satisfy a condition.</summary>
            /// <param name="condition">Whether the precondition is satisfied.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException"><paramref name="condition" /> is <c>false</c>.</exception>
            [AssertionMethod]
            [ContractAnnotation("condition:false => halt")]
            [DebuggerStepThrough]
            [GuardFunction("Predicate", "greq")]
            public ArgumentInfo<T> Require(bool condition, Func<T, string>? message = null)
                => this.Require<ArgumentException>(condition, message);

            /// <summary>
            ///     Requires the argument to satisfy a condition and throws the specified type of
            ///     exception if the condition is not met.
            /// </summary>
            /// <typeparam name="TException">
            ///     The type of the exception to throw if the argument does not satisfy the specified condition.
            /// </typeparam>
            /// <param name="condition">Whether the precondition is satisfied.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="Exception">
            ///     <paramref name="condition" /> is <c>false</c>. The exception thrown is an
            ///     instance of type <typeparamref name="TException" />.
            /// </exception>
            [AssertionMethod]
            [ContractAnnotation("condition:false => halt")]
            [DebuggerStepThrough]
            [GuardFunction("Predicate", "greqe")]
            public ArgumentInfo<T> Require<TException>(
                bool condition, Func<T, string>? message = null)
                where TException : Exception
            {
                if (HasValue && !condition)
                {
                    var m = message?.Invoke(Value) ?? Messages.Require(this);
                    throw Fail(Exception<TException>.Factory(Name, m));
                }

                return this;
            }

            /// <summary>Requires the argument to satisfy a condition.</summary>
            /// <param name="predicate">The function to test the argument value.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <paramref name="predicate" /> returned <c>false</c> when supplied the <see cref="Value" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            [GuardFunction("Predicate", "greq")]
            public ArgumentInfo<T> Require(Func<T, bool> predicate, Func<T, string>? message = null)
                => this.Require<ArgumentException>(predicate, message);

            /// <summary>
            ///     Requires the argument to satisfy a condition and throws the specified type of
            ///     exception if the condition is not met.
            /// </summary>
            /// <typeparam name="TException">
            ///     The type of the exception to throw if the argument does not satisfy the specified condition.
            /// </typeparam>
            /// <param name="predicate">The function to test the argument value.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if the
            ///     precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="Exception">
            ///     <paramref name="predicate" /> returned <c>false</c> when supplied the
            ///     <see cref="Value" />. The exception thrown is an instance of type <typeparamref name="TException" />.
            /// </exception>
            [AssertionMethod]
            [DebuggerStepThrough]
            [GuardFunction("Predicate", "greqe")]
            public ArgumentInfo<T> Require<TException>(
                Func<T, bool> predicate, Func<T, string>? message = null)
                where TException : Exception
            {
                if (HasValue && predicate?.Invoke(Value) == false)
                {
                    var m = message?.Invoke(Value) ?? Messages.Require(this);
                    throw Fail(Exception<TException>.Factory(Name, m));
                }

                return this;
            }
        }

        /// <summary>
        ///     Initializes exceptions of type <typeparamref name="T" /> for failed preconditions.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        private static class Exception<T>
            where T : Exception
        {
            /// <summary>
            ///     Initializes an exception of type <typeparamref name="T" /> using the specified
            ///     parameter name and error message.
            /// </summary>
            public static readonly Func<string, string, T> Factory = InitFactory();

            /// <summary>Initializes <see cref="Factory" />.</summary>
            /// <returns>A function that initializes exceptions.</returns>
            private static Func<string, string, T> InitFactory()
            {
                var type = typeof(T);
                if (type == typeof(ArgumentException))
                    return (paramName, message) =>
                        (new ArgumentException(message, paramName) as T)!;

                if (type.IsSubclassOf(typeof(ArgumentException)))
                {
                    if (TryGetFactoryWithTwoStringArguments(type, out var two))
                        return two;

                    if (TryGetFactoryWithOneStringArgument(type, out var one))
                        return (paramName, message) => one(paramName);
                }
                else if (TryGetFactoryWithOneStringArgument(type, out var one))
                {
                    return (paramName, message) => one(message);
                }

                if (TryGetFactoryWithNoArguments(type, out var none))
                    return (paramName, message) => none();
                else
                    return (paramName, message) =>
                    {
                        var x = new ArgumentException(message, paramName);
                        throw new ArgumentException($"An instance of {type} cannot be initialized.", x);
                    };
            }

            private static bool TryGetFactoryWithTwoStringArguments(
                Type type, [NotNullWhen(true)] out Func<string, string, T>? factory)
            {
                var ctor = type.GetConstructor(new[] { typeof(string), typeof(string) });
                if (ctor != null)
                {
                    var args = new[]
                    {
                            Expression.Parameter(typeof(string), "arg1"),
                            Expression.Parameter(typeof(string), "arg2")
                        };

                    factory = Expression.Lambda<Func<string, string, T>>(
                        Expression.New(ctor, args), args).Compile();

                    return true;
                }

                factory = null;
                return false;
            }

            private static bool TryGetFactoryWithOneStringArgument(
                Type type, [NotNullWhen(true)] out Func<string, T>? factory)
            {
                var ctor = type.GetConstructor(new[] { typeof(string) });
                if (ctor != null)
                {
                    var arg = Expression.Parameter(typeof(string), "message");
                    factory = Expression.Lambda<Func<string, T>>(
                        Expression.New(ctor, arg), arg).Compile();

                    return true;
                }

                factory = null;
                return false;
            }

            private static bool TryGetFactoryWithNoArguments(
                Type type, [NotNullWhen(true)] out Func<T>? factory)
            {
                var ctor = type.GetConstructor(Array<Type>.Empty);
                if (ctor != null)
                {
                    factory = Expression.Lambda<Func<T>>(Expression.New(ctor)).Compile();
                    return true;
                }

                factory = null;
                return false;
            }
        }
    }
}
