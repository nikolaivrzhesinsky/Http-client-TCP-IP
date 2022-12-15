using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient_.net_6._0_.PipeHttp
{
    public class Request
    {
        public static Uri requestUri;

        public static async Task RequestHttp(TcpClient tcpClient)
        {
            var stream = tcpClient.GetStream();
            //var requestMessage = $"GET /images/branding/googlelogo/tio1x/googlelogo_color_272x92dp.png HTTP/1.1\r\n" +
            //                   $"Host: {Connect.GetServer()}\r\nConnecn: keep alive\r\n\r\n";
            var requestMessage = $"GET / HTTP/1.1\r\n" +
                                 $"Host: {Connect.GetServer()}\r\nConnection: keep alive\r\n\r\n";
            Console.WriteLine(requestMessage);
            var requestData = Encoding.UTF8.GetBytes(requestMessage);
            await stream.WriteAsync(requestData);
        }

        public static void GetRequestFromUri(string uriFromWF)
        {
            requestUri = new Uri(uriFromWF);
        }

        public static void ParseUri()
        {
            
        }
    }
}
