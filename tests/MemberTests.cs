namespace Dawn.Tests
{
    using System;
    using System.Globalization;
    using Xunit;

    public sealed class MemberTests : BaseTests
    {
        [Fact(DisplayName = T + "Member w/o valid expression")]
        public void InvalidMemberCall()
        {
            var dateTime = DateTime.Now;
            var dateTimeArg = Guard.Argument(() => dateTime);

            // The final expression is not a member expression.
            Assert.Throws<ArgumentException>(
                "member",
                () => dateTimeArg.Member(dt => dt, dt => { }));

            // Expression is not composed of member expressions.
            Assert.Throws<ArgumentException>(
                "member",
                () => dateTimeArg.Member(dt => dt.ToString().Length, l => { }));

            // Member cannot be accessed.
            var accessException = new NotSupportedException();
            var obj = new TestObjectWithInaccessibleMember(accessException);
            var objArg = Guard.Argument(() => obj);
            var memberException = Assert.Throws<ArgumentException>(
                "member",
                () => objArg.Member(o => o.InaccessibleMember, m => { }));

            Assert.Same(memberException.InnerException, accessException);
        }

        [Theory(DisplayName = T + "Member w/ valid expression")]
        [InlineData(null, 18, 16)]
        [InlineData("08/19/2018 17:42:48", 18, 16)]
        public void ValidMemberCall(string dateTimeString, int hourOrGreater, int lessThanHour)
        {
            var nullableDateTime = dateTimeString is null
                ? default(DateTime?)
                : DateTime.Parse(dateTimeString, CultureInfo.InvariantCulture);

            var nullableTimeArg = Guard.Argument(() => nullableDateTime)
                .Member(dt => dt.TimeOfDay.Hours, h => h.Max(hourOrGreater))
                .Member(dt => dt.TimeOfDay.Hours, h => h.Max(hourOrGreater), true);

            if (!nullableDateTime.HasValue)
            {
                nullableTimeArg
                    .Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour))
                    .Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour), true);

                return;
            }

            var dateTime = nullableDateTime.Value;
            var timeOfDay = dateTime.TimeOfDay;
            var hour = timeOfDay.Hours;

            var innerException = null as Exception;
            var thrown = ThrowsArgumentException(
                nullableTimeArg,
                arg => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour)),
                (arg, message) => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour), (dt, h, ex) =>
                {
                    Assert.Equal(dateTime, dt);
                    Assert.Equal(hour, h);

                    innerException = ex;
                    return message;
                }));

            Assert.Same(thrown[1].InnerException, innerException);

            ThrowsArgumentOutOfRangeException(
                nullableTimeArg,
                arg => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour), true),
                (arg, message) => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour), true, (dt, h, ex) =>
                {
                    Assert.Equal(dateTime, dt);
                    Assert.Equal(hour, h);

                    innerException = ex;
                    return message;
                }));

            var dateTimeArg = Guard.Argument(() => dateTime)
                .Member(dt => dt.TimeOfDay.Hours, h => h.Max(hourOrGreater))
                .Member(dt => dt.TimeOfDay.Hours, h => h.Max(hourOrGreater), true);

            thrown = ThrowsArgumentException(
                dateTimeArg,
                arg => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour)),
                (arg, message) => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour), (dt, h, ex) =>
                {
                    Assert.Equal(dateTime, dt);
                    Assert.Equal(hour, h);

                    innerException = ex;
                    return message;
                }));

            Assert.Same(thrown[1].InnerException, innerException);

            ThrowsArgumentOutOfRangeException(
                dateTimeArg,
                arg => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour), true),
                (arg, message) => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Max(lessThanHour), true, (dt, h, ex) =>
                {
                    Assert.Equal(dateTime, dt);
                    Assert.Equal(hour, h);

                    innerException = ex;
                    return message;
                }));
        }

        private sealed class TestObjectWithInaccessibleMember
        {
            private readonly Exception accessException;

            public TestObjectWithInaccessibleMember(Exception accessException)
                => this.accessException = accessException;

            public object InaccessibleMember => throw this.accessException;
        }
    }
}
