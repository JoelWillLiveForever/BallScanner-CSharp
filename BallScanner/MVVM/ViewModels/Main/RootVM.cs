using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class RootVM : BaseViewModel
    {
        private static readonly MenuVM menuVM = new MenuVM();

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
            menuVM.ParentViewModel = this;

            SelectedViewModel = menuVM;
            ChangeRootVM_Command = new RelayCommand(ChangeVM);
        }

        private void ChangeVM(object param)
        {
            string parameter = param as string;

            if (parameter == "menu")
                SelectedViewModel = menuVM;
        }
    }
}
