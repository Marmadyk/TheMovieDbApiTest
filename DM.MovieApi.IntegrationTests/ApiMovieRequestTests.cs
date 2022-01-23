using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.Movies
{
    [TestClass]
    public class ApiMovieRequestTests
    {
        private IApiMovieRequest _api;

        [TestInitialize]
        public void TestInitialization()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiMovieRequest>().Value;

            Assert.IsInstanceOfType( _api, typeof( ApiMovieRequest ) );
        }

        [TestMethod]
        public async Task GetLatestAsyncWithValidResult()
        {
            ApiQueryResponse<Movie> response = await _api.GetLatestAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            ApiResponseUtil.AssertMovieStructure( response.Item );
        }

        [TestMethod]
        public async Task GetNowPlayingAsyncWithValidResults()
        {
            ApiSearchResponse<Movie> response = await _api.GetNowPlayingAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            ApiResponseUtil.AssertMovieStructure( response.Results );
        }

        [TestMethod]
        public async Task GetNowPlayingAsyncWithPageResults()
        {
            const int minimumPageCount = 5;
            const int minimumTotalResultsCount = 100;

            await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount, minimumTotalResultsCount,
                ( _, page ) => _api.GetNowPlayingAsync( page ), x => x.Id );
        }

        [TestMethod]
        public async Task GetUpcomingAsyncWithValidResults()
        {
            ApiSearchResponse<Movie> response = await _api.GetUpcomingAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            ApiResponseUtil.AssertMovieStructure( response.Results );
        }

        [TestMethod]
        public async Task GetUpcomingAsyncWithPageResults()
        {
            const int minimumPageCount = 3;
            const int minimumTotalResultsCount = 50;

            await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount, minimumTotalResultsCount,
                ( _, page ) => _api.GetUpcomingAsync( page ), x => x.Id );
        }

        [TestMethod]
        public async Task GetTopRatedAsyncWithValidResults()
        {
            ApiSearchResponse<MovieInfo> response = await _api.GetTopRatedAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            IReadOnlyList<MovieInfo> results = response.Results;

            ApiResponseUtil.AssertMovieInformationStructure( results );
        }

        [TestMethod]
        public async Task GetTopRatedAsyncWithPageResults()
        {
            const int minimumPageCount = 2;
            const int minimumTotalResultsCount = 40;

            await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount, minimumTotalResultsCount,
                ( _, page ) => _api.GetTopRatedAsync( page ), x => x.Id );
        }

        [TestMethod]
        public async Task GetPopularAsyncWithValidResults()
        {
            ApiSearchResponse<MovieInfo> response = await _api.GetPopularAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            IReadOnlyList<MovieInfo> results = response.Results;

            ApiResponseUtil.AssertMovieInformationStructure( results );
        }

        [TestMethod]
        public async Task GetPopularAsyncWithPageResults()
        {
            const int minimumPageCount = 2;
            const int minimumTotalResultsCount = 40;

            await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount, minimumTotalResultsCount,
                ( _, page ) => _api.GetPopularAsync( page ), x => x.Id );
        }
    }
}
