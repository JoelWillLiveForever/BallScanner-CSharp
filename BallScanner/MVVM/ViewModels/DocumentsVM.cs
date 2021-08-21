using BallScanner.MVVM.Core;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels
{
    public class DocumentsVM : PageVM
    {
        public const string PALETTE = "Green";

        public DocumentsVM()
        {

        }

        public void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Green/Dark.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Green/Light.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));

            Properties.Settings.Default.CurrentPalette = (byte)Palettes.Green;
            Properties.Settings.Default.Save();
        }
    }
}
