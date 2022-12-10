using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient_.net_6._0_
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
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.ErrorCode);
            }
        }
        private static string InitServer()
        {
            Console.WriteLine("Введите url");
            return Console.ReadLine();
        }
    }
}
