using DM.MovieApi.ApiRequest;

namespace DM.MovieApi.IntegrationTests.Infrastructure
{
    internal class IntegrationApiRequest : ApiRequestBase
    {
        public IntegrationApiRequest( IApiSettings settings ) : base( settings ) { }
    }
}
