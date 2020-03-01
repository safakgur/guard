#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;

namespace Dawn
{
    /// <content>Provides preconditions for <see cref="IEnumerable" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>Requires the argument to have a collection value that is empty.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> has one or more items.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gem")]
        public static ref readonly ArgumentInfo<TCollection> Empty<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && Collection<TCollection>.Count(argument.Value, 1) != 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.CollectionEmpty(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the argument to have a collection value that is not empty.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> is not <c>null</c> and has no items.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gnem")]
        public static ref readonly ArgumentInfo<TCollection> NotEmpty<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && Collection<TCollection>.Count(argument.Value, 1) == 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.CollectionNotEmpty(argument);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that consists of specified number of items.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="count">
        ///     The exact number of items that the argument value is required to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and does not have the exact
        ///     number of items specified in <paramref name="count" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gc")]
        public static ref readonly ArgumentInfo<TCollection> Count<TCollection>(
            in this ArgumentInfo<TCollection> argument, int count, Func<TCollection, int, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && Collection<TCollection>.Count(argument.Value, count + 1) != count)
            {
                var m = message?.Invoke(argument.Value, count) ?? Messages.CollectionCount(argument, count);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that does not consist of specified
        ///     number of items.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="count">
        ///     The exact number of items that the argument value is required not to have.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c> and has the exact number of
        ///     items specified in <paramref name="count" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gnc")]
        public static ref readonly ArgumentInfo<TCollection> NotCount<TCollection>(
            in this ArgumentInfo<TCollection> argument, int count, Func<TCollection, int, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && Collection<TCollection>.Count(argument.Value, count + 1) == count)
            {
                var m = message?.Invoke(argument.Value, count) ?? Messages.CollectionNotCount(argument, count);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that contains at least the specified
        ///     number of items.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="minCount">
        ///     The minimum number of items that the argument value is allowed to contain.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> contains less than the specified number of items.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gminc")]
        public static ref readonly ArgumentInfo<TCollection> MinCount<TCollection>(
            in this ArgumentInfo<TCollection> argument, int minCount, Func<TCollection, int, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && Collection<TCollection>.Count(argument.Value, minCount) < minCount)
            {
                var m = message?.Invoke(argument.Value, minCount) ?? Messages.CollectionMinCount(argument, minCount);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain more than the
        ///     specified number of items.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="maxCount">
        ///     The maximum number of items that the argument value is allowed to contain.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> contains more than the specified number of items.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gmaxc")]
        public static ref readonly ArgumentInfo<TCollection> MaxCount<TCollection>(
            in this ArgumentInfo<TCollection> argument, int maxCount, Func<TCollection, int, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && Collection<TCollection>.Count(argument.Value, maxCount + 1) > maxCount)
            {
                var m = message?.Invoke(argument.Value, maxCount) ?? Messages.CollectionMaxCount(argument, maxCount);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value whose number of items is between the
        ///     specified minimum and maximum values.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="minCount">
        ///     The minimum number of items that the argument value is allowed to contain.
        /// </param>
        /// <param name="maxCount">
        ///     The maximum number of items that the argument value is allowed to contain.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     The number of items that the <paramref name="argument" /> value contains is either
        ///     less than <paramref name="minCount" /> or greater than <paramref name="maxCount" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gcr")]
        public static ref readonly ArgumentInfo<TCollection> CountInRange<TCollection>(
            in this ArgumentInfo<TCollection> argument, int minCount, int maxCount, Func<TCollection, int, int, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue)
            {
                var count = Collection<TCollection>.Count(argument.Value, maxCount + 1);
                if (count < minCount || count > maxCount)
                {
                    var m = message?.Invoke(argument.Value, minCount, maxCount)
                        ?? Messages.CollectionCountInRange(argument, minCount, maxCount);

                    throw Fail(new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that contains the specified item.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the collection items.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="item">The item that the argument value is required to contain.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> does not contain <paramref name="item" />.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gcon")]
        public static ref readonly ArgumentInfo<TCollection> Contains<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument, in TItem item, Func<TCollection, TItem, string>? message = null)
            where TCollection : IEnumerable?
            => ref argument.Contains(item, null!, message);

        /// <summary>
        ///     Requires the argument to have a collection value that contains the specified item.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the collection items.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="item">The item that the argument value is required to contain.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> does not contain <paramref name="item" /> by the
        ///     comparison made by <paramref name="comparer" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gconc")]
        public static ref readonly ArgumentInfo<TCollection> Contains<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument,
            in TItem item,
            IEqualityComparer<TItem> comparer,
            Func<TCollection, TItem, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && !Collection<TCollection>.Typed<TItem>.Contains(argument.Value, item, comparer))
            {
                var m = message?.Invoke(argument.Value, item) ?? Messages.CollectionContains(argument, item);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain the specified item.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the collection items.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="item">The item that the argument value is required not to contain.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException"><paramref name="argument" /> contains <paramref name="item" />.</exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gncon")]
        public static ref readonly ArgumentInfo<TCollection> DoesNotContain<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument, in TItem item, Func<TCollection, TItem, string>? message = null)
            where TCollection : IEnumerable?
            => ref argument.DoesNotContain(item, null!, message);

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain the specified item.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the collection items.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="item">The item that the argument value is required not to contain.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> contains <paramref name="item" /> by the comparison made
        ///     by <paramref name="comparer" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gnconc")]
        public static ref readonly ArgumentInfo<TCollection> DoesNotContain<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument,
            in TItem item,
            IEqualityComparer<TItem> comparer,
            Func<TCollection, TItem, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && Collection<TCollection>.Typed<TItem>.Contains(argument.Value, item, comparer))
            {
                var m = message?.Invoke(argument.Value, item) ?? Messages.CollectionDoesNotContain(argument, item);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that contains a <c>null</c> element.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> does not contain <c>null</c>.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gconn")]
        public static ref readonly ArgumentInfo<TCollection> ContainsNull<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && !Collection<TCollection>.ContainsNull(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.CollectionContains(argument, "null");
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain a <c>null</c> element.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException"><paramref name="argument" /> contains <c>null</c>.</exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gnconn")]
        public static ref readonly ArgumentInfo<TCollection> DoesNotContainNull<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue && Collection<TCollection>.ContainsNull(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.CollectionDoesNotContain(argument, "null");
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain duplicate elements.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> contains duplicate elements.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gncond")]
        public static ref readonly ArgumentInfo<TCollection> DoesNotContainDuplicate<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, object, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue)
            {
                var (containsDuplicate, duplicateValue) = Collection<TCollection>.ContainsDuplicate(argument.Value, null);
                if (containsDuplicate)
                {
                    var m = message?.Invoke(argument.Value, duplicateValue)
                        ?? Messages.CollectionDoesNotContain(argument, "duplicate items");

                    throw Fail(new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain duplicate elements.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the collection items.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> contains duplicate elements by the comparison made by
        ///     <paramref name="comparer" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gncond")]
        public static ref readonly ArgumentInfo<TCollection> DoesNotContainDuplicate<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument,
            IEqualityComparer<TItem> comparer,
            Func<TCollection, object, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue)
            {
                var (containsDuplicate, duplicateValue) = Collection<TCollection>.ContainsDuplicate(argument.Value, comparer);
                if (containsDuplicate)
                {
                    var m = message?.Invoke(argument.Value, duplicateValue)
                        ?? Messages.CollectionDoesNotContain(argument, "duplicate items");

                    throw Fail(new ArgumentException(m, argument.Name));
                }
            }

            return ref argument;
        }

        /// <summary>Requires the specified collection to contain the argument value.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="collection">
        ///     The collection that is required to contain the argument value.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="collection" /> does not contain the <paramref name="argument" /> value.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gin")]
        public static ref readonly ArgumentInfo<TItem> In<TCollection, TItem>(
            in this ArgumentInfo<TItem> argument,
            TCollection collection,
            Func<TItem, TCollection, string>? message = null)
            where TCollection : IEnumerable?
            => ref argument.In(collection, null!, message);

        /// <summary>Requires the specified collection to contain the argument value.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="collection">
        ///     The collection that is required to contain the argument value.
        /// </param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="collection" /> does not contain the <paramref name="argument" />
        ///     value by the comparison made by <paramref name="comparer" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "ginc")]
        public static ref readonly ArgumentInfo<TItem> In<TCollection, TItem>(
            in this ArgumentInfo<TItem> argument,
            TCollection collection,
            IEqualityComparer<TItem> comparer,
            Func<TItem, TCollection, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue &&
                collection != null &&
                !Collection<TCollection>.Typed<TItem>.Contains(collection, argument.Value, comparer))
            {
                var m = message?.Invoke(argument.Value, collection)
                    ?? Messages.InCollection(argument, collection);

                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the specified items to contain the argument value.</summary>
        /// <typeparam name="TItem">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="items">The items that is required to contain the argument value.</param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items" /> does not contain the <paramref name="argument" /> value.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gin")]
        public static ref readonly ArgumentInfo<TItem> In<TItem>(
            in this ArgumentInfo<TItem> argument, params TItem[] items)
        {
            if (argument.HasValue && items != null)
            {
                var comparer = EqualityComparer<TItem>.Default;
                for (var i = 0; i < items.Length; i++)
                    if (comparer.Equals(argument.Value, items[i]))
                        return ref argument;

                var m = Messages.InCollection(argument, items);
                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the specified collection not to contain the argument value.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="collection">
        ///     The collection that is required not to contain the argument value.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="collection" /> contains the <paramref name="argument" /> value.
        /// </exception>
        [AssertionMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gnin")]
        public static ref readonly ArgumentInfo<TItem> NotIn<TCollection, TItem>(
            in this ArgumentInfo<TItem> argument,
            TCollection collection,
            Func<TItem, TCollection, string>? message = null)
            where TCollection : IEnumerable?
            => ref argument.NotIn(collection, null!, message);

        /// <summary>Requires the specified collection not to contain the argument value.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="collection">
        ///     The collection that is required not to contain the argument value.
        /// </param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="collection" /> contains the <paramref name="argument" /> value by the
        ///     comparison made by <paramref name="comparer" />.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gninc")]
        public static ref readonly ArgumentInfo<TItem> NotIn<TCollection, TItem>(
            in this ArgumentInfo<TItem> argument,
            TCollection collection,
            IEqualityComparer<TItem> comparer,
            Func<TItem, TCollection, string>? message = null)
            where TCollection : IEnumerable?
        {
            if (argument.HasValue &&
                collection != null &&
                Collection<TCollection>.Typed<TItem>.Contains(collection, argument.Value, comparer))
            {
                var m = message?.Invoke(argument.Value, collection)
                    ?? Messages.NotInCollection(argument, collection);

                throw Fail(new ArgumentException(m, argument.Name));
            }

            return ref argument;
        }

        /// <summary>Requires the specified items not to contain the argument value.</summary>
        /// <typeparam name="TItem">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="items">The items that is required not to contain the argument value.</param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items" /> contains the <paramref name="argument" /> value.
        /// </exception>
        [AssertionMethod]
        [DebuggerStepThrough]
        [GuardFunction("Collection", "gnin")]
        public static ref readonly ArgumentInfo<TItem> NotIn<TItem>(
            in this ArgumentInfo<TItem> argument, params TItem[] items)
        {
            if (argument.HasValue && items != null)
            {
                var comparer = EqualityComparer<TItem>.Default;
                for (var i = 0; i < items.Length; i++)
                    if (comparer.Equals(argument.Value, items[i]))
                    {
                        var m = Messages.NotInCollection(argument, items);
                        throw Fail(new ArgumentException(m, argument.Name));
                    }
            }

            return ref argument;
        }

        /// <summary>Provides cached, non-generic collection utilities.</summary>
        private static class Collection
        {
            /// <summary>
            ///     The <see cref="Collection{TCollection}.Count" /> delegates cached by their
            ///     collection types.
            /// </summary>
            public static readonly IDictionary<Type, Func<object, int, int>> CachedCountFunctions
                = new Dictionary<Type, Func<object, int, int>>();

            /// <summary>The locker that synchronizes access to <see cref="CachedCountFunctions" />.</summary>
            public static readonly ReaderWriterLockSlim CachedCountFunctionsLocker = new ReaderWriterLockSlim();

            /// <summary>
            ///     The <see cref="Collection{TCollection}.ContainsNull" /> delegates cached by their
            ///     collection types.
            /// </summary>
            public static readonly IDictionary<Type, Func<object, bool>> CachedContainsNullFunctions
                = new Dictionary<Type, Func<object, bool>>();

            /// <summary>The locker that synchronizes access to <see cref="CachedContainsNullFunctions" />.</summary>
            public static readonly ReaderWriterLockSlim CachedContainsNullFunctionsLocker = new ReaderWriterLockSlim();

            /// <summary>
            ///     The <see cref="Collection{TCollection}.Typed{TItem}.Contains" /> delegates cached
            ///     by their collection and item types.
            /// </summary>
            public static readonly IDictionary<(Type, Type), Delegate> CachedContainsFunctions
                = new Dictionary<(Type, Type), Delegate>();

            /// <summary>The locker that synchronizes access to <see cref="CachedContainsFunctions" />.</summary>
            public static readonly ReaderWriterLockSlim CachedContainsFunctionsLocker = new ReaderWriterLockSlim();
        }

        /// <summary>Provides cached collection utilities for the type <typeparamref name="TCollection" />.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        private static class Collection<TCollection>
            where TCollection : IEnumerable?
        {
            /// <summary>
            ///     A function that returns the number of elements in the specified collection. It
            ///     enumerates the collection and counts the elements if the collection does not
            ///     provide a Count/Length property. The integer parameter specifies the maximum
            ///     number of iterations.
            /// </summary>
            public static readonly Func<TCollection, int, int> Count = InitCount();

            /// <summary>
            ///     A function that returns a value that indicates whether the specified collection
            ///     contains a <c>null</c> element. It enumerates the collection and checks the
            ///     elements one by one if the collection does not provide a Contains method that
            ///     accepts a single, nullable argument.
            /// </summary>
            public static readonly Func<TCollection, bool> ContainsNull = InitContainsNull();

            /// <summary>
            ///     A function that returns a value that indicates whether the specified collection
            ///     contains duplicate elements. It checks for reference equality if the collection
            ///     does not implement <see cref="IEnumerable{T}" />.
            /// </summary>
            public static readonly Func<TCollection, object?, (bool, object)> ContainsDuplicate = InitContainsDuplicate();

            /// <summary>Initializes <see cref="Count" />.</summary>
            /// <returns>
            ///     A function that returns the number of elements in the specified collection.
            /// </returns>
            private static Func<TCollection, int, int> InitCount()
            {
                var countGetter = GetCountGetter();
                if (countGetter != null)
                {
                    var collectionParam = Expression.Parameter(typeof(TCollection), "collection");
                    var countCall = Expression.Call(collectionParam, countGetter);
                    var countLambda = Expression.Lambda<Func<TCollection, int>>(countCall, collectionParam);
                    var count = countLambda.Compile();
                    return (collection, max) => count(collection);
                }

                return (collection, max) =>
                {
                    if (typeof(TCollection) != collection!.GetType())
                        return ProxyCount(collection, max);

                    if (max == 0)
                        return 0;

                    var i = 0;
                    var enumerator = collection.GetEnumerator();
                    try
                    {
                        while (i < max && enumerator.MoveNext())
                            i++;
                    }
                    finally
                    {
                        (enumerator as IDisposable)?.Dispose();
                    }

                    return i;
                };

                static int ProxyCount(TCollection collection, int max)
                {
                    Func<object, int, int>? func;

                    Collection.CachedCountFunctionsLocker.EnterUpgradeableReadLock();
                    try
                    {
                        if (!Collection.CachedCountFunctions.TryGetValue(collection!.GetType(), out func))
                        {
                            var f = Expression.Field(null, typeof(Collection<>)
                                .MakeGenericType(collection.GetType())
                                .GetField(nameof(Count)));

                            var o = Expression.Parameter(typeof(object), nameof(collection));
                            var m = Expression.Parameter(typeof(int), nameof(max));
                            var i = Expression.Invoke(f, Expression.Convert(o, collection.GetType()), m);
                            var l = Expression.Lambda<Func<object, int, int>>(i, o, m);
                            func = l.Compile();

                            Collection.CachedCountFunctionsLocker.EnterWriteLock();
                            try
                            {
                                Collection.CachedCountFunctions[collection.GetType()] = func;
                            }
                            finally
                            {
                                Collection.CachedCountFunctionsLocker.ExitWriteLock();
                            }
                        }
                    }
                    finally
                    {
                        Collection.CachedCountFunctionsLocker.ExitUpgradeableReadLock();
                    }

                    return func(collection, max);
                }
            }

            /// <summary>
            ///     Returns the getter of Count or Length property of <typeparamref name="TCollection" />.
            /// </summary>
            /// <returns>
            ///     The getter of Count or Length property of <typeparamref name="TCollection" />,
            ///     if exists; otherwise, <c>null</c>.
            /// </returns>
            private static MethodInfo? GetCountGetter()
            {
                var collectionType = typeof(TCollection);
                var implementedType = typeof(ICollection);

                if (implementedType.IsAssignableFrom(collectionType))
                    return implementedType.GetPropertyGetter("Count");

                var itemType = GetItemType();
                if (itemType != null)
                {
                    implementedType = typeof(IReadOnlyCollection<>).MakeGenericType(itemType);
                    if (implementedType.IsAssignableFrom(collectionType))
                        return implementedType.GetPropertyGetter("Count");
                }

                var returnType = typeof(int);
                var getter = collectionType.GetPropertyGetter("Count");
                if (getter?.IsStatic == false && getter.ReturnType == returnType)
                    return getter;

                getter = collectionType.GetPropertyGetter("Length");
                if (getter?.IsStatic == false && getter.ReturnType == returnType)
                    return getter;

                return null;
            }

            /// <summary>Initializes <see cref="ContainsNull" />.</summary>
            /// <returns>
            ///     A function that returns a value that indicates whether the specified collection
            ///     contains a <c>null</c> element.
            /// </returns>
            private static Func<TCollection, bool> InitContainsNull()
            {
                const string name = "Contains";
                var collectionType = typeof(TCollection);

                IEnumerable<MethodInfo> search;
#if NETSTANDARD1_0
                search = collectionType.GetTypeInfo().GetDeclaredMethods(name).Where(m => m.IsPublic && !m.IsStatic);
#else
                search = collectionType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => m.Name == name);
#endif

                var methods = search.Where(m => m.ReturnType == typeof(bool)).ToList();
                if (methods.Count > 0)
                {
                    var nullableContains = null as MethodInfo;
                    var nullableContainsParamType = null as Type;
                    var foundVal = false;
                    foreach (var method in methods)
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length != 1)
                            continue;

                        var paramType = parameters[0].ParameterType;
                        if (!IsValueType(paramType) || paramType.IsGenericType(typeof(Nullable<>)))
                        {
                            nullableContains = method;
                            nullableContainsParamType = paramType;
                            break;
                        }

                        foundVal = true;
                    }

                    if (nullableContains != null)
                    {
                        var t = Expression.Parameter(collectionType, "collection");
                        var v = IsValueType(nullableContainsParamType!)
                            ? Activator.CreateInstance(nullableContainsParamType!)
                            : null;

                        var i = Expression.Constant(v, nullableContainsParamType!);
                        var c = Expression.Call(t, nullableContains, i);
                        var l = Expression.Lambda<Func<TCollection, bool>>(c, t);
                        return l.Compile();
                    }

                    if (foundVal)
                        return collection => false;
                }

                return EnumeratingContainsNull;

                bool EnumeratingContainsNull(TCollection collection)
                {
                    if (collectionType != collection!.GetType())
                        return ProxyContainsNull(collection);

                    foreach (var item in collection)
                        if (item is null)
                            return true;

                    return false;
                }

                static bool ProxyContainsNull(TCollection collection)
                {
                    Func<object, bool> func;

                    Collection.CachedContainsNullFunctionsLocker.EnterUpgradeableReadLock();
                    try
                    {
                        if (!Collection.CachedContainsNullFunctions.TryGetValue(collection!.GetType(), out var del))
                        {
                            var f = Expression.Field(null, typeof(Collection<>)
                                .MakeGenericType(collection.GetType())
                                .GetField(nameof(ContainsNull)));

                            var o = Expression.Parameter(typeof(object), nameof(collection));
                            var i = Expression.Invoke(f, Expression.Convert(o, collection.GetType()));
                            var l = Expression.Lambda<Func<object, bool>>(i, o);
                            del = l.Compile();

                            Collection.CachedContainsNullFunctionsLocker.EnterWriteLock();
                            try
                            {
                                Collection.CachedContainsNullFunctions[collection.GetType()] = del;
                            }
                            finally
                            {
                                Collection.CachedContainsNullFunctionsLocker.ExitWriteLock();
                            }
                        }

                        func = del;
                    }
                    finally
                    {
                        Collection.CachedContainsNullFunctionsLocker.ExitUpgradeableReadLock();
                    }

                    return func(collection);
                }
            }

            /// <summary>Initializes <see cref="ContainsDuplicate" />.</summary>
            /// <returns>
            ///     A function that returns a value that indicates whether the specified collection
            ///     contains duplicate elements.
            /// </returns>
            private static Func<TCollection, object?, (bool, object)> InitContainsDuplicate()
            {
                var itemType = GetItemType();
                if (itemType is null)
                    return (collection, _) =>
                    {
                        var set = new HashSet<object>();
                        foreach (var item in collection!)
                            if (!set.Add(item))
                                return (true, item);

                        return default;
                    };

                var collectionType = typeof(TCollection);
                var collectionParam = Expression.Parameter(collectionType, "collection");
                var comparerType = typeof(IEqualityComparer<>).MakeGenericType(itemType);
                var comparerParam = Expression.Parameter(typeof(object), "comparer");

                var setType = typeof(HashSet<>).MakeGenericType(itemType);
                var setAddMethod = setType.GetMethod("Add", new[] { itemType });
                var setVar = Expression.Variable(setType, "set");

                var enumerableType = typeof(IEnumerable<>).MakeGenericType(itemType);
                var enumeratorType = typeof(IEnumerator<>).MakeGenericType(itemType);
                var enumeratorVar = Expression.Variable(enumeratorType, "enumerator");
                var getEnumeratorMethod = enumerableType.GetMethod("GetEnumerator", Array<Type>.Empty);
                var getEnumeratorCall = Expression.Call(collectionParam, getEnumeratorMethod);
                var moveNextMethod = typeof(IEnumerator).GetMethod("MoveNext", Array<Type>.Empty);
                var moveNextCall = Expression.Call(enumeratorVar, moveNextMethod);

                var itemVar = Expression.Variable(itemType, "item");
                var returnType = typeof(ValueTuple<,>).MakeGenericType(typeof(bool), typeof(object));
                var returnDefault = Expression.Default(returnType);
                var returnLabelTarget = Expression.Label(returnType, "ReturnResult");
                var returnLabel = Expression.Label(returnLabelTarget, returnDefault);
                var block = Expression.Block(
                    new[] { comparerParam, setVar, enumeratorVar, itemVar },
                    Expression.IfThenElse(
                        Expression.ReferenceEqual(comparerParam, Expression.Constant(null)),
                        Expression.Assign(setVar, GetSetNew(null)),
                        Expression.Assign(setVar, GetSetNew(comparerParam))),
                    Expression.Assign(enumeratorVar, getEnumeratorCall),
                    Expression.Loop(
                        Expression.IfThenElse(
                            Expression.Equal(moveNextCall, Expression.Constant(true)),
                            Expression.Block(
                                Expression.Assign(itemVar, Expression.Property(enumeratorVar, "Current")),
                                Expression.IfThen(
                                    Expression.Not(Expression.Call(setVar, setAddMethod, itemVar)),
                                    Expression.Return(returnLabelTarget, Expression.New(
                                        returnType.GetConstructor(returnType.GetGenericArguments()),
                                        Expression.Constant(true),
                                        Expression.Convert(itemVar, typeof(object))))
                                )),
                            Expression.Return(returnLabelTarget, returnDefault)
                        )),
                    returnLabel
                );

                return Expression.Lambda<Func<TCollection, object?, (bool, object)>>(
                    block, collectionParam, comparerParam).Compile();

                NewExpression GetSetNew(ParameterExpression? comparerParam)
                {
                    if (comparerParam != null)
                    {
                        var setCtor = setType.GetConstructor(new[] { comparerType });
                        return Expression.New(setCtor, Expression.Convert(comparerParam, comparerType));
                    }

                    var countGetter = GetCountGetter();
                    if (countGetter != null)
                        try
                        {
                            var setCtor = setType.GetConstructor(new[] { typeof(int) });
                            var countCall = Expression.Call(collectionParam, countGetter);
                            return Expression.New(setCtor, countCall);
                        }
                        catch (Exception)
                        {
                            // Swallow and use default
                        }

                    return Expression.New(setType);
                }
            }

            /// <summary>Returns the item type of a generic <typeparamref name="TCollection" />.</summary>
            /// <returns>
            ///     The item type of <typeparamref name="TCollection" />, if it implements
            ///     <see cref="IEnumerable{T}" />; otherwise, <c>null</c>.
            /// </returns>
            private static Type? GetItemType()
            {
                return GetItemType(typeof(TCollection));

                static Type? GetItemType(Type collectionType)
                {
                    var openType = typeof(IEnumerable<>);
                    foreach (var interfaceType in collectionType.GetInterfaces())
                        if (interfaceType.IsGenericType(openType))
                            return interfaceType.GetGenericArguments()[0];

                    if (collectionType.IsGenericType(openType))
                        return collectionType.GetGenericArguments()[0];

                    var baseType = collectionType.GetBaseType();
                    return baseType != null
                        ? GetItemType(baseType)
                        : null;
                }
            }

            /// <summary>
            ///     Provides cached collection utilities for collections that contain instances of <typeparamref name="TItem" />.
            /// </summary>
            /// <typeparam name="TItem">The type of the collection items.</typeparam>
            public static class Typed<TItem>
            {
                /// <summary>
                ///     A function that determines whether a generic collection contains the
                ///     specified element.
                /// </summary>
                public static readonly Func<TCollection, TItem, IEqualityComparer<TItem>, bool> Contains = InitContains();

                /// <summary>Initializes <see cref="Contains" />.</summary>
                /// <returns>
                ///     A function that determines whether a generic collection contains the
                ///     specified element.
                /// </returns>
                private static Func<TCollection, TItem, IEqualityComparer<TItem>, bool> InitContains()
                {
                    var collectionType = typeof(TCollection);

                    Type? itemType = typeof(TItem);
                    do
                    {
                        var method = collectionType.GetMethod("Contains", new[] { itemType });
                        if (method?.IsStatic == false && method.ReturnType == typeof(bool))
                        {
                            var p = method.GetParameters()[0];
                            var colParam = Expression.Parameter(collectionType, "collection");
                            var itemParam = Expression.Parameter(itemType, "item");

                            var convert = itemParam as Expression;
                            if (p.ParameterType != itemType && p.ParameterType.IsGenericType(typeof(Nullable<>)))
                                convert = Expression.Convert(itemParam, p.ParameterType);

                            var call = Expression.Call(colParam, method, convert);
                            var lambda = Expression.Lambda<Func<TCollection, TItem, bool>>(call, colParam, itemParam);
                            var compiled = lambda.Compile();
                            return (collection, item, comparer) => comparer != null
                                ? EnumeratingContains(collection, item, comparer)
                                : compiled(collection, item);
                        }

                        itemType = itemType.GetBaseType();
                    }
                    while (itemType != null);
                    return EnumeratingContains;

                    bool EnumeratingContains(TCollection collection, TItem item, IEqualityComparer<TItem> comparer)
                    {
                        if (collectionType != collection!.GetType())
                            return ProxyContains(collection, item, comparer);

                        if (comparer is null)
                            comparer = EqualityComparer<TItem>.Default;

                        if (collection is IEnumerable<TItem> typed)
                        {
                            foreach (var current in typed)
                                if (comparer.Equals(current, item))
                                    return true;
                        }
                        else
                        {
                            foreach (var current in collection)
                                if (current is TItem c && comparer.Equals(c, item))
                                    return true;
                        }

                        return false;
                    }

                    static bool ProxyContains(TCollection collection, TItem item, IEqualityComparer<TItem> comparer)
                    {
                        Func<object, TItem, IEqualityComparer<TItem>, bool> func;

                        Collection.CachedContainsFunctionsLocker.EnterUpgradeableReadLock();
                        try
                        {
                            var key = (collection.GetType(), typeof(TItem));
                            if (!Collection.CachedContainsFunctions.TryGetValue(key, out var del))
                            {
                                var f = Expression.Field(null, typeof(Collection<>)
                                    .GetNestedType("Typed`1")!
                                    .MakeGenericType(collection.GetType(), typeof(TItem))
                                    .GetField(nameof(Contains)));

                                var o = Expression.Parameter(typeof(object), nameof(collection));
                                var i = Expression.Parameter(typeof(TItem), nameof(item));
                                var e = Expression.Parameter(typeof(IEqualityComparer<TItem>), nameof(comparer));
                                var n = Expression.Invoke(f, Expression.Convert(o, collection.GetType()), i, e);
                                var l = Expression.Lambda<Func<object, TItem, IEqualityComparer<TItem>, bool>>(n, o, i, e);
                                del = l.Compile();

                                Collection.CachedContainsFunctionsLocker.EnterWriteLock();
                                try
                                {
                                    Collection.CachedContainsFunctions[key] = del;
                                }
                                finally
                                {
                                    Collection.CachedContainsFunctionsLocker.ExitWriteLock();
                                }
                            }

                            func = (del as Func<object, TItem, IEqualityComparer<TItem>, bool>)!;
                        }
                        finally
                        {
                            Collection.CachedContainsFunctionsLocker.ExitUpgradeableReadLock();
                        }

                        return func(collection, item, comparer);
                    }
                }
            }
        }
    }
}
