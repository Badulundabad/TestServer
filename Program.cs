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
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Any, 8005);
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
                    Console.WriteLine($"started to receive {size} bytes");

                    buffer = new byte[8];
                    StringBuilder builder = new StringBuilder();

                    do
                    {
                        if (builder.Length >= size) break;
                        stream.Read(buffer, 0, buffer.Length);
                        String s = Encoding.UTF8.GetString(buffer);
                        builder.Append(s);
                        if (!stream.DataAvailable) break;
                        Console.WriteLine($"received {builder.Length} bytes");
                        //GC.Collect();
                    }
                    while (true);
                    HandleStringAsync(builder.ToString());
                }
            }
        }
        private async static void HandleStringAsync(String response)
        {
            await Task.Run(() =>
            {
                File.WriteAllText(@"C:\Users\Бадулундабад\Desktop\rec.txt", response);
                response = response.Split('#')[1];
                Console.WriteLine($"finally received {response.Length + 2} bytes\n\n\n");
            });
        }
        private static Operation GetOperation(String json)
        {
            return JsonSerializer.Deserialize<Operation>(json);
        }
    }
}
