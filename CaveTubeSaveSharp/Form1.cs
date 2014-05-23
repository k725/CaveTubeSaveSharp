using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.ComponentModel;
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
                sfd.FileName = textBox1.Text;
                sfd.Filter = "Flash Video File(*.flv)|*.flv";
                sfd.Title = "Save";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
					HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(textBox2.Text + textBox1.Text);
                    webreq.Referer = "http://gae.cavelis.net/view/";
                    HttpWebResponse webres = null;
                    try
                    {
                        webres = (HttpWebResponse)webreq.GetResponse();
                        WebClient wc = new WebClient();
                        string filename = sfd.FileName;
                        wc.Headers.Add("Referer", "http://gae.cavelis.net/view/");
                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += wc_DownloadFileCompleted;
						wc.DownloadFileAsync(new Uri(textBox2.Text + textBox1.Text), filename);
                    }
                    catch (WebException ex)
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
            label2.Text = e.ProgressPercentage + "% " + (e.BytesReceived / 1024) + " KByte";
            progressBar1.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, 100);
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            SystemSounds.Beep.Play();
            label2.Text = "Done!!";
            TaskbarManager.Instance.SetProgressValue(0, 100);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hoge", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}