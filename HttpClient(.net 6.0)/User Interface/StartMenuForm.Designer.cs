using System.Reflection;

namespace User_Interface
{
    partial class StartMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartMenu));
            this.SearchButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddTab = new System.Windows.Forms.ToolStripButton();
            this.URL_textbox = new System.Windows.Forms.ToolStripTextBox();
            this.NavigateBtn = new System.Windows.Forms.ToolStripButton();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.TabController = new System.Windows.Forms.TabControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(0, 0);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 4;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddTab,
            this.URL_textbox,
            this.NavigateBtn,
            this.RefreshButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 27);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // AddTab
            // 
            this.AddTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddTab.Image = global::User_Interface.Properties.Resources.AddTab;
            this.AddTab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddTab.Name = "AddTab";
            this.AddTab.Size = new System.Drawing.Size(29, 24);
            this.AddTab.Text = "toolStripButton1";
            this.AddTab.Click += new System.EventHandler(this.AddTab_Click);
            // 
            // URL_textbox
            // 
            this.URL_textbox.Name = "URL_textbox";
            this.URL_textbox.Size = new System.Drawing.Size(601, 27);
            // 
            // NavigateBtn
            // 
            this.NavigateBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NavigateBtn.Image = global::User_Interface.Properties.Resources.search;
            this.NavigateBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NavigateBtn.Name = "NavigateBtn";
            this.NavigateBtn.Size = new System.Drawing.Size(29, 24);
            this.NavigateBtn.Text = "toolStripButton2";
            this.NavigateBtn.Click += new System.EventHandler(this.NavigateBtn_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(29, 24);
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // TabController
            // 
            this.TabController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabController.Location = new System.Drawing.Point(0, 27);
            this.TabController.Name = "TabController";
            this.TabController.SelectedIndex = 0;
            this.TabController.Size = new System.Drawing.Size(800, 424);
            this.TabController.TabIndex = 5;
            // 
            // StartMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 451);
            this.Controls.Add(this.TabController);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.SearchButton);
            this.Name = "StartMenu";
            this.Text = "Our custom browser";
            this.Load += new System.EventHandler(this.StartMenu_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button SearchButton;
        
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStrip toolStrip1;
        private ToolStripButton AddTab;
        private ToolStripTextBox URL_textbox;
        private ToolStripButton NavigateBtn;
        private TabControl TabController;
        private ToolStripButton RefreshButton;
    }
}