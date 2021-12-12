using BallScanner.MVVM.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace BallScanner.MVVM.Views
{
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
            DataContext = new TestVM();

            Task.Run(() =>
            {
                RootV newWindow = new RootV();
                newWindow.Show();
            });
        }

        private void Custom_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
        }
    }
}
