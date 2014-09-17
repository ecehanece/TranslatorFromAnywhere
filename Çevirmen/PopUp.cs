using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication10
{
    public partial class PopUp : Form
    {
        int en = 0, boy = 0;
        string text = "";
        string realText = string.Empty;

        public PopUp(string text, int width1, int height1)
        {
            InitializeComponent();
            //en = this.Width = width1;
            //boy = this.Height = height1;
            text = text.Replace('\n', ' ');

            //this.AutoSize = true;
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //int space = 0;
            List<string> stringList = new List<string>();

            if (text.Contains(" "))
            {
                stringList = text.Split(' ').ToList();
                //space = stringList.Count();
            }

            else if (!string.IsNullOrWhiteSpace(text))
            {
                stringList.Add(text);
            }



            int counter = 0;

            for (int i = 0; i < stringList.Count(); i++)
            {
                string tempLine = string.Empty;

                for (int j = 0; j < text.Length; j++)
                {
                    if (tempLine.Length < System.Math.Sqrt(text.Length)*2)
                    {
                        if (counter < stringList.Count())
                        {
                            tempLine = tempLine + " " + stringList[counter];
                            counter++;
                        }
                        else break;
                    }
                    else break;
                }
                if (string.IsNullOrWhiteSpace(realText))
                    realText = tempLine;
                else
                    realText = realText + "\r\n" + tempLine;
                if (counter.Equals(stringList.Count()))
                    break;

            }

            this.text = realText;
        }

        private void PopUp_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void PopUp_Paint(object sender, PaintEventArgs e)
        {
            this.CreateGraphics().DrawRectangle(Pens.Black, 0, 0, (this.Width - 1), (this.Height - 1));
        }

        private void PopUp_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            Label lbl_text = new Label
            {
                Left =2, // sol tarafa uzaklık 30 pixel
                Top = 2, // yukarıya uzaklık 30 pixel
                AutoSize = true, // label boyutunu text'e göre  ayarla
                Font = new Font(this.Font, FontStyle.Bold), // font kalın olsun
                Text = text.Replace("\n", string.Empty) // metin parametresini ata
            };

            // oluşturulan labeli forma ekle
            this.Controls.Add(lbl_text);
            lbl_text.Click += new System.EventHandler(this.PopUp_Click);
            lbl_text.Font = new System.Drawing.Font(this.Font.FontFamily.Name, 12);
        }
    }
}
