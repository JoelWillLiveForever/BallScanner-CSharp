using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;

using System.Data.Entity;
using System.Windows;
using System.ComponentModel;

namespace BallScanner.Data.Tables
{
    //[Table("Reports")]
    public class Report
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Timer timer = new Timer(5000) { Enabled = false };

        private bool isInit = true;

        [Key]
        public int _id { get; set; }
        public double _date { get; set; }
        public string _fraction { get; set; }
        public string _partia_number { get; set; }
        public long _avg_black_pixels_value { get; set; }
        //public string _note { get; set; }

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

                if (!isInit)
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                } else
                {
                    isInit = false;
                }
            }
        }

        // foreign key
        [ForeignKey("User")]
        public int _user_id { get; set; }
        public virtual User User { get; set; }

        public string Date
        {
            get => DateTime.MinValue.AddMilliseconds(_date).ToString("dd.MM.yyyy");
        }

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

        public Report(int user_id, double date, string fraction, string partia_number, long avg_black_pixels_value)
        {
            _user_id = user_id;
            _date = date;
            _fraction = fraction;
            _partia_number = partia_number;
            _avg_black_pixels_value = avg_black_pixels_value;

            _note = null;
        }

        private void OnSaveDbContext(object source, ElapsedEventArgs e)
        {
            timer.Stop();

            try
            {
                AppDbContext dbContext = AppDbContext.GetInstance();
                dbContext.SaveChanges();

                Console.WriteLine("SAVED!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
