using System.Net.WebSockets;
using System.Text.Json;
using Uno.Core.Enums;
using Uno.Core.Exceptions;
using Uno.Core.Models;

namespace Uno.Core;

public class Events
{ 
    public event Action<Info[]>? PlayerUpdated;
    public event Action<CardPick>? CardPicked;
    public event Action<CardPenalty>? CardPenalty;
    public event Action<CardSet>? CardSet; 
        
    
    private readonly Memory<byte> _buffer = new(new byte[1024]); //TODO: Dependency Inject
    private readonly ClientWebSocket _socket = new();

    public async Task ConnectAsync(string uri,int id)
    {
        var wsUri = uri.Replace("http", "ws");
        await _socket.ConnectAsync($"{wsUri}ws/{id}");
        ListenForEvents();
    }

    private async void ListenForEvents()
    {
        while (true)
        {
            var data = await _socket.ReceiveJsonAsync<Data>(_buffer) ??
                throw new GameException("Disconnected");
            GetAction(data.Action)(data.Payload);
        }
    }

    private Action<JsonElement> GetAction(Actions action) =>
        action switch
        {
            Actions.PlayerUpdated => GetAction(PlayerUpdated),
            Actions.CardPicked => GetAction(CardPicked),
            Actions.CardPenalty => GetAction(CardPenalty),
            Actions.CardSet => GetAction(CardSet),
            _ => throw new NotImplementedException()
        };

    private static Action<JsonElement> GetAction<T>(Action<T>? action) =>
        json => action?.Invoke(json.Deserialize<T>()!);
}