namespace DM.MovieApi.ApiResponse
{
    public class ApiQueryResponse<T> : ApiResponseBase
    {
        public T Item { get; internal set; }

        public override string ToString()
            => Item.ToString();
    }
}
