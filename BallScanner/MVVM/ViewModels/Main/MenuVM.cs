using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Base;
using System.Windows;
using System;
using BallScanner.Data;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class MenuVM : BaseViewModel
    {
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
                    App.WriteMsg2Log("Нажатие на пункт меню \"Администрирование\"", LoggerTypes.INFO);
                    break;
                case 1:
                    SelectedPage = accountVM;
                    App.WriteMsg2Log("Нажатие на пункт меню \"Аккаунт\"", LoggerTypes.INFO);
                    break;
                case 2:
                    SelectedPage = scanVM;
                    App.WriteMsg2Log("Нажатие на пункт меню \"Сканирование\"", LoggerTypes.INFO);
                    break;
                case 3:
                    SelectedPage = calibrateVM;
                    App.WriteMsg2Log("Нажатие на пункт меню \"Калибровка\"", LoggerTypes.INFO);
                    break;
                case 4:
                    if (DocumentsVM.RefreshDataGrid.CanExecute(null))
                        DocumentsVM.RefreshDataGrid.Execute(null);

                    SelectedPage = documentsVM;
                    App.WriteMsg2Log("Нажатие на пункт меню \"Отчёты\"", LoggerTypes.INFO);
                    break;
                case 5:
                    SelectedPage = settingsVM;
                    App.WriteMsg2Log("Нажатие на пункт меню \"Настройки\"", LoggerTypes.INFO);
                    break;
                case 6:
                    SelectedPage = aboutVM;
                    App.WriteMsg2Log("Нажатие на пункт меню \"О программе\"", LoggerTypes.INFO);
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
                App.WriteMsg2Log("Нажатие на пункт меню \"Администрирование\"", LoggerTypes.INFO);
            } else if (name == "Account")
            {
                SelectedPage = accountVM;
                App.WriteMsg2Log("Нажатие на пункт меню \"Аккаунт\"", LoggerTypes.INFO);
            } else if (name == "Scan")
            {
                SelectedPage = scanVM;
                App.WriteMsg2Log("Нажатие на пункт меню \"Сканирование\"", LoggerTypes.INFO);
            }
            else if (name == "Calibrate")
            {
                SelectedPage = calibrateVM;
                App.WriteMsg2Log("Нажатие на пункт меню \"Калибровка\"", LoggerTypes.INFO);
            } else if (name == "Documents")
            {
                if (DocumentsVM.RefreshDataGrid.CanExecute(null))
                    DocumentsVM.RefreshDataGrid.Execute(null);

                SelectedPage = documentsVM;
                App.WriteMsg2Log("Нажатие на пункт меню \"Отчёты\"", LoggerTypes.INFO);
            } else if (name == "Settings")
            {
                SelectedPage = settingsVM;
                App.WriteMsg2Log("Нажатие на пункт меню \"Настройки\"", LoggerTypes.INFO);
            } else if (name == "About")
            {
                SelectedPage = aboutVM;
                App.WriteMsg2Log("Нажатие на пункт меню \"О программе\"", LoggerTypes.INFO);
            }

            SelectedPage.ChangePalette();
        }

        private void Logout(object param)
        {
            App.WriteMsg2Log("Нажатие на пункт меню \"Выход из аккаунта\"", LoggerTypes.INFO);

            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                App.WriteMsg2Log("Ошибка во время выполнения! Текст ошибки: " + ex.Message, LoggerTypes.ERROR);
                MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            App.CurrentUser = null;
            App.WriteMsg2Log("Выход из системы", LoggerTypes.INFO);

            // open login window
            Window old = Application.Current.MainWindow;
            Window future = new Views.Auth.RootV();

            Application.Current.MainWindow = future;

            future.Show();
            old.Close();
        }
    }
}
