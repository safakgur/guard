namespace Dawn.Tests
{
    using Xunit;

    public sealed class TypeTests : BaseTests
    {
        [Theory(DisplayName = T + "Type: Type/NotType (generic class)")]
        [InlineData(null)]
        [InlineData("A")]
        public void GenericReferenceType(string value)
        {
            var valueArg = Guard.Argument(value as object, nameof(value));
            value = valueArg.Type<string>();
            valueArg.NotType<int>();

            if (value is null)
            {
                ThrowsArgumentException(
                    valueArg,
                    arg => arg.Type<int>(),
                    (arg, message) => arg.Type<int>(o =>
                    {
                        Assert.Same(value, o);
                        return message;
                    }));

                valueArg.NotType<string>();
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.Type<int>(),
                (arg, message) => arg.Type<int>(o =>
                {
                    Assert.Same(value, o);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotType<string>(),
                (arg, message) => arg.NotType<string>(o =>
                {
                    Assert.Same(value, o);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Type: Type/NotType (non-generic class)")]
        [InlineData(null)]
        [InlineData("A")]
        public void ReferenceType(string value)
        {
            var stringType = typeof(string);
            var intType = typeof(int);
            var valueArg = Guard.Argument(value as object, nameof(value))
                .Type(stringType)
                .NotType(intType);

            if (value is null)
            {
                valueArg.Type(intType);
                valueArg.NotType(intType);
                return;
            }

            ThrowsArgumentException(
                valueArg,
                arg => arg.Type(intType),
                (arg, message) => arg.Type(intType, (o, t) =>
                {
                    Assert.Same(value, o);
                    Assert.Same(intType, t);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotType(stringType),
                (arg, message) => arg.NotType(stringType, (o, t) =>
                {
                    Assert.Same(value, o);
                    Assert.Same(stringType, t);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Type: Type/NotType (generic nullable struct)")]
        [InlineData(null)]
        [InlineData(1)]
        public void GenericValueType(int? value)
        {
            var valueArg = Guard.Argument(value as object, nameof(value));
            value = valueArg.Type<int?>();
            valueArg.NotType<string>();

            if (value is null)
            {
                ThrowsArgumentException(
                    valueArg,
                    arg => arg.Type<double>(),
                    (arg, message) => arg.Type<double>(o =>
                    {
                        Assert.Equal(value, o);
                        return message;
                    }));

                valueArg.Type<double?>();
                valueArg.Type<string>();
                valueArg.NotType<string>();
                return;
            }

            int i = valueArg.Type<int>();

            ThrowsArgumentException(
                valueArg,
                arg => arg.Type<string>(),
                (arg, message) => arg.Type<string>(o =>
                {
                    Assert.Equal(value, o);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotType<int?>(),
                (arg, message) => arg.NotType<int?>(o =>
                {
                    Assert.Equal(value, o);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotType<int>(),
                (arg, message) => arg.NotType<int>(o =>
                {
                    Assert.Equal(value, o);
                    return message;
                }));
        }

        [Theory(DisplayName = T + "Type: Type/NotType (non-generic nullable struct)")]
        [InlineData(null)]
        [InlineData(1)]
        public void ValueType(int? value)
        {
            var intType = typeof(int);
            var nullableIntType = typeof(int?);
            var stringType = typeof(string);
            var doubleType = typeof(double);

            var valueArg = Guard.Argument(value as object, nameof(value))
                .Type(nullableIntType)
                .NotType(stringType)
                .NotType(doubleType);

            if (value is null)
            {
                valueArg
                    .Type(intType)
                    .Type(doubleType)
                    .Type(stringType)
                    .NotType(nullableIntType);

                return;
            }

            valueArg.Type(intType);

            ThrowsArgumentException(
                valueArg,
                arg => arg.Type(stringType),
                (arg, message) => arg.Type(stringType, (o, t) =>
                {
                    Assert.Equal(value, o);
                    Assert.Same(stringType, t);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotType(nullableIntType),
                (arg, message) => arg.NotType(nullableIntType, (o, t) =>
                {
                    Assert.Equal(value, o);
                    Assert.Same(nullableIntType, t);
                    return message;
                }));

            ThrowsArgumentException(
                valueArg,
                arg => arg.NotType(intType),
                (arg, message) => arg.NotType(intType, (o, t) =>
                {
                    Assert.Equal(value, o);
                    Assert.Same(intType, t);
                    return message;
                }));
        }
    }
}
