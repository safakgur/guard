namespace Dawn
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    /// <content>Provides state preconditions.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Requires a method call to be valid for the current state of the instance it belongs to.
        /// </summary>
        /// <param name="valid">Whether the method call is valid.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the call is invalid.
        /// </param>
        /// <param name="caller">The name of the invoked method.</param>
        /// <exception cref="InvalidOperationException"><paramref name="valid" /> is <c>false</c>.</exception>
        [AssertionMethod]
        [ContractAnnotation("valid:false => halt")]
        [DebuggerStepThrough]
        public static void Operation(
            bool valid, string message = null, [CallerMemberName]string caller = null)
        {
            if (!valid)
                throw new InvalidOperationException(message ?? Messages.State(caller));
        }

        /// <summary>Requires a method to be supported by the instance it belongs to.</summary>
        /// <param name="supported">Whether the invoked method is supported.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the method is not supported.
        /// </param>
        /// <param name="caller">The name of the invoked method.</param>
        /// <exception cref="NotSupportedException"><paramref name="supported" /> is <c>false</c>.</exception>
        [AssertionMethod]
        [ContractAnnotation("supported:false => halt")]
        [DebuggerStepThrough]
        public static void Support(
            bool supported, string message = null, [CallerMemberName]string caller = null)
        {
            if (!supported)
                throw new NotSupportedException(message ?? Messages.Support(caller));
        }

        /// <summary>Requires an instance not to be disposed.</summary>
        /// <param name="disposed">
        ///     Whether the instance that the invoked method belongs to is disposed.
        /// </param>
        /// <param name="objectName">The name of the instance that may be disposed.</param>
        /// <param name="message">
        ///     The message of the exception that will be thrown if the instance that the invoked
        ///     method belongs to is disposed.
        /// </param>
        /// <exception cref="ObjectDisposedException"><paramref name="disposed" /> is <c>true</c>.</exception>
        [AssertionMethod]
        [ContractAnnotation("disposed:true => halt")]
        [DebuggerStepThrough]
        public static void Disposal(bool disposed, string objectName = null, string message = null)
        {
            if (disposed)
                throw new ObjectDisposedException(objectName, message ?? Messages.Disposal());
        }
    }
}
