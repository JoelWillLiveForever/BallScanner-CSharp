using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Shell;
using Microsoft.Win32;

namespace BallScanner.MVVM.Views
{
    public partial class MainV : Window
    {
        private string currentDisplayName;
        private WindowChrome MyWindowChrome;

        public MainV()
        {
            InitializeComponent();

            MyWindowChrome = WindowChrome.GetWindowChrome(this);

            Screen screen = Screen.FromHandle((new WindowInteropHelper(this)).Handle);
            ChangeMaxWidthAndMaxHeight(screen);

            currentDisplayName = screen.DeviceName;

            // Handlers
            SystemEvents.DisplaySettingsChanged += new EventHandler(OnDisplaySettingsChanged);
            AddHandler(MouseMoveEvent, new System.Windows.Input.MouseEventHandler(OnMouseMove));
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            Screen screen = Screen.FromHandle((new WindowInteropHelper(this)).Handle);
            ChangeMaxWidthAndMaxHeight(screen);

            // Refresh window state
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                WindowState = WindowState.Maximized;
            }
        }

        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Screen screen = Screen.FromHandle((new WindowInteropHelper(this)).Handle);
            if (currentDisplayName == screen.DeviceName)
            {
                return;
            }

            ChangeMaxWidthAndMaxHeight(screen);
            currentDisplayName = screen.DeviceName;
        }

        private void ChangeMaxWidthAndMaxHeight(Screen screen)
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);

            double scaleFactorX = g.DpiX / 96.0d;
            double scaleFactorY = g.DpiY / 96.0d;

            MaxHeight = screen.WorkingArea.Height / scaleFactorY
                + MyWindowChrome.ResizeBorderThickness.Top
                + MyWindowChrome.ResizeBorderThickness.Bottom;

            MaxWidth = screen.WorkingArea.Width / scaleFactorX
                + MyWindowChrome.ResizeBorderThickness.Left
                + MyWindowChrome.ResizeBorderThickness.Right;
        }

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //}

        //public T GetRequiredTemplateChild<T>(string childName) where T : DependencyObject
        //{
        //    return (T)base.GetTemplateChild(childName);
        //}
    }
}
