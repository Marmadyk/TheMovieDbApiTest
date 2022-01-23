using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;


using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests
{
    internal static class ApiResponseUtil
    {
        internal const int TestInitThrottle = 375;
        internal const int PagingThrottle = 225;

        public static void ThrottleTests()
        {
            System.Threading.Thread.Sleep( TestInitThrottle );
        }

        public static void AssertErrorIsNull( ApiResponseBase response )
        {
            Console.WriteLine( response.CommandText );
            Assert.IsNull( response.Error, response.Error?.ToString());
        }

        public static void AssertImagePath( string path )
        {
            Assert.IsTrue( path.StartsWith( "/" ), $"Actual: {path}" );

            Assert.IsTrue(
                path.EndsWith( ".jpg" ) || path.EndsWith( ".png" ),
                $"Actual: {path}" );
        }

        public static async Task AssertCanPageSearchResponse<T, TSearch>( TSearch search, int minimumPageCount, int minimumTotalResultsCount,
            Func<TSearch, int, Task<ApiSearchResponse<T>>> apiSearch, Func<T, int> keySelector )
        {
            if( minimumPageCount < 2 )
            {
                Assert.Fail( "minimumPageCount must be greater than 1." );
            }

            var allFound = new List<T>();
            int pageNumber = 1;

            var priorResults = new Dictionary<int, int>();

            do
            {
                System.Diagnostics.Trace.WriteLine( $"search: {search} | page: {pageNumber}", "ApiResponseUti.AssertCanPageSearchResponse" );
                ApiSearchResponse<T> response = await apiSearch( search, pageNumber );

                AssertErrorIsNull( response );
                Assert.IsNotNull( response.Results );
                Assert.IsTrue( response.Results.Any() );

                if( typeof( T ) == typeof( Movie ) )
                {
                    AssertMovieStructure( ( IEnumerable<Movie> )response.Results );
                }

                if( keySelector == null )
                {
                    allFound.AddRange( response.Results );
                }
                else
                {
                    var current = new List<T>();
                    foreach( T res in response.Results )
                    {
                        int key = keySelector( res );

                        if( priorResults.TryAdd( key, 1 ) )
                        {
                            current.Add( res );
                            continue;
                        }

                        System.Diagnostics.Trace.WriteLine( $"dup on page {response.PageNumber}: {res}" );

                        if( ++priorResults[key] > 2 )
                        {
                            Assert.Fail($"{res}");
                        }
                    }

                    allFound.AddRange( current );
                }

                Assert.AreEqual( pageNumber, response.PageNumber );

                Assert.IsTrue( response.TotalPages >= minimumPageCount,
                    $"Expected minimum of {minimumPageCount} TotalPages. Actual TotalPages: {response.TotalPages}" );

                pageNumber++;

                System.Threading.Thread.Sleep( PagingThrottle );
            } while( pageNumber <= minimumPageCount );

            Assert.AreEqual( minimumPageCount + 1, pageNumber );

            Assert.IsTrue( allFound.Count >= minimumTotalResultsCount, $"Actual found count: {allFound.Count} | Expected min count: {minimumTotalResultsCount}" );

            if( keySelector == null )
            {
                return;
            }

            List<IGrouping<int, T>> groupById = allFound
                .ToLookup( keySelector )
                .ToList();

            List<string> dups = groupById
                .Where( x => x.Skip( 1 ).Any() )
                .Select( x => $"({x.Count()}) {string.Join( " | ", x.Select( y => y.ToString() ) )}" )
                .ToList();

            Assert.AreEqual( 0, dups.Count, "Duplicates: " + Environment.NewLine + string.Join( Environment.NewLine, dups ) );
        }

        public static void AssertMovieStructure( IEnumerable<Movie> movies )
        {
            Assert.IsTrue( movies.Any() );

            foreach( Movie movie in movies )
            {
                AssertMovieStructure( movie );
            }
        }

        public static void AssertMovieStructure( Movie movie )
        {
            Assert.IsTrue( movie.Id > 0 );
            Assert.IsFalse( string.IsNullOrWhiteSpace( movie.Title ) );
        }

        public static void AssertMovieInformationStructure( IEnumerable<MovieInfo> movies )
        {
            Assert.IsTrue( movies.Any() );

            foreach( MovieInfo movie in movies )
            {
                AssertMovieInformationStructure( movie );
            }
        }

        public static void AssertMovieInformationStructure( MovieInfo movie )
        {
            Assert.IsFalse( string.IsNullOrWhiteSpace( movie.Title ) );
            Assert.IsTrue( movie.Id > 0 );
        }
    }
}
