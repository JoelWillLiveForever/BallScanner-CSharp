using BallScanner.MVVM.Base;
using NLog;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class AccountVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public string Surname
        {
            get => App.CurrentUser._surname;
        }

        public string Name
        {
            get => App.CurrentUser._name;
        }

        public string Lastname
        {
            get => App.CurrentUser._lastname;
        }

        public int Smena_Number
        {
            get => App.CurrentUser._smena_number;
        }

        public string Access_Level
        {
            get => App.CurrentUser._is_admin == 0 ? "Пользователь" : "Администратор";
        }

        public AccountVM()
        {
            Log.Info("Constructor called!");
        }

        public override void ChangePalette()
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
