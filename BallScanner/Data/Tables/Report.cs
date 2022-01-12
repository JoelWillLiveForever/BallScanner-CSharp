using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
//using System.Windows;

namespace BallScanner.Data.Tables
{
    //[Table("Reports")]
    public class Report
    {
        [Key]
        public int _id { get; set; }
        public double _date { get; set; }
        public string Date
        {
            get
            {
                Console.WriteLine("\n\n\nDate: " + _date);
                return DateTime.MinValue.AddMilliseconds(_date).ToString("dd.MM.yyyy");
            }
        }

        public string _fraction { get; set; }
        public string _partia_number { get; set; }
        public long _avg_black_pixels_value { get; set; }
        public string _note { get; set; }

        // foreign key
        [ForeignKey("User")]
        public int _user_id { get; set; }
        public virtual User User { get; set; }
        //public int Smena_Number
        //{
        //    get
        //    {
        //        try
        //        {
        //            using (AppDbContext db = new AppDbContext())
        //            {
        //                db.Users.Load();
        //                return User._smena_number;
        //            }
        //        } catch (Exception ex)
        //        {
        //            MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        //        }
        //        return -1;
        //    }
        //}

        public Report() { }

        public Report(int user_id, double date, string fraction, string partia_number, long avg_black_pixels_value)
        {
            _user_id = user_id;
            _date = date;
            _fraction = fraction;
            _partia_number = partia_number;
            _avg_black_pixels_value = avg_black_pixels_value;

            _note = null;
        }
    }
}
