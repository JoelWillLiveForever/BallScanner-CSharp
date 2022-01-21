using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using Joel.Utils.Services;
using NLog;
using System;
using System.Windows;
using System.Linq;

namespace BallScanner.MVVM.ViewModels.Auth
{
    public class RegistrationVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

        public RelayCommand RegisterUser_Command { get; set; }
        public RelayCommand GoToAuthView_Command { get; set; }

        public RegistrationVM()
        {
            Log.Info("Constructor called!");

            RegisterUser_Command = new RelayCommand(Register);
            GoToAuthView_Command = new RelayCommand(ChangeVM);
        }

        private void Register(object param)
        {
            try
            {
                // Null surname
                if (Surname == null || Surname.Equals("") || Surname.Length == 0)
                {
                    MessageBox.Show("Вы не указали свою фамилию!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                // Null name
                if (Name == null || Name.Equals("") || Name.Length == 0)
                {
                    MessageBox.Show("Вы не указали своё имя!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                // Null login
                if (Login == null || Login.Equals("") || Login.Length == 0)
                {
                    MessageBox.Show("Вы не указали свой логин для авторизации!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                // Null smena_number
                if (Smena_Number == null || Smena_Number.Equals("") || Smena_Number.Length == 0)
                {
                    MessageBox.Show("Вы не указали номер своей смены!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                // Null password
                if (Password == null || Password.Equals(""))
                {
                    MessageBox.Show("Вы не указали свой пароль для авторизации!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                // Null password_check
                if (Password_Check == null || Password_Check.Equals(""))
                {
                    MessageBox.Show("Вы не указали повтор пароля для проверки!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                // Bad password length or password_check length
                if (Password.Length < 4 || Password_Check.Length < 4)
                {
                    MessageBox.Show("Длина пароля меньше 4-х символов!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                // Password equals password_check
                if (!Password.Equals(Password_Check))
                {
                    MessageBox.Show("Пароли не совпадают!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                // Smena_Number is number
                int smena_number_value;
                if (!int.TryParse(Smena_Number, out smena_number_value))
                {
                    if (Smena_Number.Length > int.MaxValue.ToString().Length)
                        MessageBox.Show("Максимальный размер числа для номера смены равен " + int.MaxValue + ".", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    else
                        MessageBox.Show("Номер смены должен состоять только из цифр!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
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
                    MessageBox.Show("Пользователь с таким логином уже зарегистрирован!\nПожалуйста, выберите себе другой логин!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                } 

                user = new User(Surname, Name, Lastname, smena_number_value, Login, SHAService.ComputeSha256Hash(Password));

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                // Clear fields
                Surname = Name = Lastname = Login = Smena_Number = Password = Password_Check = null;

                ChangeVM("login");
            } catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void ChangeVM(object param)
        {
            Surname = null;
            Name = null;
            Lastname = null;

            Login = null;
            Smena_Number = null;

            Password = null;
            Password_Check = null;

            RootVM root = ParentViewModel as RootVM;

            if (root.ChangeRootVM_Command.CanExecute("login"))
            {
                root.ChangeRootVM_Command.Execute("login");
            }
        }
    }
}
