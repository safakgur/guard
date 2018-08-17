#if !NETSTANDARD1_0
namespace Dawn
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

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
        public static ref readonly ArgumentInfo<MailAddress> HasHost(
            in this ArgumentInfo<MailAddress> argument,
            string host,
            Func<MailAddress, string, string> message = null)
        {
            if (argument.HasValue() &&
                !argument.Value.Host.Equals(host, StringComparison.OrdinalIgnoreCase))
            {
                var m = message?.Invoke(argument.Value, host) ?? Messages.EmailHasHost(argument, host);
                throw new ArgumentException(m, argument.Name);
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
        public static ref readonly ArgumentInfo<MailAddress> DoesNotHaveHost(
            in this ArgumentInfo<MailAddress> argument,
            string host,
            Func<MailAddress, string, string> message = null)
        {
            if (argument.HasValue() &&
                argument.Value.Host.Equals(host, StringComparison.OrdinalIgnoreCase))
            {
                var m = message?.Invoke(argument.Value, host) ?? Messages.EmailDoesNotHaveHost(argument, host);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to have one of the specified hosts.</summary>
        /// <typeparam name="TCollection">The type of the hosts collection.</typeparam>
        /// <param name="argument">The email address argument.</param>
        /// <param name="hosts">
        ///     The hosts that the argument value is required to have one of.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value's host is not specified in
        ///     <paramref name="hosts" />.
        /// </exception>
        public static ref readonly ArgumentInfo<MailAddress> HostIn<TCollection>(
            in this ArgumentInfo<MailAddress> argument,
            TCollection hosts,
            Func<MailAddress, IEnumerable<string>, string> message = null)
            where TCollection : IEnumerable<string>
        {
            if (argument.HasValue() &&
                !Collection<TCollection>.Typed<string>.Contains(hosts, argument.Value.Host, null))
            {
                var m = message?.Invoke(argument.Value, hosts) ?? Messages.EmailHostIn(argument, hosts);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to have none of the specified hosts.</summary>
        /// <typeparam name="TCollection">The type of the hosts collection.</typeparam>
        /// <param name="argument">The email address argument.</param>
        /// <param name="hosts">
        ///     The hosts that the argument value is required not to have any.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value's host is specified in <paramref name="hosts" />.
        /// </exception>
        public static ref readonly ArgumentInfo<MailAddress> HostNotIn<TCollection>(
            in this ArgumentInfo<MailAddress> argument,
            TCollection hosts,
            Func<MailAddress, IEnumerable<string>, string> message = null)
            where TCollection : IEnumerable<string>
        {
            if (argument.HasValue() &&
                Collection<TCollection>.Typed<string>.Contains(hosts, argument.Value.Host, null))
            {
                var m = message?.Invoke(argument.Value, hosts) ?? Messages.EmailHostNotIn(argument, hosts);
                throw new ArgumentException(m, argument.Name);
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
        public static ref readonly ArgumentInfo<MailAddress> HasDisplayName(
            in this ArgumentInfo<MailAddress> argument, Func<MailAddress, string> message = null)
        {
            if (argument.HasValue() && argument.Value.DisplayName.Length == 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.EmailHasDisplayName(argument);
                throw new ArgumentException(m, argument.Name);
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
        public static ref readonly ArgumentInfo<MailAddress> DoesNotHaveDisplayName(
            in this ArgumentInfo<MailAddress> argument, Func<MailAddress, string> message = null)
        {
            if (argument.HasValue() && argument.Value.DisplayName.Length > 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.EmailDoesNotHaveDisplayName(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }
    }
}
#endif
