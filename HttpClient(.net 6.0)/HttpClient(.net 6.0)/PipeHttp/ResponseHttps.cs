using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace HttpClient_.net_6._0_.PipeHttp
{
    public class ResponseHttps
    {
        private int statusCode;
        private string type; // можно сделать объект сообщения отдельным классом
        private string subtype;
        private string encoding;
        private string MessageBody;
        private int contentLength;

        private static string? fileNameUri;
        
        public void ResponseHttp(TcpClient tcpClient) // тут было статик
        {
            var responseData = new byte[512];
            int bytes;
            var responseHeaders = new StringBuilder();
            List<byte> headersList = new List<byte>();
            string flagEndHeaders = null;

            
            SslStream sslStream = new SslStream(
                tcpClient.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
            );
            
            string serverName = Request.requestUri.Host;
            Console.WriteLine(serverName);
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            
            try
            {
                sslStream.ReadTimeout = 5000; // wait 10 seconds for a response ...
                Console.WriteLine("ConnectAndAuthenicate: AuthenticateAsClient CALLED ({0})", serverName);
                sslStream.AuthenticateAsClient(serverName,null,SslProtocols.None,false);
                Console.WriteLine("ConnectAndAuthenicate: AuthenticateAsClient COMPLETED SUCCESSFULLY");
            }
            catch (Exception x)
            {
                Console.WriteLine("ConnectAndAuthenicate: EXCEPTION >> AuthenticateAsClient: {0}", x.ToString());
                tcpClient.Close(); tcpClient = null;
                sslStream.Close(); sslStream = null;
            }
            
            do
            {
                bytes = sslStream.Read(responseData,0,1);
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
                        bytes = sslStream.Read(responseData, 0, 1);
                        chunkStr.Append((Encoding.UTF8.GetString(responseData, 0, bytes)));
                    } while (bytes > 0 && !chunkStr.ToString().Contains("\r\n"));

                    Console.WriteLine("chs: ");
                    Console.Write(chunkStr);
                    chunkLength = Convert.ToInt32(chunkStr.ToString().Remove(chunkStr.Length - 2), 16);
                    Console.WriteLine(chunkLength);
                    if (chunkLength > 0)
                    { 
                        GetBodyCL(sslStream, chunkLength);
                    }
                    bytes = sslStream.Read(responseData, 0, 2);
                } while (chunkLength != 0);

                Console.WriteLine("i'm out");
            }
            else GetBodyCL(sslStream);
            
            //downloadFile(); 
        }

        private void GetBodyCL(SslStream stream, int chunkLength = -1)
        {
            if (chunkLength >= 0)
            {
                contentLength = chunkLength;
            }
            byte[] byteBody = new byte[contentLength];
            MemoryStream streamFile = new MemoryStream(byteBody);
            var path = "test1." + subtype;
            Console.WriteLine(path);
            var responseData = new byte[1];
            int counter = 0;
            var responseBody = new StringBuilder();
            int bytes;

            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append)))
            {
                do
                {
                    bytes = stream.Read(responseData, 0, 1);
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
        
        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) {
                return true;
            }

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // refuse connection
            return false;
        }
        
       
        
    }
}
        