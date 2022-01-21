using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using BallScanner.MVVM.ViewModels.Main;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Edit
{
    public class EditUsersVM : BaseViewModel
    {
        public RelayCommand DeleteReportCommand { get; set; }

        private User user;
        private Window currDialogWindow;

        public string Title_Text
        {
            get => "Редактирование пользователей. Пользователь #" + user._id;
        }

        // Fields
        public string Surname
        {
            get => user._surname;
            set
            {
                if (user._surname == value) return;

                user._surname = value;
                OnPropertyChanged(nameof(Surname));

                RefreshDataGrid();
            }
        }

        public string Name
        {
            get => user._name;
            set
            {
                if (user._name == value) return;

                user._name = value;
                OnPropertyChanged(nameof(Name));

                RefreshDataGrid();
            }
        }

        public string Lastname
        {
            get => user._lastname;
            set
            {
                if (user._lastname == value) return;

                user._lastname = value;
                OnPropertyChanged(nameof(Lastname));

                RefreshDataGrid();
            }
        }

        public string Login
        {
            get => user._username;
            set
            {
                if (user._username == value) return;

                user._username = value;
                OnPropertyChanged(nameof(Login));

                RefreshDataGrid();
            }
        }

        public int Smena_Number
        {
            get => user._smena_number;
            set
            {
                if (user._smena_number == value) return;

                user._smena_number = value;
                OnPropertyChanged(nameof(Smena_Number));

                RefreshDataGrid();
            }
        }

        public int Access_Level
        {
            get => user._access_level;
            set
            {
                if (user._access_level == value) return;

                user._access_level = value;
                OnPropertyChanged(nameof(Access_Level));

                RefreshDataGrid();
            }
        }

        public int Is_Active
        {
            get => user._is_active;
            set
            {
                if (user._is_active == value) return;

                user._is_active = value;
                OnPropertyChanged(nameof(Is_Active));

                RefreshDataGrid();
            }
        }

        public string Password
        {
            get => user._password_hash;
            set
            {
                if (user._password_hash == value) return;

                user._password_hash = value;
                OnPropertyChanged(nameof(Password));

                RefreshDataGrid();
            }
        }

        public EditUsersVM()
        {
            DeleteReportCommand = new RelayCommand(Delete_Report);
        }

        public EditUsersVM(User report, Window currDialogWindow)
        {
            DeleteReportCommand = new RelayCommand(Delete_Report);

            this.user = report;
            this.currDialogWindow = currDialogWindow;
        }

        private void RefreshDataGrid()
        {
            if (AccountManagmentVM.RefreshDataGrid.CanExecute(null))
                AccountManagmentVM.RefreshDataGrid.Execute(null);

            if (DocumentsVM.RefreshDataGrid.CanExecute(null))
                DocumentsVM.RefreshDataGrid.Execute(null);
        }

        private void Delete_Report(object param)
        {
            if (user.Reports != null)
            {
                MessageBox.Show("У данного пользователя есть отчёты в базе данных!\nЧтобы удалить данного пользователя, нужно сначала\nудалить все его отчёты!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();

                dbContext.Users.Remove(user);
                dbContext.SaveChanges();

                currDialogWindow.Close();
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
