namespace Arkanis.External.MedRunner.API.Endpoints.OrgSettings;

using Abstractions;
using Microsoft.Extensions.Logging;
using Models;

/// <summary>
///     Endpoints for interacting with the public org settings.
/// </summary>
public class OrgSettingsEndpoint(ApiConfig config, ITokenProvider tokenProvider, ILogger logger) : ApiEndpoint(config, tokenProvider, logger)
{
    protected override string Endpoint
        => "orgSettings";

    /// <summary>
    ///     Get the public org settings.
    /// </summary>
    public async Task<ApiResponse<OrgSettings>> GetPublicSettingsAsync()
        => await GetRequestAsync<OrgSettings>("/public");
}
