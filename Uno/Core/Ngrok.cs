using System.Net.Http.Json;
using System.Text.Json;

namespace Uno.Core;

public static class Ngrok
{
    private const string Key = "2jqKLheD3ijJQS5DPIj8eQu1lwh_89reH7ikUKuy9QRbtSfKQ";
    private static readonly HttpClient Client = new()
    {
        BaseAddress = new("https://api.ngrok.com/"),
        DefaultRequestHeaders =
        {
            {"Authorization", $"Bearer {Key}"},
            {"Ngrok-Version", "2"}
        }
    };

    public static async Task<string> GetUriAsync()
    {
        var element = await Client.GetFromJsonAsync<JsonElement>("tunnels");
        return element
            .GetProperty("tunnels")[0]
            .GetProperty("public_url")
            .ToString();
    }
}