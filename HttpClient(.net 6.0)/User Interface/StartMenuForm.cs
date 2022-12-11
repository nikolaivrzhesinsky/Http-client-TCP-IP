using Engine;
using System.Net.Sockets; 

namespace User_Interface
{
    public partial class StartMenu : Form
    {
        private Client client;
        public StartMenu()
        {
            InitializeComponent();
            client = Client.GetInstance();
        }
        public async void SearchButton_Click(object sender, EventArgs e)
        {
            if (URL_inputHolder.Text == null)
                return;
            Connect.CreateConn(client.GetTcpClient(), URL_inputHolder.Text);
        }

    }
}