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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
            {
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
            }
            else
            {
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
            }

            Properties.Settings.Default.IsDarkTheme = !Properties.Settings.Default.IsDarkTheme;
            Properties.Settings.Default.Save();
        }
    }
}
