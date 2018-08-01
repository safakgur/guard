namespace Dawn.Tests
{
    using System;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = T + "Guard supports type preconditions.")]
        public void GuardSupportsTypes()
        {
            var message = RandomMessage;

            var nullRef = null as object;
            var nullRefArg = Guard.Argument(() => nullRef);

            var nullVal = default(int?) as object;
            var nullValArg = Guard.Argument(() => nullVal);

            // Generic type check for class.
            nullRefArg.Type<string>();
            nullValArg.Type<string>();

            var @ref = "1" as object;
            var refArg = Guard.Argument(() => @ref);

            string s = refArg.Type<string>();
            Assert.Same(@ref, s);

            Assert.Throws<ArgumentException>(
                nameof(@ref), () => Guard.Argument(() => @ref).Type<int>());

            var ex = Assert.Throws<ArgumentException>(
                nameof(@ref), () => Guard.Argument(() => @ref).Type<int>(o =>
                {
                    Assert.Same(@ref, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Generic type check for struct.
            Assert.Throws<ArgumentException>(
                nameof(nullRef), () => Guard.Argument(() => nullRef).Type<int>());

            ex = Assert.Throws<ArgumentException>(
                nameof(nullRef), () => Guard.Argument(() => nullRef).Type<int>(o =>
                {
                    Assert.Same(nullRef, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(nullVal), () => Guard.Argument(() => nullVal).Type<int>());

            ex = Assert.Throws<ArgumentException>(
                nameof(nullVal), () => Guard.Argument(() => nullVal).Type<int>(o =>
                {
                    Assert.Same(nullVal, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            var val = 1 as object;
            var valArg = Guard.Argument(() => val);

            int i = valArg.Type<int>();
            Assert.Equal(val, i);

            Assert.Throws<ArgumentException>(
                nameof(val), () => Guard.Argument(() => val).Type<string>());

            ex = Assert.Throws<ArgumentException>(
                nameof(val), () => Guard.Argument(() => val).Type<string>(o =>
                {
                    Assert.Same(val, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Generic type check for nullable struct.
            nullRefArg.Type<int?>();
            nullValArg.Type<int?>();

            var nil = new int?(1) as object;
            var nilArg = Guard.Argument(() => nil);

            int? n = nilArg.Type<int?>();
            Assert.Equal(nil, n);

            Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).Type<string>());

            ex = Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).Type<string>(o =>
                {
                    Assert.Same(nil, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Generic not-type check for class.
            nullRefArg.NotType<int>();
            nullValArg.NotType<int>();

            refArg.NotType<int>();

            Assert.Throws<ArgumentException>(
                nameof(@ref), () => Guard.Argument(() => @ref).NotType<string>());

            ex = Assert.Throws<ArgumentException>(
                nameof(@ref), () => Guard.Argument(() => @ref).NotType<string>(o =>
                {
                    Assert.Same(@ref, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Generic not-type check for struct
            nullRefArg.NotType<string>();
            nullValArg.NotType<string>();

            valArg.NotType<string>();

            Assert.Throws<ArgumentException>(
                nameof(val), () => Guard.Argument(() => val).NotType<int>());

            ex = Assert.Throws<ArgumentException>(
                nameof(val), () => Guard.Argument(() => val).NotType<int>(o =>
                {
                    Assert.Equal(val, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Generic not-type check for nullable struct.
            nullRefArg.NotType<int?>();
            nullValArg.NotType<int?>();

            nilArg.NotType<string>();

            Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).NotType<int?>());

            ex = Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).NotType<int?>(o =>
                {
                    Assert.Equal(nil, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).NotType<int>());

            ex = Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).NotType<int>(o =>
                {
                    Assert.Equal(nil, o);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Non-generic type check for class.
            nullRefArg.Type(typeof(string));
            nullValArg.Type(typeof(string));

            refArg.Type(typeof(string));

            Assert.Throws<ArgumentException>(
                nameof(@ref), () => Guard.Argument(() => @ref).Type(typeof(int)));

            ex = Assert.Throws<ArgumentException>(
                nameof(@ref), () => Guard.Argument(() => @ref).Type(typeof(int), (o, type) =>
                {
                    Assert.Same(@ref, o);
                    Assert.Same(typeof(int), type);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Non-generic type check for struct.
            Guard.Argument(() => nullRef).Type(typeof(int));
            Guard.Argument(() => nullVal).Type(typeof(int));

            valArg.Type(typeof(int));

            Assert.Throws<ArgumentException>(
                nameof(val), () => Guard.Argument(() => val).Type(typeof(string)));

            ex = Assert.Throws<ArgumentException>(
                nameof(val), () => Guard.Argument(() => val).Type(typeof(string), (o, type) =>
                {
                    Assert.Same(val, o);
                    Assert.Same(typeof(string), type);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Non-generic type check for nullable struct.
            nullRefArg.Type(typeof(int?));
            nullValArg.Type(typeof(int?));

            nilArg.Type(typeof(int?));

            Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).Type(typeof(string)));

            ex = Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).Type(typeof(string), (o, type) =>
                {
                    Assert.Same(nil, o);
                    Assert.Same(typeof(string), type);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Non-generic not-type check for class.
            nullRefArg.NotType(typeof(int));
            nullValArg.NotType(typeof(int));

            refArg.NotType(typeof(int));

            Assert.Throws<ArgumentException>(
                nameof(@ref), () => Guard.Argument(() => @ref).NotType(typeof(string)));

            ex = Assert.Throws<ArgumentException>(
                nameof(@ref), () => Guard.Argument(() => @ref).NotType(typeof(string), (o, type) =>
                {
                    Assert.Same(@ref, o);
                    Assert.Same(typeof(string), type);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Non-generic not-type check for struct
            nullRefArg.NotType(typeof(string));
            nullValArg.NotType(typeof(string));

            valArg.NotType(typeof(string));

            Assert.Throws<ArgumentException>(
                nameof(val), () => Guard.Argument(() => val).NotType(typeof(int)));

            ex = Assert.Throws<ArgumentException>(
                nameof(val), () => Guard.Argument(() => val).NotType(typeof(int), (o, type) =>
                {
                    Assert.Same(val, o);
                    Assert.Same(typeof(int), type);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            // Non-generic not-type check for nullable struct.
            nullRefArg.NotType(typeof(int?));
            nullValArg.NotType(typeof(int?));

            nilArg.NotType(typeof(string));

            Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).NotType(typeof(int?)));

            ex = Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).NotType(typeof(int?), (o, type) =>
                {
                    Assert.Same(nil, o);
                    Assert.Same(typeof(int?), type);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).NotType(typeof(int)));

            ex = Assert.Throws<ArgumentException>(
                nameof(nil), () => Guard.Argument(() => nil).NotType(typeof(int), (o, type) =>
                {
                    Assert.Same(nil, o);
                    Assert.Same(typeof(int), type);
                    return message;
                }));

            Assert.StartsWith(message, ex.Message);
        }
    }
}
