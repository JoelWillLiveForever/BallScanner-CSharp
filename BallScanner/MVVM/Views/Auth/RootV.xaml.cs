//using System;
//using System.Diagnostics;
//using System.Drawing;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using BallScanner.MVVM.ViewModels.Auth;
//using System.Windows.Forms;
//using System.Windows.Interop;
//using System.Windows.Shell;
//using Microsoft.Win32;

namespace BallScanner.MVVM.Views.Auth
{
    public partial class RootV : Window
    {
        //private string currentDisplayName;
        //private WindowChrome MyWindowChrome;
        //private static double prevDpiX = 0, prevDpiY = 0;

        public RootV()
        {
            InitializeComponent();
            DataContext = new RootVM();
            //SizeChanged += OnWindowSizeChanged;

            SourceInitialized += (s, e) =>
            {
                IntPtr handle = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };

            #region Old
            //MyWindowChrome = WindowChrome.GetWindowChrome(this);

            //Screen screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);
            //ChangeMaxWidthAndMaxHeight(screen);

            //currentDisplayName = screen.DeviceName;

            //// Handlers
            //SystemEvents.DisplaySettingsChanged += new EventHandler(OnDisplaySettingsChanged);
            //LocationChanged += new EventHandler(OnLocationChanged);

            //AddHandler(MouseMoveEvent, new System.Windows.Input.MouseEventHandler(OnMouseMove));
            #endregion
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
        }

        //private static void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    var window = (Window)sender;
        //    var content = (FrameworkElement)window.Content;

        //    window.MinWidth = window.ActualWidth - content.ActualWidth + content.MinWidth;
        //    window.MaxWidth = window.ActualWidth - content.ActualWidth + content.MaxWidth;

        //    window.MinHeight = window.ActualHeight - content.ActualHeight + content.MinHeight;
        //    window.MaxHeight = window.ActualHeight - content.ActualHeight + content.MaxHeight;

        //    window.SizeChanged -= OnWindowSizeChanged;
        //}

        [StructLayout(LayoutKind.Sequential)]
        public struct MyPoint
        {
            public int x, y;

