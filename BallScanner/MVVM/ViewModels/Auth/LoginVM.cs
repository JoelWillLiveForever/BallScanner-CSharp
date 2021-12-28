using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using NLog;
using System.Windows;
using Joel.Utils.Services;
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
        public RelayCommand ChangeSelectedVM_Command { get; set; }

        public LoginVM()
        {
            Log.Info("Constructor called!");
            AuthUser_Command = new RelayCommand(Authenticate);
            ChangeSelectedVM_Command = new RelayCommand(ChangeVM);
        }

        private void Authenticate(object param)
        {
            //Console.WriteLine("\nSHA " + SHAService.ComputeSha256Hash("\"C@5p&ww"));

            // superuser rights
            if (SHAService.ComputeSha256Hash(Login) == "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918" && SHAService.ComputeSha256Hash(Password) == "1001540fac666406992a208c9ad35851f9e70df938d7fde716618da6602cc2f4")
                new Views.Main.RootV().Show();
            else
                MessageBox.Show("User not found!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);

            //if (current != null)
            //{
            //    current.Close();

            //    current = new RootV();
            //    Application.Current.MainWindow = current;
            //    current.Show();
            //}
        }

        private void ChangeVM(object param)
        {
            //RootVM parent = ParentViewModel as RootVM;
            //if (parent.ChangeRootVM_Command.CanExecute(param))
            //{
            //    parent.ChangeRootVM_Command.Execute(param);
            //}
        }
    }
}

