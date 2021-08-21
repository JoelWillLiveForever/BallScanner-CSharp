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
        //private Palettes _CurrentPalette = (Palettes)BallScanner.Properties.Settings.Default.CurrentPalette;
        //private bool _IsDarkTheme = BallScanner.Properties.Settings.Default.IsDarkTheme;

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

        //public void ChangeTheme(Palettes newPalette, bool isDarkTheme)
        //{
        //    if (_CurrentPalette == newPalette && _IsDarkTheme == isDarkTheme) return;

        //    if (_CurrentPalette != newPalette)
        //    {
        //        string uri = string.Format("Resources/Palettes/{0}/{1}.xaml", newPalette.ToString(), isDarkTheme ? "Dark" : "Light");

        //        ThemeDictionary.MergedDictionaries.RemoveAt(0);
        //        ThemeDictionary.MergedDictionaries.Insert(0, new ResourceDictionary() { Source = new Uri(uri, UriKind.Relative) });

        //        _CurrentPalette = newPalette;
        //        BallScanner.Properties.Settings.Default.CurrentPalette = (byte)_CurrentPalette;
        //    }
            
        //    if (_IsDarkTheme != isDarkTheme)
        //    {
        //        string uri = string.Format("Resources/Palettes/{0}.xaml", isDarkTheme ? "Dark" : "Light");

        //        ThemeDictionary.MergedDictionaries.RemoveAt(1);
        //        ThemeDictionary.MergedDictionaries.Insert(1, new ResourceDictionary() { Source = new Uri(uri, UriKind.Relative) });

        //        _IsDarkTheme = isDarkTheme;
        //        BallScanner.Properties.Settings.Default.IsDarkTheme = _IsDarkTheme;
        //    }

        //    BallScanner.Properties.Settings.Default.Save();
        //}

        private const string LANGUAGE_URI = "Resources/Languages/Language.";
        //private const string PALETTE_AND_THEMES_URI = "Resources/Palettes/";

        //private static string _palette = BallScanner.Properties.Settings.Default.PaletteNumber;
        //public static string Palette
        //{
        //    get => _palette;
        //    set
        //    {
        //        if (value == null) throw new ArgumentException("value");
        //        if (value == _palette) return;

        //        _palette = value;

        //        ChangePaletteAndTheme();

        //        BallScanner.Properties.Settings.Default.PaletteNumber = _palette;
        //        BallScanner.Properties.Settings.Default.Save();
        //    }
        //}

        //private static string _theme = BallScanner.Properties.Settings.Default.IsDarkTheme;
        //public static string Theme
        //{
        //    get => _theme;
        //    set
        //    {
        //        if (value == null) throw new ArgumentException("value");
        //        if (value == _theme) return;

        //        _theme = value;

        //        ChangePaletteAndTheme();

        //        BallScanner.Properties.Settings.Default.IsDarkTheme = _theme;
        //        BallScanner.Properties.Settings.Default.Save();
        //    }
        //}

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

            //if (!BallScanner.Properties.Settings.Default.IsDarkTheme)
            //{
            //    ChangeTheme(new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));
            //    ChangePalette(new Uri("Resources/Palettes/Red/Light.xaml", UriKind.Relative));
            //}

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

        //private static void ChangePaletteAndTheme()
        //{
        //    ResourceDictionary newGenericPaletteDictionary = new ResourceDictionary();
        //    ResourceDictionary newPaletteDictionary = new ResourceDictionary();

        //    newGenericPaletteDictionary.Source = new Uri(PALETTE_AND_THEMES_URI + string.Format("{0}.xaml", _theme), UriKind.Relative);
        //    newPaletteDictionary.Source = new Uri(PALETTE_AND_THEMES_URI + string.Format("{0}/{1}.xaml", _palette, _theme), UriKind.Relative);

        //    //3. Find old resource dictionary and delete it. After that add a new resource dictionary to app.xaml
        //    ResourceDictionary[] oldDictionaries = (from d in Current.Resources.MergedDictionaries
        //                                            where d.Source != null && d.Source.OriginalString.StartsWith(PALETTE_AND_THEMES_URI)
        //                                            select d).ToArray();

        //    if (oldDictionaries == null)
        //    {
        //        Current.Resources.MergedDictionaries.Add(newGenericPaletteDictionary);
        //        Current.Resources.MergedDictionaries.Add(newPaletteDictionary);
        //    }
        //    else
        //    {
        //        int resourcePosition = Current.Resources.MergedDictionaries.IndexOf(oldDictionaries[0]);

        //        Current.Resources.MergedDictionaries.Remove(oldDictionaries[0]);
        //        Current.Resources.MergedDictionaries.Insert(resourcePosition, newGenericPaletteDictionary);

        //        Current.Resources.MergedDictionaries.Remove(oldDictionaries[1]);
        //        Current.Resources.MergedDictionaries.Insert(++resourcePosition, newPaletteDictionary);
        //    }
        //}
    }
}
