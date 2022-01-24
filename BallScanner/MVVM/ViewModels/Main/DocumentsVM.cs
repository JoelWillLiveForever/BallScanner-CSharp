using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using System;
using System.Windows;
using System.Linq;
using BallScanner.MVVM.Commands;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Collections.Generic;
using System.Timers;
using BallScanner.MVVM.Views.Edit;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class DocumentsVM : PageVM
    {
        private static Timer timer = new Timer(400) { Enabled = false };

        public RelayCommand OpenDialogWindowCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }

        public static RelayCommand RefreshDataGrid { get; set; }

        private ObservableCollection<Report> _reports;
        public ObservableCollection<Report> Reports
        {
            get => _reports;
            set
            {
                if (_reports == value) return;

                _reports = value;
                OnPropertyChanged(nameof(Reports));
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

        public DocumentsVM()
        {
            OpenDialogWindowCommand = new RelayCommand(OpenDialogWindow);
            SearchCommand = new RelayCommand(Search);

            RefreshDataGrid = new RelayCommand(OnRefreshDataGrid);

            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();
                Reports = new ObservableCollection<Report>(dbContext.Reports.ToList());
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
            else OnPropertyChanged(nameof(Reports));
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
                    Reports = new ObservableCollection<Report>(dbContext.Reports.ToList());
                    return;
                }

                List<Report> search_result;
                switch (Selected_Index)
                {
                    case 0:
                        App.WriteMsg2Log("Поиск отчётов на странице \"Отчёты\" по параметру \"Дата\"", LoggerTypes.INFO);

                        // date
                        double date = -1;
                        DateTime searchDateTime;

                        if (DateTime.TryParseExact(Search_Value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDateTime))
                            date = searchDateTime.Date.Subtract(DateTime.MinValue).TotalMilliseconds;

                        search_result = (from report in dbContext.Reports
                                         where report._date == searchDateTime.Date
                                         select report).ToList();

                        if (search_result != null)
                            Reports = new ObservableCollection<Report>(search_result);

                        break;
                    case 1:
                        App.WriteMsg2Log("Поиск отчётов на странице \"Отчёты\" по параметру \"Фракция\"", LoggerTypes.INFO);

                        // fraction
                        search_result = (from report in dbContext.Reports
                                         where report._fraction.Equals(Search_Value)
                                         select report).ToList();

                        if (search_result != null)
                            Reports = new ObservableCollection<Report>(search_result);

                        break;
                    case 2:
                        App.WriteMsg2Log("Поиск отчётов на странице \"Отчёты\" по параметру \"Номер партии\"", LoggerTypes.INFO);

                        // partia_number
                        search_result = (from report in dbContext.Reports
                                         where report._partia_number.Equals(Search_Value)
                                         select report).ToList();

                        if (search_result != null)
                            Reports = new ObservableCollection<Report>(search_result);

                        break;
                    case 3:
                        App.WriteMsg2Log("Поиск отчётов на странице \"Отчёты\" по параметру \"Номер смены\"", LoggerTypes.INFO);

                        // smena_number
                        search_result = (from report in dbContext.Reports
                                         where report.User._smena_number.ToString().Equals(Search_Value)
                                         select report).ToList();

                        if (search_result != null)
                            Reports = new ObservableCollection<Report>(search_result);

                        break;
                    case 4:
                        App.WriteMsg2Log("Поиск отчётов на странице \"Отчёты\" по параметру \"Значение\"", LoggerTypes.INFO);

                        // avg_value
                        search_result = (from report in dbContext.Reports
                                         where report._avg_black_pixels_value.ToString().Equals(Search_Value)
                                         select report).ToList();

                        if (search_result != null)
                            Reports = new ObservableCollection<Report>(search_result);

                        break;
                    case 5:
                        App.WriteMsg2Log("Поиск отчётов на странице \"Отчёты\" по параметру \"Примечание\"", LoggerTypes.INFO);

                        // note
                        search_result = (from report in dbContext.Reports
                                         where report._note.Equals(Search_Value)
                                         select report).ToList();

                        if (search_result != null)
                            Reports = new ObservableCollection<Report>(search_result);

                        break;
                }
                #region Поиск по всем параметрам одновременно
                //double date = -1;
                //DateTime searchDateTime;

                //if (DateTime.TryParseExact(Search_Value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDateTime))
                //    date = searchDateTime.Date.Subtract(DateTime.MinValue).TotalMilliseconds;

                //var search_result = (from report in dbContext.Reports
                //                     where report._fraction.Equals(Search_Value) || report._partia_number.Equals(Search_Value) || report._date == date || report.User._smena_number.ToString().Equals(Search_Value) || report._avg_black_pixels_value.ToString().Equals(Search_Value)
                //                     select report).ToList();

                //if (search_result != null)
                //    Reports = new ObservableCollection<Report>(search_result);

                ////return new ObservableCollection<Report>(dbContext.Reports.ToList());
                #endregion
            }
            catch (Exception ex)
            {
                App.WriteMsg2Log("Непредвиденная ошибка во время выполнения! Текст ошибки: " + ex.Message, LoggerTypes.FATAL);
                MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void OpenDialogWindow(object param)
        {
            if (param != null)
            {
                App.WriteMsg2Log("Открытие окна редактирования отчёта. Отчёт #" + (param as Report)._id + ". Страница \"Отчёты\".", LoggerTypes.INFO);

                RootV editWindow = new RootV(param);
                editWindow.ShowDialog();

                App.WriteMsg2Log("Закрытие окна редактирования отчёта!", LoggerTypes.INFO);
            }
        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Dark/Green.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Light/Green.xaml", UriKind.Relative));

            Properties.Settings.Default.SelectedPage = 4;
            Properties.Settings.Default.Save();
        }
    }
}
