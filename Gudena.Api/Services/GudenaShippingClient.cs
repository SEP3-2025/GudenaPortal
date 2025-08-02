using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Gudena.Api.DTOs;

namespace Gudena.Services
{
    public static class GudenaShippingClient
    {
        private static string server = "127.0.0.1";
        private static int port = 5001;
        
        public static async Task<double> GetShippingCostAsync(
            string originCountry, string originPostalCode,
            string destCountry, string destPostalCode)
        {
            using var client = new TcpClient();
            await client.ConnectAsync(server, port);

            using var stream = client.GetStream();
            var dto = new LocationsShippingsDto
            {
                OriginCountry = originCountry,
                OriginPostalCode = originPostalCode,
                DestinationCountry = destCountry,
                DestinationPostalCode = destPostalCode
            };
            string json = JsonSerializer.Serialize(dto);
            byte[] data = Encoding.UTF8.GetBytes(json);
            await stream.WriteAsync(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            return double.TryParse(response, NumberStyles.Float, CultureInfo.InvariantCulture, out var cost) ? cost : -1;
        }
    }
}
