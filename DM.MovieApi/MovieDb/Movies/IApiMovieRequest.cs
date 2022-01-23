using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;

namespace DM.MovieApi.MovieDb.Movies
{
    public interface IApiMovieRequest : IApiRequest
    {
        Task<ApiQueryResponse<Movie>> GetLatestAsync( string language = "en" );

        Task<ApiSearchResponse<Movie>> GetNowPlayingAsync( int pageNumber = 1, string language = "en" );

        Task<ApiSearchResponse<Movie>> GetUpcomingAsync( int pageNumber = 1, string language = "en" );

        Task<ApiSearchResponse<MovieInfo>> GetTopRatedAsync( int pageNumber = 1, string language = "en" );

        Task<ApiSearchResponse<MovieInfo>> GetPopularAsync( int pageNumber = 1, string language = "en" );
    }
}
