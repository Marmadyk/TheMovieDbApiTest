using DM.MovieApi.MovieDb.Movies;

namespace DM.MovieApi
{
    public interface IMovieDbApi
    {
        IApiMovieRequest Movies { get; }
    }
}
