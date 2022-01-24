using System.Windows.Controls;
using System.Windows;

namespace BallScanner.MVVM.Views.Edit
{
    public partial class EditReportsV : UserControl
    {
        public EditReportsV()
        {
            InitializeComponent();

            if (App.CurrentUser == null || App.CurrentUser._access_level == 1)
            {
                DeleteButton.Visibility = Visibility.Visible;
                DeleteButton.IsEnabled = true;
            } else
            {
                DeleteButton.Visibility = Visibility.Collapsed;
                DeleteButton.IsEnabled = false;
            }
        }
    }
}
