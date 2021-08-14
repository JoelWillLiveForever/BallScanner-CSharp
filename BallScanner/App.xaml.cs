using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace BallScanner
{
    public partial class App : Application
    {
        private const string LANGUAGE_URI = "Resources/Languages/Language.";
        private const string PALETTE_AND_THEMES_URI = "Resources/Palettes/";

        private static string _palette = BallScanner.Properties.Settings.Default.Palette;
        public static string Palette
        {
            get => _palette;
            set
            {
                if (value == null) throw new ArgumentException("value");
                if (value == _palette) return;

                _palette = value;

                ChangePaletteAndTheme();

                BallScanner.Properties.Settings.Default.Palette = _palette;
                BallScanner.Properties.Settings.Default.Save();
            }
        }

        private static string _theme = BallScanner.Properties.Settings.Default.Theme;
        public static string Theme
        {
            get => _theme;
            set
            {
                if (value == null) throw new ArgumentException("value");
                if (value == _theme) return;

                _theme = value;

                ChangePaletteAndTheme();

                BallScanner.Properties.Settings.Default.Theme = _theme;
                BallScanner.Properties.Settings.Default.Save();
            }
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

            Language = BallScanner.Properties.Settings.Default.Language;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            // nothing
        }

        private void OnLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // nothing
        }

        private static void ChangePaletteAndTheme()
        {
            ResourceDictionary newGenericPaletteDictionary = new ResourceDictionary();
            ResourceDictionary newPaletteDictionary = new ResourceDictionary();

            newGenericPaletteDictionary.Source = new Uri(PALETTE_AND_THEMES_URI + string.Format("Generic/{0}.xaml", _theme), UriKind.Relative);
            newPaletteDictionary.Source = new Uri(PALETTE_AND_THEMES_URI + string.Format("{0}/{1}.xaml", _palette, _theme), UriKind.Relative);

            //3. Find old resource dictionary and delete it. After that add a new resource dictionary to app.xaml
            ResourceDictionary[] oldDictionaries = (from d in Current.Resources.MergedDictionaries
                                                    where d.Source != null && d.Source.OriginalString.StartsWith(PALETTE_AND_THEMES_URI)
                                                    select d).ToArray();

            if (oldDictionaries == null)
            {
                Current.Resources.MergedDictionaries.Add(newGenericPaletteDictionary);
                Current.Resources.MergedDictionaries.Add(newPaletteDictionary);
            }
            else
            {
                int resourcePosition = Current.Resources.MergedDictionaries.IndexOf(oldDictionaries[0]);

                Current.Resources.MergedDictionaries.Remove(oldDictionaries[0]);
                Current.Resources.MergedDictionaries.Insert(resourcePosition, newGenericPaletteDictionary);

                Current.Resources.MergedDictionaries.Remove(oldDictionaries[1]);
                Current.Resources.MergedDictionaries.Insert(++resourcePosition, newPaletteDictionary);
            }
        }
    }
}
