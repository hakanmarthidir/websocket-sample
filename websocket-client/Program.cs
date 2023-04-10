using System.Net.WebSockets;
using System.Text;

public static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            using (var ws = new ClientWebSocket())
            {
                //cancellation with timespan
                //var cancelSource = new CancellationTokenSource();
                //cancelSource.CancelAfter(TimeSpan.FromSeconds(60));

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
                        //if you need any deserialize object
                        //var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        //var deserializedData = JsonConvert.DeserializeObject<TObject>(message);
                        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
                    }
                }
            }
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }
}