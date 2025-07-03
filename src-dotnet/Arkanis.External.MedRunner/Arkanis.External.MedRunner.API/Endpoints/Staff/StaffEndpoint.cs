namespace Arkanis.External.MedRunner.API.Endpoints.Staff;

using Abstractions;
using Microsoft.Extensions.Logging;
using Response;

/// <summary>
///     Endpoints for interacting with staff.
/// </summary>
public class StaffEndpoint(ApiConfig config, ITokenProvider tokenProvider, ILogger logger) : ApiEndpoint(config, tokenProvider, logger)
{
    protected override string Endpoint
        => "staff";

    /// <summary>
    ///     Gets detailed information about medals.
    /// </summary>
    public async Task<ApiResponse<List<MedalInformation>>> MedalsInformationAsync()
        => await GetRequestAsync<List<MedalInformation>>("/meta/medals");
}
