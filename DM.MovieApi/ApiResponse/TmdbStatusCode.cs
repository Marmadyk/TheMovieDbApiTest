using System.Diagnostics.CodeAnalysis;

namespace DM.MovieApi.ApiResponse
{
    [SuppressMessage( "ReSharper", "UnusedMember.Global" )]
    public enum TmdbStatusCode
    {
        Unknown = 0,

        Success = 1,

        InvalidService = 2,

        InsufficientPermissions = 3,

        InvalidFormat = 4,

        InvalidParameters = 5,

        InvalidId = 6,

        InvalidApiKey = 7,

        DuplicateEntry = 8,

        ServiceOffline = 9,

        SuspendedApiKey = 10,

        InternalError = 11,

        SuccessfulUpdate = 12,

        SuccessfulDelete = 13,

        AuthenticationFailed = 14,

        Failed = 15,

        DeviceDenied = 16,

        SessionDenied = 17,

        ValidationFailed = 18,

        InvalidAcceptHeader = 19,

        InvalidDateRange = 20,

        EntryNotFound = 21,

        InvalidPage = 22,

        InvalidDate = 23,

        ServerTimeout = 24,

        RequestOverLimit = 25,

        AuthenticationRequired = 26,

        ResponseObjectOverflow = 27,

        InvalidTimezone = 28,

        ActionMustBeConfirmed = 29,

        InvalidAuthentication = 30,

        AccountDisabled = 31,

        EmailNotVerified = 32,

        InvalidRequestToken = 33,

        ResourceNotFound = 34,
    }
}
