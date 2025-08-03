using Gudena.Api.DTOs;

namespace Gudena.Api.Services;

using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

    public static class GudenaPaymentClient
    {
        private static string server = "127.0.0.1";
        private static int port = 5002;
        
        public static async Task<string> ProcessPaymentAsync(CreditCardDto creditCardDto)
        {
            using var client = new TcpClient();
            await client.ConnectAsync(server, port);

            using var stream = client.GetStream();

            var messageJson = JsonSerializer.Serialize(creditCardDto);
            var data = Encoding.UTF8.GetBytes(messageJson);
            
            await stream.WriteAsync(data, 0, data.Length);

            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            
            return response;
        }
    }

