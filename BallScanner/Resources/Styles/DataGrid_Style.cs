using BallScanner.MVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BallScanner.Resources.Styles
{
    partial class DataGrid_Style
    {
        public DataGrid_Style()
        {
            InitializeComponent();
        }

        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            MenuVM.calibrateVM.ChangeImage(row.Item);
            // Console.WriteLine("Click!");
        }
    }
}
