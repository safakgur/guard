namespace Dawn.Tests
{
    public sealed partial class GuardTests
    {
        /// <summary>A prefix for the test names identifying the current compilation target.</summary>
#if NETCOREAPP1_0
        const string T = "NS1 "; // Targeting .NET Standard 1.0.
#elif NETCOREAPP2_0
        const string T = "NS2 "; // Targeting .NET Standard 2.0.
#endif
    }
}