            public MyPoint(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MinMaxInfo
        {
            public MyPoint pointReserved;
            public MyPoint pointMaxSize;
            public MyPoint pointMaxPosition;
            public MyPoint pointMinTrackSize;
            public MyPoint pointMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct MyRect
        {
            public static readonly MyRect Empty = new MyRect();

            public int left;
            public int top;
            public int right;
            public int bottom;

            public int Width
            {
                get => Math.Abs(right - left); // Why abs?
            }

            public int Height
            {
                get => Math.Abs(bottom - top); // Why abs?
            }

            public MyRect(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public MyRect(MyRect rectangle)
            {
                left = rectangle.left;
                top = rectangle.top;
                right = rectangle.right;
                bottom = rectangle.bottom;
            }

            public bool isEmpty
            {
                get => left >= right || top >= bottom;
            }

            public override string ToString()
            {
                if (this == Empty)
                    return "RECT { Empty }";
                return "RECT { left: " + left + " / top: " + top + " / right: " + right + " / bottom: " + bottom + " }";
            }

            public override bool Equals(object obj)
            {
                if (!(obj is MyRect)) return false;
                return this == (MyRect)obj;
            }

            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }

            public static bool operator == (MyRect rect1, MyRect rect2) => rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
            public static bool operator != (MyRect rect1, MyRect rect2) => !(rect1 == rect2);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MonitorInfo
        {
            public int cbSize = Marshal.SizeOf(typeof(MonitorInfo)); // Размер типа "MonitorInfo" в байтах в неуправляемой памяти

            public MyRect rcMonitor = new MyRect(); // Размер монитора
            public MyRect rcWork = new MyRect(); // Размер рабочей области экрана

            public int dwFlags = 0; // Какие-то флаги
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInfo lpmi);

        [DllImport("user32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        public static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MinMaxInfo mmi = (MinMaxInfo)Marshal.PtrToStructure(lParam, typeof(MinMaxInfo));
            int monitor_defaultToNearest = 0x00000002;

            IntPtr monitor = MonitorFromWindow(hwnd, monitor_defaultToNearest); // get monitor

            if (monitor != IntPtr.Zero)
            {
                MonitorInfo monitorInfo = new MonitorInfo();
                GetMonitorInfo(monitor, monitorInfo);

                MyRect workArea = monitorInfo.rcWork;
                MyRect monitorArea = monitorInfo.rcMonitor;

                mmi.pointMaxPosition.x = Math.Abs(workArea.left - monitorArea.left);
                mmi.pointMaxPosition.y = Math.Abs(workArea.top = monitorArea.top);

                mmi.pointMaxSize.x = Math.Abs(workArea.right - workArea.left);
                mmi.pointMaxSize.y = Math.Abs(workArea.bottom - workArea.top);

                //Console.WriteLine("PointMaxSize.X = " + mmi.pointMaxSize.x);
                //Console.WriteLine("PointMaxSize.Y = " + mmi.pointMaxSize.y);

                var main = Application.Current.MainWindow;
                PresentationSource source = PresentationSource.FromVisual(main);

                double scaleX, scaleY;
                if (source != null)
                {
                    scaleX = source.CompositionTarget.TransformToDevice.M11;
                    scaleY = source.CompositionTarget.TransformToDevice.M22;

                    mmi.pointMinTrackSize.x = (int)(main.MinWidth * scaleX);
                    mmi.pointMinTrackSize.y = (int)(main.MinHeight * scaleY);
                }
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        public static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }

        #region Old
        //private void OnLocationChanged(object sender, EventArgs e)
        //{
        //    Screen screen = Screen.FromHandle((new WindowInteropHelper(this)).Handle);
        //    if (currentDisplayName == screen.DeviceName)
        //    {
        //        return;
        //    }

        //    ChangeMaxWidthAndMaxHeight(screen);
        //    currentDisplayName = screen.DeviceName;
        //}

        //private void OnDisplaySettingsChanged(object sender, EventArgs e)
        //{
        //    Screen screen = Screen.FromHandle((new WindowInteropHelper(this)).Handle);
        //    ChangeMaxWidthAndMaxHeight(screen);

        //    // Refresh window state
        //    if (WindowState == WindowState.Maximized)
        //    {
        //        WindowState = WindowState.Normal;
        //        WindowState = WindowState.Maximized;
        //    }
        //}

        //private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    Screen screen = Screen.FromHandle((new WindowInteropHelper(this)).Handle);
        //    if (currentDisplayName == screen.DeviceName)
        //    {
        //        return;
        //    }

        //    ChangeMaxWidthAndMaxHeight(screen);
        //    currentDisplayName = screen.DeviceName;
        //}

        //private void ChangeMaxWidthAndMaxHeight(Screen screen)
        //{
        //    Graphics g = Graphics.FromHwnd(IntPtr.Zero);

        //    double scaleFactorX = g.DpiX / 96.0d;
        //    double scaleFactorY = g.DpiY / 96.0d;

        //    MaxHeight = screen.WorkingArea.Height / scaleFactorY
        //        + MyWindowChrome.ResizeBorderThickness.Top
        //        + MyWindowChrome.ResizeBorderThickness.Bottom;

        //    MaxWidth = screen.WorkingArea.Width / scaleFactorX
        //        + MyWindowChrome.ResizeBorderThickness.Left
        //        + MyWindowChrome.ResizeBorderThickness.Right;

        //    Debug.WriteLine("X: DPI = " + g.DpiX + " Screen Width = " + screen.WorkingArea.Width + " WPF Width = " + MaxWidth);
        //    Debug.WriteLine("Y: DPI = " + g.DpiY + " Screen Height = " + screen.WorkingArea.Height + " WPF Height = " + MaxHeight);
        //}

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //}

        //public T GetRequiredTemplateChild<T>(string childName) where T : DependencyObject
        //{
        //    return (T)base.GetTemplateChild(childName);
        //}

        #endregion
    }
}
