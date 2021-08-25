using BallScanner.MVVM.Core;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels
{
    public class AboutVM : PageVM
    {
        public AboutVM()
        {

        }

        public void ChangePalette()
        {
            var app = (App)Application.Current;
            app.CurrentPalette = Palettes.Purple;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Purple/Dark.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Purple/Light.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));

            Properties.Settings.Default.Save();
        }
    }
}
