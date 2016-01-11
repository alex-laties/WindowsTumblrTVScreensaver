using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;

namespace TumblrTVScreensaver
{
    public partial class ScreensaverForm : Form
    {
        //DLL Loads
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        //end DLL Loads

        private bool PreviewMode { get; set; }
        private ChromiumWebBrowser browser;

        private string SelectTag()
        {
            Random rnd = new Random();
            string[] tags = SettingsForm.tags.Split(',');
            return tags[rnd.Next(tags.Length)].Trim();
        }

        private void InitBrowser()
        {
            string tag = SelectTag();
            string url = String.Format("https://www.tumblr.com/tv/{0}", tag);
            this.browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill,
            };
            Console.WriteLine(
                String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", 
                Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion));

            this.browser.Enabled = false;
            this.Controls.Add(browser);
        }

        public ScreensaverForm()
        {
            InitializeComponent();
            InitBrowser();
        }

        public ScreensaverForm(Rectangle bounds)
        {
            InitializeComponent();
            this.Bounds = bounds;
            InitBrowser();
        }

        public ScreensaverForm(IntPtr windowHandle)
        {
            InitializeComponent();
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            Rectangle parentRect;
            GetClientRect(windowHandle, out parentRect);
            this.Size = parentRect.Size;
            this.Location = new Point(0, 0);
            this.PreviewMode = true;
            InitBrowser();
        }

        private void TearDown()
        {
            browser.Dispose();
            Cef.Shutdown();
            Application.Exit();
        }

        private void ScreensaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            this.Focus();
        }

        // Exit when the mouse clicks or the keyboard gets pressed
        private void ScreensaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.PreviewMode) { TearDown(); }
        }

        private void ScreensaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'q') { TearDown(); }
            if (!this.PreviewMode) { TearDown(); }
        }
    }
}
