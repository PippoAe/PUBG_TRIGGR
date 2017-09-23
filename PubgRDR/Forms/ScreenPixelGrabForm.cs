using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgTriggr
{
    public partial class ScreenPixelGrabForm : Form
    {
        //ReturnValues
        public Point PixelPosition { get; set; }
        public Color Pixel { get; set; }

        PictureBox pictureBox1;
        //These variables control the mouse position
        int selectX;
        int selectY;

        public ScreenPixelGrabForm()
        {
            InitializeComponent();
            this.BackColor = Color.Black;
            //this.StartPosition = FormStartPosition.Manual;
            //this.Top = 0;
            //this.Left = 0;
            //this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.AutoSize = true;
            this.Show();

            pictureBox1 = new PictureBox();
            this.Controls.Add(pictureBox1);
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.MouseDown += PictureBox1_MouseDown;

            //Hide the Form
            this.Hide();

            //Create the Bitmap
            Bitmap printscreen = GameCapture.Instance.MakeScreenShot();
            //Create the Graphic Variable with screen Dimensions
            Graphics graphics = Graphics.FromImage(printscreen as Image);

            //Create a temporal memory stream for the image
            using (MemoryStream s = new MemoryStream())
            {
                //save graphic variable into memory
                printscreen.Save(s, System.Drawing.Imaging.ImageFormat.Bmp);
                pictureBox1.Size = new System.Drawing.Size(this.Width, this.Height);
                //set the picture box with temporary stream
                pictureBox1.Image = Image.FromStream(s);
                pictureBox1.Show();
            }
            //Cross Cursor
            this.Cursor = Cursors.Cross;
            //Show Form
            this.Show();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //validate when user right-click

           if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    //starts coordinates for rectangle
                    selectX = e.X;
                    selectY = e.Y;
                }

            SaveToClipboard();
        }

        private void SaveToClipboard()
        {
            //validate if something selected
            this.PixelPosition = new Point(selectX, selectY);
            this.Pixel = ((Bitmap)pictureBox1.Image).GetPixel(selectX, selectY);
            this.DialogResult = DialogResult.OK;
            this.Close();

        }



        private Size GetDpiSafeResolution()
        {
            return new Size(Screen.AllScreens[1].Bounds.Width, Screen.AllScreens[1].Bounds.Height);
            //using (Graphics graphics = this.CreateGraphics())
            //{
            //    return new Size((Screen.PrimaryScreen.WorkingArea.Width * (int)graphics.DpiX) / 96
            //      , (Screen.PrimaryScreen.WorkingArea.Height * (int)graphics.DpiY) / 96);
            //}
        }
    }
}
