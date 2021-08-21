using BallScanner.MVVM.Core;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels
{
    public class ScanVM : PageVM
    {
        public ScanVM()
        {

        }

        public void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Orange/Dark.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Orange/Light.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));

            Properties.Settings.Default.CurrentPalette = (byte)Palettes.Orange;
            Properties.Settings.Default.Save();
        }
    }
}
