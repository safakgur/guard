#if !NETCOREAPP1_0

namespace Dawn.Tests
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class ScopeTests : BaseTests
    {
        [Fact(DisplayName = T + "BeginScope")]
        public void BeginScope()
        {
            Task.WaitAll(
                Task.Run(Async()),
                Task.Run(Async()));

            Func<Task> Async() => async () =>
            {
                // Outer
                var id = 0;
                Exception outerIntercepted = null;
                Guard.BeginScope(ex =>
                {
                    Assert.NotNull(ex.StackTrace);
                    id++;
                    outerIntercepted = ex;
                });

                var lastId = id;
                for (var i = 0; i < 5; i++)
                {

                    Test(ref outerIntercepted);
                    Assert.Equal(lastId + 1, id);
                    id--;

                    await Delay();

                    Test(ref outerIntercepted);
                    Assert.Equal(lastId + 1, id);
                    id--;

                    await Delay().ConfigureAwait(false);

                    Test(ref outerIntercepted);
                    Assert.Equal(lastId + 1, id);
                    id--;
                }

                // Inner with propagation
                id = 0;
                lastId = id;
                for (var i = 0; i < 5; i++)
                {
                    if (i == 3)
                        Guard.BeginScope(null); // Should have no effect.

                    Exception innerIntercepted = null;
                    using (Guard.BeginScope(ex =>
                    {
                        Assert.NotNull(ex.StackTrace);
                        id++;
                        innerIntercepted = ex;
                    }))
                    {
                        Test(ref innerIntercepted);
                        Assert.Equal(lastId + 2, id);
                        id -= 2;

                        await Delay();

                        Test(ref innerIntercepted);
                        Assert.Equal(lastId + 2, id);
                        id -= 2;

                        await Delay().ConfigureAwait(false);

                        Test(ref innerIntercepted);
                        Assert.Equal(lastId + 2, id);
                        id -= 2;
                    }
                }

                // Inner without propagation
                id = 0;
                lastId = id;
                for (var i = 0; i < 5; i++)
                {
                    if (i == 3)
                        Guard.BeginScope(null, false); // Should stop propagation.

                    Exception innerIntercepted = null;
                    using (Guard.BeginScope(ex =>
                    {
                        Assert.NotNull(ex.StackTrace);
                        id += 3;
                        innerIntercepted = ex;
                    }, i >= 3))
                    {
                        Test(ref innerIntercepted);
                        Assert.Equal(lastId + 3, id);
                        id -= 3;

                        await Delay();

                        Test(ref innerIntercepted);
                        Assert.Equal(lastId + 3, id);
                        id -= 3;

                        await Delay().ConfigureAwait(false);

                        Test(ref innerIntercepted);
                        Assert.Equal(lastId + 3, id);
                        id -= 3;
                    }
                }
            };

            void Test(ref Exception intercepted)
            {
                try
                {
                    Guard.Argument(null as string, "value").NotNull();
                }
                catch (ArgumentNullException ex)
                {
                    Assert.Same(ex, intercepted);
                }
            }

            Task Delay()
                => Task.Delay(RandomUtils.Current.Next(10, 20));
        }
    }
}

#endif
