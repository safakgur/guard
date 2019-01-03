#if !NETCOREAPP1_0

namespace Dawn.Tests
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class ScopeTests : BaseTests
    {
        [Fact(DisplayName = T + "Scope")]
        public void GuardSupportsScopes()
        {
            Task.Run(Async());
            Task.Run(Async());

            Func<Task> Async() => async () =>
            {
                var id = -1;
                Exception outerIntercepted = null;
                Guard.BeginScope(ex =>
                {
                    id = 0;
                    outerIntercepted = ex;
                });

                for (var i = 0; i < 100; i++)
                {
                    // Outer
                    Test(ref outerIntercepted);
                    Assert.Equal(0, id);

                    await Delay();

                    Test(ref outerIntercepted);
                    Assert.Equal(0, id);

                    await Delay().ConfigureAwait(false);

                    Test(ref outerIntercepted);
                    Assert.Equal(0, id);

                    // Inner
                    Exception innerIntercepted = null;
                    using (Guard.BeginScope(ex =>
                    {
                        id = 1;
                        innerIntercepted = ex;
                    }))
                    {
                        Test(ref innerIntercepted);
                        Assert.Equal(1, id);

                        await Delay();

                        Test(ref innerIntercepted);
                        Assert.Equal(1, id);

                        await Delay().ConfigureAwait(false);

                        Test(ref innerIntercepted);
                        Assert.Equal(1, id);
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

            Task Delay() => Task.Delay(RandomUtils.Current.Next(90, 100));
        }
    }
}

#endif
