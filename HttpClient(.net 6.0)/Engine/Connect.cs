using System.Net.Sockets;

namespace Engine
{
    public class Connect
    {
        private static string _server;

        public static string GetServer()
        {
            return _server;
        }
        public static async Task CreateConn(TcpClient tcpClient, string server)
        {
            try
            {
                _server = server;
                await tcpClient.ConnectAsync(_server, 80);
                Console.WriteLine("Подключение установлено");
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}