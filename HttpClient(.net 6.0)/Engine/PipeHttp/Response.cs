using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Engine.PipeHttp;

namespace Engine
{
    public class Response
    {
        private int statusCode;
        private string type; // можно сделать объект сообщения отдельным классом
        private string subtype;
        private string encoding;
        private string MessageBody;
        private int contentLength = 0;
        private string location;
        private static int _fileIndex = 0;
        private String pathFile;
        private CacheInfo cacheInfo;
        private bool isCached = false;
        private string date;
        public static bool authenticate = false;
        private static string? fileNameUri;
        
        public String GetSubType() => subtype;
        
        public async Task ResponseHttp(TcpClient tcpClient) // тут было статик
        {
            var responseData = new byte[512];
            var stream=tcpClient.GetStream();
            int bytes;
            var responseHeaders = new StringBuilder();
            List<byte> headersList = new List<byte>();
            string flagEndHeaders = null;

            _fileIndex++;

            do
            {
                bytes = await stream.ReadAsync(responseData,0,1);
                string temp = Encoding.UTF8.GetString(responseData, 0, bytes);
                //убрать
                if (temp == "\r" || temp == "\n")
                {
                    flagEndHeaders += temp;
                }
                responseHeaders.Append((Encoding.UTF8.GetString(responseData, 0, bytes)));
            }
            while (bytes > 0 && !responseHeaders.ToString().Contains("\r\n\r\n"));
            
            DecodeResponse(responseHeaders.ToString());
            //auntith
            if (authenticate)
            {
                await Chief.Ainigilyator(Request.requestUri.AbsoluteUri);
            }

            if (Chief.type != "" && Chief.pathFile != "")
            {
                return;
            }

            if (CacheInfo.CacheTable.ContainsKey(Request.requestUri.AbsoluteUri) && this.statusCode == 304)
            {
                var cacheResponse = CacheInfo.CacheTable[Request.requestUri.AbsoluteUri];
                this.subtype = cacheResponse.subtype;
                this.pathFile = cacheResponse.pathFile;
                FileManager.Log("\nCONDITIONAL IS WORKING\n");
                return;
            }
            
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

            authenticate = false;

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
            //pathFile = $"responseResult{_fileIndex}." + subtype;
            var responseData = new byte[1];
            int counter = 0;
            var responseBody = new StringBuilder();
            int bytes;

            using (BinaryWriter writer = new BinaryWriter(File.Open(pathFile, FileMode.Append)))
            {
                if (contentLength > 0)
                {
                    do
                    {
                        bytes = await stream.ReadAsync(responseData, 0, 1);
                        counter++;
                        responseBody.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                        byteBody[counter - 1] = responseData[0];
                        if (counter >= contentLength)
                        {
                            break;
                        }
                    } while (bytes > 0);
                }
                else
                {
                    String message = FileManager.GetErorMessage(statusCode.ToString());
                    
                    byteBody =  Encoding.ASCII.GetBytes(message);
                }
                writer.Write(byteBody);
                //Console.WriteLine("File has been written");
            }

            //Console.WriteLine(responseBody);
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
        
        private void SetCacheInfo(string fieldName, string fieldValue)
        {
            if (!isCached)
            {
                this.cacheInfo = new CacheInfo(fieldName, fieldValue);
                isCached = true;
            }
            else
            {
                this.cacheInfo.SetCache(fieldName, fieldValue);
            }
        }

        private void SetAuthenticate(string fieldValue)
        {
            if (fieldValue.Contains("Basic"))
            {
                authenticate = true;
            }
        }

        private void SetDate(string fieldValue)
        {
            date = fieldValue.Trim();
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
            else if (fieldName == "Date")
            {
                SetDate(fieldValue);
            }
            else if (fieldName == "Cache-Control" || fieldName == "Expires" || fieldName == "ETag" || fieldName == "Last-Modified")
            {
                SetCacheInfo(fieldName, fieldValue);
            }
            else if (fieldName == "WWW-Authenticate")
            {
                SetAuthenticate(fieldValue);
            }
        }

        
        
        public void DecodeResponse(String response) // тут было статик
        {
            var responseStrings = response.Split("\r\n");
            this.statusCode = Convert.ToInt32(responseStrings[0].Split(' ')[1]);
            FileManager.Log(responseStrings[0] + "\n");
            
            if (CacheInfo.CacheTable.ContainsKey(Request.requestUri.AbsoluteUri)) // контент устарел
            {
                //var cacheResponse = CacheInfo.CacheTable[Request.requestUri.AbsolutePath];
                if (this.statusCode == 304)
                {
                    return;
                }
                if (this.statusCode == 200)
                {
                    CacheInfo.CacheTable.Remove(Request.requestUri.AbsoluteUri);
                }
            }

            for (int i = 1; i < responseStrings.Length - 2; i++)
            {
                this.SetHeader(responseStrings[i]);
                FileManager.Log(responseStrings[i]+"\n\n");
            }
            
            
            
            pathFile = $"responseResult{_fileIndex}." + subtype;
            if (isCached)
            {
                cacheInfo.SetCacheType();
                cacheInfo.responseDT = DateTime.ParseExact(date,
                    "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                    CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat,
                    DateTimeStyles.AdjustToUniversal);
                cacheInfo.subtype = subtype;
                cacheInfo.pathFile = pathFile;
                CacheInfo.CacheTable.Add(Request.requestUri.AbsoluteUri, cacheInfo);
                //Console.WriteLine(cacheInfo.cacheType);
                FileManager.Log("Cache type"+cacheInfo.cacheType);
            }
            FileManager.Log("Authenticate:" + authenticate);
            //this.MessageBody = MessageBody.ToString();
            //Console.WriteLine(this.statusCode);
            //Console.WriteLine(this.type);
            //Console.WriteLine(this.subtype);
            //Console.WriteLine(this.encoding);
            //Console.WriteLine("Location is " + this.location);
            //Console.WriteLine("content-length: " + Convert.ToString(this.contentLength));
            //Console.WriteLine(this.MessageBody);
        }

        public String GetPath() => pathFile;

    }
}
