namespace Dawn
{
    using System;
    using System.Linq.Expressions;

    /// <summary>Validates argument preconditions.</summary>
    /// <content>Contains the argument initialization methods.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Returns an object that can be used to assert preconditions for the specified
        ///     method argument.
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
        ///     Returns an object that can be used to assert preconditions for the method argument
        ///     with the specified name and value.
        /// </summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        /// <param name="value">The value of the method argument.</param>
        /// <param name="name">
        ///     <para>
        ///         The name of the method argument. Use the <c>nameof</c> operator (<c>Nameof</c>
        ///         in Visual Basic) where possible.
        ///     </para>
        ///     <para>
        ///         It is highly recommended you don't left this value <c>null</c> so the arguments
        ///         violating the preconditions can be easily identified.
        ///     </para>
        /// </param>
        /// <returns>An object used for asserting preconditions.</returns>
        public static ArgumentInfo<T> Argument<T>(T value, string name = null)
            => new ArgumentInfo<T>(value, name);

        /// <summary>Represents a method argument.</summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        public readonly partial struct ArgumentInfo<T>
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
    }
}
