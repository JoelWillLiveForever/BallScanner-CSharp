using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using NLog;
using System;
using System.Windows;
using System.Linq;
using BallScanner.MVVM.Commands;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class DocumentsVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RelayCommand OpenDialogWindowCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }

        public RelayCommand UpdateCommand { get; set; }

        private ObservableCollection<Report> _reports;
        public ObservableCollection<Report> Reports
        {
            get => _reports;
            set
            {
                if (_reports == value) return;

                _reports = value;
                OnPropertyChanged(nameof(Reports));

                Console.WriteLine("EDIT!");
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

                Search(null);
            }
        }

        public DocumentsVM()
        {
            OpenDialogWindowCommand = new RelayCommand(OpenDialogWindow);
            SearchCommand = new RelayCommand(Search);
            UpdateCommand = new RelayCommand(Update);

            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();
                Reports = new ObservableCollection<Report>(dbContext.Reports.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        public void UpdateDataGrid()
        {
            if (Search_Value != null || Search_Value != "" || Search_Value.Length != 0) Search(null);
            else OnPropertyChanged(nameof(Reports));
        }

        private void Search(object param)
        {
            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();

                Console.WriteLine("SEARCH VALUE = " + Search_Value);

                if (Search_Value == null || Search_Value == "" || Search_Value.Length == 0)
                {
                    Reports = new ObservableCollection<Report>(dbContext.Reports.ToList());
                    return;
                }

                double date = -1;
                DateTime searchDateTime;

                if (DateTime.TryParseExact(Search_Value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDateTime))
                {
                    Console.WriteLine(searchDateTime.ToString());
                    date = searchDateTime.Date.Subtract(DateTime.MinValue).TotalMilliseconds;
                }

                var search_result = (from report in dbContext.Reports
                                     where report._fraction.Equals(Search_Value) || report._partia_number.Equals(Search_Value) || report._date == date || report.User._smena_number.ToString().Equals(Search_Value) || report._avg_black_pixels_value.ToString().Equals(Search_Value)
                                     select report).ToList();

                if (search_result != null)
                    Reports = new ObservableCollection<Report>(search_result);

                //return new ObservableCollection<Report>(dbContext.Reports.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void Update(object param)
        {
            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            Console.WriteLine("ВСЕ НОУТЫ:");
            foreach (var item in Reports)
            {
                Console.WriteLine(item._note);
            }
        }

        private void OpenDialogWindow(object param)
        {
            Report data = param as Report;
            int currentImageIndex = data._id;


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
