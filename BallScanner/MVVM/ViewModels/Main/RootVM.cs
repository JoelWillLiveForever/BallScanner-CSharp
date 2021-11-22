using BallScanner.MVVM.Base;
using NLog;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class RootVM : BaseViewModel
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

        public RootVM()
        {
            Log.Info("Constructor called!");
            SelectedViewModel = new MenuVM();
        }
    }
}
