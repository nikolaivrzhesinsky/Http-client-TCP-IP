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

        public static async Task RequestPostHttp()
        {
            var stream = Connect.tcpClient.GetStream();
            HtmlForm htmlForm = new HtmlForm("/auth/register", "POST", "application/json");
            htmlForm.AddTag("email", "fallerartem@yandex.ru");
            htmlForm.AddTag("password", "password");
            string body = htmlForm.ToEncType();
            var requestMessage = $"POST {htmlForm.action} HTTP/1.1\r\n" +
            
                                 $"Host: {requestUri.Host}:{requestUri.Port}" +

                                 $"\r\nConnection: keep alive" +
                                 
                                 $"\r\nContent-Type: {htmlForm.enctype} " +

                                 $"\r\nContent-Length: {body.Length} " +
                                 $"\r\n\r\n{body}";
            
            var requestData = Encoding.UTF8.GetBytes(requestMessage);
            await stream.WriteAsync(requestData);
            FileManager.Log(requestMessage);
        }
        public static async Task RequestHttp(bool conditional = false, bool auntificate = false)
        {
            var stream = Connect.tcpClient.GetStream();
            
            //var requestMessage = $"GET /images/branding/googlelogo/tio1x/googlelogo_color_272x92dp.png HTTP/1.1\r\n" +
            //                   $"Host: {Connect.GetServer()}\r\nConnecn: keep alive\r\n\r\n";
            var requestMessage = $"GET {requestUri.AbsolutePath} HTTP/1.1\r\n" +
            
                                 $"Host: {requestUri.Host}:{requestUri.Port}" +

                                 $"\r\nConnection: keep alive";
            if (conditional)
            {
                var cacheResponse = CacheInfo.CacheTable[requestUri.AbsoluteUri];
                if (cacheResponse.isLastModified)
                {
                    requestMessage += $"\r\nIf-Modified-Since: {cacheResponse.lastModified}";
                }
                if (cacheResponse.isETag)
                {
                    requestMessage += $"\r\nIf-Match: \"{cacheResponse.ETag}\"";
                }
            }

            if (auntificate)
            {
                requestMessage += $"\r\nAuthorization: Basic YWRtaW46YWRtaW4=";
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
