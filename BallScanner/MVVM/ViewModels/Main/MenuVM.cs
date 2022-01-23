using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Base;
using NLog;
using System.Windows;
using System;
using BallScanner.Data;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class MenuVM : BaseViewModel
    {
        private static readonly object global_locker = new object();

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly AccountManagmentVM accountManagmentVM = new AccountManagmentVM();
        private static readonly AccountVM accountVM = new AccountVM();
        private static readonly ScanVM scanVM = new ScanVM();
        private static readonly CalibrateVM calibrateVM = new CalibrateVM();
        private static readonly DocumentsVM documentsVM = new DocumentsVM();
        private static readonly SettingsVM settingsVM = new SettingsVM();
        private static readonly AboutVM aboutVM = new AboutVM();

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

        public static RelayCommand MenuButtonClick { get; set; }
        public static RelayCommand Logout_Command { get; set; }

        public MenuVM()
        {
            switch (Properties.Settings.Default.SelectedPage)
            {
                case 0:
                    if (AccountManagmentVM.RefreshDataGrid.CanExecute(null))
                        AccountManagmentVM.RefreshDataGrid.Execute(null);

                    SelectedPage = accountManagmentVM;
                    break;
                case 1:
                    SelectedPage = accountVM;
                    break;
                case 2:
                    SelectedPage = scanVM;
                    break;
                case 3:
                    SelectedPage = calibrateVM;
                    break;
                case 4:
                    if (DocumentsVM.RefreshDataGrid.CanExecute(null))
                        DocumentsVM.RefreshDataGrid.Execute(null);

                    SelectedPage = documentsVM;
                    break;
                case 5:
                    SelectedPage = settingsVM;
                    break;
                case 6:
                    SelectedPage = aboutVM;
                    break;

            }
            SelectedPage.ChangePalette();

            // Повесить команды на MenuButtonClick
            MenuButtonClick = new RelayCommand(OnMenuButtonClick);
            Logout_Command = new RelayCommand(Logout);
        }

        private void OnMenuButtonClick(object param)
        {
            string name = param as string;

            if (name == "AccountManagment" /*&& App.CurrentUser._access_level == 0*/)
            {
                if (AccountManagmentVM.RefreshDataGrid.CanExecute(null))
                    AccountManagmentVM.RefreshDataGrid.Execute(null);

                SelectedPage = accountManagmentVM;
            } else if (name == "Account")
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
                if (DocumentsVM.RefreshDataGrid.CanExecute(null))
                    DocumentsVM.RefreshDataGrid.Execute(null);

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

        private void Logout(object param)
        {
            try
            {
                lock (global_locker)
                {
                    AppDbContext dbContext = AppDbContext.GetInstance();
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            App.CurrentUser = null;

            // open login window
            Window old = Application.Current.MainWindow;
            Window future = new Views.Auth.RootV();

            Application.Current.MainWindow = future;

            future.Show();
            old.Close();
        }
    }
}
