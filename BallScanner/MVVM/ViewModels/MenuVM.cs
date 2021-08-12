using BallScanner.MVVM.Core;

namespace BallScanner.MVVM.ViewModels
{
    public class MenuVM : BaseViewModel
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

        // Команда, реагирующая на нажатие кнопки из меню
        public RelayCommand MenuButtonClick { get; set; }

        private AccountVM accountVM;
        private ScanVM scanVM;
        private CalibrateVM calibrateVM;
        private DocumentsVM documentsVM;
        private SettingsVM settingsVM;
        private AboutVM aboutVM;

        public MenuVM()
        {
            accountVM = new AccountVM();
            scanVM = new ScanVM();
            calibrateVM = new CalibrateVM();
            documentsVM = new DocumentsVM();
            settingsVM = new SettingsVM();
            aboutVM = new AboutVM();

            CurrentActionVM = scanVM;

            // Повесить команды на MenuButtonClick
            MenuButtonClick = new RelayCommand(OnMenuButtonClick);
        }

        public void OnMenuButtonClick(object param)
        {
            System.Diagnostics.Debug.WriteLine($"Clicked: {param as string}");
            string name = param as string;

            if (name == "Account")
            {
                CurrentActionVM = accountVM;
            } else if (name == "Scan")
            {
                CurrentActionVM = scanVM;
            }
            else if (name == "Calibrate")
            {
                CurrentActionVM = calibrateVM;
            } else if (name == "Documents")
            {
                CurrentActionVM = documentsVM;
            } else if (name == "Settings")
            {
                CurrentActionVM = settingsVM;
            } else if (name == "About")
            {
                CurrentActionVM = aboutVM;
            }
        }
    }
}
