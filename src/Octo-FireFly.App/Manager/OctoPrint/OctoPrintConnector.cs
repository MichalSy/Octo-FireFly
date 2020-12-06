using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Octo_FireFly.App.Manager.OctoPrint.Models;
using Octo_FireFly.Server.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Manager.OctoPrint
{
    public class OctoPrintConnector : IOctoPrintConnector
    {
        private readonly ILogger<OctoPrintConnector> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private ClientWebSocket _websocketConnection;

        public EventHandler<TemperatureStatusDTO> OnTemperatureChanged { get; set; }

        public OctoPrintConnector(ILogger<OctoPrintConnector> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            
        }

        public async Task ConnectAsync()
        {
            var host = _configuration.GetValue<string>("OctoPrintConnector:Host");
            var apiKey = _configuration.GetValue<string>("OctoPrintConnector:ApiKey");

            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", apiKey);

            var data = await _httpClient.SendPostAsync<LoginInfoDTO>($"http://{host}/api/login", new { passive = true });
            _logger.LogInformation($"Data: {data.SessionToken}");



            _websocketConnection = new ClientWebSocket();
            var uri = new Uri($"ws://{host}/sockjs/websocket");
            var cts = new CancellationTokenSource();
            await _websocketConnection.ConnectAsync(uri, cts.Token);

            var authObject = new
            {
                auth = $"michal:{data.SessionToken}",
            };
            var message = JsonSerializer.Serialize(authObject);

            await _websocketConnection.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, cts.Token);


            var throttleObject = new
            {
                throttle = 10
            };
            message = JsonSerializer.Serialize(throttleObject);
            await _websocketConnection.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, cts.Token);

            ReaderThreadAsync();
        }


        private async void ReaderThreadAsync()
        {
            var cts = new CancellationTokenSource();
            IEnumerable<byte> messageArray = Array.Empty<byte>();
            while (true)
            {
                var rcvBuffer = new ArraySegment<byte>(new byte[128]);

                WebSocketReceiveResult rcvResult = await _websocketConnection.ReceiveAsync(rcvBuffer, cts.Token);
                byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();

                messageArray = messageArray.Concat(msgBytes);

                if (rcvResult.EndOfMessage)
                {
                    string rcvMsg = Encoding.UTF8.GetString(messageArray.ToArray());

                    if (rcvMsg.StartsWith("{\"current\""))
                    {
                        var printerStatus = JsonSerializer.Deserialize<CurrentPrinterStatusDTO>(rcvMsg);
                        var lastState = printerStatus.Current?.TemperatureStatus?.OrderByDescending(t => t.UnixTime).FirstOrDefault();
                        if (lastState != null)
                        {
                            _logger.LogInformation($"Data: {lastState.Tool0.Actual} / {lastState.Tool0.Target} || {lastState.Bed.Actual} / {lastState.Bed.Target} ");
                            OnTemperatureChanged?.Invoke(this, lastState);
                        }
                    }

                    messageArray = Array.Empty<byte>();
                }
            }

        }

    }
}
