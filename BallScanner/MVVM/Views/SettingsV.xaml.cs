using System;
using System.Windows;
using System.Windows.Controls;

namespace BallScanner.MVVM.Views
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
            switch ((Palettes)Properties.Settings.Default.CurrentPalette)
            {
                case Palettes.Red:
                    app.ChangeTheme(new Uri("Resources/Palettes/Red/Dark.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
                    break;
                case Palettes.Orange:
                    app.ChangeTheme(new Uri("Resources/Palettes/Orange/Dark.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
                    break;
                case Palettes.Yellow:
                    app.ChangeTheme(new Uri("Resources/Palettes/Yellow/Dark.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
                    break;
                case Palettes.Green:
                    app.ChangeTheme(new Uri("Resources/Palettes/Green/Dark.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
                    break;
                case Palettes.Blue:
                    app.ChangeTheme(new Uri("Resources/Palettes/Blue/Dark.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
                    break;
                case Palettes.Purple:
                    app.ChangeTheme(new Uri("Resources/Palettes/Purple/Dark.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
                    break;
            }

            Properties.Settings.Default.IsDarkTheme = true;
            Properties.Settings.Default.Save();
        }

        private void Button_LightTheme_Click(object sender, RoutedEventArgs e)
        {
            if (!Properties.Settings.Default.IsDarkTheme) return;

            var app = (App)Application.Current;
            switch ((Palettes)Properties.Settings.Default.CurrentPalette)
            {
                case Palettes.Red:
                    app.ChangeTheme(new Uri("Resources/Palettes/Red/Light.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));
                    break;
                case Palettes.Orange:
                    app.ChangeTheme(new Uri("Resources/Palettes/Orange/Light.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));
                    break;
                case Palettes.Yellow:
                    app.ChangeTheme(new Uri("Resources/Palettes/Yellow/Light.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));
                    break;
                case Palettes.Green:
                    app.ChangeTheme(new Uri("Resources/Palettes/Green/Light.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));
                    break;
                case Palettes.Blue:
                    app.ChangeTheme(new Uri("Resources/Palettes/Blue/Light.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));
                    break;
                case Palettes.Purple:
                    app.ChangeTheme(new Uri("Resources/Palettes/Purple/Light.xaml", UriKind.Relative),
                        new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));
                    break;
            }

            Properties.Settings.Default.IsDarkTheme = false;
            Properties.Settings.Default.Save();
        }

        private void Button_LanguageRussian_Click(object sender, RoutedEventArgs e)
        {
            App.Language = new System.Globalization.CultureInfo("ru-RU");
        }

        private void Button_LanguageEnglish_Click(object sender, RoutedEventArgs e)
        {
            App.Language = new System.Globalization.CultureInfo("en-US");
        }
    }
}
