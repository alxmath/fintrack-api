using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Infrastructure;

public static class AuthHelper
{
    public static async Task AuthenticateAsync(HttpClient client)
    {
        var response = await client.PostAsync("/api/v1/auth/login", null);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result!.AccessToken);
    }

    private record AuthResponse(string AccessToken);
}