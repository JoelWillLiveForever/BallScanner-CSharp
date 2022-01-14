using BallScanner.Data.Tables;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace BallScanner
{
    //public enum Palettes
    //{
    //    Pink,
    //    Red,
    //    Orange,
    //    Yellow,
    //    Green,
    //    Blue,
    //    Purple
    //}

    public partial class App : Application
    {
        // Auth
        public static User CurrentUser { get; set; }

        private const string LANGUAGE_URI = "Resources/Languages/Language.";

        // Palette & Theme
        public ResourceDictionary ThemeDictionary
        {
            get => Resources.MergedDictionaries[0];
        }

        public void ChangeTheme(Uri themeUri)
        {
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = themeUri });
        }

        public static CultureInfo Language
        {
            get => System.Threading.Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                //1. Change app language
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                //2. Create new ResourceDictionary for new culture
                ResourceDictionary newDictionary = new ResourceDictionary();
                newDictionary.Source = new Uri(LANGUAGE_URI + string.Format("{0}.xaml", value.Name), UriKind.Relative);

                //3. Find old resource dictionary and delete it. After that add a new resource dictionary to app.xaml
                ResourceDictionary oldDictionary = (from d in Current.Resources.MergedDictionaries
                                                    where d.Source != null && d.Source.OriginalString.StartsWith(LANGUAGE_URI)
                                                    select d).First();

                if (oldDictionary == null)
                {
                    Current.Resources.MergedDictionaries.Add(newDictionary);
                }
                else
                {
                    int resourcePosition = Current.Resources.MergedDictionaries.IndexOf(oldDictionary);

                    Current.Resources.MergedDictionaries.Remove(oldDictionary);
                    Current.Resources.MergedDictionaries.Insert(resourcePosition, newDictionary);
                }

                //4. Save new app language 
                BallScanner.Properties.Settings.Default.Language = Language;
                BallScanner.Properties.Settings.Default.Save();
            }
        }

        public App()
        {
            InitializeComponent();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Language = BallScanner.Properties.Settings.Default.Language;

            var app = (App)Current;
            if (BallScanner.Properties.Settings.Default.IsDarkTheme)
                switch (BallScanner.Properties.Settings.Default.SelectedPage)
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
            else
                switch (BallScanner.Properties.Settings.Default.SelectedPage)
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
        }

        private void OnLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
