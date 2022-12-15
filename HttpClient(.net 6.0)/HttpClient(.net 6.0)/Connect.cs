using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpClient_.net_6._0_.PipeHttp;

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
                _server = Request.requestUri.DnsSafeHost;
                int port = Request.requestUri.Scheme == "http" ?  80 : 443;
                
                await tcpClient.ConnectAsync(_server, port);
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

        public static void CloseConn(TcpClient tcpClient)
        {
            tcpClient.Close();      
            Console.WriteLine("Is server in work?: "+tcpClient.Connected);     
        }
    }
}
