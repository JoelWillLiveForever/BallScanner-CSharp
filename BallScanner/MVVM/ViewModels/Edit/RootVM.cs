using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Edit
{
    public class RootVM : BaseViewModel
    {
        private static EditReportsVM editReportsVM;
        private static EditUsersVM editUsersVM;

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
            editReportsVM = new EditReportsVM();
            editUsersVM = new EditUsersVM();

            editReportsVM.ParentViewModel = this;
            editUsersVM.ParentViewModel = this;

            SelectedViewModel = editReportsVM;
            ChangeRootVM_Command = new RelayCommand(ChangeVM);
        }

        public RootVM(object param, Window currDialogWindow)
        {
            if (param is Report)
            {
                editReportsVM = new EditReportsVM(param as Report, currDialogWindow);
                editReportsVM.ParentViewModel = this;
                SelectedViewModel = editReportsVM;
            }
            else if (param is User)
            {
                editUsersVM = new EditUsersVM(param as User, currDialogWindow);
                editUsersVM.ParentViewModel= this;
                SelectedViewModel = editUsersVM;
            }

            ChangeRootVM_Command = new RelayCommand(ChangeVM);
        }

        private void ChangeVM(object param)
        {
            string parameter = param as string;

            if (parameter == "reports")
                SelectedViewModel = editReportsVM;
            else if (parameter == "users")
                SelectedViewModel = editUsersVM;
        }
    }
}
