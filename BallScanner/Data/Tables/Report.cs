using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
using System.Timers;
using System.ComponentModel;

namespace BallScanner.Data.Tables
{
    //[Table("Reports")]
    public class Report : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Timer timer = new Timer(5000) { Enabled = false };

        private bool[] isInit = new bool[5] {true, true, true, true, true};

        [Key]
        public int _id { get; set; }

        private DateTime _my_date;
        public DateTime _date
        {
            get => _my_date; 
            set
            {
                if (_my_date == value) return;

                _my_date = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_date)));

                if (!isInit[0])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                }
                else
                {
                    isInit[0] = false;
                }
            }
        }

        private string _my_fraction;
        public string _fraction
        {
            get => _my_fraction;
            set
            {
                if (_my_fraction == value) return;

                _my_fraction = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_fraction)));

                if (!isInit[1])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                }
                else
                {
                    isInit[1] = false;
                }
            }
        }

        private string _my_partia_number;
        public string _partia_number 
        { 
            get => _my_partia_number; 
            set
            {
                if (_my_partia_number == value) return;

                _my_partia_number = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_partia_number)));

                if (!isInit[2])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                }
                else
                {
                    isInit[2] = false;
                }
            }
        }

        private long _my_avg_black_pixels_value;
        public long _avg_black_pixels_value
        {
            get => _my_avg_black_pixels_value;
            set
            {
                if (_my_avg_black_pixels_value == value) return;

                _my_avg_black_pixels_value = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_avg_black_pixels_value)));

                if (!isInit[3])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                }
                else
                {
                    isInit[3] = false;
                }
            }
        }        //public string _note { get; set; }

        private string _my_note;
        public string _note
        {
            get => _my_note;
            set
            {
                if (_my_note == value) return;

                _my_note = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_note)));

                if (!isInit[4])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                } else
                {
                    isInit[4] = false;
                }
            }
        }

        // foreign key
        [ForeignKey("User")]
        public int _user_id { get; set; }
        public virtual User User { get; set; }

        //public string Date
        //{
        //    get => DateTime.MinValue.AddMilliseconds(_date).ToString("dd.MM.yyyy");
        //    //set
        //    //{
        //    //    DateTime searchDateTime;
        //    //    if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDateTime))
        //    //    {
        //    //        double date = searchDateTime.Date.Subtract(DateTime.MinValue).TotalMilliseconds;

        //    //        if (_date == date) return;
        //    //        _date = date;

        //    //        if (PropertyChanged != null)
        //    //            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Date)));
        //    //    } else
        //    //    {
        //    //        MessageBox.Show("Неправильный формат даты! Пожалуйста, введите дату в формате \"dd.MM.yyyy\"!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        //    //    }
        //    //}
        //}

        //public string Note
        //{
        //    get => _note;
        //    set
        //    {
        //        if (_note == value) return;

        //        _note = value;
        //        if (PropertyChanged != null)
        //            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Note)));
        //    }
        //}

        public Report() 
        {
            timer.Elapsed += new ElapsedEventHandler(OnSaveDbContext);
        }

        public Report(int user_id, DateTime date, string fraction, string partia_number, long avg_black_pixels_value)
        {
            _user_id = user_id;
            _date = date;
            _fraction = fraction;
            _partia_number = partia_number;
            _avg_black_pixels_value = avg_black_pixels_value;

            _note = null;

            timer.Elapsed += new ElapsedEventHandler(OnSaveDbContext);
        }

        private void OnSaveDbContext(object source, ElapsedEventArgs e)
        {
            timer.Stop();
            App.WriteMsg2Log("Изменение отчёта. Информация об отчёте: номер = " + _id + "; дата = " + _date + "; фракция = " + _fraction + "; номер партии = " + _partia_number + "; среднее значение чёрных пикселей = " + _avg_black_pixels_value + "; примечание = " + _note + "; номер пользователя, который сделал отчёт = " + _user_id + ";", LoggerTypes.INFO);

            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();
                dbContext.SaveChanges();

                //Console.WriteLine("SAVED!");
            }
            catch (Exception ex)
            {
                App.WriteMsg2Log("Непредвиденная ошибка во время выполнения! Текст ошибки: " + ex.Message, LoggerTypes.FATAL);
                MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
