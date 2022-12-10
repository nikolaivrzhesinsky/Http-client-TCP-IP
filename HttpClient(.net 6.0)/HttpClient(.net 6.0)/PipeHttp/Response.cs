using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient_.net_6._0_.PipeHttp
{
    public class Response
    {
        private int statusCode;
        private string type; // можно сделать объект сообщения отдельным классом
        private string subtype;
        private string encoding;
        private string MessageBody;
        public async Task ResponseHttp(TcpClient tcpClient) // тут было статик
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
            while (bytes > 0);

            //Console.WriteLine(response);

            String responseToString = response.ToString();
            
            DecodeResponse(responseToString);
        }

        private void SetContentType(string fieldValue)
        {
            string mediaType;
            int indexSep = fieldValue.IndexOf(';');
            if (indexSep >= 0)
            {
                mediaType = fieldValue.Substring(0, fieldValue.IndexOf(';')).Trim();
            }
            else
            {
                mediaType = fieldValue.Trim();
            }
            this.type = mediaType.Substring(0, mediaType.IndexOf('/'));
            this.subtype = mediaType.Substring(mediaType.IndexOf('/') + 1);
        }

        private void SetTransferEncoding(string fieldValue)
        {
            this.encoding = fieldValue.Trim();
        }
        
        private void SetHeader(string header)
        {
            string fieldName = header.Substring(0, header.IndexOf(':'));
            string fieldValue = header.Substring(header.IndexOf(':') + 1);
            if (fieldName == "Content-Type")
            {
                SetContentType(fieldValue);
            }
            else if (fieldName == "Transfer-Encoding")
            {
                SetTransferEncoding(fieldValue);
            }
            // Console.WriteLine(fieldName + "\n" + fieldValue);
        }

        public void DecodeResponse(String response) // тут было статик
        {
            Console.WriteLine(response);
            var responseStrings = response.Split("\r\n");
            this.statusCode = Convert.ToInt32(responseStrings[0].Split(' ')[1]);
            int i = 1;
            while (!String.IsNullOrEmpty(responseStrings[i]))
            {
                this.SetHeader(responseStrings[i]);
                i++;
            }
            StringBuilder MessageBody = new StringBuilder();
            i++;
            while (i + 1 < responseStrings.Length) // case if chunked 
            {
                MessageBody.Append(responseStrings[i + 1]);
                i += 2;
            }
            this.MessageBody = MessageBody.ToString();
            Console.WriteLine(this.statusCode);
            Console.WriteLine(this.type);
            Console.WriteLine(this.subtype);
            Console.WriteLine(this.encoding);

            Console.WriteLine(this.MessageBody);
        }
    }
}
