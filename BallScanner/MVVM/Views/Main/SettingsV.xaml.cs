using System;
using System.Windows;
using System.Windows.Controls;

namespace BallScanner.MVVM.Views.Main
{
    public partial class SettingsV : UserControl
    {
        public SettingsV()
        {
            InitializeComponent();

            if (Properties.Settings.Default.IsDarkTheme) Button_DarkTheme.IsChecked = true;
            else Button_LightTheme.IsChecked = true;

            if (Properties.Settings.Default.Language.Name == "ru-RU") Button_LanguageRussian.IsChecked = true;
            else Button_LanguageEnglish.IsChecked = true;
        }

        private void Button_DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.IsDarkTheme) return;

            var app = (App)Application.Current;
            switch (Properties.Settings.Default.SelectedPage)
            {
                case 0:
                    app.ChangeTheme(new Uri("Resources/Palettes/Dark/Pink.xaml", UriKind.Relative));
                    break;
                case 1:
                    app.ChangeTheme(new Uri("Resources/Palettes/Dark/Red.xaml", UriKind.Relative));
                    break;
                case 2:
                    app.ChangeTheme(new Uri("Resources/Palettes/Dark/Orange.xaml", UriKind.Relative));
                    break;
                case 3:
                    app.ChangeTheme(new Uri("Resources/Palettes/Dark/Yellow.xaml", UriKind.Relative));
                    break;
                case 4:
                    app.ChangeTheme(new Uri("Resources/Palettes/Dark/Green.xaml", UriKind.Relative));
                    break;
                case 5:
                    app.ChangeTheme(new Uri("Resources/Palettes/Dark/Blue.xaml", UriKind.Relative));
                    break;
                case 6:
                    app.ChangeTheme(new Uri("Resources/Palettes/Dark/Purple.xaml", UriKind.Relative));
                    break;
            }

            Properties.Settings.Default.IsDarkTheme = true;
            Properties.Settings.Default.Save();

            App.WriteMsg2Log("Переключение темы приложения на \"Тёмная\"", LoggerTypes.INFO);
        }

        private void Button_LightTheme_Click(object sender, RoutedEventArgs e)
        {
            if (!Properties.Settings.Default.IsDarkTheme) return;

            var app = (App)Application.Current;
            switch (Properties.Settings.Default.SelectedPage)
            {
                case 0:
                    app.ChangeTheme(new Uri("Resources/Palettes/Light/Pink.xaml", UriKind.Relative));
                    break;
                case 1:
                    app.ChangeTheme(new Uri("Resources/Palettes/Light/Red.xaml", UriKind.Relative));
                    break;
                case 2:
                    app.ChangeTheme(new Uri("Resources/Palettes/Light/Orange.xaml", UriKind.Relative));
                    break;
                case 3:
                    app.ChangeTheme(new Uri("Resources/Palettes/Light/Yellow.xaml", UriKind.Relative));
                    break;
                case 4:
                    app.ChangeTheme(new Uri("Resources/Palettes/Light/Green.xaml", UriKind.Relative));
                    break;
                case 5:
                    app.ChangeTheme(new Uri("Resources/Palettes/Light/Blue.xaml", UriKind.Relative));
                    break;
                case 6:
                    app.ChangeTheme(new Uri("Resources/Palettes/Light/Purple.xaml", UriKind.Relative));
                    break;
            }

            Properties.Settings.Default.IsDarkTheme = false;
            Properties.Settings.Default.Save();

            App.WriteMsg2Log("Переключение темы приложения на \"Светлая\"", LoggerTypes.INFO);
        }

        private void Button_LanguageRussian_Click(object sender, RoutedEventArgs e)
        {
            App.Language = new System.Globalization.CultureInfo("ru-RU");
            App.WriteMsg2Log("Переключение языка приложения на \"Русский, Россия\"", LoggerTypes.INFO);
        }

        private void Button_LanguageEnglish_Click(object sender, RoutedEventArgs e)
        {
            App.Language = new System.Globalization.CultureInfo("en-US");
            App.WriteMsg2Log("Переключение языка приложения на \"Английский, США\"", LoggerTypes.INFO);
        }
    }
}
