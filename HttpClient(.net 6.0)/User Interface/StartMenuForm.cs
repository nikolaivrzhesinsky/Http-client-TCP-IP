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
        private void WebBrowser_DocumentCompleted(object? sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }


        private void StartMenu_Load(object sender, EventArgs e)
        {
            AddTab_Click(sender, e);
        }
        #region Buttons
        private async void NavigateBtn_Click(object sender, EventArgs e)
        {
            
            if (URL_textbox.Text == null)
                return;
            string type = URL_textbox.Text;//Here should be a method from Engine namespace
            TabController.SelectedTab.Text = type;
            TypeSwitchingFunction(type);
            //await Connect.CreateConn(client.GetTcpClient(), URL_textbox.Text);
        }

        private void AddTab_Click(object sender, EventArgs e)
        {            
            TabController.TabPages.Add("New Page");
            TabController.SelectTab(_tabIndex);
            _tabIndex += 1;
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (TabController.SelectedTab.Text == "New Page")
                return;
            TypeSwitchingFunction(TabController.SelectedTab.Text);
        }
        #endregion
        
        #region Support Methods
        private void TypeSwitchingFunction(string _type)
        {
            switch (_type)
            {
                case "html":
                    SetHtml();
                    break;
                case "png":
                    SetImage();
                    break;
            }
        }
        private void SetHtml()
        {
            TabController.SelectedTab.Controls.Clear();
            WebBrowser webBrowser = new WebBrowser();
            SetWebBrowserInfo(webBrowser);
            TabController.SelectedTab.Controls.Add(webBrowser);
            StreamReader sr = new StreamReader("code.html");
            ((WebBrowser)TabController.SelectedTab.Controls[0]).DocumentText = sr.ReadToEnd();
        }
        private void SetImage()
        {
            TabController.SelectedTab.Controls.Clear();
            TabController.SelectedTab.BackgroundImageLayout = ImageLayout.Stretch;
            TabController.SelectedTab.BackgroundImage = Image.FromFile("AddTab.png");
        }
        private void SetVideo()
        {
            TabController.SelectedTab.Controls.Clear();


        }
        private void SetWebBrowserInfo(WebBrowser webBrowser)
        {
            webBrowser.Visible = true;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }
        #endregion

    }
}