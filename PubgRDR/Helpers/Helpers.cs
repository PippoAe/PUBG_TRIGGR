using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tesseract;
//using Win32Interop.WinHandles;

namespace PubgTriggr
{
    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }

    class Helpers
    {
        //OCR Engine
        private TesseractEngine OCREngine;


        //OBS Projector WindowName
        private string _ObsProjectorWindowName = "obs64";

        public string ObsProjectorWindowName
        {
            get { return _ObsProjectorWindowName; }
            set { _ObsProjectorWindowName = value; }
        }

        //General ColorThreshold
        private int _generalColorThreshold = 106;
        public int GeneralColorThreshold
        {
            get { return _generalColorThreshold; }
            set { _generalColorThreshold = value; }
        }


        private static Helpers instance;

        private Helpers()
        {
            //CreateOCR Engine
            OCREngine = new TesseractEngine(@"./tessdata", "eng", EngineMode.TesseractOnly);
            OCREngine.DefaultPageSegMode = PageSegMode.SingleBlock;
            OCREngine.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_");
            _generalColorThreshold = 100;

            //Old Version to get the window handle.
            //Set ObsProjectorWindowHandle to get screenshots from
            //var obsProkjectorWindows = TopLevelWindowUtils.FindWindows(wh => wh.GetWindowText().Equals(_ObsProjectorWindowName));
            //var obsProkjectorWindows = TopLevelWindowUtils.FindWindows(wh => wh.GetClassName().Equals("Qt5QWindowIcon") && (wh.IsVisible() == true));
            //ObsProjectorHandle = obsProkjectorWindows.FirstOrDefault().RawPtr;

        }

        public static Helpers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Helpers();
                }
                return instance;
            }
        }


        /// <summary>
        /// Get Cleaned number from String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int CleanupNumber(string input)
        {
            int returnvalue = 0;

            input = new String(input.Where(c => char.IsDigit(c)).ToArray());


            try
            {
                returnvalue = Convert.ToInt16(input);
            }
            catch (Exception e)
            {

            }
            return returnvalue;
        }

        /// <summary>
        /// Cleans Image to enhance for known color
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public Bitmap CleanForColor(Bitmap original, Color reference)
        {
            Bitmap copy = new Bitmap(original);
            for (int i = 0; i < copy.Width; i++)
            {
                for (int j = 0; j < copy.Height; j++)
                {
                    Color c = copy.GetPixel(i, j);
                    int colorDiff = ColorDiff(reference, c);
                    //if (colorDiff > 100)
                    if (colorDiff > _generalColorThreshold)
                    {
                        copy.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        copy.SetPixel(i, j, Color.White);
                    }
                }
            }
            original.Dispose();
            return copy;
        }

        /// <summary>
        /// Get ColorDifference in Value from 0 - 250
        /// </summary>
        /// <param name="color"></param>
        /// <param name="curr"></param>
        /// <returns></returns>
        public int ColorDiff(Color color, Color curr)
        {
            return Math.Abs(color.R - curr.R) + Math.Abs(color.G - curr.G) + Math.Abs(color.B - curr.B);
        }

        //Checks if killrectangle indeed contains a killmessage
        public int GetColorPercentage(Bitmap Image, Color reference)
        {
            //Get MiddleLine
            int middleLine = Image.Height / 2;

            int noRed = 0;
            int noNonRed = 0;

            //CheckMiddleLine for Redpercentage
            for (int i = 0; i < Image.Width; i++)
            {
                Color c = Image.GetPixel(i, middleLine);
                //GetRedNess in respect to Reference Red. (0 is perfect same color)
                int colorDiff = ColorDiff(reference, c);

                //if (colorDiff > 50)
                //if (colorDiff > _generalColorThreshold)
                if (colorDiff > _generalColorThreshold)
                    noNonRed++;
                else
                    noRed++;
            }
            //Dispose the image
            int Total = Image.Width;
            Image.Dispose();


            int percentage = 0;
            if (noRed != 0)
                percentage = (int)Math.Round((double)(100 * noRed) / Total);


            return percentage;
        }


        public Bitmap CleanKillMessage(Bitmap original)
        {
            Bitmap copy = new Bitmap(original);

            //Get whitest white in picture
            float white = 1;

            float upperlimit = white - 0.001f * 235;
            float lowerlimit = white - 0.001f * 0;

            //float upperlimit = whitest - 0.001f * trackBar2.Value;
            //float lowerlimit = whitest - 0.001f * trackBar1.Value;


            int noBlack = 0;
            int noWhite = 0;

            for (int i = 0; i < copy.Width; i++)
            {
                for (int j = 0; j < copy.Height; j++)
                {
                    Color c = copy.GetPixel(i, j);
                    if (c.GetBrightness() < lowerlimit)
                    {
                        copy.SetPixel(i, j, Color.Black);
                        noBlack++;
                    }
                    if (c.GetBrightness() > upperlimit)
                    {
                        copy.SetPixel(i, j, Color.White);
                        noWhite++;
                    }
                }
            }

            int bwPercent = 0;

            if (noWhite != 0)
                bwPercent = noBlack / noWhite;

            if (bwPercent >= 3 && bwPercent <= 20)
                return copy;
            else
                return null;
        }


        public int OCRNumber(Bitmap input)
        {
            
            string ocrResult = "";
            try
            {
                using (var page = OCREngine.Process(input, PageSegMode.SingleLine))
                {
                    ocrResult = page.GetText();
                    input.Dispose();
                    return CleanupNumber(ocrResult);
                }

            }
            catch
            {
                return 0;
            }
        }

        public string OCRText(Bitmap input)
        {

            string ocrResult = "";
            try
            {
                using (var page = OCREngine.Process(input, PageSegMode.SingleLine))
                {
                    ocrResult = page.GetText();
                    return ocrResult;
                }

            }
            catch
            {
                return "";
            }
        }


        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        /// <summary>
        /// Screenshot Helper
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        //public Bitmap MakeScreenShot(Rectangle rect)
        //{
        //    RECT rc;
        //    GetWindowRect(ObsProjectorHandle, out rc);
        //    //TODO only copy small rectangle...
        //    //Bitmap bmp = new Bitmap(rc.Height, rc.Height, PixelFormat.Format32bppArgb);
        //    Bitmap bmp = new Bitmap(rect.Height, rect.Width, PixelFormat.Format32bppArgb);

        //    Graphics gfxBmp = Graphics.FromImage(bmp);
        //    IntPtr hdcBitmap = gfxBmp.GetHdc();

        //    PrintWindow(ObsProjectorHandle, hdcBitmap, 0);

        //    gfxBmp.ReleaseHdc(hdcBitmap);
        //    gfxBmp.Dispose();

        //    return bmp;
        //}

        /// <summary>
        /// Rect Helper Class for ImageCapturing.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            private int _Left;
            private int _Top;
            private int _Right;
            private int _Bottom;

            public RECT(RECT Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
            {
            }
            public RECT(int Left, int Top, int Right, int Bottom)
            {
                _Left = Left;
                _Top = Top;
                _Right = Right;
                _Bottom = Bottom;
            }

            public int X
            {
                get { return _Left; }
                set { _Left = value; }
            }
            public int Y
            {
                get { return _Top; }
                set { _Top = value; }
            }
            public int Left
            {
                get { return _Left; }
                set { _Left = value; }
            }
            public int Top
            {
                get { return _Top; }
                set { _Top = value; }
            }
            public int Right
            {
                get { return _Right; }
                set { _Right = value; }
            }
            public int Bottom
            {
                get { return _Bottom; }
                set { _Bottom = value; }
            }
            public int Height
            {
                get { return _Bottom - _Top; }
                set { _Bottom = value + _Top; }
            }
            public int Width
            {
                get { return _Right - _Left; }
                set { _Right = value + _Left; }
            }
            public Point Location
            {
                get { return new Point(Left, Top); }
                set
                {
                    _Left = value.X;
                    _Top = value.Y;
                }
            }
            public Size Size
            {
                get { return new Size(Width, Height); }
                set
                {
                    _Right = value.Width + _Left;
                    _Bottom = value.Height + _Top;
                }
            }

            public static implicit operator Rectangle(RECT Rectangle)
            {
                return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
            }
            public static implicit operator RECT(Rectangle Rectangle)
            {
                return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
            }
            public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
            {
                return Rectangle1.Equals(Rectangle2);
            }
            public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
            {
                return !Rectangle1.Equals(Rectangle2);
            }

            public override string ToString()
            {
                return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public bool Equals(RECT Rectangle)
            {
                return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
            }

            public override bool Equals(object Object)
            {
                if (Object is RECT)
                {
                    return Equals((RECT)Object);
                }
                else if (Object is Rectangle)
                {
                    return Equals(new RECT((Rectangle)Object));
                }

                return false;
            }
        }

        public Process GetProcessByName(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);

            foreach (Process p in processes)
            {
                IntPtr windowHandle = p.MainWindowHandle;
                // do something with windowHandle
            }
            return processes[0];
        }

        public IntPtr GetWindowHandleByName(string windowname)
        {
            IntPtr hWnd = IntPtr.Zero;
            Process[] processes = Process.GetProcesses();
            foreach (Process pList in processes)
            {
                if (pList.MainWindowTitle.Contains(windowname))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;

        }

        public Bitmap MakeDarkestImage(List<Bitmap> pictures)
        {
            //New Bitmap for darkest and lightest Version
            Bitmap darkestPixels = new Bitmap(pictures[0].Width, pictures[0].Height);
            Bitmap brightestPixels = new Bitmap(pictures[0].Width, pictures[0].Height);

            //Go the whole X axis
            for (int x = 0; x < darkestPixels.Width; x++)
            {
                //Go the whole Y axis
                for (int y = 0; y < darkestPixels.Height; y++)
                {
                    float darkest = 1;
                    int darkestlayer = 0;
                    float brightest = 0;
                    int brightestlayer = 0;

                    for (int i = 0; i < pictures.Count; i++)
                    {
                        float brightness = pictures[i].GetPixel(x, y).GetBrightness();
                        if (brightness < darkest)
                        {
                            darkest = brightness;
                            darkestlayer = i;
                        }
                        if (brightness > brightest)
                        {
                            brightest = brightness;
                            brightestlayer = i;
                        }

                    }
                    darkestPixels.SetPixel(x, y, pictures[darkestlayer].GetPixel(x, y));
                    brightestPixels.SetPixel(x, y, pictures[brightestlayer].GetPixel(x, y));
                }
            }
            return darkestPixels;
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
