using System;
using System.Windows.Controls;

namespace BallScanner.MVVM.Views
{
    public partial class MenuV : UserControl
    {
        private bool isMinState = false;

        public MenuV()
        {
            InitializeComponent();
        }

        private void MyCollapseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            isMinState = !isMinState;
            Resources["IsMinStateForMenu"] = isMinState;

            if (isMinState)
            {
                MyLogo.Visibility = System.Windows.Visibility.Collapsed;
                MyBlockHeader.Visibility = System.Windows.Visibility.Collapsed;
                MyMenuContainer.Width = 40;
            } else
            {
                MyLogo.Visibility = System.Windows.Visibility.Visible;
                MyBlockHeader.Visibility = System.Windows.Visibility.Visible;
                MyMenuContainer.Width = 180;
            }
        }

        private void MyLogoutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
