using DM.MovieApi.IntegrationTests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests
{
    [TestClass]
    public class AssemblyInit
    {
        internal static readonly IApiSettings Settings = new IntegrationMovieDbSettings();

        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            RegisterFactorySettings();
        }

        public static void RegisterFactorySettings()
        {
            MovieDbFactory.RegisterSettings( Settings.BearerToken );
        }
    }
}
