using BallScanner.Data.Tables;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace BallScanner
{
    public enum Palettes
    {
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple
    }

    public partial class App : Application
    {
        // Auth
        public static User CurrentUser { get; set; }

        private const string LANGUAGE_URI = "Resources/Languages/Language.";
        public Palettes CurrentPalette = Palettes.Red;

        // Palette & Theme
        public ResourceDictionary ThemeDictionary
        {
            get => Resources.MergedDictionaries[0];
        }

        public void ChangeTheme(Uri paletteUri, Uri themeUri)
        {
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = paletteUri });
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

            if (!BallScanner.Properties.Settings.Default.IsDarkTheme)
            {
                var app = (App)Current;
                app.ChangeTheme(new Uri("Resources/Palettes/Red/Light.xaml", UriKind.Relative),
                            new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));
            }
        }

        private void OnLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
