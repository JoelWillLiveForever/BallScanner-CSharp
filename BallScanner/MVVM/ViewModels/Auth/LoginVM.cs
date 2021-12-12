using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using NLog;

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

                _login = value;
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
            ChangeVM("menu");
            new Views.Main.RootV().Show();

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

