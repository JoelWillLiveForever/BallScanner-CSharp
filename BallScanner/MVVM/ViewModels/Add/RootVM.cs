using BallScanner.MVVM.Base;
using NLog;

namespace BallScanner.MVVM.ViewModels.Add
{
    public class RootVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

        public RootVM()
        {
            registrationVM.ParentViewModel = this;
            SelectedViewModel = registrationVM;
        }
    }
}
