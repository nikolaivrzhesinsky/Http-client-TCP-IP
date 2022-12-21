using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Engine.PipeHttp;

namespace Engine
{
    public class Request
    {
        public static Uri requestUri;

        public static async Task RequestHttp(bool conditional = false)
        {
            var stream = Connect.tcpClient.GetStream();
            
            //var requestMessage = $"GET /images/branding/googlelogo/tio1x/googlelogo_color_272x92dp.png HTTP/1.1\r\n" +
            //                   $"Host: {Connect.GetServer()}\r\nConnecn: keep alive\r\n\r\n";
            var requestMessage = $"GET {requestUri.AbsolutePath} HTTP/1.1\r\n" +
                                 $"Host: {requestUri.Host}:{requestUri.Port}\r\nAuthorization: Basic YWRtaW46YWRtaW4=" +
                                 $"\r\nConnection: keep alive";
            if (conditional)
            {
                var cacheResponse = CacheInfo.CacheTable[requestUri.AbsolutePath];
                if (cacheResponse.isLastModified)
                {
                    requestMessage += $"\r\nIf-Modified-Since: {cacheResponse.lastModified}";
                }
                if (cacheResponse.isETag)
                {
                    requestMessage += $"\r\nIf-Match: \"{cacheResponse.ETag}\"";
                }
            }
            requestMessage += "\r\n\r\n";
            FileManager.Log(requestMessage);
            var requestData = Encoding.UTF8.GetBytes(requestMessage);
            await stream.WriteAsync(requestData);
        }

        public static void GetRequestFromUri(string uriFromWF)
        {
            requestUri = new Uri(uriFromWF);
        }
        
    }
}
