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

        private ScanVM scanVM;
        private CalibrateVM calibrateVM;

        public MenuVM()
        {
            scanVM = new ScanVM();
            calibrateVM = new CalibrateVM();

            CurrentActionVM = scanVM;

            // Повесить команды на MenuButtonClick
            MenuButtonClick = new RelayCommand(OnMenuButtonClick);
        }

        public void OnMenuButtonClick(object param)
        {
            System.Diagnostics.Debug.WriteLine($"Clicked: {param as string}");
            string name = param as string;

            if (name == "Scan")
            {
                CurrentActionVM = scanVM;
            }
            else if (name == "Calibrate")
            {
                CurrentActionVM = calibrateVM;
            }
        }
    }
}
