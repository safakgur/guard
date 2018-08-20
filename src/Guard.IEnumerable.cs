namespace Dawn
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <content>Provides preconditions for <see cref="IEnumerable" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>Requires the argument to have a collection value that is empty.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> has one or more items.
        /// </exception>
        public static ref readonly ArgumentInfo<TCollection> Empty<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() && Collection<TCollection>.Count(argument.Value, 1) != 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.CollectionEmpty(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that is not empty.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> is not <c>null</c> and has no items.
        /// </exception>
        public static ref readonly ArgumentInfo<TCollection> NotEmpty<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() && Collection<TCollection>.Count(argument.Value, 1) == 0)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.CollectionNotEmpty(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that contains at least the
        ///     specified number of items.
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
        public static ref readonly ArgumentInfo<TCollection> MinCount<TCollection>(
            in this ArgumentInfo<TCollection> argument, int minCount, Func<TCollection, int, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() && Collection<TCollection>.Count(argument.Value, minCount) < minCount)
            {
                var m = message?.Invoke(argument.Value, minCount) ?? Messages.CollectionMinCount(argument, minCount);
                throw new ArgumentException(m, argument.Name);
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
        public static ref readonly ArgumentInfo<TCollection> MaxCount<TCollection>(
            in this ArgumentInfo<TCollection> argument, int maxCount, Func<TCollection, int, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() && Collection<TCollection>.Count(argument.Value, maxCount + 1) > maxCount)
            {
                var m = message?.Invoke(argument.Value, maxCount) ?? Messages.CollectionMaxCount(argument, maxCount);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value whose number of items is between
        ///     the specified minimum and maximum values.
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
        public static ref readonly ArgumentInfo<TCollection> CountInRange<TCollection>(
            in this ArgumentInfo<TCollection> argument, int minCount, int maxCount, Func<TCollection, int, int, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue())
            {
                var count = Collection<TCollection>.Count(argument.Value, maxCount + 1);
                if (count < minCount || count > maxCount)
                {
                    var m = message?.Invoke(argument.Value, minCount, maxCount)
                        ?? Messages.CollectionCountInRange(argument, minCount, maxCount);

                    throw new ArgumentException(m, argument.Name);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<TCollection> Contains<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument, in TItem item, Func<TCollection, TItem, string> message = null)
            where TCollection : IEnumerable
            => ref argument.Contains(item, null, message);

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
        public static ref readonly ArgumentInfo<TCollection> Contains<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument,
            in TItem item,
            IEqualityComparer<TItem> comparer,
            Func<TCollection, TItem, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() && !Collection<TCollection>.Typed<TItem>.Contains(argument.Value, item, comparer))
            {
                var m = message?.Invoke(argument.Value, item) ?? Messages.CollectionContains(argument, item);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain the
        ///     specified item.
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
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> contains <paramref name="item" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<TCollection> DoesNotContain<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument, in TItem item, Func<TCollection, TItem, string> message = null)
            where TCollection : IEnumerable
            => ref argument.DoesNotContain(item, null, message);

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain the
        ///     specified item.
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
        ///     <paramref name="argument" /> contains <paramref name="item" /> by the comparison
        ///     made by <paramref name="comparer" />.
        /// </exception>
        public static ref readonly ArgumentInfo<TCollection> DoesNotContain<TCollection, TItem>(
            in this ArgumentInfo<TCollection> argument,
            in TItem item,
            IEqualityComparer<TItem> comparer,
            Func<TCollection, TItem, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() && Collection<TCollection>.Typed<TItem>.Contains(argument.Value, item, comparer))
            {
                var m = message?.Invoke(argument.Value, item) ?? Messages.CollectionDoesNotContain(argument, item);
                throw new ArgumentException(m, argument.Name);
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
        public static ref readonly ArgumentInfo<TCollection> ContainsNull<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() && !Collection<TCollection>.ContainsNull(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.CollectionContains(argument, "null");
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have a collection value that does not contain a
        ///     <c>null</c> element.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="argument">The collection argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that will be thrown if the
        ///     precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> contains <c>null</c>.
        /// </exception>
        public static ref readonly ArgumentInfo<TCollection> DoesNotContainNull<TCollection>(
            in this ArgumentInfo<TCollection> argument, Func<TCollection, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() && Collection<TCollection>.ContainsNull(argument.Value))
            {
                var m = message?.Invoke(argument.Value) ?? Messages.CollectionDoesNotContain(argument, "null");
                throw new ArgumentException(m, argument.Name);
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
        ///     <paramref name="collection" /> does not contain the <paramref name="argument" />
        ///     value.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<TItem> In<TCollection, TItem>(
            in this ArgumentInfo<TItem> argument,
            TCollection collection,
            Func<TItem, TCollection, string> message = null)
            where TCollection : IEnumerable
            => ref argument.In(collection, null, message);

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
        public static ref readonly ArgumentInfo<TItem> In<TCollection, TItem>(
            in this ArgumentInfo<TItem> argument,
            TCollection collection,
            IEqualityComparer<TItem> comparer,
            Func<TItem, TCollection, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() &&
                NullChecker<TCollection>.HasValue(collection) &&
                !Collection<TCollection>.Typed<TItem>.Contains(collection, argument.Value, comparer))
            {
                var m = message?.Invoke(argument.Value, collection)
                    ?? Messages.InCollection(argument, collection);

                throw new ArgumentException(m, argument.Name);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly ArgumentInfo<TItem> NotIn<TCollection, TItem>(
            in this ArgumentInfo<TItem> argument,
            TCollection collection,
            Func<TItem, TCollection, string> message = null)
            where TCollection : IEnumerable
            => ref argument.NotIn(collection, null, message);

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
        ///     <paramref name="collection" /> contains the <paramref name="argument" /> value by
        ///     the comparison made by <paramref name="comparer" />.
        /// </exception>
        public static ref readonly ArgumentInfo<TItem> NotIn<TCollection, TItem>(
            in this ArgumentInfo<TItem> argument,
            TCollection collection,
            IEqualityComparer<TItem> comparer,
            Func<TItem, TCollection, string> message = null)
            where TCollection : IEnumerable
        {
            if (argument.HasValue() &&
                NullChecker<TCollection>.HasValue(collection) &&
                Collection<TCollection>.Typed<TItem>.Contains(collection, argument.Value, comparer))
            {
                var m = message?.Invoke(argument.Value, collection)
                    ?? Messages.NotInCollection(argument, collection);

                throw new ArgumentException(m, argument.Name);
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

            /// <summary>
            ///     The locker that synchronizes access to <see cref="CachedCountFunctions" />.
            /// </summary>
            public static readonly ReaderWriterLockSlim CachedCountFunctionsLocker = new ReaderWriterLockSlim();

            /// <summary>
            ///     The <see cref="Collection{TCollection}.ContainsNull" /> delegates cached by
            ///     their collection types.
            /// </summary>
            public static readonly IDictionary<Type, Func<object, bool>> CachedContainsNullFunctions
                = new Dictionary<Type, Func<object, bool>>();

            /// <summary>
            ///     The locker that synchronizes access to <see cref="CachedContainsNullFunctions" />.
            /// </summary>
            public static readonly ReaderWriterLockSlim CachedContainsNullFunctionsLocker = new ReaderWriterLockSlim();

            /// <summary>
            ///     The <see cref="Collection{TCollection}.Typed{TItem}.Contains" /> delegates
            ///     cached by their collection and item types.
            /// </summary>
            public static readonly IDictionary<(Type, Type), Delegate> CachedContainsFunctions
                = new Dictionary<(Type, Type), Delegate>();

            /// <summary>
            ///     The locker that synchronizes access to <see cref="CachedContainsFunctions" />.
            /// </summary>
            public static readonly ReaderWriterLockSlim CachedContainsFunctionsLocker = new ReaderWriterLockSlim();
        }

        /// <summary>
        ///     Provides cached collection utilities for the type
        ///     <typeparamref name="TCollection" />.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        private static class Collection<TCollection>
            where TCollection : IEnumerable
        {
            /// <summary>
            ///     A function that returns the number of elements in the specified collection.
            ///     It enumerates the collection and counts the elements if the collection does not
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

            /// <summary>Initializes <see cref="Count" />.</summary>
            /// <returns>
            ///     A function that returns the number of elements in the specified collection.
            /// </returns>
            private static Func<TCollection, int, int> InitCount()
            {
                var type = typeof(TCollection);
                var integer = typeof(int);

                var getter = type.GetPropertyGetter("Count");
                if (getter?.IsStatic == false && getter.ReturnType == integer)
                    return CompileGetter();

                getter = type.GetPropertyGetter("Length");
                if (getter?.IsStatic == false && getter.ReturnType == integer)
                    return CompileGetter();

                return EnumerableCount;

                Func<TCollection, int, int> CompileGetter()
                {
                    var t = Expression.Parameter(type, "collection");
                    var c = Expression.Call(t, getter);
                    var l = Expression.Lambda<Func<TCollection, int>>(c, t);
                    var count = l.Compile();
                    return (collection, max) => count(collection);
                }

                int EnumerableCount(TCollection collection, int max)
                {
                    if (type != collection.GetType())
                        return ProxyCount(collection, max);

                    if (max == 0)
                        return 0;

                    var i = 0;
                    var enumerator = collection.GetEnumerator();
                    try
                    {
                        for (; i < max && enumerator.MoveNext(); i++)
                        {
                        }
                    }
                    finally
                    {
                        (enumerator as IDisposable)?.Dispose();
                    }

                    return i;
                }

                int ProxyCount(TCollection collection, int max)
                {
                    Collection.CachedCountFunctionsLocker.EnterUpgradeableReadLock();
                    try
                    {
                        if (!Collection.CachedCountFunctions.TryGetValue(collection.GetType(), out var func))
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

                        return func(collection, max);
                    }
                    finally
                    {
                        Collection.CachedCountFunctionsLocker.ExitUpgradeableReadLock();
                    }
                }
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
                        var v = IsValueType(nullableContainsParamType)
                            ? Activator.CreateInstance(nullableContainsParamType)
                            : null;

                        var i = Expression.Constant(v, nullableContainsParamType);
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
                    if (collectionType != collection.GetType())
                        return ProxyContainsNull(collection);

                    foreach (var item in collection)
                        if (item is null)
                            return true;

                    return false;
                }

                bool ProxyContainsNull(TCollection collection)
                {
                    Collection.CachedContainsNullFunctionsLocker.EnterUpgradeableReadLock();
                    try
                    {
                        if (!Collection.CachedContainsNullFunctions.TryGetValue(collection.GetType(), out var func))
                        {
                            var f = Expression.Field(null, typeof(Collection<>)
                                .MakeGenericType(collection.GetType())
                                .GetField(nameof(ContainsNull)));

                            var o = Expression.Parameter(typeof(object), nameof(collection));
                            var i = Expression.Invoke(f, Expression.Convert(o, collection.GetType()));
                            var l = Expression.Lambda<Func<object, bool>>(i, o);
                            func = l.Compile();

                            Collection.CachedContainsNullFunctionsLocker.EnterWriteLock();
                            try
                            {
                                Collection.CachedContainsNullFunctions[collection.GetType()] = func;
                            }
                            finally
                            {
                                Collection.CachedContainsNullFunctionsLocker.ExitWriteLock();
                            }
                        }

                        return (func as Func<object, bool>)(collection);
                    }
                    finally
                    {
                        Collection.CachedContainsNullFunctionsLocker.ExitUpgradeableReadLock();
                    }
                }
            }

            /// <summary>
            ///     Provides cached collection utilities for collections that contain instances of
            ///     <typeparamref name="TItem" />.
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
                    var itemType = typeof(TItem);
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
                        if (collectionType != collection.GetType())
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

                    bool ProxyContains(TCollection collection, TItem item, IEqualityComparer<TItem> comparer)
                    {
                        Collection.CachedContainsFunctionsLocker.EnterUpgradeableReadLock();
                        try
                        {
                            var key = (collection.GetType(), typeof(TItem));
                            if (!Collection.CachedContainsFunctions.TryGetValue(key, out var func))
                            {
                                var f = Expression.Field(null, typeof(Collection<>)
                                    .GetNestedType("Typed`1")
                                    .MakeGenericType(collection.GetType(), typeof(TItem))
                                    .GetField(nameof(Contains)));

                                var o = Expression.Parameter(typeof(object), nameof(collection));
                                var i = Expression.Parameter(typeof(TItem), nameof(item));
                                var e = Expression.Parameter(typeof(IEqualityComparer<TItem>), nameof(comparer));
                                var n = Expression.Invoke(f, Expression.Convert(o, collection.GetType()), i, e);
                                var l = Expression.Lambda<Func<object, TItem, IEqualityComparer<TItem>, bool>>(n, o, i, e);
                                func = l.Compile();

                                Collection.CachedContainsFunctionsLocker.EnterWriteLock();
                                try
                                {
                                    Collection.CachedContainsFunctions[key] = func;
                                }
                                finally
                                {
                                    Collection.CachedContainsFunctionsLocker.ExitWriteLock();
                                }
                            }

                            return (func as Func<object, TItem, IEqualityComparer<TItem>, bool>)(collection, item, comparer);
                        }
                        finally
                        {
                            Collection.CachedContainsFunctionsLocker.ExitUpgradeableReadLock();
                        }
                    }
                }
            }
        }
    }
}
