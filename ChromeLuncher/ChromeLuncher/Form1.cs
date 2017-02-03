using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Reflection;

namespace ChromeLuncher
{
    public partial class Form1 : Form
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        String resourceName = "ChromeLuncher.websites.txt";
        List<string> websites = new List<string>();
        List<string> addedWebsites = new List<string>();
        

        public Form1()
        {
            InitializeComponent();

        }

        private void initList()
        {
            listBox1.DataSource = websites;
            listBox1.SelectedIndex = -1;
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
            if (String.IsNullOrEmpty(txt_newWebsite.Text.ToString()) || !isReachable(txt_newWebsite.Text.ToString()))
            {
                MessageBox.Show("please valid website");
            }
            else
            {
                string newWebsite = txt_newWebsite.Text;
                addedWebsites.Add(newWebsite);
            }
        }

        private bool isReachable(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }


        }


        private void readFromFile()
        {
            try
            {   // Open the text file using a stream reader.
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
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
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {   // Open the text file using a stream reader.
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamWriter writer = new StreamWriter(stream))
                {

                    foreach (var s in addedWebsites)
                    {
                        writer.Write(s);
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
