using BallScanner.MVVM.ViewModels.Main;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BallScanner.MVVM.Views.Main
{
    public partial class MenuV : UserControl
    {
        public MenuV()
        {
            InitializeComponent();
            ChangeMenuState(true);

            if (App.CurrentUser == null)
            {
                AccountManagmentButton.Visibility = Visibility.Visible;
                AccountManagmentButton.IsEnabled = true;
            } else
            {
                switch (App.CurrentUser._access_level)
                {
                    case 0:
                        // user
                        AccountButton.Margin = new Thickness(0,0,0,0);
                        break;
                    case 1:
                        // admin
                        AccountButton.Margin = new Thickness(0, 7, 0, 0);

                        AccountManagmentButton.Visibility = Visibility.Visible;
                        AccountManagmentButton.IsEnabled = true;
                        break;
                }
            }

            switch (Properties.Settings.Default.SelectedPage)
            {
                case 0:
                    if (App.CurrentUser == null)
                        AccountManagmentButton.IsChecked = true;
                    else if (App.CurrentUser._access_level == 1)
                        AccountManagmentButton.IsChecked = true;
                    else
                    {
                        if (MenuVM.MenuButtonClick.CanExecute("Account"))
                            MenuVM.MenuButtonClick.Execute("Account");

                        AccountButton.IsChecked = true;
                    }
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

        private void MyCollapseButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenuState(false);
        }

        private void ChangeMenuState(bool isInit)
        {
            if (!isInit)
            {
                Properties.Settings.Default.IsSmallMenuState = !Properties.Settings.Default.IsSmallMenuState;
                Properties.Settings.Default.Save();
            }

            Resources["IsMinStateForMenu"] = Properties.Settings.Default.IsSmallMenuState;

            if (Properties.Settings.Default.IsSmallMenuState)
            {
                MyLogo.Visibility = Visibility.Collapsed;
                MyBlockHeader.Visibility = Visibility.Collapsed;
                MyMenuContainer.Width = 40.0d;

                MyCollapseButton.Icon = (Geometry)FindResource("Geometry_Icon_ExpandMore");
                App.WriteMsg2Log("Нажатие на пункт меню \"Сжать меню\"", LoggerTypes.INFO);
            }
            else
            {
                MyLogo.Visibility = Visibility.Visible;
                MyBlockHeader.Visibility = Visibility.Visible;
                MyMenuContainer.Width = double.NaN;

                MyCollapseButton.Icon = (Geometry)FindResource("Geometry_Icon_ExpandLess");
                App.WriteMsg2Log("Нажатие на пункт меню \"Расширить меню\"", LoggerTypes.INFO);
            }
        }
    }
}
