using Engine;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace User_Interface
{
    public partial class StartMenu : Form
    {
        private Client client;
        private Int32 _tabIndex;
        
        public StartMenu()
        {
            InitializeComponent();
            client = Client.GetInstance();
            _tabIndex = 0;
        }
        
        private async void NavigateBtn_Click(object sender, EventArgs e)
        {
            if (URL_textbox.Text == null)
                return;
            //await Connect.CreateConn(client.GetTcpClient(), URL_textbox.Text);
            //StreamReader sr = new StreamReader("code.html");
            //((WebBrowser)TabController.SelectedTab.Controls[0]).DocumentText = sr.ReadToEnd();
        }

        private void AddTab_Click(object sender, EventArgs e)
        {
            WebBrowser webBrowser = new WebBrowser();
            SetWebBrowserInfo(webBrowser);
            TabController.TabPages.Add("New Page");
            TabController.SelectTab(_tabIndex);
            TabController.SelectedTab.Controls.Add(webBrowser);
            _tabIndex += 1;
        }

        private void WebBrowser_DocumentCompleted(object? sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        private void SetWebBrowserInfo(WebBrowser webBrowser)
        {
            webBrowser.Visible = true;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        private void StartMenu_Load(object sender, EventArgs e)
        {
            AddTab_Click(sender, e);
        }
    }
}