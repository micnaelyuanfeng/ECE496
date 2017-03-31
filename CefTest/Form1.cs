using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CefTest
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser browser;
        
        public Form1()
        {
            InitializeComponent();
            
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("www.google.com");
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            this.AutoScroll = true;
            this.FormClosing += onFormClosing;
            
        }
        private void onFormClosing(object s,EventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
