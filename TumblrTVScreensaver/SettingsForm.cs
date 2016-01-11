using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;

namespace TumblrTVScreensaver
{
    public partial class SettingsForm : Form
    {
        public static string tags
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\TumblrTVScreensaver");
                if (key == null || key.GetValue("text") == null) {
                    return "trippy, text";
                }
                return (string)key.GetValue("text");
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\TumblrTVScreensaver");
                key.SetValue("text", value);
            }
        }

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void SaveSettings()
        {
            tags = textBox1.Text;
        }

        private void LoadSettings()
        {
            textBox1.Text = tags;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
