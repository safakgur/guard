#nullable enable

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Dawn
{
    /// <summary>Validates argument preconditions.</summary>
    /// <content>Contains the argument initialization methods.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Returns an object that can be used to assert preconditions for the method argument
        ///     with the specified name and value.
        /// </summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        /// <param name="value">The value of the method argument.</param>
        /// <param name="name">
        ///     <para>
        ///         The name of the method argument. Use the <c>nameof</c> operator ( <c>Nameof</c>
        ///         in Visual Basic) where possible.
        ///     </para>
        ///     <para>
        ///         It is highly recommended you don't left this value <c>null</c> so the arguments
        ///         violating the preconditions can be easily identified.
        ///     </para>
        /// </param>
        /// <param name="secure">
        ///     Pass <c>true</c> for the validation parameters to be excluded from the exception
        ///     messages of failed validations.
        /// </param>
        /// <returns>An object used for asserting preconditions.</returns>
        [DebuggerStepThrough]
        [GuardFunction("Initialization", "ga", order: 1)]
        public static ArgumentInfo<T> Argument<T>(
            T value, [InvokerParameterName] string? name = null, bool secure = false)
            => new ArgumentInfo<T>(value, name, secure: secure);

        /// <summary>
        ///     Returns an object that can be used to assert preconditions for the specified method argument.
        /// </summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        /// <param name="e">An expression that specifies a method argument.</param>
        /// <param name="secure">
        ///     Pass <c>true</c> for the validation parameters to be excluded from the exception
        ///     messages of failed validations.
        /// </param>
        /// <returns>An object used for asserting preconditions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="e" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="e" /> is not a <see cref="MemberExpression" />.</exception>
        [ContractAnnotation("e:null => halt")]
        [DebuggerStepThrough]
        [GuardFunction("Initialization", order: 2)]
        public static ArgumentInfo<T> Argument<T>(Expression<Func<T>> e, bool secure = false)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));

            return e.Body is MemberExpression m
                ? Argument(e.Compile()(), m.Member.Name, secure)
                : throw new ArgumentException("A member expression is expected.", nameof(e));
        }

        /// <summary>Invokes the current scope's exception interceptor on precondition failures.</summary>
        /// <param name="exception">The exception to intercept.</param>
        /// <returns><paramref name="exception" />.</returns>
        [GuardFunction("Extensibility")]
        public static Exception Fail(Exception exception)
        {
#if !NETSTANDARD1_0
            StackTrace? stackTrace = null;
            for (var scope = Scope.Current; scope != null; scope = scope.Parent)
            {
                scope.ExceptionInterceptor?.Invoke(
                    exception,
                    stackTrace ?? (stackTrace = new StackTrace(1, true)));

                if (!scope.Propagates)
                    break;
            }
#endif

            return exception;
        }

        /// <summary>Represents a method argument.</summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        [DebuggerDisplay("{DebuggerDisplay,nq}")]
        [StructLayout(LayoutKind.Auto)]
        public readonly partial struct ArgumentInfo<T>
        {
            /// <summary>The default name for the arguments of type <typeparamref name="T" />.</summary>
            private static readonly string DefaultName = $"The {typeof(T)} argument";

            /// <summary>The argument name.</summary>
            private readonly string? _name;

            /// <summary>
            ///     Initializes a new instance of the <see cref="ArgumentInfo{T} " /> struct.
            /// </summary>
            /// <param name="value">The value of the method argument.</param>
            /// <param name="name">The name of the method argument.</param>
            /// <param name="modified">
            ///     Whether the original method argument is modified before the initialization of
            ///     this instance.
            /// </param>
            /// <param name="secure">
            ///     Pass <c>true</c> for the validation parameters to be excluded from the exception
            ///     messages of failed validations.
            /// </param>
            [DebuggerStepThrough]
            public ArgumentInfo(
                T value,
                [InvokerParameterName] string? name,
                bool modified = false,
                bool secure = false)
            {
                Value = value;
                _name = name;
                Modified = modified;
                Secure = secure;
            }

            /// <summary>Determines whether the argument value is not <c>null</c>.</summary>
            public bool HasValue => Value != null;

            /// <summary>Gets the argument value.</summary>
            public T Value { get; }

            /// <summary>Gets the argument name.</summary>
            public string Name => _name ?? DefaultName;

            /// <summary>
            ///     Gets a value indicating whether the original method argument is modified before
            ///     the initialization of this instance.
            /// </summary>
            public bool Modified { get; }

            /// <summary>
            ///     Gets a value indicating whether sensitive information may be used to validate the
            ///     argument. If <c>true</c>, exception messages provide less information about the
            ///     validation parameters.
            /// </summary>
            public bool Secure { get; }

            /// <summary>Gets how the layout is displayed in the debugger variable windows.</summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal string DebuggerDisplay
            {
                get
                {
                    var name = _name;
                    var value = Value?.ToString() ?? "null";
                    var result = name is null ? value : $"{name}: {value}";
                    return Secure ? $"[SECURE] {result}" : result;
                }
            }

            /// <summary>Gets the value of an argument.</summary>
            /// <param name="argument">The argument whose value to return.</param>
            /// <returns><see cref="Value" />.</returns>
            public static implicit operator T(ArgumentInfo<T> argument) => argument.Value;

            /// <summary>Returns the string representation of the argument value.</summary>
            /// <returns>String representation of the argument value.</returns>
            public override string ToString()
                => Value?.ToString() ?? string.Empty;
        }
    }
}
