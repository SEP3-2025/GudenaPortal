using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Gudena.Api.Services
{
    public static class GudenaShippingClient
    {
        public static async Task<double> GetShippingCostAsync(
            string originCountry, string originPostalCode,
            string destCountry, string destPostalCode,
            string server = "127.0.0.1", int port = 5001)
        {
            using var client = new TcpClient();
            await client.ConnectAsync(server, port);

            using var stream = client.GetStream();
            string message = $"{originCountry}|{originPostalCode}|{destCountry}|{destPostalCode}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            return double.TryParse(response, out var cost) ? cost : -1;
        }
    }
}