using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text.Json;

namespace Uno.Core;

public static class Extensions
{
    public static Task ConnectAsync(this ClientWebSocket socket, string uri, CancellationToken token = default) => 
        socket.ConnectAsync(new(uri), token);

    // public static async Task SendJsonAsync<T>(this WebSocket socket,T value,CancellationToken  token =default)
    // {
    //     var buffer = JsonSerializer.SerializeToUtf8Bytes(value);
    //     await socket.SendAsync(buffer, WebSocketMessageType.Text, true,token);
    // }
    //
    public static async Task<T?> ReceiveJsonAsync<T>(this WebSocket socket,Memory<byte> buffer,CancellationToken  token =default) where T : class
    {
        var result = await socket.ReceiveAsync(buffer, token);
        return result.MessageType is WebSocketMessageType.Close ? null :
        JsonSerializer.Deserialize<T>(buffer.Span[..result.Count]);
    }

    public static async Task<T?> PostResponseAsJsonAsync<T>(this HttpClient client,string? requestUri, HttpContent? content = null,CancellationToken token = default)
    {
        var response = await client.PostAsync(requestUri, content, token);
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: token);
    }
    
    public static async Task<TOut?> PostAsJsonResponseAsJsonAsync<TIn,TOut>(this HttpClient client,string? requestUri,TIn value,CancellationToken token = default)
    {
        var response = await client.PostAsJsonAsync(requestUri,value, token);
        return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken: token);
    }
}