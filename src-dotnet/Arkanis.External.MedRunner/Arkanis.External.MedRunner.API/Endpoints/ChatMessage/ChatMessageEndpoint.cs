namespace Arkanis.External.MedRunner.API.Endpoints.ChatMessage;

using System.Globalization;
using Abstractions;
using Microsoft.Extensions.Logging;
using Models;

/// <summary>
///     Endpoints for interacting with chat messages.
/// </summary>
public class ChatMessageEndpoint(ApiConfig config, ITokenProvider tokenProvider, ILogger logger) : ApiEndpoint(config, tokenProvider, logger)
{
    protected override string Endpoint
        => "chatMessage";

    /// <summary>
    ///     Fetch a chat message by id.
    /// </summary>
    public async Task<ApiResponse<ChatMessage>> GetMessageAsync(string id)
        => await GetRequestAsync<ChatMessage>(id);

    /// <summary>
    ///     Gets the specified amount of chat messages for a given emergency.
    /// </summary>
    public async Task<ApiResponse<ApiPaginatedResponse<ChatMessage>>> GetMessageHistoryAsync(string emergencyId, int limit, string? paginationToken = null)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["limit"] = limit.ToString(CultureInfo.InvariantCulture),
        };
        if (!string.IsNullOrEmpty(paginationToken))
        {
            queryParams["paginationToken"] = paginationToken;
        }

        return await GetRequestAsync<ApiPaginatedResponse<ChatMessage>>($"/conversation/{emergencyId}", queryParams);
    }

    /// <summary>
    ///     Sends a new chat message.
    /// </summary>
    public async Task<ApiResponse<ChatMessage>> SendMessageAsync(object message)
        => await PostRequestAsync<ChatMessage>("", message);

    /// <summary>
    ///     Update a chat message.
    /// </summary>
    public async Task<ApiResponse<ChatMessage>> UpdateMessageAsync(string id, string contents)
    {
        var payload = new { contents };
        return await PutRequestAsync<ChatMessage>(id, payload);
    }

    /// <summary>
    ///     Delete a chat message.
    /// </summary>
    public async Task<ApiResponse<string>> DeleteMessageAsync(string id)
        => await DeleteRequestAsync<string>(id);
}
