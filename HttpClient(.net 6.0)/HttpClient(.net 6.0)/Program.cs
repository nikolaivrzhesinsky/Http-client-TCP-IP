using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpClient_.net_6._0_.PipeHttp;

namespace HttpClient_.net_6._0_
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using TcpClient tcpClient = new TcpClient();

            Request.GetRequestFromUri("https://www.google.com//");
            
            await Connect.CreateConn(tcpClient);

            await Request.RequestHttp(tcpClient);

            if (Request.requestUri.Scheme == "http")
            {
                await new Response().ResponseHttp(tcpClient); // тут было статик
            }
            else
            {
                new ResponseHttps().ResponseHttp(tcpClient);
            }

            Connect.CloseConn(tcpClient);
            

        }
    }

}
