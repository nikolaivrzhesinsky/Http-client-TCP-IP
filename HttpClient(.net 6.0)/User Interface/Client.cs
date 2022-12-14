using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace User_Interface
{
    public class Client
    {
        private TcpClient client;
        private Client()
        {
            client = new TcpClient();
        } 
        
        private static Client? _instance;

        public static Client GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Client();
            }
            return _instance;
        }

        public TcpClient GetTcpClient() => client;
        
    }
}
