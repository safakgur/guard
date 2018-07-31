namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        private const string HttpUriScheme = "http"; // Uri.HttpUriScheme

        private const string HttpsUriScheme = "https"; // Uri.HttpsUriScheme

        [Fact(DisplayName = T + "Guard supports URI preconditions.")]
        public void GuardSupportsUris()
        {
            var message = RandomMessage;

            // Absolute.
            var @null = null as Uri;
            var nullArg = Guard.Argument(() => @null);
            nullArg.Absolute();

            var https = new Uri("https://github.com/safakgur/guard");
            var httpsArg = Guard.Argument(() => https);
            httpsArg.Absolute();

            var relative = new Uri("/safakgur/guard", UriKind.Relative);
            var relativeArg = Guard.Argument(() => relative);
            Assert.Throws<ArgumentException>(
                nameof(relative), () => Guard.Argument(() => relative).Absolute());

            var ex = Assert.Throws<ArgumentException>(
                nameof(relative), () => Guard.Argument(() => relative).Absolute(u =>
                {
                    Assert.Same(relative, u);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Relative.
            nullArg.Relative();

            relativeArg.Relative();
            Assert.Throws<ArgumentException>(
                nameof(https), () => Guard.Argument(() => https).Relative());

            ex = Assert.Throws<ArgumentException>(
                nameof(https), () => Guard.Argument(() => https).Relative(u =>
                {
                    Assert.Same(https, u);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Scheme.
            nullArg.Scheme(HttpsUriScheme).Scheme(HttpUriScheme);

            httpsArg.Scheme(HttpsUriScheme);
            Assert.Throws<ArgumentException>(
                nameof(https), () => Guard.Argument(() => https).Scheme(HttpUriScheme));

            ex = Assert.Throws<ArgumentException>(
                nameof(https), () => Guard.Argument(() => https).Scheme(HttpUriScheme, (u, scheme) =>
                {
                    Assert.Same(https, u);
                    Assert.Same(HttpUriScheme, scheme);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            var http = new Uri("http://github.com/safakgur/guard");
            var httpArg = Guard.Argument(() => http);
            httpArg.Scheme(HttpUriScheme);

            Assert.Throws<ArgumentException>(
                nameof(http), () => Guard.Argument(() => http).Scheme(HttpsUriScheme));

            ex = Assert.Throws<ArgumentException>(
                nameof(http), () => Guard.Argument(() => http).Scheme(HttpsUriScheme, (u, scheme) =>
                {
                    Assert.Same(http, u);
                    Assert.Same(HttpsUriScheme, scheme);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(relative), () => Guard.Argument(() => relative).Scheme(HttpsUriScheme));

            ex = Assert.Throws<ArgumentException>(
                nameof(relative), () => Guard.Argument(() => relative).Scheme(HttpsUriScheme, (u, scheme) =>
                {
                    Assert.Same(relative, u);
                    Assert.Same(HttpsUriScheme, scheme);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // HTTP or HTTPS
            nullArg.Http();

            httpsArg.Http();
            httpsArg.Http(true);
            httpArg.Http();
            httpArg.Http(true);
            Assert.Throws<ArgumentException>(
                nameof(relative), () => Guard.Argument(() => relative).Http());

            ex = Assert.Throws<ArgumentException>(
                nameof(relative), () => Guard.Argument(() => relative).Http(u =>
                {
                    Assert.Same(relative, u);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // HTTP only.
            nullArg.Http(false);

            httpArg.Http(false);
            Assert.Throws<ArgumentException>(
                nameof(https), () => Guard.Argument(() => https).Http(false));

            ex = Assert.Throws<ArgumentException>(
                nameof(https), () => Guard.Argument(() => https).Http(false, u =>
                {
                    Assert.Same(https, u);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // HTTPS only.
            nullArg.Https();

            httpsArg.Https();
            Assert.Throws<ArgumentException>(
                nameof(http), () => Guard.Argument(() => http).Https());

            ex = Assert.Throws<ArgumentException>(
                nameof(http), () => Guard.Argument(() => http).Https(u =>
                {
                    Assert.Same(http, u);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);
        }
    }
}
