using System.Net.Http.Json;
using Uno.Core.Exceptions;
using Uno.Core.Models;

namespace Uno.Core;

public class Game(HttpClient client)
{
    public Events Events { get; } = new(); //TODO: Dependency Inject
    private int? _id;
    private int _gameId;

    public async Task JoinAsync(string name,int skin,int? gameId)
    {
        _gameId = gameId ?? await client.PostResponseAsJsonAsync<int>("create");
        await Events.ConnectAsync(client.BaseAddress!.ToString(),_gameId);
        _id = await client.PostAsJsonResponseAsJsonAsync<object,int>($"join/{_gameId}",
            new Info
            {
                Name = name,
                Skin = skin
            });
    }

    public Task<int> PickCardAsync() =>
        client.GetFromJsonAsync<int>($"pick/{_gameId}/{_id}");
}