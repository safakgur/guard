#if !NETSTANDARD1_0

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="MailAddress" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>Requires the argument value to have the specified host.</summary>
        /// <param name="argument">The email address argument.</param>
        /// <param name="host">The host that the argument value is required to have.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value's host is not <paramref name="host" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Email")]
        public static ref readonly ArgumentInfo<MailAddress> HasHost(
            in this ArgumentInfo<MailAddress> argument,
            string host,
            Func<MailAddress, string, string> message = null)
        {
            if (argument.TryGetValue(out var value) &&
                !value.Host.Equals(host, StringComparison.OrdinalIgnoreCase))
            {
                var m = message?.Invoke(value, host) ?? Messages.EmailHasHost(argument, host);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to not have the specified host.</summary>
        /// <param name="argument">The email address argument.</param>
        /// <param name="host">The host that the argument value is required not to have.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value's host is <paramref name="host" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Email")]
        public static ref readonly ArgumentInfo<MailAddress> DoesNotHaveHost(
            in this ArgumentInfo<MailAddress> argument,
            string host,
            Func<MailAddress, string, string> message = null)
        {
            if (argument.TryGetValue(out var value) &&
                value.Host.Equals(host, StringComparison.OrdinalIgnoreCase))
            {
                var m = message?.Invoke(value, host) ?? Messages.EmailDoesNotHaveHost(argument, host);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to have one of the specified hosts.</summary>
        /// <typeparam name="TCollection">The type of the hosts collection.</typeparam>
        /// <param name="argument">The email address argument.</param>
        /// <param name="hosts">The hosts that the argument value is required to have one of.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value's host is not specified in <paramref name="hosts" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Email")]
        public static ref readonly ArgumentInfo<MailAddress> HostIn<TCollection>(
            in this ArgumentInfo<MailAddress> argument,
            TCollection hosts,
            Func<MailAddress, IEnumerable<string>, string> message = null)
            where TCollection : IEnumerable<string>
        {
            if (argument.TryGetValue(out var value) &&
                !Collection<TCollection>.Typed<string>.Contains(hosts, value.Host, null))
            {
                var m = message?.Invoke(value, hosts) ?? Messages.EmailHostIn(argument, hosts);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to have none of the specified hosts.</summary>
        /// <typeparam name="TCollection">The type of the hosts collection.</typeparam>
        /// <param name="argument">The email address argument.</param>
        /// <param name="hosts">The hosts that the argument value is required not to have any.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value's host is specified in <paramref name="hosts" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Email")]
        public static ref readonly ArgumentInfo<MailAddress> HostNotIn<TCollection>(
            in this ArgumentInfo<MailAddress> argument,
            TCollection hosts,
            Func<MailAddress, IEnumerable<string>, string> message = null)
            where TCollection : IEnumerable<string>
        {
            if (argument.TryGetValue(out var value) &&
                Collection<TCollection>.Typed<string>.Contains(hosts, value.Host, null))
            {
                var m = message?.Invoke(value, hosts) ?? Messages.EmailHostNotIn(argument, hosts);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to have a display name specified.</summary>
        /// <param name="argument">The email address argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value does not have a display name specified.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Email")]
        public static ref readonly ArgumentInfo<MailAddress> HasDisplayName(
            in this ArgumentInfo<MailAddress> argument, Func<MailAddress, string> message = null)
        {
            if (argument.TryGetValue(out var value) && value.DisplayName.Length == 0)
            {
                var m = message?.Invoke(value) ?? Messages.EmailHasDisplayName(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to not have a display name specified.</summary>
        /// <param name="argument">The email address argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value has a display name specified.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Email")]
        public static ref readonly ArgumentInfo<MailAddress> DoesNotHaveDisplayName(
            in this ArgumentInfo<MailAddress> argument, Func<MailAddress, string> message = null)
        {
            if (argument.TryGetValue(out var value) && value.DisplayName.Length > 0)
            {
                var m = message?.Invoke(value) ?? Messages.EmailDoesNotHaveDisplayName(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }
    }
}

#endif
