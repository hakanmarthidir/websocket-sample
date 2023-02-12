using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using websocket_server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(60)
};

app.UseWebSockets(webSocketOptions);

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
        {
            if (webSocket.State != WebSocketState.Open)
                return;

            while (true)
            {
                var engine = new EngineInfo() { Id = Guid.NewGuid(), MachineId = Guid.NewGuid(), Status = EngineStatus.Idle, TimeStamp = DateTime.Now };
                var serializedMessage = JsonSerializer.Serialize(engine);
                await webSocket.SendAsync(Encoding.ASCII.GetBytes(serializedMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay(1000);
            }
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});
await app.RunAsync();


