namespace Dawn.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public sealed partial class GuardTests
    {
        [Fact(DisplayName = "Guard supports collection preconditions.")]
        public void GuardSupportsCollections()
        {
            var message = RandomMessage;

            // Define struct sources.
            var withoutNullVal = Enumerable.Range('A', 4).Select(i => (char)i).ToList();
            var withNullVal = new List<char?>(withoutNullVal.Select(c => c as char?));
            var nullIndex = withNullVal.Count - 2;
            withNullVal.Insert(nullIndex, null);

            var withNullValEnumerable = new TestEnumerable<char?>(withNullVal);
            var withNullValEnumerableArg = Guard.Argument(() => withNullValEnumerable);

            var withNullValCollection = new TestCollection<char?>(withNullVal);
            var withNullValCollectionArg = Guard.Argument(() => withNullValCollection);

            var withoutNullValEnumerable = new TestEnumerable<char>(withoutNullVal);
            var withoutNullValEnumerableArg = Guard.Argument(() => withoutNullValEnumerable);

            var withoutNullValCollection = new TestCollection<char>(withoutNullVal);
            var withoutNullValCollectionArg = Guard.Argument(() => withoutNullValCollection);

            // Define class sources.
            var withoutNullRef = withoutNullVal.Select(i => i.ToString()).ToList();
            var withNullRef = new List<string>(withoutNullRef);
            withNullRef.Insert(nullIndex, null);

            var nullRefEnumerable = null as IEnumerable<string>;
            var nullRefEnumerableArg = Guard.Argument(() => nullRefEnumerable);

            var emptyRefEnumerable = new TestEnumerable<string>(new string[0]);
            var emptyRefEnumerableArg = Guard.Argument(() => emptyRefEnumerable);

            var withNullRefEnumerable = new TestEnumerable<string>(withNullRef);
            var withNullRefEnumerableArg = Guard.Argument(() => withNullRefEnumerable);

            var emptyRefCollection = new TestCollection<string>(new string[0]);
            var emptyRefCollectionArg = Guard.Argument(() => emptyRefCollection);

            var withNullRefCollection = new TestCollection<string>(withNullRef);
            var withNullRefCollectionArg = Guard.Argument(() => withNullRefCollection);

            var validNullableRefIndex = RandomUtils.Current.Next(withNullRef.Count);
            var validNullableRef = withNullRef[validNullableRefIndex];
            var invalidNullableRef = "Z";

            var withoutNullRefEnumerable = new TestEnumerable<string>(withoutNullRef);
            var withoutNullRefEnumerableArg = Guard.Argument(() => withoutNullRefEnumerable);

            var withoutNullRefCollection = new TestCollection<string>(withoutNullRef);
            var withoutNullRefCollectionArg = Guard.Argument(() => withoutNullRefCollection);

            // Empty enumerable.
            nullRefEnumerableArg.Empty();
            emptyRefEnumerableArg.Empty();
            CheckAndResetEnumerable(emptyRefEnumerable, 0, true);

            Assert.Throws<ArgumentException>(
               nameof(withNullRefEnumerable), () => Guard.Argument(() => withNullRefEnumerable).Empty());

            CheckAndResetEnumerable(withNullRefEnumerable, 1);

            var ex = Assert.Throws<ArgumentException>(
               nameof(withNullRefEnumerable), () => Guard.Argument(() => withNullRefEnumerable).Empty(e =>
               {
                   Assert.Same(withNullRefEnumerable, e);
                   return message;
               }));

            CheckAndResetEnumerable(withNullRefEnumerable, 1);
            Assert.StartsWith(message, ex.Message);

            // Empty collection.
            emptyRefCollectionArg.Empty();
            CheckAndResetCollection(emptyRefCollection, true, false);

            Assert.Throws<ArgumentException>(
               nameof(withNullRefCollection), () => Guard.Argument(() => withNullRefCollection).Empty());

            CheckAndResetCollection(withNullRefCollection, true, false);

            ex = Assert.Throws<ArgumentException>(
               nameof(withNullRefCollection), () => Guard.Argument(() => withNullRefCollection).Empty(c =>
               {
                   Assert.Same(withNullRefCollection, c);
                   return message;
               }));

            CheckAndResetCollection(withNullRefCollection, true, false);
            Assert.StartsWith(message, ex.Message);

            // Non-empty enumerable.
            nullRefEnumerableArg.NotEmpty();
            withNullRefEnumerableArg.NotEmpty();
            CheckAndResetEnumerable(withNullRefEnumerable, 1);

            Assert.Throws<ArgumentException>(
               nameof(emptyRefEnumerable), () => Guard.Argument(() => emptyRefEnumerable).NotEmpty());

            CheckAndResetEnumerable(emptyRefEnumerable, 0, true);

            ex = Assert.Throws<ArgumentException>(
               nameof(emptyRefEnumerable), () => Guard.Argument(() => emptyRefEnumerable).NotEmpty(e =>
               {
                   Assert.Same(emptyRefEnumerable, e);
                   return message;
               }));

            CheckAndResetEnumerable(emptyRefEnumerable, 0, true);
            Assert.StartsWith(message, ex.Message);

            // Non-empty collection.
            withNullRefCollectionArg.NotEmpty();
            CheckAndResetCollection(withNullRefCollection, true, false);

            Assert.Throws<ArgumentException>(
               nameof(emptyRefCollection), () => Guard.Argument(() => emptyRefCollection).NotEmpty());

            CheckAndResetCollection(emptyRefCollection, true, false);

            ex = Assert.Throws<ArgumentException>(
               nameof(emptyRefCollection), () => Guard.Argument(() => emptyRefCollection).NotEmpty(c =>
               {
                   Assert.Same(emptyRefCollection, c);
                   return message;
               }));

            CheckAndResetCollection(emptyRefCollection, true, false);
            Assert.StartsWith(message, ex.Message);

            // Min count of enumerable.
            nullRefEnumerableArg.MinCount(withNullRef.Count - 2).MinCount(withNullRef.Count);

            withNullRefEnumerableArg.MinCount(withNullRef.Count - 2);
            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count - 2);

            withNullRefEnumerableArg.MinCount(withNullRef.Count);
            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            Assert.Throws<ArgumentException>(
               nameof(withNullRefEnumerable),
               () => Guard.Argument(() => withNullRefEnumerable).MinCount(withNullRef.Count + 1));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            ex = Assert.Throws<ArgumentException>(
               nameof(withNullRefEnumerable), () =>
               Guard.Argument(() => withNullRefEnumerable).MinCount(withNullRef.Count + 1, (e, min) =>
               {
                   Assert.Same(withNullRefEnumerable, e);
                   Assert.Equal(withNullRef.Count + 1, min);
                   return message;
               }));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);
            Assert.StartsWith(message, ex.Message);

            // Min count of collection.
            withNullRefCollectionArg.MinCount(withNullRef.Count - 2).MinCount(withNullRef.Count);
            CheckAndResetCollection(withNullRefCollection, true, false);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).MinCount(withNullRef.Count + 1));

            CheckAndResetCollection(withNullRefCollection, true, false);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).MinCount(withNullRef.Count + 1, (c, min) =>
                {
                    Assert.Same(withNullRefCollection, c);
                    Assert.Equal(withNullRef.Count + 1, min);
                    return message;
                }));

            CheckAndResetCollection(withNullRefCollection, true, false);
            Assert.StartsWith(message, ex.Message);

            // Max count of enumerable.
            nullRefEnumerableArg.MaxCount(withNullRef.Count).MaxCount(withNullRef.Count + 2);

            withNullRefEnumerableArg.MaxCount(withNullRef.Count);
            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            withNullRefEnumerableArg.MaxCount(withNullRef.Count + 2);
            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            Assert.Throws<ArgumentException>(
               nameof(withNullRefEnumerable),
               () => Guard.Argument(() => withNullRefEnumerable).MaxCount(withNullRef.Count - 2));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count - 1);

            ex = Assert.Throws<ArgumentException>(
               nameof(withNullRefEnumerable),
               () => Guard.Argument(() => withNullRefEnumerable).MaxCount(withNullRef.Count - 2, (e, max) =>
               {
                   Assert.Same(withNullRefEnumerable, e);
                   Assert.Equal(withNullRef.Count - 2, max);
                   return message;
               }));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count - 1);
            Assert.StartsWith(message, ex.Message);

            // Max count of collection.
            withNullRefCollectionArg.MaxCount(withNullRef.Count).MaxCount(withNullRef.Count + 2);
            CheckAndResetCollection(withNullRefCollection, true, false);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).MaxCount(withNullRef.Count - 1));

            CheckAndResetCollection(withNullRefCollection, true, false);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).MaxCount(withNullRef.Count - 1, (c, max) =>
                {
                    Assert.Same(withNullRefCollection, c);
                    Assert.Equal(withNullRef.Count - 1, max);
                    return message;
                }));

            CheckAndResetCollection(withNullRefCollection, true, false);
            Assert.StartsWith(message, ex.Message);

            // Count range of enumerable.
            nullRefEnumerableArg
                .CountInRange(0, withNullRef.Count)
                .CountInRange(withNullRef.Count, withNullRef.Count + 1);

            withNullRefEnumerableArg.CountInRange(0, withNullRef.Count);
            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            withNullRefEnumerableArg.CountInRange(withNullRef.Count, withNullRef.Count + 1);
            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable),
                () => Guard.Argument(() => withNullRefEnumerable).CountInRange(0, withNullRef.Count - 2));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count - 1);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable),
                () => Guard.Argument(() => withNullRefEnumerable).CountInRange(0, withNullRef.Count - 2, (e, min, max) =>
                {
                    Assert.Same(withNullRefEnumerable, e);
                    Assert.Equal(0, min);
                    Assert.Equal(withNullRef.Count - 2, max);
                    return message;
                }));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count - 1);
            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable),
                () => Guard.Argument(() => withNullRefEnumerable).CountInRange(withNullRef.Count + 1, withNullRef.Count + 2));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable),
                () => Guard.Argument(() => withNullRefEnumerable).CountInRange(withNullRef.Count + 1, withNullRef.Count + 2, (e, min, max) =>
                {
                    Assert.Same(withNullRefEnumerable, e);
                    Assert.Equal(withNullRef.Count + 1, min);
                    Assert.Equal(withNullRef.Count + 2, max);
                    return message;
                }));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);
            Assert.StartsWith(message, ex.Message);

            // Count range of collection.
            withNullRefCollectionArg.CountInRange(0, withNullRef.Count);
            CheckAndResetCollection(withNullRefCollection, true, false);

            withNullRefCollectionArg.CountInRange(withNullRef.Count, withNullRef.Count + 1);
            CheckAndResetCollection(withNullRefCollection, true, false);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).CountInRange(0, withNullRef.Count - 2));

            CheckAndResetCollection(withNullRefCollection, true, false);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).CountInRange(0, withNullRef.Count - 2, (c, min, max) =>
                {
                    Assert.Same(withNullRefCollection, c);
                    Assert.Equal(0, min);
                    Assert.Equal(withNullRef.Count - 2, max);
                    return message;
                }));

            CheckAndResetCollection(withNullRefCollection, true, false);
            Assert.StartsWith(message, ex.Message);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).CountInRange(withNullRef.Count + 1, withNullRef.Count + 2));

            CheckAndResetCollection(withNullRefCollection, true, false);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).CountInRange(withNullRef.Count + 1, withNullRef.Count + 2, (c, min, max) =>
                {
                    Assert.Same(withNullRefCollection, c);
                    Assert.Equal(withNullRef.Count + 1, min);
                    Assert.Equal(withNullRef.Count + 2, max);
                    return message;
                }));

            CheckAndResetCollection(withNullRefCollection, true, false);
            Assert.StartsWith(message, ex.Message);

            // Enumerable contains.
            nullRefEnumerableArg.Contains(validNullableRef);

            withNullRefEnumerableArg.Contains(validNullableRef);
            CheckAndResetEnumerable(withNullRefEnumerable, validNullableRefIndex + 1);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable), () => Guard.Argument(() => withNullRefEnumerable).Contains(invalidNullableRef));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable), () => Guard.Argument(() => withNullRefEnumerable).Contains(invalidNullableRef, (e, item) =>
                {
                    Assert.Same(withNullRefEnumerable, e);
                    Assert.Equal(invalidNullableRef, item);
                    return message;
                }));

            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);
            Assert.StartsWith(message, ex.Message);

            // Collection contains.
            withNullRefCollectionArg.Contains(validNullableRef);
            CheckAndResetCollection(withNullRefCollection, false, true);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection), () => Guard.Argument(() => withNullRefCollection).Contains(invalidNullableRef));

            CheckAndResetCollection(withNullRefCollection, false, true);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection), () => Guard.Argument(() => withNullRefCollection).Contains(invalidNullableRef, (c, item) =>
                {
                    Assert.Same(withNullRefCollection, c);
                    Assert.Equal(invalidNullableRef, item);
                    return message;
                }));

            CheckAndResetCollection(withNullRefCollection, false, true);
            Assert.StartsWith(message, ex.Message);

            // Enumerable does not contain.
            nullRefEnumerableArg.DoesNotContain(validNullableRef);

            withNullRefEnumerableArg.DoesNotContain(invalidNullableRef);
            CheckAndResetEnumerable(withNullRefEnumerable, withNullRef.Count);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable),
                () => Guard.Argument(() => withNullRefEnumerable).DoesNotContain(validNullableRef));

            CheckAndResetEnumerable(withNullRefEnumerable, validNullableRefIndex + 1);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable),
                () => Guard.Argument(() => withNullRefEnumerable).DoesNotContain(validNullableRef, (e, item) =>
                {
                    Assert.Same(withNullRefEnumerable, e);
                    Assert.Equal(validNullableRef, item);
                    return message;
                }));

            CheckAndResetEnumerable(withNullRefEnumerable, validNullableRefIndex + 1);
            Assert.StartsWith(message, ex.Message);

            // Collection does not contain.
            withNullRefCollectionArg.DoesNotContain(invalidNullableRef);
            CheckAndResetCollection(withNullRefCollection, false, true);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).DoesNotContain(validNullableRef));

            CheckAndResetCollection(withNullRefCollection, false, true);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection),
                () => Guard.Argument(() => withNullRefCollection).DoesNotContain(validNullableRef, (c, item) =>
                {
                    Assert.Same(withNullRefCollection, c);
                    Assert.Equal(validNullableRef, item);
                    return message;
                }));

            CheckAndResetCollection(withNullRefCollection, false, true);
            Assert.StartsWith(message, ex.Message);

            // Enumerable contains null class.
            nullRefEnumerableArg.ContainsNull();

            withNullRefEnumerableArg.ContainsNull();
            CheckAndResetEnumerable(withNullRefEnumerable, nullIndex + 1);

            Assert.Throws<ArgumentException>(
                nameof(withoutNullRefEnumerable),
                () => Guard.Argument(() => withoutNullRefEnumerable).ContainsNull());

            CheckAndResetEnumerable(withoutNullRefEnumerable, withoutNullRef.Count);

            ex = Assert.Throws<ArgumentException>(
                nameof(withoutNullRefEnumerable),
                () => Guard.Argument(() => withoutNullRefEnumerable).ContainsNull(e =>
                {
                    Assert.Same(withoutNullRefEnumerable, e);
                    return message;
                }));

            CheckAndResetEnumerable(withoutNullRefEnumerable, withoutNullRef.Count);
            Assert.StartsWith(message, ex.Message);

            // Enumerable contains null struct.
            withNullValEnumerableArg.ContainsNull();
            CheckAndResetEnumerable(withNullValEnumerable, nullIndex + 1);

            Assert.Throws<ArgumentException>(
               nameof(withoutNullValEnumerable),
               () => Guard.Argument(() => withoutNullValEnumerable).ContainsNull());

            CheckAndResetEnumerable(withoutNullValEnumerable, withoutNullVal.Count);

            ex = Assert.Throws<ArgumentException>(
               nameof(withoutNullValEnumerable),
               () => Guard.Argument(() => withoutNullValEnumerable).ContainsNull(e =>
               {
                   Assert.Same(withoutNullValEnumerable, e);
                   return message;
               }));

            CheckAndResetEnumerable(withoutNullValEnumerable, withoutNullVal.Count);
            Assert.StartsWith(message, ex.Message);

            // Collection contains null class.
            withNullRefCollectionArg.ContainsNull();
            CheckAndResetCollection(withNullRefCollection, false, true);

            Assert.Throws<ArgumentException>(
                nameof(withoutNullRefCollection),
                () => Guard.Argument(() => withoutNullRefCollection).ContainsNull());

            CheckAndResetCollection(withoutNullRefCollection, false, true);

            ex = Assert.Throws<ArgumentException>(
                nameof(withoutNullRefCollection),
                () => Guard.Argument(() => withoutNullRefCollection).ContainsNull(c =>
                {
                    Assert.Same(withoutNullRefCollection, c);
                    return message;
                }));

            CheckAndResetCollection(withoutNullRefCollection, false, true);
            Assert.StartsWith(message, ex.Message);

            // Collection contains null struct.
            withNullValCollectionArg.ContainsNull();
            CheckAndResetCollection(withNullValCollection, false, true);

            Assert.Throws<ArgumentException>(
               nameof(withoutNullValCollection),
               () => Guard.Argument(() => withoutNullValCollection).ContainsNull());

            CheckAndResetCollection(withoutNullValCollection, false, false);

            ex = Assert.Throws<ArgumentException>(
               nameof(withoutNullValCollection),
               () => Guard.Argument(() => withoutNullValCollection).ContainsNull(c =>
               {
                   Assert.Same(withoutNullValCollection, c);
                   return message;
               }));

            CheckAndResetCollection(withoutNullValCollection, false, false);
            Assert.StartsWith(message, ex.Message);

            // Enumerable does not contain null class.
            nullRefEnumerableArg.DoesNotContainNull();

            withoutNullRefEnumerableArg.DoesNotContainNull();
            CheckAndResetEnumerable(withoutNullRefEnumerable, withoutNullRef.Count);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable),
                () => Guard.Argument(() => withNullRefEnumerable).DoesNotContainNull());

            CheckAndResetEnumerable(withNullRefEnumerable, nullIndex + 1);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefEnumerable),
                () => Guard.Argument(() => withNullRefEnumerable).DoesNotContainNull(e =>
                {
                    Assert.Same(withNullRefEnumerable, e);
                    return message;
                }));

            CheckAndResetEnumerable(withNullRefEnumerable, nullIndex + 1);
            Assert.StartsWith(message, ex.Message);

            // Enumerable does not contain nullable struct.
            withoutNullValEnumerableArg.DoesNotContainNull();
            CheckAndResetEnumerable(withoutNullValEnumerable, withoutNullVal.Count);

            Assert.Throws<ArgumentException>(
               nameof(withNullValEnumerable), () => Guard.Argument(() => withNullValEnumerable).DoesNotContainNull());

            CheckAndResetEnumerable(withNullValEnumerable, nullIndex + 1);

            ex = Assert.Throws<ArgumentException>(
               nameof(withNullValEnumerable), () => Guard.Argument(() => withNullValEnumerable).DoesNotContainNull(e =>
               {
                   Assert.Same(withNullValEnumerable, e);
                   return message;
               }));

            CheckAndResetEnumerable(withNullValEnumerable, nullIndex + 1);
            Assert.StartsWith(message, ex.Message);

            // Collection does not contain null class.
            withoutNullRefCollectionArg.DoesNotContainNull();
            CheckAndResetCollection(withoutNullRefCollection, false, true);

            Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection), () => Guard.Argument(() => withNullRefCollection).DoesNotContainNull());

            CheckAndResetCollection(withNullRefCollection, false, true);

            ex = Assert.Throws<ArgumentException>(
                nameof(withNullRefCollection), () => Guard.Argument(() => withNullRefCollection).DoesNotContainNull(c =>
                {
                    Assert.Same(withNullRefCollection, c);
                    return message;
                }));

            CheckAndResetCollection(withNullRefCollection, false, true);
            Assert.StartsWith(message, ex.Message);

            // Collection does not contain null struct.
            withoutNullValCollectionArg.DoesNotContainNull();
            CheckAndResetCollection(withoutNullValCollection, false, false);

            Assert.Throws<ArgumentException>(
               nameof(withNullValCollection), () => Guard.Argument(() => withNullValCollection).DoesNotContainNull());

            CheckAndResetCollection(withNullValCollection, false, true);

            ex = Assert.Throws<ArgumentException>(
               nameof(withNullValCollection), () => Guard.Argument(() => withNullValCollection).DoesNotContainNull(c =>
               {
                   Assert.Same(withNullValCollection, c);
                   return message;
               }));

            CheckAndResetCollection(withNullValCollection, false, true);
            Assert.StartsWith(message, ex.Message);

            // Testing helpers.
            void CheckAndResetEnumerable<T>(TestEnumerable<T> enumerable, int iterationCount, bool? enumerated = null)
            {
                if (!enumerated.HasValue)
                    enumerated = iterationCount > 0;

                Assert.Equal(enumerated, enumerable.Enumerated);
                Assert.Equal(iterationCount, enumerable.IterationCount);

                enumerable.Reset();
                Assert.False(enumerable.Enumerated);
                Assert.Equal(0, enumerable.IterationCount);
            }

            void CheckAndResetCollection<T>(TestCollection<T> collection, bool countCalled, bool containsCalled)
            {
                Assert.Equal(countCalled, collection.CountCalled);
                Assert.Equal(containsCalled, collection.ContainsCalled);

                Assert.False(collection.Enumerated);
                Assert.Equal(0, collection.IterationCount);

                collection.Reset();
                Assert.False(collection.CountCalled);
                Assert.False(collection.ContainsCalled);
            }
        }

        private class TestEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> items;

            public TestEnumerable(IEnumerable<T> items)
                => this.items = items;

            public bool Enumerated { get; private set; }

            public int IterationCount { get; private set; }

            public IEnumerator<T> GetEnumerator()
            {
                this.Enumerated = true;
                foreach (var item in this.items)
                {
                    this.IterationCount++;
                    yield return item;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public virtual void Reset()
            {
                this.Enumerated = false;
                this.IterationCount = 0;
            }
        }

        private sealed class TestCollection<T> : TestEnumerable<T>
        {
            private readonly IReadOnlyCollection<T> items;

            public TestCollection(IReadOnlyCollection<T> items)
                : base(items) => this.items = items;

            public int Count
            {
                get
                {
                    this.CountCalled = true;
                    return this.items.Count;
                }
            }

            public bool CountCalled { get; private set; }

            public bool ContainsCalled { get; private set; }

            public bool Contains(T item)
            {
                this.ContainsCalled = true;
                return this.items.Contains(item);
            }

            public override void Reset()
            {
                base.Reset();
                this.CountCalled = false;
                this.ContainsCalled = false;
            }
        }
    }
}
