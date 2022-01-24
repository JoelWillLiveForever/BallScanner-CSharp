using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using BallScanner.MVVM.ViewModels.Main;
using Joel.Utils.Services;
using System;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Edit
{
    public class EditUsersVM : BaseViewModel
    {
        public RelayCommand DeleteReportCommand { get; set; }
        public RelayCommand ChangePasswordCommand { get; set; }

        private Window currDialogWindow;
        private User user;

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

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword == value) return;

                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        private string _newPassword_check;
        public string NewPassword_Check
        {
            get => _newPassword_check;
            set
            {
                if (_newPassword_check == value) return;

                _newPassword_check = value;
                OnPropertyChanged(nameof(NewPassword_Check));
            }
        }

        public EditUsersVM()
        {
            DeleteReportCommand = new RelayCommand(Delete_User);
            ChangePasswordCommand = new RelayCommand(ChangePassword);
        }

        public EditUsersVM(User user, Window currDialogWindow)
        {
            DeleteReportCommand = new RelayCommand(Delete_User);
            ChangePasswordCommand = new RelayCommand(ChangePassword);

            this.user = user;
            this.currDialogWindow = currDialogWindow;
        }

        private void RefreshDataGrid()
        {
            if (AccountManagmentVM.RefreshDataGrid.CanExecute(null))
                AccountManagmentVM.RefreshDataGrid.Execute(null);

            if (DocumentsVM.RefreshDataGrid.CanExecute(null))
                DocumentsVM.RefreshDataGrid.Execute(null);
        }

        private void Delete_User(object param)
        {
            if (user == App.CurrentUser)
            {
                App.WriteMsg2Log("Попытка удалить авторизованного пользователя (самого себя)!", LoggerTypes.ERROR);
                MessageBox.Show("Нельзя удалить авторизованного пользователя!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }
            
            if (user.Reports != null && user.Reports.Count != 0)
            {
                App.WriteMsg2Log("Попытка удалить пользователя, у которого есть отчёты в базе данных!", LoggerTypes.ERROR);
                MessageBox.Show("У данного пользователя есть отчёты в базе данных!\nЧтобы удалить данного пользователя, нужно сначала\nудалить все его отчёты!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            App.WriteMsg2Log("Удаление пользователя. Информация о пользователе: номер = " + user._id + "; фамилия = " + user._surname + "; имя = " + user._name + "; отчество = " + user._lastname + "; логин = " + user._username + "; номер смены = " + user._smena_number + "; уровень доступа = " + user.Access_Level + "; статус аккаунта = " + user.Is_Active + ";", LoggerTypes.INFO);
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
                App.WriteMsg2Log("Непредвиденная ошибка во время выполнения! Текст ошибки: " + ex.Message, LoggerTypes.FATAL);
                MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void ChangePassword(object param)
        {
            // Null password
            if (NewPassword == null || NewPassword.Equals(""))
            {
                App.WriteMsg2Log("Предупреждение пользователя! Поле \"Новый пароль\" пустое!", LoggerTypes.WARN);
                MessageBox.Show("Вы не указали новый пароль для авторизации!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }

            // Null password_check
            if (NewPassword_Check == null || NewPassword_Check.Equals(""))
            {
                App.WriteMsg2Log("Предупреждение пользователя! Поле \"Повторите новый пароль\" пустое!", LoggerTypes.WARN);
                MessageBox.Show("Вы не указали повтор нового пароля для проверки!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }

            // Bad password length or password_check length
            if (NewPassword.Length < 4 || NewPassword_Check.Length < 4)
            {
                App.WriteMsg2Log("Предупреждение пользователя! Длина пароля или повтора пароля меньше 4-х символов!", LoggerTypes.WARN);
                MessageBox.Show("Длина пароля меньше 4-х символов!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }

            // Password equals password_check
            if (!NewPassword.Equals(NewPassword_Check))
            {
                App.WriteMsg2Log("Предупреждение пользователя! Пароли не совпадают!", LoggerTypes.WARN);
                MessageBox.Show("Пароли не совпадают!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }

            user._password_hash = SHAService.ComputeSha256Hash(NewPassword);

            App.WriteMsg2Log("Изменение пароля для пользователя. Информация о пользователе: номер = " + user._id + "; фамилия = " + user._surname + "; имя = " + user._name + "; отчество = " + user._lastname + "; логин = " + user._username + "; номер смены = " + user._smena_number + "; уровень доступа = " + user.Access_Level + "; статус аккаунта = " + user.Is_Active + ";", LoggerTypes.INFO);
            MessageBox.Show("Пароль был успешно изменён!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }
    }
}
