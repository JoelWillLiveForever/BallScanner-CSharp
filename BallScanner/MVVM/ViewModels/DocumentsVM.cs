using BallScanner.MVVM.Base;
using NLog;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels
{
    public class DocumentsVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DocumentsVM()
        {
            Log.Info("Constructor called!");
        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;
            app.CurrentPalette = Palettes.Green;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Green/Dark.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Green/Light.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));

            Properties.Settings.Default.Save();
        }
    }
}
