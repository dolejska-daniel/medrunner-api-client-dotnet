namespace Arkanis.External.MedRunner.API.Endpoints.WebSocket;

using Abstractions;
using Microsoft.Extensions.Logging;

/// <summary>
///     Endpoints for interacting with websocket/realtime updates.
/// </summary>
public class WebsocketEndpoint(ApiConfig config, ITokenProvider tokenProvider, ILogger logger) : ApiEndpoint(config, tokenProvider, logger)
{
    private readonly WebsocketManager _websocketManager = new(config, tokenProvider);

    protected override string Endpoint
        => "websocket";

    /// <summary>
    ///     Gets realtime updates (establishes websocket connection).
    /// </summary>
    public async Task<object> InitializeAsync()
        => await _websocketManager.EstablishConnectionAsync();
}
