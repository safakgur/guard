#nullable enable

namespace Dawn
{
    using System;

    /// <summary>Marks a method as a non-guarding utility.</summary>
    /// <remarks>Methods with this attribute are ignored by annotation tests.</remarks>
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class NonGuardAttribute : Attribute
    {
    }
}
