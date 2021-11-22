using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using NLog;

namespace BallScanner.MVVM.ViewModels.Auth
{
    public class RootVM : BaseViewModel
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

        public static RelayCommand ChangeSelectedVM_Command { get; set; }

        public RootVM()
        {
            Log.Info("Constructor called!");
            SelectedViewModel = loginVM;

            ChangeSelectedVM_Command = new RelayCommand(ChangeSelectedVM);
        }

        private void ChangeSelectedVM(object param)
        {
            string name = param as string;

            if (name == "Registration")
            {
                SelectedViewModel = registrationVM;
            } else if (name == "Login")
            {
                SelectedViewModel = loginVM;
            }
        }
    }
}
