namespace JetBrains.Annotations
{
    using System;

    /// <summary>
    ///     Indicates that the marked method is an assertion method, i.e. it halts the control flow
    ///     unless a condition is satisfied.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class AssertionMethodAttribute : Attribute { }

    /// <summary>Describes the relations between the marked method's inputs and outputs.</summary>
    /// <see href="https://www.jetbrains.com/help/resharper/Contract_Annotations.html" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class ContractAnnotationAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContractAnnotationAttribute" /> class.
        /// </summary>
        /// <param name="contract">The contract string.</param>
        public ContractAnnotationAttribute([NotNull] string contract) => this.Contract = contract;

        /// <summary>Gets the contract string.</summary>
        [NotNull]
        public string Contract { get; }
    }

    /// <summary>
    ///     Indicates that the marked argument should be a string literal and match one of the
    ///     parameters of the method it belongs to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class InvokerParameterNameAttribute : Attribute { }

    /// <summary>Indicates that the value of the marked element could never be <c>null</c>.</summary>
    [AttributeUsage(
      AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
      AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event |
      AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
    internal sealed class NotNullAttribute : Attribute { }

    /// <summary>Indicates that the marked parameter is a regular expression pattern.</summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class RegexPatternAttribute : Attribute { }
}
