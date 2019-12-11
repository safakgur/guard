#nullable enable

namespace Dawn
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>Validates argument preconditions.</summary>
    /// <content>Contains the argument initialization methods.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Returns an object that can be used to assert preconditions for the method argument
        ///     with the specified name and value.
        ///     Validation parameters are excluded from the exception messages of failed validations.
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
        /// <returns>An object used for asserting preconditions.</returns>
        [DebuggerStepThrough]
        [GuardFunction("Initialization", "gsa", order: 1)]
        public static ArgumentInfo<T> SensitiveArgument<T>(
            T value, [InvokerParameterName] string? name = null)
            => new ArgumentInfo<T>(value, name, sensitive: true);

        /// <summary>
        ///     Returns an object that can be used to assert preconditions for the specified method argument.
        ///     Validation parameters are excluded from the exception messages of failed validations.
        /// </summary>
        /// <typeparam name="T">The type of the method argument.</typeparam>
        /// <param name="e">An expression that specifies a method argument.</param>
        /// <returns>An object used for asserting preconditions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="e" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="e" /> is not a <see cref="MemberExpression" />.</exception>
        [ContractAnnotation("e:null => halt")]
        [DebuggerStepThrough]
        [GuardFunction("Initialization", order: 2)]
        public static ArgumentInfo<T> SensitiveArgument<T>(Expression<Func<T>> e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));

            return e.Body is MemberExpression m
                ? SensitiveArgument(e.Compile()(), m.Member.Name)
                : throw new ArgumentException("A member expression is expected.", nameof(e));
        }
    }
}
