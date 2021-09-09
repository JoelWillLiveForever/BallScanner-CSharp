using BallScanner.MVVM.Base;
using NLog;

namespace BallScanner.MVVM.ViewModels
{
    public class MainVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public MainVM()
        {
            Log.Info("Constructor called!");
            SelectedViewModel = new MenuVM();
        }
    }
}
