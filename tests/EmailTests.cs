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
        [Theory(DisplayName = "Email: HasHost/DoesNotHaveHost")]
        [InlineData(null, "A", "B", false)]
        [InlineData("a@b.c", "b.c", "c.b", false)]
        [InlineData("a@b.c", "b.c", "c.b", true)]
        [InlineData("a@b.c", "B.C", "C.B", false)] // Ordinal case-insensitive.
        [InlineData("a@b.c", "B.C", "C.B", true)]
        public void HasHost(string emailString, string host, string nonHost, bool sensitive)
        {
            var email = emailString is null ? null : new MailAddress(emailString);
            var emailArgument = (sensitive
                    ? Guard.SensitiveArgument(() => email)
                    : Guard.Argument(() => email))
                .HasHost(host).DoesNotHaveHost(nonHost);

            if (email is null)
            {
                emailArgument.HasHost(nonHost).DoesNotHaveHost(host);
                return;
            }

            ThrowsArgumentException(
                emailArgument,
                arg => arg.HasHost(nonHost),
                m => sensitive != m.Contains(nonHost),
                (arg, message) => arg.HasHost(nonHost, (e, h) =>
                {
                    Assert.Same(email, e);
                    Assert.Same(nonHost, h);
                    return message;
                }));

            ThrowsArgumentException(
                emailArgument,
                arg => arg.DoesNotHaveHost(host),
                m => sensitive != m.Contains(host),
                (arg, message) => arg.DoesNotHaveHost(host, (e, h) =>
                {
                    Assert.Same(email, e);
                    Assert.Same(host, h);
                    return message;
                }));
        }

        [Theory(DisplayName = "Email: HostIn/HostNotIn")]
        [InlineData(null, "A;B", "C;D", false, false)]
        [InlineData(null, "A;B", "C;D", true, false)]
        [InlineData(null, "A;B", "C;D", true, true)]
        [InlineData("a@b.c", "a.b;b.c", "c.d;d.e", false, false)]
        [InlineData("a@b.c", "a.b;b.c", "c.d;d.e", false, true)]
        [InlineData("a@b.c", "a.b;b.c", "c.d;d.e", true, false)]
        [InlineData("a@b.c", "a.b;b.c", "c.d;d.e", true, true)]
        [InlineData("a@b.c", "a.b;b.c", "A.B;B.C", false, false)] // The default comparer.
        [InlineData("a@b.c", "a.b;b.c", "A.B;B.C", false, true)]
        [InlineData("a@b.c", "a.b;b.c", "A.B;B.C", true, false)] // Collection type's Contains method.
        [InlineData("a@b.c", "a.b;b.c", "A.B;B.C", true, true)]
        [InlineData("a@b.c", "a.b;b.c", "", false, false)]
        [InlineData("a@b.c", "a.b;b.c", "", false, true)]
        [InlineData("a@b.c", "a.b;b.c", "", true, false)]
        [InlineData("a@b.c", "a.b;b.c", "", true, true)]
        public void HostIn(
            string emailString, string hostsString, string nonHostsString, bool hasContains, bool sensitive)
        {
            var email = emailString is null ? null : new MailAddress(emailString);
            var emailArg = sensitive
                ? Guard.SensitiveArgument(() => email)
                : Guard.Argument(() => email);
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
                m => TestGeneratedMessage(m, nonHosts),
                (arg, message) => arg.HostIn(nonHosts, (e, h) =>
                {
                    Assert.Same(email, e);
                    Assert.Same(nonHosts, h);
                    return message;
                }));

            var enumerationCount = GetEnumerationCount(null, nonHostsCount);
            var forceEnumerated = !sensitive ? true : default(bool?);
            CheckAndReset(nonHosts, containsCalled: true, enumerationCount: enumerationCount, forceEnumerated: forceEnumerated);

            CheckAndReset(hosts, containsCalled: true, enumerationCount: hostIndex + 1);
            ThrowsArgumentException(
                emailArg,
                arg => arg.HostNotIn(hosts),
                m => TestGeneratedMessage(m, hosts),
                (arg, message) => arg.HostNotIn(hosts, (e, h) =>
                {
                    Assert.Same(email, e);
                    Assert.Same(hosts, h);
                    return message;
                }));

            enumerationCount = GetEnumerationCount(hostIndex, hostsCount);
            CheckAndReset(hosts, containsCalled: true, enumerationCount: enumerationCount, forceEnumerated: forceEnumerated);

            int GetEnumerationCount(int? index, int count)
            {
                var result = index.HasValue
                    ? (index.Value + 1) * 2 + (sensitive ? 0 : count)
                    : count * (sensitive ? 2 : 3);

                if (result == 0)
                    result++;

                return result;
            }

            bool TestGeneratedMessage(string message, ITestEnumerable<string> enumerable)
                => sensitive || enumerable.Items.All(i => message.Contains(i.ToString()));
        }

        [Theory(DisplayName = "Email: HasDisplayName/DoesNotHaveDisplayName")]
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
