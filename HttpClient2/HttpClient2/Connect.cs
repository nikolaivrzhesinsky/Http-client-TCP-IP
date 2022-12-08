using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HttpClient2
{
    public class Connect
    {
        private static string _server;

        public static string GetServer()
        {
            return _server;
        }
        public static async Task CreateConn(TcpClient tcpClient)
        {
            try
            {
                _server = InitServer();
                await tcpClient.ConnectAsync(_server, 80);
                Console.WriteLine("Подключение установлено");
            }
            catch(SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static string InitServer()
        {
            Console.WriteLine("Введите url");
            return Console.ReadLine();
        }
    }
    
  
}