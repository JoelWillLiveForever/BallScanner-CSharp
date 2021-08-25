using BallScanner.MVVM.Core;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels
{
    public class AccountVM : PageVM
    {
        public AccountVM()
        {

        }

        public void ChangePalette()
        {
            var app = (App)Application.Current;
            app.CurrentPalette = Palettes.Red;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Red/Dark.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Red/Light.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));

            Properties.Settings.Default.Save();
        }
    }
}
