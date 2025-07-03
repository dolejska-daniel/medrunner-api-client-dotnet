namespace Arkanis.External.MedRunner.API.Endpoints;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Abstractions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

/// <summary>
///     Base class for API endpoints.
/// </summary>
public abstract class ApiEndpoint(ApiConfig config, ITokenProvider tokenProvider, ILogger logger, HttpClient? httpClient = null)
{
    private readonly HttpClient _httpClient = httpClient ?? new HttpClient();

    private static readonly Action<ILogger, string, string, Exception?> LogRequest = LoggerMessage.Define<string, string>(LogLevel.Debug, default, "{Method} {Url}");
    private static readonly Action<ILogger, string, string, string, Exception?> LogRequestError = LoggerMessage.Define<string, string, string>(LogLevel.Error, default, "Received API error response for {Method} {Url}: {ErrorMessage}");
    private static readonly Action<ILogger, string, string, Exception?> LogRequestException = LoggerMessage.Define<string, string>(LogLevel.Error, default, "An exception occured while handling {Method} {Url}");

    /// <summary>
    ///     Returns the endpoint path (to be implemented by derived classes).
    /// </summary>
    protected abstract string Endpoint { get; }

    /// <summary>
    ///     Returns the full endpoint URL.
    /// </summary>
    protected string EndpointUrl()
        => $"{config.BaseUrl.TrimEnd('/')}/{Endpoint.TrimStart('/')}";

    /// <summary>
    ///     Builds the full URL for a request.
    /// </summary>
    protected string BuildUrl(string endpoint, Dictionary<string, string>? queryParams = null)
    {
        var baseUrl = EndpointUrl().TrimEnd('/');
        var url = $"{baseUrl}/{endpoint.TrimStart('/')}";
        return QueryHelpers.AddQueryString(url, queryParams);
    }

    /// <summary>
    ///     Adds headers for a request, including authentication if needed.
    /// </summary>
    protected virtual async Task<HttpRequestMessage> CreateRequestMessageAsync(
        HttpMethod method,
        string url,
        bool noAuthentication = false,
        object? body = null
    )
    {
        var request = new HttpRequestMessage(method, url);
        if (!noAuthentication)
        {
            var accessToken = await tokenProvider.GetAccessTokenAsync("API makeRequest");
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return request;
    }

    /// <summary>
    ///     Sends a GET request.
    /// </summary>
    protected async Task<ApiResponse<T>> GetRequestAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null, bool noAuthentication = false)
    {
        var url = BuildUrl(endpoint, queryParams);
        var request = await CreateRequestMessageAsync(HttpMethod.Get, url, noAuthentication);
        return await SendRequestAsync<T>(request, "GET", url);
    }

    /// <summary>
    ///     Sends a POST request.
    /// </summary>
    protected async Task<ApiResponse<T>> PostRequestAsync<T>(string endpoint, object? data = null, bool noAuthentication = false)
    {
        var url = BuildUrl(endpoint);
        var request = await CreateRequestMessageAsync(HttpMethod.Post, url, noAuthentication, data);
        return await SendRequestAsync<T>(request, "POST", url);
    }

    /// <summary>
    ///     Sends a PUT request.
    /// </summary>
    protected async Task<ApiResponse<T>> PutRequestAsync<T>(string endpoint, object? data = null, bool noAuthentication = false)
    {
        var url = BuildUrl(endpoint);
        var request = await CreateRequestMessageAsync(HttpMethod.Put, url, noAuthentication, data);
        return await SendRequestAsync<T>(request, "PUT", url);
    }

    /// <summary>
    ///     Sends a PATCH request.
    /// </summary>
    protected async Task<ApiResponse<T>> PatchRequestAsync<T>(string endpoint, object? data = null, bool noAuthentication = false)
    {
        var url = BuildUrl(endpoint);
        var request = await CreateRequestMessageAsync(new HttpMethod("PATCH"), url, noAuthentication, data);
        return await SendRequestAsync<T>(request, "PATCH", url);
    }

    /// <summary>
    ///     Sends a DELETE request.
    /// </summary>
    protected async Task<ApiResponse<T>> DeleteRequestAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null, bool noAuthentication = false)
    {
        var url = BuildUrl(endpoint, queryParams);
        var request = await CreateRequestMessageAsync(HttpMethod.Delete, url, noAuthentication);
        return await SendRequestAsync<T>(request, "DELETE", url);
    }

    /// <summary>
    ///     Sends the HTTP request and handles the response and errors.
    /// </summary>
    private async Task<ApiResponse<T>> SendRequestAsync<T>(HttpRequestMessage request, string method, string url)
    {
        LogRequest(logger, method, url, null);
        try
        {
            using var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<T>(content);
                return new ApiResponse<T>
                {
                    Success = true,
                    Data = data,
                };
            }

            LogRequestError(logger, method, url, content, null);
            return new ApiResponse<T>
            {
                Success = false,
                ErrorMessage = content,
                StatusCode = (int)response.StatusCode,
            };
        }
        catch (Exception ex)
        {
            LogRequestException(logger, method, url, ex);
            return new ApiResponse<T>
            {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
    }
}
