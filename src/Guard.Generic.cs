namespace Dawn
{
    using System;
    using System.Linq.Expressions;

    /// <content>Provides generic preconditions.</content>
    public static partial class Guard
    {
        /// <content>Contains the preconditions with generic type arguments that cannot be inferred.</content>
        public readonly partial struct ArgumentInfo<T>
        {
            /// <summary>Requires the argument to satisfy a condition.</summary>
            /// <param name="predicate">The function to test the argument value.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if
            ///     the precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <paramref name="predicate" /> returned <c>false</c> when supplied the
            ///     <see cref="Value" />.
            /// </exception>
            public ArgumentInfo<T> Require(Func<T, bool> predicate, Func<T, string> message = null)
                => this.Require<ArgumentException>(predicate, message);

            /// <summary>
            ///     Requires the argument to satisfy a condition and throws the specified type of
            ///     exception if the condition is not met.
            /// </summary>
            /// <typeparam name="TException">
            ///     The type of the exception to throw if the argument does not satisfy the
            ///     specified condition.
            /// </typeparam>
            /// <param name="predicate">The function to test the argument value.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if
            ///     the precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="Exception">
            ///     <paramref name="predicate" /> returned <c>false</c> when supplied the
            ///     <see cref="Value" />. The exception thrown is an instance of type
            ///     <typeparamref name="TException" />.
            /// </exception>
            public ArgumentInfo<T> Require<TException>(
                Func<T, bool> predicate, Func<T, string> message = null)
                where TException : Exception
            {
                if (predicate?.Invoke(this.Value) == false)
                {
                    var m = message?.Invoke(this.Value) ?? Messages.Require(this);
                    throw Exception<TException>.Factory(this.Name, m);
                }

                return this;
            }

            /// <summary>
            ///     Requires the argument to have a value that can be assigned to an instance of the
            ///     specified type.
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should be assignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if
            ///     the precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> cannot be assigned to type <typeparamref name="TTarget" />.
            /// </exception>
            public ArgumentInfo<T> Compatible<TTarget>(Func<T, string> message = null)
            {
                if (!this.HasValue() || this.Value is TTarget value)
                    return this;

                var m = message?.Invoke(this.Value) ?? Messages.Compatible<T, TTarget>(this);
                throw new ArgumentException(m, this.Name);
            }

            /// <summary>
            ///     Requires the argument to have a value that cannot be assigned to an instance of
            ///     the specified type.
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should not be assignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if
            ///     the precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> can be assigned to type <typeparamref name="TTarget" />.
            /// </exception>
            public ArgumentInfo<T> NotCompatible<TTarget>(Func<TTarget, string> message = null)
            {
                if (this.HasValue() && this.Value is TTarget value)
                {
                    var m = message?.Invoke(value) ?? Messages.NotCompatible<T, TTarget>(this);
                    throw new ArgumentException(m, this.Name);
                }

                return this;
            }

            /// <summary>
            ///     <para>
            ///         Requires the argument to have a value that can be assigned to an instance of
            ///         the specified type.
            ///     </para>
            ///     <para>
            ///         The return value will be a new argument of type <typeparamref name="TTarget" />.
            ///     </para>
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should be assignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that will be thrown if
            ///     the precondition is not satisfied.
            /// </param>
            /// <returns>A new <see cref="ArgumentInfo{TTarget}" />.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> cannot be assigned to type <typeparamref name="TTarget" />.
            /// </exception>
            public ArgumentInfo<TTarget> Cast<TTarget>(Func<T, string> message = null)
            {
                if (this.Value is TTarget value)
                    return new ArgumentInfo<TTarget>(value, this.Name, this.Modified);

                var m = message?.Invoke(this.Value) ?? Messages.Compatible<T, TTarget>(this);
                throw new ArgumentException(m, this.Name);
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
                        new ArgumentException(message, paramName) as T;

                if (type.IsSubclassOf(typeof(ArgumentException)))
                {
                    if (TryGetFactoryWithTwoStringArguments(out var two))
                        return two;

                    if (TryGetFactoryWithOneStringArgument(out var one))
                        return (paramName, message) => one(paramName);
                }
                else if (TryGetFactoryWithOneStringArgument(out var one))
                {
                    return (paramName, message) => one(message);
                }

                if (TryGetFactoryWithNoArguments(out var none))
                    return (paramName, message) => none();
                else
                    return (paramName, message) =>
                    {
                        var x = new ArgumentException(message, paramName);
                        throw new ArgumentException($"An instance of {type} cannot be initialized.", x);
                    };

                bool TryGetFactoryWithTwoStringArguments(out Func<string, string, T> factory)
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

                bool TryGetFactoryWithOneStringArgument(out Func<string, T> factory)
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

                bool TryGetFactoryWithNoArguments(out Func<T> factory)
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
}
