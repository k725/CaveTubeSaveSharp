using System;
using System.Media;
using System.Net;
using System.Windows.Forms;

namespace CaveTubeSaveSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            button1.Enabled = false;

            if (textBox1.Text != "")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = textBox1.Text + ".flv";
                sfd.Filter = "Flash Video File(*.flv)|*.flv";
                sfd.Title = "Save";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string urls = "http://rec.cavelis.net/media/" + textBox1.Text + ".flv";

                    HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(urls);
                    webreq.Referer = "http://gae.cavelis.net/view/";
                    HttpWebResponse webres = null;
                    try
                    {
                        webres = (System.Net.HttpWebResponse)webreq.GetResponse();
                        WebClient wc = new WebClient();
                        Uri url = new Uri("http://rec.cavelis.net/media/" + textBox1.Text + ".flv");
                        string filename = sfd.FileName;
                        wc.Headers.Add("Referer", "http://gae.cavelis.net/view/");
                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                        wc.DownloadFileAsync(url, filename);
                    }
                    catch (System.Net.WebException ex)
                    {
                        if (ex.Status == System.Net.WebExceptionStatus.ProtocolError)
                        {
                            HttpWebResponse errres = (HttpWebResponse)ex.Response;
                            MessageBox.Show(errres.StatusDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    finally
                    {
                        if (webres != null)
                        {
                            webres.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Media URL is empty!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            textBox1.Enabled = true;
            button1.Enabled = true;
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label2.Text = e.ProgressPercentage + "% " + e.BytesReceived + " Byte";
            progressBar1.Value = e.ProgressPercentage;
            Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, 100);
        }

        private void wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            SystemSounds.Beep.Play();
            label2.Text = "Done!!";
            Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance.SetProgressValue(0, 100);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hoge", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}