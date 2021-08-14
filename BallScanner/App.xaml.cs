using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace BallScanner
{
    public partial class App : Application
    {
        private const string LANGUAGE_URI = "Resources/Languages/Language.";

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }

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
                BallScanner.Properties.Settings.Default.DefaultLanguage = Language;
                BallScanner.Properties.Settings.Default.Save();
            }
        }

        public App()
        {
            InitializeComponent();

            // Load last saved app language
            Language = BallScanner.Properties.Settings.Default.DefaultLanguage;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {

        }

        private void OnLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
