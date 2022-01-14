using BallScanner.MVVM.Base;
using NLog;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class SettingsVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public SettingsVM()
        {
            Log.Info("Constructor called!");
        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Dark/Blue.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Light/Blue.xaml", UriKind.Relative));

            Properties.Settings.Default.SelectedPage = 5;
            Properties.Settings.Default.Save();
        }
    }
}
