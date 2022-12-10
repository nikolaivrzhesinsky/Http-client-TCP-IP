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

            await Connect.CreateConn(tcpClient);

            await Request.RequestHttp(tcpClient);
            
            await new Response().ResponseHttp(tcpClient); // тут было статик
            
           
            

        }
    }

}
