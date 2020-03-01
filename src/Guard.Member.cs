#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides routed member preconditions.</content>
    public static partial class Guard
    {
        /// <summary>Requires a member of the argument to satisfy specified preconditions.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member to validate.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="member">An expression that specifies the argument member to validate.</param>
        /// <param name="validation">The function to test the argument member against.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="member" /> is not composed of <see cref="MemberExpression" /> s,
        ///     member value cannot be retrieved using the compiled member expression, or
        ///     <paramref name="validation" /> has thrown an exception.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Member", "gmem")]
        public static ref readonly ArgumentInfo<T> Member<T, TMember>(
            in this ArgumentInfo<T> argument,
            Expression<Func<T, TMember>> member,
            Action<ArgumentInfo<TMember>> validation,
            Func<T, TMember, Exception, string>? message = null)
            => ref argument.Member(member, validation, false, message);

        /// <summary>Requires a member of the argument to satisfy specified preconditions.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member to validate.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="member">An expression that specifies the argument member to validate.</param>
        /// <param name="validation">The function to test the argument member against.</param>
        /// <param name="validatesRange">
        ///     Pass <c>true</c> to throw an <see cref="ArgumentOutOfRangeException" /> instead of an
        ///     <see cref="ArgumentException" /> if the precondition is not satisfied.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="member" /> is not composed of <see cref="MemberExpression" /> s,
        ///     member value cannot be retrieved using the compiled member expression, or
        ///     <paramref name="validation" /> has thrown an exception when
        ///     <paramref name="validatesRange" /> passed <c>false</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="validation" /> has thrown an exception when
        ///     <paramref name="validatesRange" /> passed <c>true</c>.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Member", "gmemr")]
        public static ref readonly ArgumentInfo<T> Member<T, TMember>(
            in this ArgumentInfo<T> argument,
            Expression<Func<T, TMember>> member,
            Action<ArgumentInfo<TMember>> validation,
            bool validatesRange,
            Func<T, TMember, Exception, string>? message = null)
        {
            if (argument.HasValue && member != null && validation != null)
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
                    throw Fail(!validatesRange || argument.Modified
                        ? new ArgumentException(m, argument.Name, ex)
                        : new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m));
                }
            }

            return ref argument;
        }

        /// <summary>Requires a member of the argument to satisfy specified preconditions.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member to validate.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="member">An expression that specifies the argument member to validate.</param>
        /// <param name="validation">The function to test the argument member against.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="member" /> is not composed of <see cref="MemberExpression" /> s,
        ///     member value cannot be retrieved using the compiled member expression, or
        ///     <paramref name="validation" /> has thrown an exception.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Member", "gmem")]
        public static ref readonly ArgumentInfo<T?> Member<T, TMember>(
            in this ArgumentInfo<T?> argument,
            Expression<Func<T, TMember>> member,
            Action<ArgumentInfo<TMember>> validation,
            Func<T, TMember, Exception, string>? message = null)
            where T : struct
            => ref argument.Member(member, validation, false, message);

        /// <summary>Requires a member of the argument to satisfy specified preconditions.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member to validate.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="member">An expression that specifies the argument member to validate.</param>
        /// <param name="validation">The function to test the argument member against.</param>
        /// <param name="validatesRange">
        ///     Pass <c>true</c> to throw an <see cref="ArgumentOutOfRangeException" /> instead of an
        ///     <see cref="ArgumentException" /> if the precondition is not satisfied.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="member" /> is not composed of <see cref="MemberExpression" /> s,
        ///     member value cannot be retrieved using the compiled member expression, or
        ///     <paramref name="validation" /> has thrown an exception when
        ///     <paramref name="validatesRange" /> passed <c>false</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="validation" /> has thrown an exception when
        ///     <paramref name="validatesRange" /> passed <c>true</c>.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Member", "gmemr")]
        public static ref readonly ArgumentInfo<T?> Member<T, TMember>(
            in this ArgumentInfo<T?> argument,
            Expression<Func<T, TMember>> member,
            Action<ArgumentInfo<TMember>> validation,
            bool validatesRange,
            Func<T, TMember, Exception, string>? message = null)
            where T : struct
        {
            if (argument.HasValue && member != null && validation != null)
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
                    memberValue = info.GetValue(argument.GetValueOrDefault());
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
                    var m = message?.Invoke(argument.GetValueOrDefault(), memberValue, ex) ?? ex.Message;
                    throw Fail(!validatesRange || argument.Modified
                        ? new ArgumentException(m, argument.Name, ex)
                        : new ArgumentOutOfRangeException(argument.Name, argument.Secure ? null : argument.Value as object, m));
                }
            }

            return ref argument;
        }

        /// <summary>Represents an argument member.</summary>
        private abstract class ArgumentMemberInfo
        {
            /// <summary>Cached root nodes of member trees.</summary>
            private static readonly IDictionary<MemberInfo, Node> Nodes
                = new Dictionary<MemberInfo, Node>();

            /// <summary>The lock that synchronizes access to <see cref="Nodes" />.</summary>
            private static readonly ReaderWriterLockSlim NodesLock
                = new ReaderWriterLockSlim();

            /// <summary>Returns the cached argument member for the specified lambda expression.</summary>
            /// <typeparam name="T">The type of the argument.</typeparam>
            /// <typeparam name="TMember">The type of the argument member.</typeparam>
            /// <param name="lexp">
            ///     The lambda expression that specifies the argument member to get.
            /// </param>
            /// <returns>A cached, generic argument member.</returns>
            /// <exception cref="ArgumentException">
            ///     <paramref name="lexp" /> is not composed of <see cref="MemberExpression" /> s.
            /// </exception>
            public static ArgumentMemberInfo<T, TMember> GetMemberInfo<T, TMember>(Expression<Func<T, TMember>> lexp)
            {
                Node? node = null;

                var exp = lexp.Body;
                while (exp is MemberExpression e)
                {
                    IDictionary<MemberInfo, Node> source;
                    ReaderWriterLockSlim sourceLock;
                    if (node is null)
                    {
                        source = Nodes;
                        sourceLock = NodesLock;
                    }
                    else
                    {
                        source = node.Owners;
                        sourceLock = node.Lock;
                    }

                    sourceLock.EnterUpgradeableReadLock();
                    try
                    {
                        if (!source.TryGetValue(e.Member, out node))
                        {
                            sourceLock.EnterWriteLock();
                            try
                            {
                                node = new Node();
                                source[e.Member] = node;
                            }
                            finally
                            {
                                sourceLock.ExitWriteLock();
                            }
                        }
                    }
                    finally
                    {
                        sourceLock.ExitUpgradeableReadLock();
                    }

                    exp = e.Expression;
                }

                if (node is null || exp.NodeType != ExpressionType.Parameter)
                {
                    var m = "The selector must be composed of member accesses.";
                    throw new ArgumentException(m, nameof(lexp));
                }

                node.Lock.EnterUpgradeableReadLock();
                try
                {
                    if (node.Info is ArgumentMemberInfo<T, TMember> info)
                        return info;

                    node.Lock.EnterWriteLock();
                    try
                    {
                        info = new ArgumentMemberInfo<T, TMember>((lexp.Body as MemberExpression)!, lexp.Compile());
                        node.Info = info;
                    }
                    finally
                    {
                        node.Lock.ExitWriteLock();
                    }

                    return info;
                }
                finally
                {
                    node.Lock.ExitUpgradeableReadLock();
                }
            }

            /// <summary>Represents a node in a tree of members.</summary>
            private sealed class Node
            {
                /// <summary>Initializes a new instance of the <see cref="Node" /> class.</summary>
                public Node()
                {
                    Owners = new Dictionary<MemberInfo, Node>(1);
                    Lock = new ReaderWriterLockSlim();
                }

                /// <summary>Gets the owners of the member that the current node represents.</summary>
                public IDictionary<MemberInfo, Node> Owners { get; }

                /// <summary>The lock that synchronizes access to the node.</summary>
                public ReaderWriterLockSlim Lock { get; }

                /// <summary>Gets or sets the argument member that the current node represents.</summary>
                public ArgumentMemberInfo? Info { get; set; }
            }
        }

        /// <summary>Represents a generic argument member.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TMember">The type of the argument member.</typeparam>
        private sealed class ArgumentMemberInfo<T, TMember> : ArgumentMemberInfo
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="ArgumentMemberInfo{T, TMember}" /> class.
            /// </summary>
            /// <param name="mexp">The member expression.</param>
            /// <param name="getValue">
            ///     A function that returns the member value from the argument it belongs to.
            /// </param>
            public ArgumentMemberInfo(MemberExpression mexp, Func<T, TMember> getValue)
            {
                var memberName = mexp.ToString();

                Name = memberName.Substring(memberName.IndexOf('.') + 1);
                GetValue = getValue;
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
