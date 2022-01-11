using System.ComponentModel.DataAnnotations;

namespace BallScanner.Data.Tables
{
    public class User
    {
        [Key]
        public int _id { get; set; }
        public string _surname { get; set; }
        public string _name { get; set; }
        public string _lastname { get; set; }
        public int _smena_number { get; set; }
        public int _is_active { get; set; }
        public int _access_level { get; set; }
        public string _username { get; set; }
        public string _password_hash { get; set; }
    }
}
