namespace Dawn
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <content>Provides routed member preconditions.</content>
    public static partial class Guard
    {
        /// <summary>Requires a member of the argument to satisfy specified preconditions.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member to validate.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="member">
        ///     An expression that specifies the argument member to validate.
        /// </param>
        /// <param name="validation">The function to test the argument member against.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="member" /> is not composed of <see cref="MemberExpression" />s,
        ///     member value cannot be retrieved using the compiled member expression, or
        ///     <paramref name="validation" /> has thrown an exception.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<T> Member<T, TMember>(
            in this ArgumentInfo<T> argument,
            Expression<Func<T, TMember>> member,
            Action<ArgumentInfo<TMember>> validation,
            Func<T, TMember, Exception, string> message = null)
            => ref argument.Member(member, validation, false, message);

        /// <summary>Requires a member of the argument to satisfy specified preconditions.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member to validate.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="member">
        ///     An expression that specifies the argument member to validate.
        /// </param>
        /// <param name="validation">The function to test the argument member against.</param>
        /// <param name="validatesRange">
        ///     Pass <c>true</c> to throw an <see cref="ArgumentOutOfRangeException" /> instead of
        ///     an <see cref="ArgumentException" /> if the precondition is not satisfied.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="member" /> is not composed of <see cref="MemberExpression" />s,
        ///     member value cannot be retrieved using the compiled member expression, or
        ///     <paramref name="validation" /> has thrown an exception when
        ///     <paramref name="validatesRange" /> passed <c>false</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="validation" /> has thrown an exception when
        ///     <paramref name="validatesRange" /> passed <c>true</c>.
        /// </exception>
        public static ref readonly ArgumentInfo<T> Member<T, TMember>(
            in this ArgumentInfo<T> argument,
            Expression<Func<T, TMember>> member,
            Action<ArgumentInfo<TMember>> validation,
            bool validatesRange,
            Func<T, TMember, Exception, string> message = null)
        {
            if (argument.HasValue() && member != null && validation != null)
            {
                // Get member info.
                ArgumentMemberInfo<T, TMember> info;
                try
                {
                    info = ArgumentMemberInfo.GetMemberInfo(member);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(ex.Message, nameof(member), ex);
                }

                // Get member value.
                TMember memberValue;
                try
                {
                    memberValue = info.GetValue(argument.Value);
                }
                catch (Exception ex)
                {
                    var m = $"{argument.Name}.{member.Name} cannot be retrieved.";
                    throw new ArgumentException(m, nameof(member), ex);
                }

                // Validate the member.
                var memberArgument = Argument(memberValue, info.Name, argument.Secure);
                try
                {
                    validation(memberArgument);
                }
                catch (Exception ex)
                {
                    var m = message?.Invoke(argument.Value, memberValue, ex) ?? ex.Message;
                    throw !validatesRange || argument.Modified
                        ? new ArgumentException(m, argument.Name, ex)
                        : new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
                }
            }

            return ref argument;
        }

        /// <summary>Requires a member of the argument to satisfy specified preconditions.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member to validate.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="member">
        ///     An expression that specifies the argument member to validate.
        /// </param>
        /// <param name="validation">The function to test the argument member against.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="member" /> is not composed of <see cref="MemberExpression" />s,
        ///     member value cannot be retrieved using the compiled member expression, or
        ///     <paramref name="validation" /> has thrown an exception.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<T?> Member<T, TMember>(
            in this ArgumentInfo<T?> argument,
            Expression<Func<T, TMember>> member,
            Action<ArgumentInfo<TMember>> validation,
            Func<T, TMember, Exception, string> message = null)
            where T : struct
            => ref argument.Member(member, validation, false, message);

        /// <summary>Requires a member of the argument to satisfy specified preconditions.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member to validate.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="member">
        ///     An expression that specifies the argument member to validate.
        /// </param>
        /// <param name="validation">The function to test the argument member against.</param>
        /// <param name="validatesRange">
        ///     Pass <c>true</c> to throw an <see cref="ArgumentOutOfRangeException" /> instead of
        ///     an <see cref="ArgumentException" /> if the precondition is not satisfied.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="member" /> is not composed of <see cref="MemberExpression" />s,
        ///     member value cannot be retrieved using the compiled member expression, or
        ///     <paramref name="validation" /> has thrown an exception when
        ///     <paramref name="validatesRange" /> passed <c>false</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="validation" /> has thrown an exception when
        ///     <paramref name="validatesRange" /> passed <c>true</c>.
        /// </exception>
        public static ref readonly ArgumentInfo<T?> Member<T, TMember>(
            in this ArgumentInfo<T?> argument,
            Expression<Func<T, TMember>> member,
            Action<ArgumentInfo<TMember>> validation,
            bool validatesRange,
            Func<T, TMember, Exception, string> message = null)
            where T : struct
        {
            if (argument.HasValue() && member != null && validation != null)
            {
                // Get member info.
                ArgumentMemberInfo<T, TMember> info;
                try
                {
                    info = ArgumentMemberInfo.GetMemberInfo(member);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(ex.Message, nameof(member), ex);
                }

                // Get member value.
                TMember memberValue;
                try
                {
                    memberValue = info.GetValue(argument.Value.Value);
                }
                catch (Exception ex)
                {
                    var m = $"{argument.Name}.{member.Name} cannot be retrieved.";
                    throw new ArgumentException(m, nameof(member), ex);
                }

                // Validate the member.
                var memberArgument = Argument(memberValue, info.Name, argument.Secure);
                try
                {
                    validation(memberArgument);
                }
                catch (Exception ex)
                {
                    var m = message?.Invoke(argument.Value.Value, memberValue, ex) ?? ex.Message;
                    throw !validatesRange || argument.Modified
                        ? new ArgumentException(m, argument.Name, ex)
                        : new ArgumentOutOfRangeException(argument.Name, argument.Value, m);
                }
            }

            return ref argument;
        }

        /// <summary>Represents an argument member.</summary>
        private abstract class ArgumentMemberInfo
        {
            /// <summary>Argument members cached by their distances to the root type.</summary>
            private static readonly IDictionary<(MemberInfo, int), ArgumentMemberInfo> Cache
                = new Dictionary<(MemberInfo, int), ArgumentMemberInfo>();

            /// <summary>The lock that synchronizes access to <see cref="Cache" />.</summary>
            private static readonly ReaderWriterLockSlim CacheLock
                = new ReaderWriterLockSlim();

            /// <summary>
            ///     Returns the cached argument member for the specified lambda expression.
            /// </summary>
            /// <typeparam name="T">The type of the argument.</typeparam>
            /// <typeparam name="TMember">The type of the argument member.</typeparam>
            /// <param name="lexp">
            ///     The lambda expression that specifies the argument member to get.
            /// </param>
            /// <returns>A cached, generic argument member.</returns>
            public static ArgumentMemberInfo<T, TMember> GetMemberInfo<T, TMember>(Expression<Func<T, TMember>> lexp)
            {
                var mexp = lexp.Body as MemberExpression;
                if (mexp is null)
                    throw new ArgumentException("A member expression is expected.", nameof(lexp));

                var rootType = typeof(T);

                var i = 0;
                try
                {
                    for (var e = mexp; rootType != e.Member.DeclaringType; i++)
                        e = e.Expression as MemberExpression;
                }
                catch (NullReferenceException ex)
                {
                    var m = "The expression must be composed of member accesses.";
                    throw new ArgumentException(m, nameof(lexp), ex);
                }

                CacheLock.EnterUpgradeableReadLock();
                try
                {
                    var key = (mexp.Member, i);
                    if (!Cache.TryGetValue(key, out var info))
                    {
                        info = new ArgumentMemberInfo<T, TMember>(mexp, lexp.Compile());
                        CacheLock.EnterWriteLock();
                        try
                        {
                            Cache[key] = info;
                        }
                        finally
                        {
                            CacheLock.ExitWriteLock();
                        }
                    }

                    return info as ArgumentMemberInfo<T, TMember>;
                }
                finally
                {
                    CacheLock.ExitUpgradeableReadLock();
                }
            }
        }

        /// <summary>Represents a generic argument member.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member.</typeparam>
        private sealed class ArgumentMemberInfo<T, TMember> : ArgumentMemberInfo
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="ArgumentMemberInfo{T, TMember}" />
            ///     class.
            /// </summary>
            /// <param name="mexp">The member expression.</param>
            /// <param name="getValue">
            ///     A function that returns the member value from the argument it belongs to.
            /// </param>
            public ArgumentMemberInfo(MemberExpression mexp, Func<T, TMember> getValue)
            {
                var memberName = mexp.ToString();

                this.Name = memberName.Substring(memberName.IndexOf('.') + 1);
                this.GetValue = getValue;
            }

            /// <summary>Gets the member name.</summary>
            public string Name { get; }

            /// <summary>
            ///     Gets a function that returns the member value from the argument it belongs to.
            /// </summary>
            public Func<T, TMember> GetValue { get; }
        }
    }
}
