//using System;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Shell;

namespace BallScanner.Resources.Styles
{
    partial class CustomWindowStyle
    {
        public CustomWindowStyle()
        {
            InitializeComponent();
        }

        private void OnMaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            //WindowChrome windowChrome = WindowChrome.GetWindowChrome(window);

            //var template = window.Template;
            //Border border = (Border)template.FindName("RootBorder", window);

            window.WindowState = window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            //window.Padding = new Thickness(0, 0, 0, 0);
            //window.Margin = new Thickness(0, 0, 0, 0);
            //Console.WriteLine(border.BorderThickness);
            //border.BorderThickness = new Thickness(6, 7, 7, 6);
            //Console.WriteLine(border.BorderThickness);
        }

        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            window.WindowState = WindowState.Minimized;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            window.Close();
        }
    }
}
