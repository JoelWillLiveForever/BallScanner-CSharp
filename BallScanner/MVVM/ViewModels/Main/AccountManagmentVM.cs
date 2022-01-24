using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class AccountManagmentVM : PageVM
    {
        private static Timer timer = new Timer(400) { Enabled = false };

        public static RelayCommand RefreshDataGrid { get; set; }
        public RelayCommand OpenDialogWindowCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand AddUser_Command { get; set; }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                if (_users == value) return;

                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private string _search_value;
        public string Search_Value
        {
            get => _search_value;
            set
            {
                if (_search_value == value) return;

                _search_value = value;
                OnPropertyChanged(nameof(Search_Value));

                timer.Stop();
                timer.Start();
            }
        }

        private int _selected_index = 0;
        public int Selected_Index
        {
            get => _selected_index;
            set
            {
                if (_selected_index == value) return;

                _selected_index = value;
                OnPropertyChanged(nameof(Selected_Index));

                timer.Stop();
                timer.Start();
            }
        }

        public AccountManagmentVM()
        {
            OpenDialogWindowCommand = new RelayCommand(OpenDialogWindow);
            SearchCommand = new RelayCommand(Search);
            AddUser_Command = new RelayCommand(AddUser);

            RefreshDataGrid = new RelayCommand(OnRefreshDataGrid);

            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();
                Users = new ObservableCollection<User>(dbContext.Users.ToList());
            }
            catch (Exception ex)
            {
                App.WriteMsg2Log("Непредвиденная ошибка во время выполнения! Текст ошибки: " + ex.Message, LoggerTypes.FATAL);
                MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            timer.Elapsed += new ElapsedEventHandler(OnSearch);
        }

        private void OnRefreshDataGrid(object param)
        {
            if (Search_Value != null || Search_Value != "" || Search_Value.Length != 0) Search(null);
            else OnPropertyChanged(nameof(Users));
        }

        private void OnSearch(object source, ElapsedEventArgs e)
        {
            timer.Stop();
            //Console.WriteLine("ПРОШЛО 400 мс.!");
            Search(null);
        }

        private void Search(object param)
        {
            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();

                //Console.WriteLine("SEARCH VALUE = " + Search_Value);

                if (Search_Value == null || Search_Value == "" || Search_Value.Length == 0)
                {
                    Users = new ObservableCollection<User>(dbContext.Users.ToList());
                    return;
                }

                List<User> search_result;
                switch (Selected_Index)
                {
                    case 0:
                        App.WriteMsg2Log("Поиск пользователей на странице \"Администрирование\" по параметру \"Логин\"", LoggerTypes.INFO);

                        // login
                        search_result = (from user in dbContext.Users
                                         where user._username.Equals(Search_Value)
                                         select user).ToList();

                        if (search_result != null)
                            Users = new ObservableCollection<User>(search_result);

                        break;
                    case 1:
                        App.WriteMsg2Log("Поиск пользователей на странице \"Администрирование\" по параметру \"Фамилия\"", LoggerTypes.INFO);

                        // surname
                        search_result = (from user in dbContext.Users
                                         where user._surname.Equals(Search_Value)
                                         select user).ToList();

                        if (search_result != null)
                            Users = new ObservableCollection<User>(search_result);

                        break;
                    case 2:
                        App.WriteMsg2Log("Поиск пользователей на странице \"Администрирование\" по параметру \"Имя\"", LoggerTypes.INFO);

                        // name
                        search_result = (from user in dbContext.Users
                                         where user._name.Equals(Search_Value)
                                         select user).ToList();

                        if (search_result != null)
                            Users = new ObservableCollection<User>(search_result);

                        break;
                    case 3:
                        App.WriteMsg2Log("Поиск пользователей на странице \"Администрирование\" по параметру \"Отчество\"", LoggerTypes.INFO);

                        // lastname
                        search_result = (from user in dbContext.Users
                                         where user._lastname.Equals(Search_Value)
                                         select user).ToList();

                        if (search_result != null)
                            Users = new ObservableCollection<User>(search_result);

                        break;
                    case 4:
                        App.WriteMsg2Log("Поиск пользователей на странице \"Администрирование\" по параметру \"Номер смены\"", LoggerTypes.INFO);

                        // smena_number
                        search_result = (from user in dbContext.Users
                                         where user._smena_number.ToString().Equals(Search_Value)
                                         select user).ToList();

                        if (search_result != null)
                            Users = new ObservableCollection<User>(search_result);

                        break;
                    case 5:
                        App.WriteMsg2Log("Поиск пользователей на странице \"Администрирование\" по параметру \"Уровень доступа\"", LoggerTypes.INFO);

                        // access level
                        switch (Search_Value.ToUpper())
                        {
                            case "ПОЛЬЗОВАТЕЛЬ":
                                search_result = (from user in dbContext.Users
                                                 where user._access_level == 0
                                                 select user).ToList();
                                break;
                            case "АДМИНИСТРАТОР":
                                search_result = (from user in dbContext.Users
                                                 where user._access_level == 1
                                                 select user).ToList();
                                break;
                            default:
                                search_result = (from user in dbContext.Users
                                                 where user._access_level.ToString().Equals(Search_Value)
                                                 select user).ToList();
                                break;
                        }

                        if (search_result != null)
                            Users = new ObservableCollection<User>(search_result);

                        break;
                    case 6:
                        App.WriteMsg2Log("Поиск пользователей на странице \"Администрирование\" по параметру \"Статус аккаунта\"", LoggerTypes.INFO);

                        // is active user
                        switch (Search_Value.ToUpper())
                        {
                            case "АКТИВЕН":
                                search_result = (from user in dbContext.Users
                                                 where user._is_active == 1
                                                 select user).ToList();
                                break;
                            case "НЕАКТИВЕН":
                                search_result = (from user in dbContext.Users
                                                 where user._is_active == 0
                                                 select user).ToList();
                                break;
                            default:
                                search_result = (from user in dbContext.Users
                                                 where user._is_active.ToString().Equals(Search_Value)
                                                 select user).ToList();
                                break;
                        }

                        if (search_result != null)
                            Users = new ObservableCollection<User>(search_result);

                        break;
                }
            }
            catch (Exception ex)
            {
                App.WriteMsg2Log("Непредвиденнная ошибка во время выполнения! Текст ошибки: " + ex.Message, LoggerTypes.FATAL);
                MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void OpenDialogWindow(object param)
        {
            if (param != null)
            {
                App.WriteMsg2Log("Открытие окна редактирования пользователя. Пользователь #" + (param as User)._id + ". Страница \"Администрирование\".", LoggerTypes.INFO);

                Views.Edit.RootV editWindow = new Views.Edit.RootV(param);
                editWindow.ShowDialog();

                App.WriteMsg2Log("Закрытие окна редактирования пользователя!", LoggerTypes.INFO);
            }
        }

        private void AddUser(object param)
        {
            App.WriteMsg2Log("Открытие окна добавления пользователя на странице \"Администрирование\".", LoggerTypes.INFO);

            Views.Add.RootV addWindow = new Views.Add.RootV();
            addWindow.ShowDialog();

            App.WriteMsg2Log("Закрытие окна добавления пользователя!", LoggerTypes.INFO);
        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Dark/Pink.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Light/Pink.xaml", UriKind.Relative));

            Properties.Settings.Default.SelectedPage = 0;
            Properties.Settings.Default.Save();
        }
    }
}
