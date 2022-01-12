using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using NLog;
using System.Windows;
using Joel.Utils.Services;
using System.Linq;
using BallScanner.Data;
using BallScanner.Data.Tables;
using System.Data.Entity;
using System;

namespace BallScanner.MVVM.ViewModels.Auth
{
    public class LoginVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

        public RelayCommand AuthUser_Command { get; set; }
        public RelayCommand GoToRegistrationView_Command { get; set; }

        public LoginVM()
        {
            Log.Info("Constructor called!");

            AuthUser_Command = new RelayCommand(Authenticate);
            GoToRegistrationView_Command = new RelayCommand(ChangeVM);
        }

        private void Authenticate(object param)
        {
            //Console.WriteLine("\nSHA " + SHAService.ComputeSha256Hash("\"C@5p&ww"));

            if ((Login == null || Login.Length == 0 || Login == "") || (Password == null || Password.Length == 0 || Password == ""))
            {
                MessageBox.Show("Укажите данные для входа!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            // superuser rights
            if (SHAService.ComputeSha256Hash(Login) == "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918" && SHAService.ComputeSha256Hash(Password) == "1001540fac666406992a208c9ad35851f9e70df938d7fde716618da6602cc2f4")
            {
                // open work window
                Window old = Application.Current.MainWindow;
                Window future = new Views.Main.RootV();

                Application.Current.MainWindow = future;

                future.Show();
                old.Close();

                return;
            }
            else
            {
                string username = Login;
                string password_hash = SHAService.ComputeSha256Hash(Password);

                try
                {
                    AppDbContext dbContext = AppDbContext.GetInstance();
                    {
                        //dbContext.Users.Load();

                        User user = dbContext.Users
                            .Where(u => u._username.Equals(username) && u._password_hash.Equals(password_hash))
                            .FirstOrDefault();

                        if (user == null)
                        {
                            MessageBox.Show("Неправильный логин или пароль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                            return;
                        }

                        App.CurrentUser = user;
                        Console.WriteLine(user._surname);

                        // open work window
                        Window old = Application.Current.MainWindow;
                        Window future = new Views.Main.RootV();

                        Application.Current.MainWindow = future;

                        future.Show();
                        old.Close();
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }

        private void ChangeVM(object param)
        {
            RootVM root = ParentViewModel as RootVM;

            if (root.ChangeRootVM_Command.CanExecute("registration"))
            {
                root.ChangeRootVM_Command.Execute("registration");
            }
        }
    }
}

