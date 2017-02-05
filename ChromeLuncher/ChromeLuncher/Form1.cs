using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Reflection;
using System.Net.NetworkInformation;

namespace ChromeLuncher
{
    public partial class Form1 : Form
    {

        const string FileNameChromeLuncher = "ChromeLuncher.txt";
        List<string> websites = new List<string>();
        List<string> addedWebsites = new List<string>();


        public Form1()
        {
            InitializeComponent();

        }

        private void initList()
        {
            
            listBox1.DataSource = null;
            listBox1.DataSource = websites;
            listBox1.SelectedIndex = -1;
            listBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                foreach (var item in listBox1.Items)
                {
                    Process.Start("chrome.exe", item.ToString());
                }
            }
            else
            {
                Process.Start("chrome.exe", listBox1.SelectedItem.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_newWebsite.Text.ToString()) || 
                !isReachable(txt_newWebsite.Text.ToString().Trim()))
            {
                MessageBox.Show("please valid website");
            }
            else
            {
                string newWebsite = txt_newWebsite.Text;
                websites.Add(newWebsite);
                initList();
            }
        }

        private bool isReachable(string url)
        {
            var ping = new Ping();
            var result = ping.Send(url);
            if (result.Status == IPStatus.Success)
                return true;
            return false;
        }


        private string getFile()
        {
            string driveName = DriveInfo.GetDrives()[1].Name;
            if (!File.Exists(Path.Combine(driveName, FileNameChromeLuncher)))
            {
                File.Create(Path.Combine(driveName, FileNameChromeLuncher));
            }
            return Path.Combine(driveName, FileNameChromeLuncher);
        }
        private void readFromFile()
        {
            try
            {
                string fileName = getFile();
                MessageBox.Show(fileName);
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string[] result = reader.ReadToEnd().Split('\n');
                    foreach (var s in result)
                    {
                        websites.Add(s);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("The file could not be read:");
                MessageBox.Show(e.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            readFromFile();
            initList();

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!e.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateFile();
            e.Cancel = true;
            this.Dispose();
        }

        private void UpdateFile()
        {
            try
            {
                string fileName = getFile();
                System.IO.File.WriteAllText(fileName, string.Empty);
                using (StreamWriter writer = new StreamWriter(fileName))
                {

                    foreach (var s in websites)
                    {
                        writer.Write(s);
                        writer.Write("\n");
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("The file could not be read:");
                MessageBox.Show(e1.Message);
            }
        }
    }


}
