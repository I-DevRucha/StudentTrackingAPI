using System.Net.Sockets;
using System.Text;
using System.Net;

namespace StudentTrackingAPI.Controllers
{
    public class GpsTcpListener
    {
        private readonly int _port;
        private readonly TcpListener _tcpListener;

        public GpsTcpListener(int port = 9000)
        {
            _port = port;
            _tcpListener = new TcpListener(IPAddress.Any, _port);
        }

        public async Task StartAsync()
        {
            _tcpListener.Start();
            Console.WriteLine($"✅ GPS TCP Listener started on port {_port}");

            while (true)
            {
                var client = await _tcpListener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        //private async Task HandleClientAsync(TcpClient client)
        //{
        //    var buffer = new byte[1024];
        //    using var stream = client.GetStream();

        //    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        //    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        //    Console.WriteLine($"📩 Received: {message}");

        //    // Example message: imei:868120103643505,tracker,1609300401,+22.5726,+88.3639,50,0,100%
        //    var data = message.Split(',');
        //    if (data.Length > 5)
        //    {
        //        string imei = data[0].Replace("imei:", "");
        //        string latitude = data[3];
        //        string longitude = data[4];

        //        Console.WriteLine($"📍 IMEI: {imei}, Lat: {latitude}, Lng: {longitude}");

        //        // TODO: Save to database (inject DbContext or service)
        //    }
        //}
        private async Task HandleClientAsync(TcpClient client)
        {
            var buffer = new byte[1024];
            using var stream = client.GetStream();

            while (true)
            {
                int bytesRead;
                try
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // client disconnected
                }
                catch
                {
                    break; // connection error
                }

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine($"📩 Received: {message}");

                var data = message.Split(',');
                if (data.Length > 5)
                {
                    string imei = data[0].Replace("imei:", "");
                    string latitude = data[3];
                    string longitude = data[4];

                    Console.WriteLine($"📍 IMEI: {imei}, Lat: {latitude}, Lng: {longitude}");

                    // TODO: Save to database
                }
            }
        }
    }
}
