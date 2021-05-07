using RealtyModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TestSERVER
{
    class Program
    {
        static void Main(string[] args)
        {
            Receive(Listen());
            Console.ReadLine();
        }

        private static NetworkStream Listen()
        {
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Any, 8321);
            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Bind(serverAddress);
            listeningSocket.Listen(10);
            Socket socket = listeningSocket.Accept();
            return new NetworkStream(socket);
        }

        private static void Receive(NetworkStream stream)
        {
            Console.WriteLine("Connected");
            while (true)
            {
                if (stream.DataAvailable)
                {
                    Byte[] buffer = new Byte[4];
                    stream.Read(buffer, 0, 4);
                    Int32 size = BitConverter.ToInt32(buffer, 0);
                    StringBuilder builder = new StringBuilder(size);
                    buffer = new byte[32];

                    Console.WriteLine($"{DateTime.Now} started to receive {size} bytes");
                    do
                    {
                        size -= stream.Read(buffer, 0, buffer.Length);
                        builder.Append(Encoding.UTF8.GetString(buffer));
                    }
                    while (size > 0);
                    builder.Length = 0;
                    builder.Capacity = 0;
                    Console.WriteLine($"{DateTime.Now} received bytes");
                    GC.Collect();
                }
            }
        }
        private async static void HandleStringAsync(String response)
        {
            await Task.Run(() =>
            {
            });
        }
        private static Operation GetOperation(String json)
        {
            return JsonSerializer.Deserialize<Operation>(json);
        }
    }
}
