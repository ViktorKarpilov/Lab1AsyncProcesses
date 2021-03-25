using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            bool isAllFinded = false;
            int i = 1;
            List<string> urls = new();

            while (!isAllFinded)
            {
                var elements = this.Controls.Find($"textBox{i}", false);
                isAllFinded = elements.Length == 0;
                if (!isAllFinded)
                    urls.Add(elements.First().Text);
                i++;
            }

            Dictionary<string,bool> threadsState = new();
            Dictionary<Thread,string> threads = new();
            
            foreach(string url in urls)
            {
                Thread thread = new(() =>
                {
                    string html = string.Empty;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip;

                    using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using Stream stream = response.GetResponseStream();
                    using StreamReader reader = new(stream);
                    html = reader.ReadToEnd();

                    using StreamWriter file = new(Path.Combine(OutPutFolder.Text + url.Replace("http://", "")));
                    file.Write(html);
                    threadsState[url] = true;
                });

                threadsState.Add(url,false);
                threads.Add(thread,url);
            }
            
            foreach(var thread in threads)
            {
                thread.Key.Start();
                if (!AsyncFlag.Checked)
                    while (!threadsState[thread.Value])
                        Thread.Sleep(10);
            }

            bool IsDone = false;
            while (!IsDone)
            {
                IsDone = !threadsState.Any(state => state.Value == false);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void OutPutFolder_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
