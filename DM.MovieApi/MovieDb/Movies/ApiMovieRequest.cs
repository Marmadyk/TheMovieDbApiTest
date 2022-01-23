using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.Shims;

namespace DM.MovieApi.MovieDb.Movies
{
    internal class ApiMovieRequest : ApiRequestBase, IApiMovieRequest
    {
        [ImportingConstructor]
        public ApiMovieRequest(IApiSettings settings) : base(settings) { }

        public async Task<ApiQueryResponse<Movie>> GetLatestAsync( string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
                {"append_to_response", "keywords"},
            };

            const string command = "movie/latest";

            ApiQueryResponse<Movie> response = await base.QueryAsync<Movie>( command, param );

            return response;
        }

        public async Task<ApiSearchResponse<Movie>> GetNowPlayingAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
                {"append_to_response", "keywords"},
            };

            const string command = "movie/now_playing";

            ApiSearchResponse<Movie> response = await base.SearchAsync<Movie>( command, pageNumber, param );

            return response;
        }

        public async Task<ApiSearchResponse<Movie>> GetUpcomingAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
                {"append_to_response", "keywords"},
            };

            const string command = "movie/upcoming";

            ApiSearchResponse<Movie> response = await base.SearchAsync<Movie>( command, pageNumber, param );

            return response;
        }

        public async Task<ApiSearchResponse<MovieInfo>> GetTopRatedAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            const string command = "movie/top_rated";

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

            return response;
        }

        public async Task<ApiSearchResponse<MovieInfo>> GetPopularAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            const string command = "movie/popular";

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

            return response;
        }
    }
}
