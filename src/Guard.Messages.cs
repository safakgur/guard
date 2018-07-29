namespace Dawn
{
    using System;

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
                => $"{argument.Name} must have a positive value.";

            public static string Negative<T>(in ArgumentInfo<T> argument)
                => $"{argument.Name} must have a negative value.";

            public static string InRange<T>(in ArgumentInfo<T> argument, in T minValue, in T maxValue)
                => $"{argument.Name} must be between {minValue} and {maxValue}";

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

            public static string UriAbsolute(in ArgumentInfo<Uri> argument)
                => $"{argument.Name} must be an absolute URI.";

            public static string UriRelative(in ArgumentInfo<Uri> argument)
                => $"{argument.Name} must be a relative URI.";

            public static string UriScheme(in ArgumentInfo<Uri> argument, string scheme)
                => $"{argument.Name} must be an absolute URI with the {scheme} scheme.";

            public static string UriHttp(in ArgumentInfo<Uri> argument)
                => $"{argument.Name} must be an absolute URI with the HTTP scheme.";

            public static string UriHttps(in ArgumentInfo<Uri> argument)
                => $"{argument.Name} must be an absolute URI with the HTTPS scheme.";
        }
    }
}
