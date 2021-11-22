using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Base;
using NLog;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class MenuVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly AccountVM accountVM = new AccountVM();
        private static readonly ScanVM scanVM = new ScanVM();
        private static readonly CalibrateVM calibrateVM = new CalibrateVM();
        private static readonly DocumentsVM documentsVM = new DocumentsVM();
        private static readonly SettingsVM settingsVM = new SettingsVM();
        private static readonly AboutVM aboutVM = new AboutVM();

        public static RelayCommand MenuButtonClick { get; set; }

        private PageVM _selectedPage;
        public PageVM SelectedPage
        {
            get => _selectedPage;
            set
            {
                _selectedPage = value;
                OnPropertyChanged(nameof(SelectedPage));
            }
        }

        public MenuVM()
        {
            Log.Info("Constructor called!");
            SelectedPage = accountVM;

            // Повесить команды на MenuButtonClick
            MenuButtonClick = new RelayCommand(OnMenuButtonClick);
        }

        public void OnMenuButtonClick(object param)
        {
            string name = param as string;

            if (name == "Account")
            {
                SelectedPage = accountVM;
            } else if (name == "Scan")
            {
                SelectedPage = scanVM;
            }
            else if (name == "Calibrate")
            {
                SelectedPage = calibrateVM;
            } else if (name == "Documents")
            {
                SelectedPage = documentsVM;
            } else if (name == "Settings")
            {
                SelectedPage = settingsVM;
            } else if (name == "About")
            {
                SelectedPage = aboutVM;
            }

            SelectedPage.ChangePalette();
        }
    }
}
