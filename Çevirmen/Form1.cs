using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Automation;
using HtmlAgilityPack;
using System.Net;
using System.Web;
using mshtml;
using System.Web.UI;
using System.Web.Administration;
using System.Web.Hosting;
using System.Globalization;
using System.Xml;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using System.Collections.Generic;


//using System.Timers;
namespace WindowsFormsApplication10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Clipboard.Clear();
            System.Threading.Thread thread1 = new System.Threading.Thread(new System.Threading.ThreadStart(listen));
            thread1.ApartmentState = System.Threading.ApartmentState.STA;
            thread1.Start();
            System.Threading.Thread.Sleep(5000);
            //this.WindowState = FormWindowState.Minimized;
        }



        string oldSelectedText = string.Empty;
        string myTranslatedWord = string.Empty;
        bool translateIsDone = false;
        int popUpX = 0;
        int popUpY = 0;
        string fromLanguage = "English";
        string toLanguage = "Turkish";

        private void Yaz(string kayit)
        {
            SetText(kayit);
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (rtxtKelimeler.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                rtxtKelimeler.Text += text;
            }
        }

        TranslateWord translateWord = new TranslateWord();
        private void listen()
        {
            Timer t = new Timer();
            t.Interval = 1000;
            t.Start();

            while (true)
            {
                if (t.Interval == 1000)
                {
                    string tempString = string.Empty;
                    tempString = System.Windows.Forms.Clipboard.GetText();
                    var element = AutomationElement.FocusedElement;
                    if ((!string.IsNullOrWhiteSpace(tempString)))
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine(tempString);
                        string selectedText = string.Empty;
                        if (sb.ToString() != "\r\n")
                        {
                            popUpX = GetCursorPosition().X;
                            popUpY = GetCursorPosition().Y;
                            translateWord.originalWord = sb.ToString();
                            try
                            {
                                string content = translateWord.originalWord;
                                translateIsDone = false;

                                // Set the From and To language
                                //string fromLanguage = "English";
                                //string toLanguage = "Turkish";

                                // Create a Language mapping
                                var languageMap = new Dictionary<string, string>();
                                InitLanguageMap(languageMap);

                                // Create an instance of WebClient in order to make the language translation
                                //Uri address = new Uri("http://translate.google.com/");
                                Uri address = new Uri("https://translate.google.com/");
                                WebClient wc = new WebClient();
                                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                                wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);

                                // Async Upload to the specified source
                                // i.e http://translate.google.com/translate_t for handling the translation.
                                wc.UploadStringAsync(address,
                                   GetPostData(languageMap[fromLanguage], languageMap[toLanguage], content));

                                while (wc.IsBusy || !translateIsDone)
                                {
                                    System.Threading.Thread.Sleep(2000);
                                }
                                if (!string.IsNullOrWhiteSpace(translateWord.translatedWord))
                                {
                                    if (oldSelectedText != translateWord.originalWord)
                                    {
                                        oldSelectedText = translateWord.originalWord;
                                        int uzunluk = oldSelectedText.Length;
                                        Yaz(oldSelectedText.Remove(uzunluk-2,2) + " :   " + translateWord.translatedWord + "\n");
                                        PopUp frm = new PopUp(translateWord.translatedWord, 500, 500);
                                        frm.Location = new Point(popUpX, popUpY);
                                        frm.ShowDialog();
                                        translateWord.originalWord = string.Empty;
                                        translateWord.translatedWord = string.Empty;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Bir hata oluştu." + ex.Message);
                            }
                        }
                        element = null;

                    }
                }


            }
        }

        
        /// <summary>
        /// Initialize Language Mapping, Key value pair of Language Name, Language Code
        /// </summary>
        /// <param name="languageMap"></param>
        static void InitLanguageMap(Dictionary<string, string> languageMap)
        {
            languageMap.Add("Afrikaans", "af");
            languageMap.Add("Albanian", "sq");
            languageMap.Add("Arabic", "ar");
            languageMap.Add("Armenian", "hy");
            languageMap.Add("Azerbaijani", "az");
            languageMap.Add("Basque", "eu");
            languageMap.Add("Belarusian", "be");
            languageMap.Add("Bengali", "bn");
            languageMap.Add("Bulgarian", "bg");
            languageMap.Add("Catalan", "ca");
            languageMap.Add("Chinese", "zh-CN");
            languageMap.Add("Croatian", "hr");
            languageMap.Add("Czech", "cs");
            languageMap.Add("Danish", "da");
            languageMap.Add("Dutch", "nl");
            languageMap.Add("English", "en");
            languageMap.Add("Esperanto", "eo");
            languageMap.Add("Estonian", "et");
            languageMap.Add("Filipino", "tl");
            languageMap.Add("Finnish", "fi");
            languageMap.Add("French", "fr");
            languageMap.Add("Galician", "gl");
            languageMap.Add("German", "de");
            languageMap.Add("Georgian", "ka");
            languageMap.Add("Greek", "el");
            languageMap.Add("Haitian Creole", "ht");
            languageMap.Add("Hebrew", "iw");
            languageMap.Add("Hindi", "hi");
            languageMap.Add("Hungarian", "hu");
            languageMap.Add("Icelandic", "is");
            languageMap.Add("Indonesian", "id");
            languageMap.Add("Irish", "ga");
            languageMap.Add("Italian", "it");
            languageMap.Add("Japanese", "ja");
            languageMap.Add("Korean", "ko");
            languageMap.Add("Lao", "lo");
            languageMap.Add("Latin", "la");
            languageMap.Add("Latvian", "lv");
            languageMap.Add("Lithuanian", "lt");
            languageMap.Add("Macedonian", "mk");
            languageMap.Add("Malay", "ms");
            languageMap.Add("Maltese", "mt");
            languageMap.Add("Norwegian", "no");
            languageMap.Add("Persian", "fa");
            languageMap.Add("Polish", "pl");
            languageMap.Add("Portuguese", "pt");
            languageMap.Add("Romanian", "ro");
            languageMap.Add("Russian", "ru");
            languageMap.Add("Serbian", "sr");
            languageMap.Add("Slovak", "sk");
            languageMap.Add("Slovenian", "sl");
            languageMap.Add("Spanish", "es");
            languageMap.Add("Swahili", "sw");
            languageMap.Add("Swedish", "sv");
            languageMap.Add("Tamil", "ta");
            languageMap.Add("Telugu", "te");
            languageMap.Add("Thai", "th");
            languageMap.Add("Turkish", "tr");
            languageMap.Add("Ukrainian", "uk");
            languageMap.Add("Urdu", "ur");
            languageMap.Add("Vietnamese", "vi");
            languageMap.Add("Welsh", "cy");
            languageMap.Add("Yiddish", "yi");
        }
        /// <summary>
        /// Construct the Post data required for Google Translation
        /// </summary>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <returns></returns>
        static string GetPostData(string fromLanguage, string toLanguage, string content)
        {
            // Set the language translation. All we need is the language pair, from and to.
            string strPostData = string.Format("hl=en&ie=UTF8&oe=UTF8submit=Translate&langpair={0}|{1}",
                                                 fromLanguage,
                                                 toLanguage);

            // Encode the content and set the text query string param
            return strPostData += "&text=" + HttpUtility.UrlEncode(content);
        }


        public void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(e.Result);
                    var node = doc.DocumentNode.SelectSingleNode("//span[@id='result_box']");
                    //var node = doc.DocumentNode.SelectSingleNode("//div[@class='file'");
                    var output = node != null ? node.InnerText : e.Error.Message;

                    //Console.WriteLine("Çevrilmiş Hali: " + output);
                    //translateWord.translatedWord = output.Replace("#39;", @"'");

                    translateWord.translatedWord = output.Replace("&quot;", @"""").Replace("#39;", @"'");

                    translateIsDone = true;

                }
            }
            catch (Exception)
            {
                
                //throw;
                //Application.Restart();
            }
   
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Environment.Exit(0);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton buton = sender as RadioButton;
            fromLanguage = buton.Text;
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton buton = sender as RadioButton;
            toLanguage = buton.Text;
        }

        //private void btnKaydet_Click(object sender, EventArgs e)
        //{
        //    System.IO.File.WriteAllText(@"C:\CevrilenKelimeler.txt", rtxtKelimeler.Text);
        //}

    }
}
