using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using System.Windows;
using Joel.Utils.Services;
using System.Linq;
using BallScanner.Data;
using BallScanner.Data.Tables;
using System;
using NLog;

namespace BallScanner.MVVM.ViewModels.Auth
{
    public class LoginVM : BaseViewModel
    {
        //private static readonly Logger Log = LogManager.GetLogger("Неавторизованный пользователь");

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                if (_login == value) return;

                _login = value;
                OnPropertyChanged(nameof(Login));

                //Log.Info("Изменение данных в поле \"Логин\"");
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

                //Log.Info("Изменение данных в поле \"Пароль\"");
            }
        }

        public RelayCommand AuthUser_Command { get; set; }

        public LoginVM()
        {
            AuthUser_Command = new RelayCommand(Authenticate);
        }

        private void Authenticate(object param)
        {
            Logger Log = LogManager.GetLogger("Неавторизованный пользователь");

            if ((Login == null || Login.Length == 0 || Login.Equals("")) || (Password == null || Password.Length == 0 || Password.Equals("")))
            {
                Log.Warn("Предупреждение пользователя! Не были указаны данные (логин или пароль) для авторизации!");
                MessageBox.Show("Укажите данные для входа!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }

            // superuser rights
            if (SHAService.ComputeSha256Hash(Login) == "4813494d137e1631bba301d5acab6e7bb7aa74ce1185d456565ef51d737677b2" && SHAService.ComputeSha256Hash(Password) == "e2a5cb13dd352aabe3568985c8af2498f1edf4045ce434f36fac21101092439e")
            {
                // open work window
                Window old = Application.Current.MainWindow;
                Window future = new Views.Main.RootV();

                Application.Current.MainWindow = future;

                // Clear fields
                Login = Password = null;

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
                    //dbContext.Users.Load();

                    User user = dbContext.Users
                        .Where(u => u._username.Equals(username) && u._password_hash.Equals(password_hash) && u._is_active == 1)
                        .FirstOrDefault();

                    if (user == null)
                    {
                        Log.Error("Ошибка авторизации! Неправильный логин или пароль!");
                        MessageBox.Show("Неправильный логин или пароль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        return;
                    }

                    App.CurrentUser = user;
                    //Console.WriteLine(user._surname);

                    // open work window
                    Window old = Application.Current.MainWindow;
                    Window future = new Views.Main.RootV();

                    Application.Current.MainWindow = future;

                    // Clear fields
                    Login = Password = null;

                    future.Show();
                    old.Close();
                }
                catch (Exception ex)
                {
                    Log.Fatal("Непредвиденная ошибка во время выполнения! Текст ошибки: " + ex.Message);
                    MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }
    }
}

