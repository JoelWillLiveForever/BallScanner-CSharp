using System.Windows.Controls;

namespace BallScanner.MVVM.Views
{
    public partial class SettingsV : UserControl
    {
        public SettingsV()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (App.Theme == "Dark")
            {
                App.Theme = "Light";
            } else
            {
                App.Theme = "Dark";
            }
        }
    }
}
