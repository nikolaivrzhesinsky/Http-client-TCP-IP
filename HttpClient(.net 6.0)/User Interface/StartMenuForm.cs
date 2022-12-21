using Engine;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Engine.PipeHttp;
using HttpClient_.net_6._0_.PipeHttp;

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
            await Chief.Ainigilyator(URL_textbox.Text);
            TypeSwitchingFunction(Chief.type, Chief.pathFile);
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
            //TypeSwitchingFunction(TabController.SelectedTab.Text);
        }
        #endregion
        
        #region Support Methods
        private void TypeSwitchingFunction(string _type, String path)
        {
            switch (_type)
            {
                case "html":
                    SetHtml(path);
                    break;
                case "png":
                    SetImage(path);
                    break;
            }
        }
        private void SetHtml(String path)
        {
            TabController.SelectedTab.Controls.Clear();
            WebBrowser webBrowser = new WebBrowser();
            SetWebBrowserInfo(webBrowser);
            TabController.SelectedTab.Controls.Add(webBrowser);
            StreamReader sr = new StreamReader(path);
            ((WebBrowser)TabController.SelectedTab.Controls[0]).DocumentText = sr.ReadToEnd();
        }
        private void SetImage(String path)
        {
            TabController.SelectedTab.Controls.Clear();
            TabController.SelectedTab.BackgroundImageLayout = ImageLayout.Stretch;
            TabController.SelectedTab.BackgroundImage = Image.FromFile(path);
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