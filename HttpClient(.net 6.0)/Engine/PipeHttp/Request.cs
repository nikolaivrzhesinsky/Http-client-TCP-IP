using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Request
    {
        public static async Task RequestHttp(TcpClient tcpClient)
        {
            var stream = tcpClient.GetStream();
            var requestMessage = $"GET / HTTP/1.1\r\nHost: {Connect.GetServer()}\r\nConnection: Close\r\n\r\n";
            var requestData = Encoding.UTF8.GetBytes(requestMessage);
            await stream.WriteAsync(requestData);
        }
    }
}
