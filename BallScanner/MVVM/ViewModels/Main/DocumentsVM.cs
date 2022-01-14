using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using BallScanner.MVVM.Commands;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class DocumentsVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RelayCommand OpenDialogWindowCommand { get; set; }

        public List<Report> Reports
        {
            get
            {
                try
                {
                    AppDbContext dbContext = AppDbContext.GetInstance();
                    return dbContext.Reports.ToList();
                } catch (Exception ex)
                {
                    MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                return null;
            }
            set
            {
                //if (Reports == value) return;

                //Reports = value;
                //OnPropertyChanged(nameof(Reports));
            }
        }

        public DocumentsVM()
        {
            OpenDialogWindowCommand = new RelayCommand(OpenDialogWindow);
        }

        public void UpdateDataGrid()
        {
            OnPropertyChanged(nameof(Reports));
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
