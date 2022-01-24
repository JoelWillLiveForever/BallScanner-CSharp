using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using BallScanner.MVVM.ViewModels.Main;
using Joel.Utils.Services;
using System;
using System.Linq;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Add
{
    public class RegistrationVM : BaseViewModel
    {
        // Fields
        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname == value) return;

                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;

                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _lastname;
        public string Lastname
        {
            get => _lastname;
            set
            {
                if (_lastname == value) return;

                _lastname = value;
                OnPropertyChanged(nameof(Lastname));
            }
        }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                if (_login == value) return;

                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _smena_number;
        public string Smena_Number
        {
            get => _smena_number;
            set
            {
                if (_smena_number == value) return;

                _smena_number = value;
                OnPropertyChanged(nameof(Smena_Number));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;

                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _password_check;
        public string Password_Check
        {
            get => _password_check;
            set
            {
                if (_password_check == value) return;

                _password_check = value;
                OnPropertyChanged(nameof(Password_Check));
            }
        }

        public RelayCommand AddUser_Command { get; set; }

        public RegistrationVM()
        {
            AddUser_Command = new RelayCommand(Register);
        }

        private void Register(object param)
        {
            try
            {
                // Null surname
                if (Surname == null || Surname.Equals("") || Surname.Length == 0)
                {
                    App.WriteMsg2Log("Предупреждение пользователя! Поле \"Фамилия\" пустое!", LoggerTypes.WARN);
                    MessageBox.Show("Вы не указали фамилию пользователя!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }

                // Null name
                if (Name == null || Name.Equals("") || Name.Length == 0)
                {
                    App.WriteMsg2Log("Предупреждение пользователя! Поле \"Имя\" пустое!", LoggerTypes.WARN);
                    MessageBox.Show("Вы не указали имя пользователя!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }

                // Null login
                if (Login == null || Login.Equals("") || Login.Length == 0)
                {
                    App.WriteMsg2Log("Предупреждение пользователя! Поле \"Логин\" пустое!", LoggerTypes.WARN);
                    MessageBox.Show("Вы не указали логин аккаунта пользователя!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }

                // Null smena_number
                if (Smena_Number == null || Smena_Number.Equals("") || Smena_Number.Length == 0)
                {
                    App.WriteMsg2Log("Предупреждение пользователя! Поле \"Номер смены\" пустое!", LoggerTypes.WARN);
                    MessageBox.Show("Вы не указали номер смены пользователя!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }

                // Null password
                if (Password == null || Password.Equals(""))
                {
                    App.WriteMsg2Log("Предупреждение пользователя! Поле \"Пароль\" пустое!", LoggerTypes.WARN);
                    MessageBox.Show("Вы не указали пароль для авторизации!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }

                // Null password_check
                if (Password_Check == null || Password_Check.Equals(""))
                {
                    App.WriteMsg2Log("Предупреждение пользователя! Поле \"Повторите пароль\" пустое!", LoggerTypes.WARN);
                    MessageBox.Show("Вы не указали повтор пароля для проверки!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }

                // Bad password length or password_check length
                if (Password.Length < 4 || Password_Check.Length < 4)
                {
                    App.WriteMsg2Log("Предупреждение пользователя! Длина пароля или повтора пароля меньше 4-х символов!", LoggerTypes.WARN);
                    MessageBox.Show("Длина пароля меньше 4-х символов!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }

                // Password equals password_check
                if (!Password.Equals(Password_Check))
                {
                    App.WriteMsg2Log("Предупреждение пользователя! Пароли не совпадают!", LoggerTypes.WARN);
                    MessageBox.Show("Пароли не совпадают!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }

                // Smena_Number is number
                int smena_number_value;
                if (!int.TryParse(Smena_Number, out smena_number_value))
                {
                    if (Smena_Number.Length > int.MaxValue.ToString().Length)
                    {
                        App.WriteMsg2Log("Предупреждение пользователя! Превышено максимально допустимое значение (" + int.MaxValue + ") для ввода номера смены!", LoggerTypes.WARN);
                        MessageBox.Show("Максимальный размер числа для номера смены равен " + int.MaxValue + ".", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    }
                    else
                    {
                        App.WriteMsg2Log("Предупреждение пользователя! Номер смены должен состоять только из цифр!", LoggerTypes.WARN);
                        MessageBox.Show("Номер смены должен состоять только из цифр!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    }

                    return;
                }

                // init db connection
                AppDbContext dbContext = AppDbContext.GetInstance();

                // Check login free to use
                var user = (from u in dbContext.Users
                            where u._username == Login
                            select u).FirstOrDefault();

                if (user != null)
                {
                    App.WriteMsg2Log("Попытка зарегистрировать пользователя с логином, который уже имеется в базе данных!", LoggerTypes.ERROR);
                    MessageBox.Show("Пользователь с таким логином уже зарегистрирован!\nПожалуйста, выберите другой логин!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                user = new User(Surname, Name, Lastname, smena_number_value, Login, SHAService.ComputeSha256Hash(Password));

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                // Clear fields
                Surname = Name = Lastname = Login = Smena_Number = Password = Password_Check = null;

                // Refresh datagrid
                if (AccountManagmentVM.RefreshDataGrid.CanExecute(null))
                    AccountManagmentVM.RefreshDataGrid.Execute(null);
            }
            catch (Exception ex)
            {
                App.WriteMsg2Log("Непредвиденная ошибка во время выполнения! Текст ошибки: " + ex.Message, LoggerTypes.FATAL);
                MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
