using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HttpClient_.net_6._0_.PipeHttp;

namespace HttpClient_.net_6._0_
{
    public class Connect
    {
        private static string _server;

        public static SslStream? sslStream;

        public static string GetServer()
        {
            return _server;
        }
        public static async Task CreateConn(TcpClient tcpClient)
        {
            try
            {
                _server = Request.requestUri.DnsSafeHost;
                int port = Request.requestUri.Scheme == "http" ?  80 : 443;
                
                await tcpClient.ConnectAsync(_server, port);
                Console.WriteLine("Подключение установлено");
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.ErrorCode);
            }
            sslStream = new SslStream(
                tcpClient.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
            );
            //sslStream = new SslStream(tcpClient.GetStream(), true, null, null, EncryptionPolicy.AllowNoEncryption);
            
            string serverName = Request.requestUri.Host;
            Console.WriteLine(serverName);
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            
            try
            {
                sslStream.ReadTimeout = 10000; // wait 10 seconds for a response ...
                Console.WriteLine("ConnectAndAuthenicate: AuthenticateAsClient CALLED ({0})", serverName);
                //await sslStream.AuthenticateAsClientAsync(serverName,null,SslProtocols.Tls,false);
                sslStream.AuthenticateAsClient(serverName,null,SslProtocols.Tls12,false);
                Console.WriteLine("ConnectAndAuthenicate: AuthenticateAsClient COMPLETED SUCCESSFULLY");
            }
            catch (Exception x)
            {
                Console.WriteLine("ConnectAndAuthenicate: EXCEPTION >> AuthenticateAsClient: {0}", x.ToString());
                tcpClient.Close(); tcpClient = null;
                sslStream.Close(); sslStream = null;
            }
        }
        private static string InitServer()
        {
            Console.WriteLine("Введите url");
            return Console.ReadLine();
        }

        public static void CloseConn(TcpClient tcpClient)
        {
            tcpClient.Close();      
            Console.WriteLine("Is server in work?: "+tcpClient.Connected);     
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
