using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DM.MovieApi.IntegrationTests.Infrastructure
{
    internal class IntegrationMovieDbSettings : IApiSettings
    {
        public const string FileName = "api.creds.json";
        public readonly string FilePath = Path.Combine( RootDirectory, FileName );

        public string ApiUrl => MovieDbFactory.TheMovieDbApiUrl;

        public string BearerToken { get; private set; }

        public IntegrationMovieDbSettings()
        {
            Hydrate();
        }

        public IntegrationMovieDbSettings( string apiBearerToken )
        {
            BearerToken = apiBearerToken;
        }

        private void Hydrate()
        {
            var anon = new
            {
                BearerToken = "your-v4-bearer-token-here"
            };

            if( File.Exists( FilePath ) == false )
            {
                string structure = JsonConvert.SerializeObject( anon, Formatting.Indented );

                Assert.Fail($"{FileName}{FilePath}" + $"{FileName}" + $"{structure}");
            }

            var json = File.ReadAllText( FilePath );
            var api = JsonConvert.DeserializeAnonymousType( json, anon );

            BearerToken = api.BearerToken;
        }

        private static readonly Lazy<string> _rootDirectory = new( GetRootDirectory );

        internal static string RootDirectory => _rootDirectory.Value;

        private static string GetRootDirectory()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string dir = Path.GetDirectoryName( location );

            Assert.IsFalse( string.IsNullOrEmpty( dir ) );
            Assert.IsTrue( Directory.Exists( dir ) );

            return dir;
        }
    }
}
