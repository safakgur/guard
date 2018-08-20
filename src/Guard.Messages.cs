namespace Dawn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if !NETSTANDARD1_0
    using System.Net.Mail;
#endif

    /// <content>Provides error messages for the common preconditions.</content>
    public static partial class Guard
    {
        private static class Messages
        {
            public static string Null<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be null.";

            public static string NotNull<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be null.";

            public static string Default<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be {default(T)}.";

            public static string NotDefault<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be {default(T)}.";

            public static string Equal<T>(in ArgumentInfo<T> argument, in T other)
                => $"{argument.Name} must be {other}.";

            public static string NotEqual<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be {argument.Value}.";

            public static string Require<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} is invalid.";

            public static string Type(in ArgumentInfo<object> argument, Type type)
                => $"{argument.Name} must be an instance of type {type}.";

            public static string NotType(in ArgumentInfo<object> argument, Type type)
                => $"{argument.Name} cannot be an instance of type {type}.";

            public static string Compatible<TArgument, TTarget>(in ArgumentInfo<TArgument> argument)
                => $"{argument.Name} must be assignable to type {typeof(TTarget)}.";

            public static string NotCompatible<TArgument, TTarget>(in ArgumentInfo<TArgument> argument)
                => $"{argument.Name} cannot be assignable to type {typeof(TTarget)}.";

            public static string Min<T>(in ArgumentInfo<T> argument, in T minValue)
                => $"{argument.Name} cannot be less than {minValue}.";

            public static string Max<T>(in ArgumentInfo<T> argument, in T maxValue)
                => $"{argument.Name} cannot be greater than {maxValue}.";

            public static string Zero<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be zero.";

            public static string NotZero<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be zero.";

            public static string Positive<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be greater than zero.";

            public static string NotPositive<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be zero or less.";

            public static string Negative<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be less than zero.";

            public static string NotNegative<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be zero or greater.";

            public static string InRange<T>(in ArgumentInfo<T> argument, in T minValue, in T maxValue)
                => $"{argument.Name} must be between {minValue} and {maxValue}";

            public static string NaN<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be not a number (NaN).";

            public static string NotNaN<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be not a number (NaN).";

            public static string Infinity<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be positive or negative infinity.";

            public static string NotInfinity<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be positive or negative infinity.";

            public static string PositiveInfinity<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be positive infinity (∞).";

            public static string NotPositiveInfinity<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be positive infinity (∞).";

            public static string NegativeInfinity<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be negative infinity -(∞).";

            public static string NotNegativeInfinity<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be negative infinity (-∞).";

            public static string StringEmpty(in ArgumentInfo<string> argument)
                => $"{argument.Name} must be empty.";

            public static string StringNotEmpty(in ArgumentInfo<string> argument)
                => $"{argument.Name} cannot be empty.";

            public static string StringWhiteSpace(in ArgumentInfo<string> argument)
                => $"{argument.Name} must be empty or consist only of white-space characters.";

            public static string StringNotWhiteSpace(in ArgumentInfo<string> argument)
                => $"{argument.Name} cannot be empty or consist only of white-space characters.";

            public static string StringMinLength(in ArgumentInfo<string> argument, int minLength)
                => $"{argument.Name} cannot be shorter than {minLength} characters.";

            public static string StringMaxLength(in ArgumentInfo<string> argument, int maxLength)
                => $"{argument.Name} cannot be longer than {maxLength} characters.";

            public static string StringLengthInRange(in ArgumentInfo<string> argument, int minLength, int maxLength)
                => $"{argument.Name} must contain {minLength} to {maxLength} characters.";

            public static string StringStartsWith(in ArgumentInfo<string> argument, string value)
                => $"{argument.Name} must start with '{value}'.";

            public static string StringDoesNotStartWith(in ArgumentInfo<string> argument, string value)
                => $"{argument.Name} cannot start with '{value}'.";

            public static string StringEndsWith(in ArgumentInfo<string> argument, string value)
                => $"{argument.Name} must end with '{value}'.";

            public static string StringDoesNotEndWith(in ArgumentInfo<string> argument, string value)
                => $"{argument.Name} cannot end with '{value}'.";

            public static string StringMatches(in ArgumentInfo<string> argument, string pattern)
                => $"No match in {argument.Name} could be found by the regular expression '{pattern}'.";

            public static string StringMatchesTimeout(in ArgumentInfo<string> argument, string pattern, TimeSpan matchTimeout)
                => $"No match in {argument.Name} could be found by the regular expression '{pattern}' in {matchTimeout}";

            public static string StringDoesNotMatch(in ArgumentInfo<string> argument, string pattern)
                => $"A match in {argument.Name} is found by the regular expression '{pattern}'.";

            public static string StringDoesNotMatchTimeout(in ArgumentInfo<string> argument, string pattern, TimeSpan matchTimeout)
                => $"{argument.Name} could not entirely be searched by the regular expression '{pattern}' due to time-out {matchTimeout}";

            public static string True<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be true.";

            public static string False<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be false.";

            public static string GuidEmpty(in ArgumentInfo<Guid> argument)
                => $"{argument.Name} must be empty (uninitialized).";

            public static string GuidNotEmpty(in ArgumentInfo<Guid> argument)
                => $"{argument.Name} cannot be empty. Use Guid.NewGuid() method to initialize a GUID.";

            public static string Enum<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} is not an enum value.";

            public static string EnumDefined<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} is not a defined {typeof(T)} member.";

            public static string EnumNone<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot have any of its bits set.";

            public static string EnumNotNone<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must have at least one of its bits set.";

            public static string EnumHasFlag<T>(in ArgumentInfo<T> argument, T flag)
                => $"{argument.Name} does not has the {flag} flag.";

            public static string EnumDoesNotHaveFlag<T>(in ArgumentInfo<T> argument, T flag)
                => $"{argument.Name} cannot have the {flag} flag.";

            public static string CollectionEmpty<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must be empty.";

            public static string CollectionNotEmpty<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} cannot be empty.";

            public static string CollectionMinCount<T>(in ArgumentInfo<T> argument, int minCount)
                => $"{argument.Name} must contain at least {minCount} items.";

            public static string CollectionMaxCount<T>(in ArgumentInfo<T> argument, int maxCount)
                => $"{argument.Name} cannot contain more than {maxCount} items.";

            public static string CollectionCountInRange<T>(in ArgumentInfo<T> argument, int minCount, int maxCount)
                => $"{argument.Name} must contain {minCount} to {maxCount} items.";

            public static string CollectionContains<TCollection, TItem>(ArgumentInfo<TCollection> argument, TItem item)
                => $"{argument.Name} must contain {item}.";

            public static string CollectionDoesNotContain<TCollection, TItem>(ArgumentInfo<TCollection> argument, TItem item)
                => $"{argument.Name} cannot contain {item}.";

            public static string InCollection<TCollection, TItem>(ArgumentInfo<TItem> argument, TCollection collection)
                where TCollection : IEnumerable<TItem>
                => $"{argument.Name} must be one of {Join(collection)}.";

            public static string NotInCollection<TCollection, TItem>(ArgumentInfo<TItem> argument, TCollection collection)
                where TCollection : IEnumerable<TItem>
                => $"{argument.Name} cannot be one of {Join(collection)}.";

            public static string UriAbsolute(in ArgumentInfo<Uri> argument)
                => $"{argument.Name} must be an absolute URI.";

            public static string UriRelative(in ArgumentInfo<Uri> argument)
                => $"{argument.Name} must be a relative URI.";

            public static string UriScheme(in ArgumentInfo<Uri> argument, string scheme)
                => $"{argument.Name} must be an absolute URI with the {scheme} scheme.";

            public static string UriNotScheme(in ArgumentInfo<Uri> argument, string scheme)
                => $"{argument.Name} cannot have the {scheme} scheme.";

            public static string UriHttp(in ArgumentInfo<Uri> argument)
                => $"{argument.Name} must be an absolute URI with the HTTP scheme.";

            public static string UriHttps(in ArgumentInfo<Uri> argument)
                => $"{argument.Name} must be an absolute URI with the HTTPS scheme.";

