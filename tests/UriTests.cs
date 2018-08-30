namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed class UriTests : BaseTests
    {
        private const string HttpUriScheme = "http"; // Uri.HttpUriScheme

        private const string HttpsUriScheme = "https"; // Uri.HttpsUriScheme

        private const string HttpsUriString = "https://github.com/safakgur/guard";

        private const string HttpUriString = "http://github.com/safakgur/guard";

        private const string RelativeUriString = "/safakgur/guard";

        [Theory(DisplayName = T + "URI: Absolute/Relative")]
        [InlineData(null, null)]
        [InlineData(HttpsUriString, RelativeUriString)]
        public void Kind(string absoluteUriString, string relativeUriString)
        {
            var absoluteUri = GetUri(absoluteUriString);
            var absoluteUriArg = Guard.Argument(() => absoluteUri).Absolute();

            var relativeUri = GetUri(relativeUriString);
            var relativeUriArg = Guard.Argument(() => relativeUri).Relative();

            if (absoluteUri == null)
            {
                absoluteUriArg.Relative();
                relativeUriArg.Absolute();
                return;
            }

            ThrowsArgumentException(
                relativeUriArg,
                arg => arg.Absolute(),
                (arg, message) => arg.Absolute(u =>
                {
                    Assert.Same(relativeUri, u);
                    return message;
                }));

            ThrowsArgumentException(
                absoluteUriArg,
                arg => arg.Relative(),
                (arg, message) => arg.Relative(u =>
                {
                    Assert.Same(absoluteUri, u);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "URI: Scheme")]
        [InlineData(null, null, null)]
        [InlineData(HttpsUriScheme, HttpsUriString, RelativeUriString)]
        [InlineData(HttpsUriScheme, HttpsUriString, HttpUriString)]
        [InlineData(HttpUriScheme, HttpUriString, RelativeUriString)]
        [InlineData(HttpUriScheme, HttpUriString, HttpsUriScheme)]
        public void Scheme(string scheme, string validUriString, string invalidUriString)
        {
            var validUri = GetUri(validUriString);
            var validUriArg = Guard.Argument(() => validUri).Scheme(scheme);

            var invalidUri = GetUri(invalidUriString);
            var invalidUriArg = Guard.Argument(() => invalidUri).NotScheme(scheme);

            if (validUri == null)
            {
                validUriArg.NotScheme(scheme);
                invalidUriArg.Scheme(scheme);
                return;
            }

            ThrowsArgumentException(
                invalidUriArg,
                arg => arg.Scheme(scheme),
                (arg, message) => arg.Scheme(scheme, (u, s) =>
                {
                    Assert.Same(invalidUri, u);
                    Assert.Same(scheme, s);
                    return message;
                }));

            ThrowsArgumentException(
                validUriArg,
                arg => arg.NotScheme(scheme),
                (arg, message) => arg.NotScheme(scheme, (u, s) =>
                {
                    Assert.Same(validUri, u);
                    Assert.Same(scheme, s);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "URI: HTTP")]
        [InlineData(null, null)]
        [InlineData(HttpUriString, RelativeUriString)]
        [InlineData(HttpUriString, HttpsUriString)]
        public void Http(string validUriString, string invalidUriString)
        {
            var validUri = GetUri(validUriString);
            var validUriArg = Guard.Argument(() => validUri).Http().Http(false);

            var invalidUri = GetUri(invalidUriString);
            var invalidUriArg = Guard.Argument(() => invalidUri);

            if (validUri == null)
            {
                invalidUriArg.Http(false);
                return;
            }

            ThrowsArgumentException(
                invalidUriArg,
                arg => arg.Http(false),
                (arg, message) => arg.Http(false, u =>
                {
                    Assert.Same(invalidUri, u);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "URI: HTTP/S")]
        [InlineData(null, null)]
        [InlineData(HttpUriString, RelativeUriString)]
        [InlineData(HttpsUriString, RelativeUriString)]
        public void HttpOrHttps(string validUriString, string invalidUriString)
        {
            var validUri = GetUri(validUriString);
            var validUriArg = Guard.Argument(() => validUri).Http().Http(true);

            var invalidUri = GetUri(invalidUriString);
            var invalidUriArg = Guard.Argument(() => invalidUri);

            if (validUri == null)
            {
                invalidUriArg.Http();
                invalidUriArg.Http(true);
                return;
            }

            ThrowsArgumentException(
                invalidUriArg,
                arg => arg.Http(),
                (arg, message) => arg.Http(u =>
                {
                    Assert.Same(invalidUri, u);
                    return message;
                }));

            ThrowsArgumentException(
                invalidUriArg,
                arg => arg.Http(true),
                (arg, message) => arg.Http(true, u =>
                {
                    Assert.Same(invalidUri, u);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "URI: HTTPS")]
        [InlineData(null, null)]
        [InlineData(HttpsUriString, RelativeUriString)]
        [InlineData(HttpsUriString, HttpUriString)]
        public void Https(string validUriString, string invalidUriString)
        {
            var validUri = GetUri(validUriString);
            var validUriArg = Guard.Argument(() => validUri).Https();

            var invalidUri = GetUri(invalidUriString);
            var invalidUriArg = Guard.Argument(() => invalidUri);

            if (validUri == null)
            {
                invalidUriArg.Https();
                return;
            }

            ThrowsArgumentException(
                invalidUriArg,
                arg => arg.Https(),
                (arg, message) => arg.Https(u =>
                {
                    Assert.Same(invalidUri, u);
                    return message;
                }));
        }

        private static Uri GetUri(string uriString)
            => uriString != null ? new Uri(uriString, UriKind.RelativeOrAbsolute) : null;
    }
}
