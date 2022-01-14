using System.Windows.Controls;
using System.Windows.Media;

namespace BallScanner.MVVM.Views.Main
{
    public partial class MenuV : UserControl
    {
        private bool isMinState = false;

        public MenuV()
        {
            InitializeComponent();

            switch (Properties.Settings.Default.SelectedPage)
            {
                case 0:
                    AccountManagmentButton.IsChecked = true;
                    break;
                case 1:
                    AccountButton.IsChecked = true;
                    break;
                case 2:
                    ScanButton.IsChecked= true;
                    break;
                case 3:
                    CalibrateButton.IsChecked= true;
                    break;
                case 4:
                    DocumentsButton.IsChecked= true;
                    break;
                case 5:
                    SettingsButton.IsChecked= true;
                    break;
                case 6:
                    AboutButton.IsChecked= true;
                    break;
            }
        }

        private void MyCollapseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            isMinState = !isMinState;
            Resources["IsMinStateForMenu"] = isMinState;

            if (isMinState)
            {
                MyLogo.Visibility = System.Windows.Visibility.Collapsed;
                MyBlockHeader.Visibility = System.Windows.Visibility.Collapsed;
                MyMenuContainer.Width = 40.0d;

                MyCollapseButton.Icon = (Geometry) FindResource("Geometry_Icon_ExpandMore");
            } else
            {
                MyLogo.Visibility = System.Windows.Visibility.Visible;
                MyBlockHeader.Visibility = System.Windows.Visibility.Visible;
                MyMenuContainer.Width = double.NaN;

                MyCollapseButton.Icon = (Geometry) FindResource("Geometry_Icon_ExpandLess");
            }
        }
    }
}
