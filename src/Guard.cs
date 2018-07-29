namespace Dawn
{
    using System;
    using System.Linq.Expressions;

    /// <summary>Validates argument preconditions.</summary>
    public static partial class Guard
    {
        #region Methods

        /// <summary>
        ///     Returns an object that can be used to assert preconditions
        ///     for the specified method argument.
        /// </summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        /// <param name="e">An expression that specifies a method argument.</param>
        /// <returns>An object used for asserting preconditions.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="e" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="e" /> is not a <see cref="MemberExpression" />.
        /// </exception>
        public static ArgumentInfo<T> Argument<T>(Expression<Func<T>> e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            return e.Body is MemberExpression m
                ? Argument(e.Compile()(), m.Member.Name)
                : throw new ArgumentException("A member expression is expected.", nameof(e));
        }

        /// <summary>
        ///     Returns an object that can be used to assert preconditions
        ///     for the method argument with the specified name and value.
        /// </summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        /// <param name="value">The value of the method argument.</param>
        /// <param name="name">
        ///     <para>
        ///         The name of the method argument. Use the <c>nameof</c>
        ///         operator (<c>Nameof</c> in Visual Basic) where possible.
        ///     </para>
        ///     <para>
        ///         It is highly recommended you don't left this value
        ///         <c>null</c> so the arguments violating the
        ///         preconditions can be easily identified.
        ///     </para>
        /// </param>
        /// <returns>An object used for asserting preconditions.</returns>
        public static ArgumentInfo<T> Argument<T>(T value, string name = null)
            => new ArgumentInfo<T>(value, name);

        #endregion Methods

        #region Structs

        /// <summary>Represents a method argument.</summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        public readonly ref struct ArgumentInfo<T>
        {
            #region Fields

            /// <summary>
            ///     A function that determines whether a specified instance
            ///     of type <typeparamref name="T" /> is not <c>null</c>.
            /// </summary>
            private static readonly IsNotNull hasValue = InitHasValue();

            /// <summary>
            ///     The default name for the arguments
            ///     of type <typeparamref name="T" />.
            /// </summary>
            private static readonly string defaultName = $"The {typeof(T)} argument";

            /// <summary>The argument name.</summary>
            private readonly string name;

            #endregion Fields

            #region Constructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="ArgumentInfo{T} "/> struct.
            /// </summary>
            /// <param name="value">The value of the method argument.</param>
            /// <param name="name">The name of the method argument.</param>
            /// <param name="modified">
            ///     Whether the original method argument is modified
            ///     before the initialization of this instance.
            /// </param>
            public ArgumentInfo(T value, string name, bool modified = false)
            {
                this.Value = value;
                this.name = name;
                this.Modified = modified;
            }

            #endregion Constructors

            #region Properties

            /// <summary>Gets the argument value.</summary>
            public T Value { get; }

            /// <summary>Gets the argument name.</summary>
            public string Name => this.name ?? defaultName;

            /// <summary>
            ///     Gets a value indicating whether the the original
            ///     method argument is modified before the
            ///     initialization of this instance.
            /// </summary>
            public bool Modified { get; }

            #endregion Properties

            #region Operators

            /// <summary>Gets the value of an argument.</summary>
            /// <param name="argument">The argument whose value to return.</param>
            /// <returns><see cref="Value" />.</returns>
            public static implicit operator T(ArgumentInfo<T> argument)
                => argument.Value;

            #endregion Operators

            #region Delegates

            /// <summary>
            ///     A delegate that checks whether an instance of
            ///     <typeparamref name="T" /> is not <c>null</c>.
            /// </summary>
            /// <param name="value">The value to check against <c>null</c>.</param>
            /// <returns>
            ///     <c>true</c>, if <paramref name="value" /> is
            ///     not <c>null</c>; otherwise, <c>false</c>.
            /// </returns>
            private delegate bool IsNotNull(in T value);

            #endregion Delegates

            #region Methods

            /// <summary>Requires the argument to satisfy a condition.</summary>
            /// <param name="predicate">The function to test the the argument value.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that
            ///     will be thrown if the precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <paramref name="predicate" /> returned <c>false</c>
            ///     when supplied the <see cref="Value" />.
            /// </exception>
            public ArgumentInfo<T> Require(Func<T, bool> predicate, Func<T, string> message = null)
                => this.Require<ArgumentException>(predicate, message);

            /// <summary>
            ///     Requires the argument to satisfy a condition and throws the
            ///     specified type of exception if the condition is not met.
            /// </summary>
            /// <typeparam name="TException">
            ///     The type of the exception to throw if the argument
            ///     does not satisfy the specified condition.
            /// </typeparam>
            /// <param name="predicate">The function to test the the argument value.</param>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that
            ///     will be thrown if the precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="Exception">
            ///     <paramref name="predicate" /> returned <c>false</c>
            ///     when supplied the <see cref="Value" />.
            ///     The exception thrown is an instance of
            ///     type <typeparamref name="TException" />.
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
            ///     Requires the argument to have a value that can be
            ///     assigned to an instance of the specified type.
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should be assignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that
            ///     will be thrown if the precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> cannot be assigned
            ///     to type <typeparamref name="TTarget" />.
            /// </exception>
            public ArgumentInfo<T> Compatible<TTarget>(Func<T, string> message = null)
            {
                if (!this.HasValue() || this.Value is TTarget value)
                    return this;

                var m = message?.Invoke(this.Value) ?? Messages.Compatible<T, TTarget>(this);
                throw new ArgumentException(m, this.Name);
            }

            /// <summary>
            ///     Requires the argument to have a value that cannot
            ///     be assigned to an instance of the specified type.
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should be unassignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that
            ///     will be thrown if the precondition is not satisfied.
            /// </param>
            /// <returns>The current argument.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> can be assigned
            ///     to type <typeparamref name="TTarget" />.
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
            ///         Requires the argument to have a value that can be
            ///         assigned to an instance of the specified type.
            ///     </para>
            ///     <para>
            ///         The return value will be a new argument
            ///         of type <typeparamref name="TTarget" />.
            ///     </para>
            /// </summary>
            /// <typeparam name="TTarget">
            ///     The type that the argument's value should be assignable to.
            /// </typeparam>
            /// <param name="message">
            ///     The factory to initialize the message of the exception that
            ///     will be thrown if the precondition is not satisfied.
            /// </param>
            /// <returns>A new <see cref="ArgumentInfo{TTarget}" />.</returns>
            /// <exception cref="ArgumentException">
            ///     <see cref="Value" /> cannot be assigned
            ///     to type <typeparamref name="TTarget" />.
            /// </exception>
            public ArgumentInfo<TTarget> Cast<TTarget>(Func<T, string> message = null)
            {
                if (this.Value is TTarget value)
                    return new ArgumentInfo<TTarget>(value, this.Name, this.Modified);

                var m = message?.Invoke(this.Value) ?? Messages.Compatible<T, TTarget>(this);
                throw new ArgumentException(m, this.Name);
            }

            /// <summary>Determines whether the argument value is not <c>null</c>.</summary>
            /// <returns>
            ///     <c>true</c>, if <see cref="Value" /> is
            ///     not <c>null</c>; otherwise, <c>false</c>.
            /// </returns>
            public bool HasValue() => hasValue(this.Value);

            /// <summary>Determines whether the argument value is <c>null</c>.</summary>
            /// <returns>
            ///     <c>true</c>, if <see cref="Value" /> is
            ///     <c>null</c>; otherwise, <c>false</c>.
            /// </returns>
            public bool IsNull() => !hasValue(this.Value);

            /// <summary>Initializes <see cref="hasValue" />.</summary>
            /// <returns>
            ///     A function that determines whether a specified instance
            ///     of type <typeparamref name="T" /> is not <c>null</c>.
            /// </returns>
            private static IsNotNull InitHasValue()
            {
                var type = typeof(T);
                if (!type.IsValueType())
                    return (in T v) => v != null;

                if (type.IsGenericType(typeof(Nullable<>)))
                {
                    var value = Expression.Parameter(type.MakeByRefType(), "value");
                    var get = type.GetPropertyGetter("HasValue");
                    var call = Expression.Call(value, get);
                    var lambda = Expression.Lambda<IsNotNull>(call, value);
                    return lambda.Compile();
                }

                return (in T v) => true;
            }

            #endregion Methods
        }

        #endregion Structs

        #region Classes

        /// <summary>
        ///     Initializes exceptions of type <typeparamref name="T" />
        ///     for failed preconditions.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        private static class Exception<T>
            where T : Exception
        {
            /// <summary>
            ///     Initializes an exception of type <typeparamref name="T" />
            ///     using the specified parameter name and error message.
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

        #endregion Classes
    }
}
