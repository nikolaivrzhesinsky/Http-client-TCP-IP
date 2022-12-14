using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpClient2.PipeHttp;

namespace HttpClient2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using TcpClient tcpClient = new TcpClient();

            await Connect.CreateConn(tcpClient);

            await Request.RequestHttp(tcpClient);

            await Response.ResponseHttp(tcpClient);

        }
    }
    
}