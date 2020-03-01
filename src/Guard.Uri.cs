using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="Uri" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>The URI scheme for Hypertext Transfer Protocol (HTTP).</summary>
        private const string HttpUriScheme = "http"; // Uri.UriSchemeHttp

        /// <summary>The URI scheme for Secure Hypertext Transfer Protocol (HTTPS).</summary>
        private const string HttpsUriScheme = "https"; // Uri.UriSchemeHttps

        /// <summary>Requires the argument value to be an absolute URI.</summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor an absolute URI.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Uri", "gabs")]
        public static ref readonly ArgumentInfo<Uri> Absolute(
            in this ArgumentInfo<Uri> argument, Func<Uri, string> message = null)
        {
            if (argument.TryGetValue(out var value) && !value.IsAbsoluteUri)
            {
                var m = message?.Invoke(value) ?? Messages.UriAbsolute(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to be a relative URI.</summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor a relative URI.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Uri", "grel")]
        public static ref readonly ArgumentInfo<Uri> Relative(
            in this ArgumentInfo<Uri> argument, Func<Uri, string> message = null)
        {
            if (argument.TryGetValue(out var value) && value.IsAbsoluteUri)
            {
                var m = message?.Invoke(value) ?? Messages.UriRelative(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument value to be an absolute URI with the specified scheme.
        /// </summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="scheme">The URI scheme to compare.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is neither <c>null</c> nor an absolute URI with
        ///     the scheme specified by <paramref name="scheme" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Uri", "gsch")]
        public static ref readonly ArgumentInfo<Uri> Scheme(
            in this ArgumentInfo<Uri> argument, string scheme, Func<Uri, string, string> message = null)
        {
            if (argument.TryGetValue(out var value))
            {
                if (!value.IsAbsoluteUri ||
                    !value.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                {
                    var m = message?.Invoke(value, scheme) ?? Messages.UriScheme(argument, scheme);
                    throw Fail(new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to not have the specified scheme.</summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="scheme">The URI scheme to compare.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is an absolute URI with the scheme specified by <paramref name="scheme" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Uri", "gnsch")]
        public static ref readonly ArgumentInfo<Uri> NotScheme(
            in this ArgumentInfo<Uri> argument, string scheme, Func<Uri, string, string> message = null)
        {
            if (argument.TryGetValue(out var value) &&
                value.IsAbsoluteUri &&
                value.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
            {
                var m = message?.Invoke(value, scheme) ?? Messages.UriNotScheme(argument, scheme);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument value to be an absolute URI with the HTTP or HTTPS scheme.
        /// </summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and its scheme is neither HTTP
        ///     nor HTTPS.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Uri", "ghttp")]
        public static ref readonly ArgumentInfo<Uri> Http(
            in this ArgumentInfo<Uri> argument, Func<Uri, string> message = null)
            => ref argument.Http(true, message);

        /// <summary>
        ///     Requires the argument value to be an absolute URI with the HTTP or HTTPS scheme.
        /// </summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="allowHttps">
        ///     Pass <c>true</c> to allow both the HTTP and HTTPS schemes or <c>false</c> to allow
        ///     only the HTTP scheme.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and does not have one of the
        ///     required schemes.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Uri")]
        public static ref readonly ArgumentInfo<Uri> Http(
            in this ArgumentInfo<Uri> argument, bool allowHttps, Func<Uri, string> message = null)
        {
            if (!argument.TryGetValue(out var value))
                return ref argument;

            if (value.IsAbsoluteUri)
            {
                if (value.Scheme == HttpUriScheme)
                    return ref argument;

                if (allowHttps && value.Scheme == HttpsUriScheme)
                    return ref argument;
            }

            var m = message?.Invoke(value) ?? Messages.UriHttp(argument);
            throw Fail(new ArgumentException(m, argument.Name));
        }

        /// <summary>Requires the argument value to be an absolute URI with the HTTPS scheme.</summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and does not have the HTTPS scheme.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Uri", "ghttps")]
        public static ref readonly ArgumentInfo<Uri> Https(
            in this ArgumentInfo<Uri> argument, Func<Uri, string> message = null)
        {
            if (argument.TryGetValue(out var value))
                if (!value.IsAbsoluteUri || value.Scheme != HttpsUriScheme)
                {
                    var m = message?.Invoke(value) ?? Messages.UriHttps(argument);
                    throw Fail(new ArgumentException(m, argument.Name));
                }

            return ref argument;
        }
    }
}
