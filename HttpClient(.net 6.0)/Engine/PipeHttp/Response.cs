using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Response
    {
        public static async Task ResponseHttp(TcpClient tcpClient)
        {
            var stream = tcpClient.GetStream();
            var responseData = new byte[512];

            var response = new StringBuilder();
            int bytes;
            do
            {
                bytes = await stream.ReadAsync(responseData);
                response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
            }
            while (tcpClient.Available > 0);

            //Console.WriteLine(response);
        }
    }
}
