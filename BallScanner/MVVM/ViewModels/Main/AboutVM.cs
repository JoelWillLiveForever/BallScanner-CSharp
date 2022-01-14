using BallScanner.MVVM.Base;
using NLog;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class AboutVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public AboutVM()
        {
            Log.Info("Constructor called!");
        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Dark/Purple.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Light/Purple.xaml", UriKind.Relative));

            Properties.Settings.Default.SelectedPage = 6;
            Properties.Settings.Default.Save();
        }
    }
}
