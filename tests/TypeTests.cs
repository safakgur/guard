using System.IO;
using Xunit;

namespace Dawn.Tests
{
    public sealed class TypeTests : BaseTests
    {
        [Theory(DisplayName = "Type: Type/NotType (generic class)")]
        [InlineData(null)]
        [InlineData("A")]
        public void GenericReferenceType(string value)
        {
            for (var i = 0; i < 2; i++)
            {
                var arg = new Guard.ArgumentInfo<object>(
                    value, nameof(value), i % 2 == 0, i % 2 != 0);

                var typedArg = arg.Type<string>();
                Assert.IsType<Guard.ArgumentInfo<string>>(typedArg);
                Assert.Equal(arg.Modified, typedArg.Modified);
                Assert.Equal(arg.Secure, typedArg.Secure);
            }

            var valueArg = Guard.Argument(value as object, nameof(value))
                .NotType<int>();

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

        [Theory(DisplayName = "Type: Type/NotType (non-generic class)")]
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

        [Theory(DisplayName = "Type: Type/NotType (generic nullable struct)")]
        [InlineData(null)]
        [InlineData(1)]
        public void GenericValueType(int? value)
        {
            for (var i = 0; i < 2; i++)
            {
                var arg = new Guard.ArgumentInfo<object>(
                    value, nameof(value), i % 2 == 0, i % 2 != 0);

                var typedArg = arg.Type<int?>();
                Assert.IsType<Guard.ArgumentInfo<int?>>(typedArg);
                Assert.Equal(arg.Modified, typedArg.Modified);
                Assert.Equal(arg.Secure, typedArg.Secure);
            }

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

            valueArg.Type<int>();

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

        [Theory(DisplayName = "Type: Type/NotType (non-generic nullable struct)")]
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

        [Fact(DisplayName = "Type: Compatible/NotCompatible")]
        public void Compatible()
        {
            using var memory = new MemoryStream() as Stream;
            var @null = null as Stream;
            var nullArg = Guard.Argument(() => @null)
                .Compatible<object>()
                .Compatible<MemoryStream>()
                .Compatible<string>();

            var memoryArg = Guard.Argument(() => memory)
                .Compatible<object>()
                .Compatible<MemoryStream>();

            ThrowsArgumentException(
                memoryArg,
                arg => arg.Compatible<string>(),
                (arg, message) => arg.Compatible<string>(s =>
                {
                    Assert.Same(memory, s);
                    return message;
                }));

            nullArg
                .NotCompatible<object>()
                .NotCompatible<MemoryStream>()
                .NotCompatible<string>();

            memoryArg.NotCompatible<string>();

            ThrowsArgumentException(
                memoryArg,
                arg => arg.NotCompatible<object>(),
                (arg, message) => arg.NotCompatible<object>(o =>
                {
                    Assert.Same(memory, o);
                    return message;
                }));

            ThrowsArgumentException(
                memoryArg,
                arg => arg.NotCompatible<MemoryStream>(),
                (arg, message) => arg.NotCompatible<MemoryStream>(s =>
                {
                    Assert.Same(memory, s);
                    return message;
                }));
        }

        [Fact(DisplayName = "Type: Cast")]
        public void Cast()
        {
            using var stream = new MemoryStream() as Stream;
            var @null = null as Stream;
            var nullArg = Guard.Argument(() => @null);

            ThrowsArgumentException(
                nullArg,
                arg => arg.Cast<object>(),
                (arg, message) => arg.Cast<object>(s =>
                {
                    Assert.Null(s);
                    return message;
                }));

            ThrowsArgumentException(
                nullArg,
                arg => arg.Cast<MemoryStream>(),
                (arg, message) => arg.Cast<MemoryStream>(s =>
                {
                    Assert.Null(s);
                    return message;
                }));

            for (var i = 0; i < 2; i++)
            {
                var streamArg = Guard.Argument(() => stream, i == 1);

                var objectCastedArg = streamArg.Cast<object>();
                Assert.Same(streamArg.Name, objectCastedArg.Name);
                Assert.Equal(streamArg.Modified, objectCastedArg.Modified);
                Assert.Equal(streamArg.Secure, objectCastedArg.Secure);
                Assert.Same(stream, objectCastedArg.Value);

                var msCastedArg = streamArg.Cast<MemoryStream>();
                Assert.Equal(streamArg.Modified, msCastedArg.Modified);
                Assert.Equal(streamArg.Secure, msCastedArg.Secure);
                Assert.Same(stream, msCastedArg.Value);

                ThrowsArgumentException(
                    streamArg,
                    arg => arg.Cast<string>(),
                    (arg, message) => arg.Cast<string>(s =>
                    {
                        Assert.Same(stream, s);
                        return message;
                    }));
            }
        }
    }
}
