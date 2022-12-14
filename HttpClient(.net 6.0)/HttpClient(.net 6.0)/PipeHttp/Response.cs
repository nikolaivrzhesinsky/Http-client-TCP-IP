using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
        private string location;

        private static string? fileNameUri;
        
        public async Task ResponseHttp(TcpClient tcpClient) // тут было статик
        {
            var responseData = new byte[512];
            var stream=tcpClient.GetStream();
            int bytes;
            var responseHeaders = new StringBuilder();
            List<byte> headersList = new List<byte>();
            string flagEndHeaders = null;

            do
            {
                bytes = await stream.ReadAsync(responseData,0,1);
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
            if (this.encoding == "chunked")
            {
                int chunkLength = 0;
                do
                {
                    var chunkStr = new StringBuilder();
                    do
                    {
                        bytes = await stream.ReadAsync(responseData, 0, 1);
                        chunkStr.Append((Encoding.UTF8.GetString(responseData, 0, bytes)));
                    } while (bytes > 0 && !chunkStr.ToString().Contains("\r\n"));

                    Console.WriteLine("chs: ");
                    Console.Write(chunkStr);
                    chunkLength = Convert.ToInt32(chunkStr.ToString().Remove(chunkStr.Length - 2), 16);
                    Console.WriteLine(chunkLength);
                    if (chunkLength > 0)
                    {
                        await GetBodyCL(stream, chunkLength);
                    }
                    bytes = await stream.ReadAsync(responseData, 0, 2);
                } while (chunkLength != 0);

                Console.WriteLine("i'm out");
            }
            else await GetBodyCL(stream);
            
            //downloadFile(); 
        }

        private async Task GetBodyCL(NetworkStream stream, int chunkLength = -1)
        {
            if (chunkLength >= 0)
            {
                contentLength = chunkLength;
            }
            byte[] byteBody = new byte[contentLength];
            MemoryStream streamFile = new MemoryStream(byteBody);
            var path = "test2." + subtype;
            Console.WriteLine(path);
            var responseData = new byte[1];
            int counter = 0;
            var responseBody = new StringBuilder();
            int bytes;

            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append)))
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

        private void SetLocation(string fieldValue)
        {
            this.location = fieldValue.Trim();
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
            else if (fieldName == "Location")
            {
                SetLocation(fieldValue);
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
            
            //this.MessageBody = MessageBody.ToString();
            Console.WriteLine(this.statusCode);
            Console.WriteLine(this.type);
            Console.WriteLine(this.subtype);
            Console.WriteLine(this.encoding);
            Console.WriteLine("Location is " + this.location);
            Console.WriteLine("content-length: " + Convert.ToString(this.contentLength));
            //Console.WriteLine(this.MessageBody);
        }
        
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        
    }
}
