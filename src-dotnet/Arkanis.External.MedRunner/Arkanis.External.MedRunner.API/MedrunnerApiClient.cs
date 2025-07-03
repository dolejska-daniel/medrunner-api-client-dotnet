namespace Arkanis.External.MedRunner.API;

using Abstractions;
using Endpoints.Auth;
using Endpoints.ChatMessage;
using Endpoints.Client;
using Endpoints.Code;
using Endpoints.Emergency;
using Endpoints.OrgSettings;
using Endpoints.Staff;
using Endpoints.WebSocket;
using Microsoft.Extensions.Logging;

/// <summary>
///     An API client for basic client interactions with the Medrunner API.
/// </summary>
public class MedrunnerApiClient(ApiConfig config, ITokenProvider tokenProvider, ILogger<MedrunnerApiClient> logger) : IApiClient
{
    public EmergencyEndpoint Emergency { get; } = new(config, tokenProvider, logger);

    public ClientEndpoint Client { get; } = new(config, tokenProvider, logger);

    public StaffEndpoint Staff { get; } = new(config, tokenProvider, logger);

    public OrgSettingsEndpoint OrgSettings { get; } = new(config, tokenProvider, logger);

    public ChatMessageEndpoint ChatMessage { get; } = new(config, tokenProvider, logger);

    public CodeEndpoint Code { get; } = new(config, tokenProvider, logger);

    public AuthEndpoint Auth { get; } = new(config, tokenProvider, logger);

    public WebsocketEndpoint WebSocket { get; } = new(config, tokenProvider, logger);
}
