using Engine;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Engine.PipeHttp;
using HttpClient_.net_6._0_.PipeHttp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace User_Interface
{
    public partial class StartMenu : Form
    {
        private Client client;
        private Int32 _tabIndex;
        private List<File> _files = new List<File>();

        public StartMenu()
        {
            InitializeComponent();
            client = Client.GetInstance();
            _tabIndex = 0;
        }
        private struct File
        {
            public File(int _index, string _path, string _url)
            {
                index = _index;
                filePath = _path;
                url = _url;
            }
            public int index { get; private set; }
            public string filePath { get; private set; }
            public string url { get; private set; }
        }
            

        private void WebBrowser_DocumentCompleted(object? sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        #region Form Events
        private void StartMenu_Load(object sender, EventArgs e)
        {
            AddTab_Click(sender, e);
        }
        private void StartMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (TabController.TabPages.Count != 0)
            {
                if (TabController.TabPages.Count != 0)
                    TabController.SelectTab(TabController.TabPages.Count - 1);
                CloseTab_Click(sender, e);
            }
            //if (TabController.TabPages.Count != 0)
            FileManager.DeleteCaches();
        }
        #endregion

        #region Buttons
        private async void NavigateBtn_Click(object sender, EventArgs e)
        {
            if (URL_textbox.Text == "")
                return;
            await Chief.Ainigilyator(URL_textbox.Text);
            TypeSwitchingFunction(Chief.type, Chief.pathFile, new Uri(URL_textbox.Text).AbsoluteUri, sender, e);
            URL_textbox.Text = "";
        }

        private void AddTab_Click(object sender, EventArgs e)
        {            
            TabController.TabPages.Add("New Page");
            TabController.SelectTab(_tabIndex);
            _tabIndex += 1;
        }
        private void CloseTab_Click(object sender, EventArgs e)
        {
            var file = _files.Find(file => file.index == TabController.SelectedIndex);
            if(TabController.TabPages.Count != 0)
                TabController.SelectTab(TabController.TabPages.Count - 1);
            _tabIndex -= 1;
            TabController.SelectedTab.Controls.Clear();
            TabController.TabPages.RemoveAt(TabController.SelectedIndex);
            //tabPage.Controls[0].Dispose();
            //MessageBox.Show(file.filePath);
            //Send file path using file.path
            FileManager.DeleteResponseFile(file.filePath, file.url, e.ToString() == "System.Windows.Forms.FormClosingEventArgs");
            _files.Remove(file);

        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (TabController.SelectedTab.Text == "New Page")
                return;
            //TypeSwitchingFunction(TabController.SelectedTab.Text);
        }
        #endregion
        
        #region Support Methods
        private void TypeSwitchingFunction(string _type, String path, string url, object sender, EventArgs e)
        {
            switch (_type)
            {
                case "html":
                    SetHtml(path, url, sender, e);
                    break;
                case "png":
                    SetImage(path, url, sender, e);
                    break;
            }
        }
        private void SetHtml(String path, String url, object sender, EventArgs e)
        {
            if (TabController.TabPages.Count == 0)
                AddTab_Click(sender, e);
            TabController.SelectedTab.Controls.Clear();
            WebBrowser webBrowser = new WebBrowser();
            SetWebBrowserInfo(webBrowser);
            TabController.SelectedTab.Controls.Add(webBrowser);
            StreamReader sr = new StreamReader(path);
            ((WebBrowser)TabController.SelectedTab.Controls[0]).DocumentText = sr.ReadToEnd();
            _files.Add(new File(TabController.SelectedIndex, path, url));
            sr.Close();
        }
        private void SetImage(String path, String url, object sender, EventArgs e)
        {
            if (TabController.TabPages.Count == 0)
                AddTab_Click(sender, e);
            TabController.SelectedTab.Controls.Clear();
            TabController.SelectedTab.BackgroundImageLayout = ImageLayout.Stretch;
            TabController.SelectedTab.BackgroundImage = Image.FromFile(path);
            _files.Add(new File(TabController.SelectedIndex, path, url));
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