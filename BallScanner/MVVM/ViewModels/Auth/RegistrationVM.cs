using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using Joel.Utils.Services;
using NLog;
using System;
using System.Windows;

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

        private int _smena_number;
        public int Smena_Number
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
                using (AppDbContext dbContext = new AppDbContext())
                {
                    User newUser = new User(Surname, Name, Lastname, Smena_Number, Login, SHAService.ComputeSha256Hash(Password));

                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();

                    ChangeVM("login");
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void ChangeVM(object param)
        {
            RootVM root = ParentViewModel as RootVM;

            if (root.ChangeRootVM_Command.CanExecute("login"))
            {
                root.ChangeRootVM_Command.Execute("login");
            }
        }
    }
}
