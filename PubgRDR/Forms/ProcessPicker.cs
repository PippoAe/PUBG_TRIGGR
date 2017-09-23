using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PubgTriggr
{
    public partial class ProcessPicker : Form
    {
            private delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

            [DllImport("user32.dll")]
            private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

            [DllImport("user32.Dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

            private IntPtr _windowHandle;
            public IntPtr WindowHandle { get { return _windowHandle; } }

            public ProcessPicker(string processname)
            {
                this.Text = "Capture Process";
                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                this.AutoSize = true;
                this.AutoSizeMode = AutoSizeMode.GrowOnly;
                this.Width = 100;
                this.Height = 100;
                this.StartPosition = FormStartPosition.CenterScreen;


                InitializeComponent();
                this.Refresh();
                //Filter Processes by name
                List<Process> processes = new List<Process>();

                Process[] processlist = Process.GetProcesses();

                foreach (Process proc in processlist)
                {
                    if (proc.ProcessName.Contains(processname))
                        processes.Add(proc);
                }
                //Process id = 00003490;
                //Handle = 00280DB8;

                List<IntPtr> childwindows;
                foreach (Process proc in processes)
                {
                List<IntPtr> rootWindows = GetRootWindowsOfProcess(proc.Id);


                    //if (rootWindows.Count < 0)
                    //{
                    //        processes.Remove(proc);
                    //}
                    //else
                    //{
                    //foreach (IntPtr roothwnd in rootWindows)
                    //{
                    //    if (GetChildWindows(roothwnd).Count > 0)
                    //    {
                    //        childwindows = GetChildWindows(roothwnd);
                            
                    //    }
                    //}
                    //}
                }



                if (processes.Count() > 0)
                {

                    ListView listView1 = new ListView();
                    listView1.View = View.Details;
                    listView1.GridLines = true;
                    listView1.FullRowSelect = true;



                    //Add column header
                    listView1.Columns.Add("Id", -1);
                    listView1.Columns.Add("Name", -1);
                    listView1.Columns.Add("Window", -1);
                    listView1.MouseDoubleClick += listView1_MouseDoubleClick;

                    //Add ImageList
                    var ImageList = new ImageList();
                    foreach (Process proc in processes)
                    {
                        Image icon = GetAppIcon(proc.MainWindowHandle).ToBitmap();
                        ImageList.Images.Add(proc.Id.ToString(), icon);
                    }
                    listView1.SmallImageList = ImageList;
                    listView1.View = View.Details;


                    foreach (Process proc in processes)
                    {
                        string[] arr = new string[4];
                        ListViewItem itm;
                        arr[0] = proc.Id.ToString();
                        arr[1] = proc.ProcessName;
                        arr[2] = proc.MainWindowTitle;
                        itm = new ListViewItem(arr);
                        itm.ImageKey = proc.Id.ToString();
                        itm.Tag = proc;
                        //itm.ImageKey;
                        listView1.Items.Add(itm);
                    }


                    this.Controls.Add(listView1);
                    Button Refresh = new Button();
                    Refresh.Text = "Refresh";
                   
                    listView1.Width = 450;
                    listView1.Height = 200;
                }
            else
            {
                MessageBox.Show("No OBS process found. Start OBS and start preview in projector- or fullscreen mode!","No OBS-Process found",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
            }
            }



            private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
            {
                if (((ListView)sender).SelectedItems.Count == 1)
                {
                    ListView.SelectedListViewItemCollection items = ((ListView)sender).SelectedItems;

                    ListViewItem lvItem = items[0];
                    //WindowHandle = ((Process)items[0].Tag).MainWindowHandle;
                    Process proc = (Process)items[0].Tag;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    _windowHandle = proc.MainWindowHandle;
                    this.Close();
                }
            }



            private List<IntPtr> GetRootWindowsOfProcess(string processname)
            {
                int pid = 0;

                Process[] processlist = Process.GetProcesses();

                foreach (Process theprocess in processlist)
                {
                    if (theprocess.ProcessName.Contains(processname))
                        pid = theprocess.Id;
                }

                if (pid == 0)
                {
                    this.DialogResult = DialogResult.None;
                    this.Close();
                }

                List<IntPtr> rootWindows = GetChildWindows(IntPtr.Zero);
                List<IntPtr> dsProcRootWindows = new List<IntPtr>();
                foreach (IntPtr hWnd in rootWindows)
                {
                    uint lpdwProcessId;
                    GetWindowThreadProcessId(hWnd, out lpdwProcessId);
                    if (lpdwProcessId == pid)
                        dsProcRootWindows.Add(hWnd);
                }
                return dsProcRootWindows;
            }

            private List<IntPtr> GetRootWindowsOfProcess(int pid)
            {
                List<IntPtr> rootWindows = GetChildWindows(IntPtr.Zero);
                List<IntPtr> dsProcRootWindows = new List<IntPtr>();
                foreach (IntPtr hWnd in rootWindows)
                {
                    uint lpdwProcessId;
                    GetWindowThreadProcessId(hWnd, out lpdwProcessId);
                    if (lpdwProcessId == pid)
                        dsProcRootWindows.Add(hWnd);
                }
                return dsProcRootWindows;
            }

            private List<IntPtr> GetChildWindows(IntPtr parent)
            {
                List<IntPtr> result = new List<IntPtr>();
                GCHandle listHandle = GCHandle.Alloc(result);
                try
                {
                    Win32Callback childProc = new Win32Callback(EnumWindow);
                    EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
                }
                finally
                {
                    if (listHandle.IsAllocated)
                        listHandle.Free();
                }
                return result;
            }

            private bool EnumWindow(IntPtr handle, IntPtr pointer)
            {
                GCHandle gch = GCHandle.FromIntPtr(pointer);
                List<IntPtr> list = gch.Target as List<IntPtr>;
                if (list == null)
                {
                    throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
                }
                list.Add(handle);
                //  You can modify this to check to see if you want to cancel the operation, then return a null here
                return true;
            }

            public const int GCL_HICONSM = -34;
            public const int GCL_HICON = -14;

            public const int ICON_SMALL = 0;
            public const int ICON_BIG = 1;
            public const int ICON_SMALL2 = 2;

            public const int WM_GETICON = 0x7F;

            public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
            {
                if (IntPtr.Size > 4)
                    return GetClassLongPtr64(hWnd, nIndex);
                else
                    return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
            }

            [DllImport("user32.dll", EntryPoint = "GetClassLong")]
            public static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
            public static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
            static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

            public Icon GetAppIcon(IntPtr hwnd)
            {
                IntPtr iconHandle = SendMessage(hwnd, WM_GETICON, ICON_SMALL2, 0);
                if (iconHandle == IntPtr.Zero)
                    iconHandle = SendMessage(hwnd, WM_GETICON, ICON_SMALL, 0);
                if (iconHandle == IntPtr.Zero)
                    iconHandle = SendMessage(hwnd, WM_GETICON, ICON_BIG, 0);
                if (iconHandle == IntPtr.Zero)
                    iconHandle = GetClassLongPtr(hwnd, GCL_HICON);
                if (iconHandle == IntPtr.Zero)
                    iconHandle = GetClassLongPtr(hwnd, GCL_HICONSM);

                if (iconHandle == IntPtr.Zero)
                    return null;

                Icon icn = Icon.FromHandle(iconHandle);

                return icn;
            }
        }
    }