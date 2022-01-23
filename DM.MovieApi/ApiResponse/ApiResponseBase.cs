namespace DM.MovieApi.ApiResponse
{
    public abstract class ApiResponseBase
    {
        public ApiError Error { get; internal set; }

        public string CommandText { get; internal set; }

        public string Json { get; internal set; }

        public override string ToString()
            => CommandText;
    }
}
