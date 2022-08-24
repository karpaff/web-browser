using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PWebBrowser
{
    public partial class Browser : Form
    {
        private string strINIFile = "browser.ini";
        private const string HeadWindow = "[Window]";
        private const string HeadBrowser = "[Browser]";


        public Browser()
        {
            InitializeComponent();
        }

        // Form closing event handler
        private void Browser_FormClosed(Object sender, FormClosedEventArgs e)
        {
            StreamWriter sw = new StreamWriter(new FileInfo(strINIFile).Open(FileMode.Create));

            sw.WriteLine(HeadWindow);
            sw.WriteLine("Lft=" + this.Left);
            sw.WriteLine("Top=" + this.Top);
            sw.WriteLine("Wdt=" + this.Width);
            sw.WriteLine("Hgh=" + this.Height);
            sw.WriteLine("Sta=" + this.WindowState);

            sw.WriteLine(HeadBrowser);
            sw.WriteLine("URL=" + this.wbBrowser.Url);

            sw.Close();

        }

        // Form loading event handler
        private void Browser_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader(new FileInfo(strINIFile).Open(FileMode.Open));
                while (!sr.EndOfStream)
                {
                    string firstLine = sr.ReadLine();
                    if (firstLine == HeadWindow)
                    {
                        // Until you get to the end or '[' symbol
                        while (sr.Peek() != -1 && (Convert.ToChar(sr.Peek()).CompareTo('[') != 0))
                        {
                            string line = sr.ReadLine();
                            if (line != "")
                            {
                                string setName = line.Substring(0, 3);
                                string setValue = line.Substring(4);

                                switch (setName)
                                {
                                    case "Lft":
                                        this.Left = int.Parse(setValue);
                                        break;
                                    case "Top":
                                        this.Top = int.Parse(setValue);
                                        break;
                                    case "Wdt":
                                        this.Width = int.Parse(setValue);
                                        break;
                                    case "Hgh":
                                        this.Height = int.Parse(setValue);
                                        break;
                                    case "Sta":
                                        switch (setValue)
                                        {
                                            case "Normal":
                                                this.WindowState = FormWindowState.Normal;
                                                break;
                                            case "Minimized":
                                                this.WindowState = FormWindowState.Minimized;
                                                break;
                                            case "Maximized":
                                                this.WindowState = FormWindowState.Maximized;
                                                break;
                                        }
                                        break;
                                }
                            }
                        }

                    }

                    else if (firstLine == HeadBrowser)
                    {
                        // Until you get to the end or '[' symbol
                        while (sr.Peek() != -1 && (Convert.ToChar(sr.Peek()).CompareTo('[') != 0))
                        {
                            string line = sr.ReadLine();
                            if (line != "")
                            {
                                string setName = line.Substring(0, 3);
                                string setValue = line.Substring(4);

                                if (setName == "URL")
                                {
                                    this.txtURL.Text = setValue;
                                    this.wbBrowser.Navigate(this.txtURL.Text);
                                }
                            }
                        }
                    }

                }

                sr.Close();
            }
            // If the file does not exist or the value is incorrect, set the default setting
            catch { }
        }


        // Key event handler
        private void txtURL_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (e.KeyChar == (char)Keys.Return)
            {
                wbBrowser.Navigate(txtURL.Text);
                e.Handled = true;
            }

        }
    }
}

