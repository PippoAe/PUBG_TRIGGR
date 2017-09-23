using System;
using System.Drawing;
using System.Runtime.InteropServices;


namespace PubgTriggr
{
    class GameCapture
    {
        private IntPtr _windowHandle;
        private static GameCapture instance;
        public IntPtr SetWindowHandle
        {
            get
            {
                return _windowHandle;
            }
            set
            {
                _windowHandle = value;
            }
        }

        private GameCapture()
        {
        }

        public static GameCapture Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameCapture();
                return instance;
            }
        }

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);


        [DllImport("gdi32", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject([In] IntPtr hObject);



        [DllImport("gdi32", EntryPoint = "BitBlt")]
        private static extern int BitBlt(
         IntPtr hdcDest,     // handle to destination DC (device context)
         int nXDest,         // x-coord of destination upper-left corner
         int nYDest,         // y-coord of destination upper-left corner
         int nWidth,         // width of destination rectangle
         int nHeight,        // height of destination rectangle
         IntPtr hdcSrc,      // handle to source DC
         int nXSrc,          // x-coordinate of source upper-left corner
         int nYSrc,          // y-coordinate of source upper-left corner
         System.Int32 dwRop  // raster operation code
         );

        [DllImport("gdi32", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hDC);

        public Bitmap MakeScreenShot(Rectangle rect = default(Rectangle))
        {
            IntPtr windowDC = GetWindowDC(_windowHandle);
            IntPtr mem = CreateCompatibleDC(windowDC);

            RECT rc;
            GetWindowRect(new HandleRef(null, _windowHandle), out rc);
            if (rect.X == 0 && rect.Y == 0 && rect.Width == 0 && rect.Height == 0)
            {
                rect.Width = rc.Width;
                rect.Height = rc.Height;
            }

            IntPtr windowImage = CreateCompatibleBitmap(windowDC, rect.Width, rect.Height);
            IntPtr oldBmp = (IntPtr)SelectObject(mem, windowImage);

            BitBlt(mem, 0, 0, rect.Width, rect.Height, windowDC, rect.X, rect.Y, 13369376);
            Bitmap image = Bitmap.FromHbitmap(windowImage);

            DeleteDC(mem);
            ReleaseDC(_windowHandle, mem);
            DeleteObject(windowImage);
            DeleteObject(oldBmp);
            DeleteObject(windowDC);
            DeleteObject(mem);

            windowImage = IntPtr.Zero;
            oldBmp = IntPtr.Zero;

            return image;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }
    }
}