using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;

namespace BallScanner.MVVM.ViewModels.Auth
{
    public class RootVM : BaseViewModel
    {
        //private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly LoginVM loginVM = new LoginVM();

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

        public RelayCommand ChangeRootVM_Command { get; set; }

        public RootVM()
        {
            loginVM.ParentViewModel = this;
            SelectedViewModel = loginVM;

            ChangeRootVM_Command = new RelayCommand(ChangeVM);
        }

        private void ChangeVM(object param)
        {
            string parameter = param as string;

            if (parameter == "login")
                SelectedViewModel = loginVM;
        }
    }
}
