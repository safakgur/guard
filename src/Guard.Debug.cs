using System.Diagnostics;

namespace Dawn
{
    public static partial class Guard
    {
        /// <summary>
        ///     Works similar to #if DEBUG directive.
        ///     The whole method and its argument is ignored in release build configuration.
        ///     Example Guard.IfDebug(Guard.Argument(age, nameof(age)).NotNegative())
        /// </summary>
        /// <param name="validation">
        ///     Argument validation chain which will be only executed if DEBUG symbol is defined.
        /// </param>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IfDebug(object validation) { }
    }
}
