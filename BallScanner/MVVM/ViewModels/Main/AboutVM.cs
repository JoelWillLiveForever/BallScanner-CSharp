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
