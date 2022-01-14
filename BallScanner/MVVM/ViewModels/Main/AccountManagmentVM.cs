using BallScanner.MVVM.Base;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class AccountManagmentVM : PageVM
    {



        public override void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Dark/Pink.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Light/Pink.xaml", UriKind.Relative));

            Properties.Settings.Default.SelectedPage = 0;
            Properties.Settings.Default.Save();
        }
    }
}
