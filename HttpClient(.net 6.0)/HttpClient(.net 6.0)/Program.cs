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

            Request.GetRequestFromUri("https://wikipedia.org//");
            
            await Connect.CreateConn(tcpClient);

            await Request.RequestHttp(tcpClient);

            if (Request.requestUri.Scheme == "http")
            {
                await new Response().ResponseHttp(tcpClient); // тут было статик
            }
            else
            {
                if (Connect.sslStream != null) 
                    new ResponseHttps().ResponseHttp(Connect.sslStream);
            }

            Connect.CloseConn(tcpClient);
            

        }
    }

}
