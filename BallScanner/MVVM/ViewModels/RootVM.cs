using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using BallScanner.MVVM.ViewModels.Auth;
using BallScanner.MVVM.ViewModels.Main;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BallScanner.MVVM.ViewModels
{
    public class RootVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private MenuVM menuVM;
        private LoginVM loginVM;
        private RegistrationVM registrationVM;

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

        private ResizeMode _currentResizeMode = ResizeMode.CanMinimize;
        public ResizeMode CurrentResizeMode
        {
            get => _currentResizeMode;
            set
            {
                if (_currentResizeMode != value)
                {
                    _currentResizeMode = value;
                    OnPropertyChanged(nameof(CurrentResizeMode));
                }
            }
        }

        private int _currentWidth = 500;
        public int CurrentWidth
        {
            get => _currentWidth;
            set
            {
                if (_currentWidth != value)
                {
                    _currentWidth = value;
                    OnPropertyChanged(nameof(CurrentWidth));
                }
            }
        }

        private int _currentHeight = 400;
        public int CurrentHeight
        {
            get => _currentHeight;
            set
            {
                if (_currentHeight != value)
                {
                    _currentHeight = value;
                    OnPropertyChanged(nameof(CurrentHeight));
                }
            }
        }

        public RelayCommand ChangeRootVM_Command { get; set; }

        public RootVM()
        {
            Log.Info("Constructor called!");

            loginVM = new LoginVM();
            loginVM.ParentViewModel = this;

            SelectedViewModel = loginVM;
            ChangeRootVM_Command = new RelayCommand(ChangeVM);
            ChangeVM("menu");
        }

        private void ChangeVM(object param)
        {
            Task.Run(() =>
            {
                string parameter = param as string;
                Console.WriteLine("[PARAMETER] = " + parameter);

                Thread.Sleep(4000);

                if (parameter == "menu")
                {
                    CurrentWidth = 800;
                    CurrentHeight = 450;
                    CurrentResizeMode = ResizeMode.CanResize;

                    menuVM = new MenuVM();
                    menuVM.ParentViewModel = this;

                    SelectedViewModel = menuVM;
                }
                else if (parameter == "login")
                {
                    CurrentWidth = 500;
                    CurrentHeight = 400;
                    CurrentResizeMode = ResizeMode.CanMinimize;

                    loginVM = new LoginVM();
                    loginVM.ParentViewModel = this;

                    //SelectedViewModel = loginVM;
                }
                else if (parameter == "registration")
                {
                    CurrentWidth = 500;
                    CurrentHeight = 400;
                    CurrentResizeMode = ResizeMode.CanMinimize;

                    registrationVM = new RegistrationVM();
                    registrationVM.ParentViewModel = this;

                    //SelectedViewModel = registrationVM;
                }
            });
        }
    }
}
