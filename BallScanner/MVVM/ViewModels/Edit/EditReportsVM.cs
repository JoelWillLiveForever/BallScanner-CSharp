using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using BallScanner.MVVM.ViewModels.Main;
using System;
using System.Globalization;
using System.Windows;

namespace BallScanner.MVVM.ViewModels.Edit
{
    public class EditReportsVM : BaseViewModel
    {
        private static readonly object global_locker = new object();

        public RelayCommand DeleteReportCommand { get; set; }

        private Report report;
        private Window currDialogWindow;

        public string Title_Text
        {
            get => "Редактирование отчётов. Отчёт #" + report._id;
        }

        // Fields
        public string Date
        {
            get => report._date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            set
            {
                if (report._date == DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture)) return;

                report._date = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                OnPropertyChanged(nameof(Date));

                RefreshDataGrid();
            }
        }

        public int Smena_Number
        {
            get => report.User._smena_number;
            set
            {
                if (report.User._smena_number == value) return;

                report.User._smena_number = value;
                OnPropertyChanged(nameof(Smena_Number));

                RefreshDataGrid();
            }
        }

        public long Avg_Black_Pixels_Value
        {
            get => report._avg_black_pixels_value;
            set
            {
                if (report._avg_black_pixels_value == value) return;

                report._avg_black_pixels_value = value;
                OnPropertyChanged(nameof(Avg_Black_Pixels_Value));

                RefreshDataGrid();
            }
        }

        public string Fraction
        {
            get => report._fraction;
            set
            {
                if (report._fraction == value) return;

                report._fraction = value;
                OnPropertyChanged(nameof(Fraction));

                RefreshDataGrid();
            }
        }

        public string Partia_Number
        {
            get => report._partia_number;
            set
            {
                if (report._partia_number == value) return;

                report._partia_number = value;
                OnPropertyChanged(nameof(Partia_Number));

                RefreshDataGrid();
            }
        }

        public string Note
        {
            get => report._note;    
            set
            {
                if (report._note == value) return;

                report._note = value;
                OnPropertyChanged(nameof(Note));

                RefreshDataGrid();
            }
        }

        // Access levels
        private bool _note_isReadOnly = false;
        public bool Note_IsReadOnly
        {
            get
            {
                // superuser
                if (App.CurrentUser == null)
                    return false;

                switch (App.CurrentUser._access_level)
                {
                    case 0:
                    case 1:
                        // user or admin
                        _note_isReadOnly = false;
                        break;
                }

                return _note_isReadOnly;
            }
            set
            {
                if (_note_isReadOnly == value) return;

                _note_isReadOnly = value;
                OnPropertyChanged(nameof(Note_IsReadOnly));
            }
        }

        private bool otherFields_isReadOnly = true;
        public bool OtherFields_IsReadOnly
        {
            get
            {
                // superuser
                if (App.CurrentUser == null)
                    return false;

                switch (App.CurrentUser._access_level)
                {
                    case 0:
                        // user
                        otherFields_isReadOnly = true;
                        break;
                    case 1:
                        // admin
                        otherFields_isReadOnly = false;
                        break;
                }

                return otherFields_isReadOnly;
            }
            set
            {
                if (otherFields_isReadOnly == value) return;

                otherFields_isReadOnly = value;
                OnPropertyChanged(nameof(OtherFields_IsReadOnly));
            }
        }

        public EditReportsVM()
        {
            DeleteReportCommand = new RelayCommand(Delete_Report);
        }

        public EditReportsVM(Report report, Window currDialogWindow)
        {
            DeleteReportCommand = new RelayCommand(Delete_Report);

            this.report = report;
            this.currDialogWindow = currDialogWindow;
        }

        private void RefreshDataGrid()
        {
            if (DocumentsVM.RefreshDataGrid.CanExecute(null))
                DocumentsVM.RefreshDataGrid.Execute(null);

            if (AccountManagmentVM.RefreshDataGrid.CanExecute(null))
                AccountManagmentVM.RefreshDataGrid.Execute(null);
        }

        private void Delete_Report(object param)
        {
            try
            {
                lock (global_locker)
                {
                    AppDbContext dbContext = AppDbContext.GetInstance();

                    dbContext.Reports.Remove(report);
                    dbContext.SaveChanges();
                }

                currDialogWindow.Close();
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
