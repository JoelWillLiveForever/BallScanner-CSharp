using BallScanner.MVVM.Base;
using NLog;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class AccountVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public string Login
        {
            get => App.CurrentUser._username;
        }

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
            get
            {
                // superuser
                if (App.CurrentUser == null)
                    return "Суперпользователь";

                switch (App.CurrentUser._access_level)
                {
                    case 0: return "Пользователь";  // user
                    case 1: return "Администратор"; // admin
                }

                return "Не определён";
            }
        }

        public AccountVM()
        {
            Log.Info("Constructor called!");
        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Dark/Red.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Light/Red.xaml", UriKind.Relative));

            Properties.Settings.Default.SelectedPage = 1;
            Properties.Settings.Default.Save();
        }
    }
}
