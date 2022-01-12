using BallScanner.Data;
using BallScanner.Data.Tables;
using BallScanner.MVVM.Base;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class DocumentsVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

            }
        }

        public DocumentsVM()
        {

        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;
            app.CurrentPalette = Palettes.Green;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Green/Dark.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Green/Light.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));

            Properties.Settings.Default.Save();
        }
    }
}
