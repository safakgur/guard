using System;
using System.Globalization;
using Xunit;

namespace Dawn.Tests
{
    public sealed class MemberTests : BaseTests
    {
        [Fact(DisplayName = "Member w/o valid expression")]
        public void InvalidMemberCall()
        {
            // Nullable struct

            var nullableTime = DateTime.UtcNow as DateTime?;
            var nullableTimeArg = Guard.Argument(() => nullableTime);

            // The final expression is not a member expression.
            Assert.Throws<ArgumentException>(
                "member",
                () => nullableTimeArg.Member(dt => dt, dt => { }));

            // Expression is not composed of member expressions.
            Assert.Throws<ArgumentException>(
                "member",
                () => nullableTimeArg.Member(dt => dt.Day + 2, l => { }));

            // Member cannot be accessed.
            var accessException = new NotSupportedException();
            var nullableObj = new TestObjectWithInaccessibleMember(accessException) as TestObjectWithInaccessibleMember?;
            var nullableObjArg = Guard.Argument(() => nullableObj);
            var memberException = Assert.Throws<ArgumentException>(
                "member",
                () => nullableObjArg.Member(o => o.InaccessibleMember, m => { }));

            Assert.Same(memberException.InnerException, accessException);

            // Regular struct

            var time = nullableTime.Value;
            var timeArg = Guard.Argument(() => time);

            // The final expression is not a member expression.
            Assert.Throws<ArgumentException>(
                "member",
                () => timeArg.Member(dt => dt, dt => { }));

            // Expression is not composed of member expressions.
            Assert.Throws<ArgumentException>(
                "member",
                () => timeArg.Member(dt => dt.ToString().Length, l => { }));

            // Member cannot be accessed.
            var obj = nullableObj.Value;
            var objArg = Guard.Argument(() => obj);
            memberException = Assert.Throws<ArgumentException>(
                "member",
                () => objArg.Member(o => o.InaccessibleMember, m => { }));

            Assert.Same(memberException.InnerException, accessException);
        }

        [Theory(DisplayName = "Member w/ valid expression")]
        [InlineData(null, 17, 18, false)]
        [InlineData("08/19/2018 17:42:48", 17, 18, false)]
        [InlineData("08/19/2018 17:42:48", 17, 18, true)]
        public void ValidMemberCall(string dateTimeString, int hour, int nonHour, bool secure)
        {
            var nullableTime = dateTimeString is null
                ? default(DateTime?)
                : DateTime.Parse(dateTimeString, CultureInfo.InvariantCulture);

            var nullableTimeArg = Guard.Argument(() => nullableTime, secure)
                .Member(dt => dt.TimeOfDay.Hours, h => h.Equal(hour))
                .Member(dt => dt.TimeOfDay.Hours, h => h.Equal(hour), true);

            if (!nullableTime.HasValue)
            {
                nullableTimeArg
                    .Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour))
                    .Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour), true);

                return;
            }

            var dateTime = nullableTime.Value;

            var innerException = null as Exception;
            var thrown = ThrowsArgumentException(
                nullableTimeArg,
                arg => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour)),
                TestGeneratedMessage,
                (arg, message) => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour), (dt, h, ex) =>
                {
                    Assert.Equal(dateTime, dt);
                    Assert.Equal(hour, h);

                    TestGeneratedMessage(ex.Message);
                    innerException = ex;
                    return message;
                }));

            Assert.Same(thrown[1].InnerException, innerException);

            ThrowsArgumentOutOfRangeException(
                nullableTimeArg,
                arg => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour), true),
                TestGeneratedMessage,
                (arg, message) => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour), true, (dt, h, ex) =>
                {
                    Assert.Equal(dateTime, dt);
                    Assert.Equal(hour, h);

                    TestGeneratedMessage(ex.Message);
                    return message;
                }));

            var dateTimeArg = Guard.Argument(() => dateTime, secure)
                .Member(dt => dt.TimeOfDay.Hours, h => h.Equal(hour))
                .Member(dt => dt.TimeOfDay.Hours, h => h.Equal(hour), true);

            thrown = ThrowsArgumentException(
                dateTimeArg,
                arg => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour)),
                TestGeneratedMessage,
                (arg, message) => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour), (dt, h, ex) =>
                {
                    Assert.Equal(dateTime, dt);
                    Assert.Equal(hour, h);

                    TestGeneratedMessage(ex.Message);
                    innerException = ex;
                    return message;
                }));

            Assert.Same(thrown[1].InnerException, innerException);

            ThrowsArgumentOutOfRangeException(
                dateTimeArg,
                arg => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour), true),
                TestGeneratedMessage,
                (arg, message) => arg.Member(dt => dt.TimeOfDay.Hours, h => h.Equal(nonHour), true, (dt, h, ex) =>
                {
                    Assert.Equal(dateTime, dt);
                    Assert.Equal(hour, h);

                    TestGeneratedMessage(ex.Message);
                    return message;
                }));

            bool TestGeneratedMessage(string message)
                => secure != message.Contains(nonHour.ToString());
        }

        private struct TestObjectWithInaccessibleMember
        {
            private readonly Exception _accessException;

            public TestObjectWithInaccessibleMember(Exception accessException)
                => _accessException = accessException;

            public object InaccessibleMember => throw _accessException;
        }
    }
}
