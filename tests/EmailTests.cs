#if !NETCOREAPP1_0
namespace Dawn.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using Xunit;

    public sealed class EmailTests : BaseTests
    {
        [Theory(DisplayName = T + "Email: HasHost/DoesNotHaveHost")]
        [InlineData(null, "A", "B")]
        [InlineData("a@b.c", "b.c", "c.b")]
        [InlineData("a@b.c", "B.C", "C.B")] // Ordinal case-insensitive.
        public void HasHost(string emailString, string host, string nonHost)
        {
            var email = emailString is null ? null : new MailAddress(emailString);
            var emailArgument = Guard.Argument(() => email).HasHost(host).DoesNotHaveHost(nonHost);

            if (email is null)
            {
                emailArgument.HasHost(nonHost).DoesNotHaveHost(host);
                return;
            }

            ThrowsArgumentException(
                emailArgument,
                arg => arg.HasHost(nonHost),
                (arg, message) => arg.HasHost(nonHost, (e, h) =>
                {
                    Assert.Same(email, e);
                    Assert.Same(nonHost, h);
                    return message;
                }));

            ThrowsArgumentException(
                emailArgument,
                arg => arg.DoesNotHaveHost(host),
                (arg, message) => arg.DoesNotHaveHost(host, (e, h) =>
                {
                    Assert.Same(email, e);
                    Assert.Same(host, h);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Email: HostIn/HostNotIn")]
        [InlineData(null, "A;B", "C;D", false)]
        [InlineData(null, "A;B", "C;D", true)]
        [InlineData("a@b.c", "a.b;b.c", "c.d;d.e", false)]
        [InlineData("a@b.c", "a.b;b.c", "c.d;d.e", true)]
        [InlineData("a@b.c", "a.b;b.c", "A.B;B.C", false)] // The default comparer.
        [InlineData("a@b.c", "a.b;b.c", "A.B;B.C", true)] // Collection type's Contains method.
        [InlineData("a@b.c", "a.b;b.c", "", false)]
        [InlineData("a@b.c", "a.b;b.c", "", true)]
        public void HostIn(string emailString, string hostsString, string nonHostsString, bool hasContains)
        {
            var email = emailString is null ? null : new MailAddress(emailString);
            var emailArg = Guard.Argument(() => email);
            var hosts = GetHosts(hostsString, hasContains, out var hostsCount);
            var hostIndex = email is null ? RandomNumber : hosts.Items.TakeWhile(h => h != email.Host).Count();
            var nonHosts = GetHosts(nonHostsString, hasContains, out var nonHostsCount);

            emailArg.HostIn(hosts).HostNotIn(nonHosts);

            if (email is null)
            {
                emailArg.HostIn(nonHosts).HostNotIn(hosts);
                return;
            }

            CheckAndReset(nonHosts, containsCalled: true, enumerationCount: nonHostsCount, enumerated: true);
            ThrowsArgumentException(
                emailArg,
                arg => arg.HostIn(nonHosts),
                (arg, message) => arg.HostIn(nonHosts, (e, h) =>
                {
                    Assert.Same(email, e);
                    Assert.Same(nonHosts, h);
                    return message;
                }));

            // 1st for test w/o message, 2nd for the auto-generated message and 3rd for test w/ message.
            var enumerationCount = nonHostsCount * 3;
            if (enumerationCount == 0)
                enumerationCount++;

            CheckAndReset(nonHosts, containsCalled: true, enumerationCount: enumerationCount, forceEnumerated: true);

            CheckAndReset(hosts, containsCalled: true, enumerationCount: hostIndex + 1);
            ThrowsArgumentException(
                emailArg,
                arg => arg.HostNotIn(hosts),
                (arg, message) => arg.HostNotIn(hosts, (e, h) =>
                {
                    Assert.Same(email, e);
                    Assert.Same(hosts, h);
                    return message;
                }));

            // 1st for test w/o message, 2nd for the auto-generated message and 3rd for test w/ message.
            enumerationCount = (hostIndex + 1) * 3;
            if (enumerationCount == 0)
                enumerationCount++;

            CheckAndReset(hosts, containsCalled: true, enumerationCount: enumerationCount, forceEnumerated: true);
        }

        [Theory(DisplayName = T + "Email: HasDisplayName/DoesNotHaveDisplayName")]
        [InlineData(null, null)]
        [InlineData("A <a@b.c>", "a@b.c")]
        public void HasDisplayName(string stringWithDisplayName, string stringWithoutDisplayName)
        {
            var withDisplayName = stringWithDisplayName is null ? null : new MailAddress(stringWithDisplayName);
            var withDisplayNameArg = Guard.Argument(() => withDisplayName).HasDisplayName();

            var withoutDisplayName = stringWithoutDisplayName is null ? null : new MailAddress(stringWithoutDisplayName);
            var withoutDisplayNameArg = Guard.Argument(() => withoutDisplayName).DoesNotHaveDisplayName();

            if (withDisplayName is null)
            {
                withDisplayNameArg.DoesNotHaveDisplayName();
                withoutDisplayNameArg.HasDisplayName();
                return;
            }

            ThrowsArgumentException(
                withoutDisplayNameArg,
                arg => arg.HasDisplayName(),
                (arg, message) => arg.HasDisplayName(e =>
                {
                    Assert.Same(withoutDisplayName, e);
                    return message;
                }));

            ThrowsArgumentException(
                withDisplayNameArg,
                arg => arg.DoesNotHaveDisplayName(),
                (arg, message) => arg.DoesNotHaveDisplayName(e =>
                {
                    Assert.Same(withDisplayName, e);
                    return message;
                }));
        }

        private static ITestEnumerable<string> GetHosts(
            string hostsString, bool hasContains, out int count)
        {
            var hosts = hostsString.Split(';');
            count = hosts.Length;

            return hasContains
                ? new EnumerableTests.TestEnumerableWithContains<string>(hosts)
                : new EnumerableTests.TestEnumerable<string>(hosts);
        }
    }
}
#endif