#if !NETSTANDARD1_0
            public static string EmailHasHost(in ArgumentInfo<MailAddress> argument, string host)
                => $"{argument.Name} must have the host '{host}'.";

            public static string EmailDoesNotHaveHost(in ArgumentInfo<MailAddress> argument, string host)
                => $"{argument.Name} cannot have the host '{host}'.";

            public static string EmailHostIn(in ArgumentInfo<MailAddress> argument, IEnumerable<string> hosts)
                => $"{argument.Name} must have one of the following hosts: {Join(hosts)}.";

            public static string EmailHostNotIn(in ArgumentInfo<MailAddress> argument, IEnumerable<string> hosts)
                => $"{argument.Name} cannot have one of the following hosts: {Join(hosts)}.";

            public static string EmailHasDisplayName(in ArgumentInfo<MailAddress> argument)
                => $"{argument.Name} must have a display name specified.";

            public static string EmailDoesNotHaveDisplayName(in ArgumentInfo<MailAddress> argument)
                => $"{argument.Name} cannot have a display name specified.";
#endif

            private static string Join<T>(IEnumerable<T> collection)
            {
                return typeof(T) == typeof(string)
                    ? string.Join(", ", collection.Select(i => $"'{i}'"))
                    : string.Join(", ", collection);
            }
        }
    }
}
