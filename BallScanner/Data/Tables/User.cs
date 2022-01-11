using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace BallScanner.Data.Tables
{
    //[Table("Users")]
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

        public User() { }

        public User(string surname, string name, string lastname, int smena_number, string username, string password_hash)
        {
            _surname = surname;
            _name = name;
            _lastname = lastname;

            _smena_number = smena_number;

            _username = username;
            _password_hash = password_hash;

            _is_active = 1;
            _access_level = 2;
        }
    }
}
