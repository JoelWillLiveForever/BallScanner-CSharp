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

            if (isMinState)
            {
                MyLogo.Visibility = System.Windows.Visibility.Collapsed;
            } else
            {
                MyLogo.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
