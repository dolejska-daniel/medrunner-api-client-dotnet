namespace Arkanis.External.MedRunner.API.Abstractions;

public interface ITokenProvider
{
    Task<string?> GetAccessTokenAsync();

    Task<string?> GetAccessTokenAsync(string source);
}
