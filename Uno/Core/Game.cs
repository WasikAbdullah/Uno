using System.Net.Http.Json;
using Uno.Core.Exceptions;
using Uno.Core.Models;

namespace Uno.Core;

public class Game()
{
    public Events Events { get; } = new(); //TODO: Dependency Inject
    public int Id { get; private set; }
    private int _gameId;
    private readonly HttpClient _client = new();
    // {
    //     BaseAddress = new(Ngrok.GetUriAsync().Result)
    // };

    public async Task JoinAsync(string name, int skin, int? gameId)
    {
        _client.BaseAddress = new(await Ngrok.GetUriAsync());
        _gameId = gameId ?? await _client.PostResponseAsJsonAsync<int>("create");
        await Events.ConnectAsync(_client.BaseAddress!.ToString(), _gameId);
        Id = await _client.PostAsJsonResponseAsJsonAsync<object, int>($"join/{_gameId}", new Info
        {
            Name = name,
            Skin = skin
        });
    }

    public Task<int> PickCardAsync() =>
        _client.GetFromJsonAsync<int>($"pick/{_gameId}/{Id}");

    public Task<int> SetCardAsync(int? card, bool isUno = false) =>
        _client.PostAsJsonResponseAsJsonAsync<object, int>("card", new
        {
            Card = card,
            IsUno = isUno
        });
}