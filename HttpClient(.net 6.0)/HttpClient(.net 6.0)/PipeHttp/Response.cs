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
        private int contentLength;
        public async Task ResponseHttp(TcpClient tcpClient) // тут было статик
        {
            var stream = tcpClient.GetStream();
            var responseData = new byte[512];

            int bytes;
            var responseHeaders = new StringBuilder();
            List<byte> headersList = new List<byte>();
            string flagEndHeaders=null;
            //byte[] byteFlag = Encoding.UTF8.GetBytes(flagEndHeaders);
            
            do
            {
                bytes = await stream.ReadAsync(responseData,0,1);
                //response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                string temp = Encoding.UTF8.GetString(responseData, 0, bytes);
                
                if (temp == "\r" || temp == "\n")
                {
                    flagEndHeaders += temp;
                }
                responseHeaders.Append((Encoding.UTF8.GetString(responseData, 0, bytes)));
            }
            while (bytes > 0 && !responseHeaders.ToString().Contains("\r\n\r\n"));

            Console.WriteLine(responseHeaders);
            DecodeResponse(responseHeaders.ToString());

            await GetBodyCL(stream);

            //downloadFile(); 
        }

        private async Task GetBodyCL(NetworkStream stream)
        {
            byte[] byteBody = new byte[contentLength];
            MemoryStream streamFile = new MemoryStream(byteBody);
            var path = "test3." + subtype;
            
            var responseData = new byte[1];
            int counter = 0;
            var responseBody = new StringBuilder();
            int bytes;

            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                do
                {
                    bytes = await stream.ReadAsync(responseData, 0, 1);
                    counter++;
                    responseBody.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                    byteBody[counter-1] = responseData[0];
                    if (counter >= contentLength)
                    {
                        break;
                    }
                } while (bytes > 0);
                
                writer.Write(byteBody);
                Console.WriteLine("File has been written");
            }

            Console.WriteLine(responseBody);
        }
        
        public async Task downloadFile()
        {

            byte[] byteBody = Encoding.ASCII.GetBytes(MessageBody);
            MemoryStream stream = new MemoryStream(byteBody);

            var path = "test3." + subtype;
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                Console.WriteLine(Environment.CurrentDirectory);
                writer.Write(byteBody);
                
                Console.WriteLine("File has been written");
            }
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

        private void SetContentLength(string fieldValue)
        {
            this.contentLength = Convert.ToInt32((fieldValue.Trim()));
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
            else if (fieldName == "Content-Length")
            {
                SetContentLength(fieldValue);
            }
        }

        public void DecodeResponse(String response) // тут было статик
        {
            var responseStrings = response.Split("\r\n");
            this.statusCode = Convert.ToInt32(responseStrings[0].Split(' ')[1]);
            for (int i = 1; i < responseStrings.Length - 2; i++)
            {
                this.SetHeader(responseStrings[i]);
            }
            /*while (!String.IsNullOrEmpty(responseStrings[i]))
            {
                this.SetHeader(responseStrings[i]);
                i++;
            }*/
            /*StringBuilder MessageBody = new StringBuilder();
            i++;
            while (i + 1 < responseStrings.Length) // case if chunked 
            {
                MessageBody.Append(responseStrings[i + 1]);
                i += 2;
            }
            this.MessageBody = MessageBody.ToString();*/
            Console.WriteLine(this.statusCode);
            Console.WriteLine(this.type);
            Console.WriteLine(this.subtype);
            Console.WriteLine(this.encoding);
            Console.WriteLine("content-length: " + Convert.ToString(this.contentLength));
            //Console.WriteLine(this.MessageBody);
        }
    }
}
