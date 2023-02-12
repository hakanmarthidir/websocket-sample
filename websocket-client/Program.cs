
using System.Net.WebSockets;
using System.Text;

public static class Program
{
    public static async Task Main(string[] args)
    {        
        using (var ws = new ClientWebSocket())
        {
            await ws.ConnectAsync(new Uri("ws://localhost:5012/ws"), CancellationToken.None);
            var buffer = new byte[256];
            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                }
                else
                {
                    Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
                }
            }
        }
    }
}