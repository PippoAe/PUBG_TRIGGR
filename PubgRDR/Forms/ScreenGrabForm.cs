using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PubgTriggr
{
    public partial class ScreenGrabForm : Form
    {
        //ReturnValues
        public Rectangle SelectedRectangle { get; set; }

        PictureBox pictureBox1;
        //These variables control the mouse position
        int selectX;
        int selectY;
        int selectWidth;
        int selectHeight;
        private Pen selectPen;

        //This variable control when you start the right click
        bool start = false;

        public ScreenGrabForm()
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
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseDown += pictureBox1_MouseDown;

            //Hide the Form
            this.Hide();

            //Create the Bitmap
            Bitmap printscreen = GameCapture.Instance.MakeScreenShot();
            //Create the Graphic Variable with screen Dimensions
            Graphics graphics = Graphics.FromImage(printscreen as Image);
            //Copy Image from the screen
            //graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
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

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //validate if there is an image
            if (pictureBox1.Image == null)
                return;
            //validate if right-click was trigger
            if (start)
            {
                //refresh picture box
                pictureBox1.Refresh();
                //set corner square to mouse coordinates
                selectWidth = e.X - selectX;
                selectHeight = e.Y - selectY;
                //draw dotted rectangle
                pictureBox1.CreateGraphics().DrawRectangle(selectPen,
                          selectX, selectY, selectWidth, selectHeight);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //validate when user right-click
            if (!start)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    //starts coordinates for rectangle
                    selectX = e.X;
                    selectY = e.Y;
                    selectPen = new Pen(Color.Red, 1);
                    selectPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                }
                //refresh picture box
                pictureBox1.Refresh();
                //start control variable for draw rectangle
                start = true;
            }
            else
            {
                //validate if there is image
                if (pictureBox1.Image == null)
                    return;
                //same functionality when mouse is over
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    pictureBox1.Refresh();
                    selectWidth = e.X - selectX;
                    selectHeight = e.Y - selectY;
                    pictureBox1.CreateGraphics().DrawRectangle(selectPen, selectX,
                             selectY, selectWidth, selectHeight);

                }
                start = false;
                //function save image to clipboard
                SaveToClipboard();
            }
        }

        private void SaveToClipboard()
        {
            //validate if something selected
            if (selectWidth > 0)
            {
                Rectangle rect = new Rectangle(selectX, selectY, selectWidth, selectHeight);
                this.SelectedRectangle = rect;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }



        private Size GetDpiSafeResolution()
        {
            return new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }
    }
}
