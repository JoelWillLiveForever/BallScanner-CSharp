using BallScanner.MVVM.Base;
using BallScanner.MVVM.ViewModels.Auth;
using NLog;

namespace BallScanner.MVVM.ViewModels
{
    public class TestVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly LoginVM loginVM = new LoginVM();
        private static readonly RegistrationVM registrationVM = new RegistrationVM();

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

        public TestVM()
        {
            Log.Info("Constructor called!");
            SelectedViewModel = loginVM;
        }
    }
}
