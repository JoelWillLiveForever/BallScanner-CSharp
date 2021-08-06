using BallScanner.MVVM.Core;

namespace BallScanner.MVVM.ViewModels
{
    public class MainVM : BaseViewModel
    {
        private object _currentActionVM;
        public object CurrentActionVM
        {
            get { return _currentActionVM; }
            set
            {
                _currentActionVM = value;
                OnPropertyChanged("CurrentActionVM");
            }
        }

        public static RelayCommand MenuVM_RelayCommand { get; set; }
        //public static RelayCommand LoginVM_RelayCommand { get; set; }
        //public static RelayCommand RegistrationVM_RelayCommand { get; set; }

        public static MenuVM menuVM { get; set; }
        //public static LoginVM loginVM { get; set; }
        //public static RegistrationVM registrationVM { get; set; }

        public MainVM()
        {
            // Create ViewModels
            menuVM = new MenuVM();
            //loginVM = new LoginVM();
            //registrationVM = new RegistrationVM();

            // Set current ViewModel
            CurrentActionVM = menuVM;

            // Set handles
            MenuVM_RelayCommand = new RelayCommand(o =>
            {
                //if (Properties.Settings.Default.IsAdmin)
                //    Properties.Settings.Default.Visibility = "Visible";
                //else
                //    Properties.Settings.Default.Visibility = "Collapsed";

                CurrentActionVM = menuVM;
            });

            //LoginVM_RelayCommand = new RelayCommand(o =>
            //{
            //    CurrentActionVM = loginVM;
            //});

            //RegistrationVM_RelayCommand = new RelayCommand(o =>
            //{
            //    CurrentActionVM = registrationVM;
            //});
        }
    }
}
