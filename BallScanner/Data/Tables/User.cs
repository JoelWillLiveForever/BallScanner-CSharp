using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Timers;
using System.Windows;
//using System.ComponentModel.DataAnnotations.Schema;

namespace BallScanner.Data.Tables
{
    //[Table("Users")]
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Timer timer = new Timer(5000) { Enabled = false };

        private bool[] isInit = new bool[8] { true, true, true, true, true, true, true, true };

        [Key]
        public int _id { get; set; }

        private string _my_surname;
        public string _surname
        {
            get => _my_surname;
            set
            {
                if (_my_surname == value) return;

                _my_surname = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_surname)));

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

        private string _my_name;
        public string _name
        {
            get => _my_name;
            set
            {
                if (_my_name == value) return;

                _my_name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_name)));

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

        private string _my_lastname;
        public string _lastname
        {
            get => _my_lastname;
            set
            {
                if (_my_lastname == value) return;

                _my_lastname = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_lastname)));

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

        private int _my_smena_number;
        public int _smena_number
        {
            get => _my_smena_number;
            set
            {
                if (_my_smena_number == value) return;

                _my_smena_number = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_smena_number)));

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
        }

        private int _my_is_active;
        public int _is_active
        {
            get => _my_is_active;
            set
            {
                if (_my_is_active == value) return;

                _my_is_active = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_is_active)));

                if (!isInit[4])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                }
                else
                {
                    isInit[4] = false;
                }
            }
        }

        private int _my_access_level;
        public int _access_level
        {
            get => _my_access_level;
            set
            {
                if (_my_access_level == value) return;

                _my_access_level = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_access_level)));

                if (!isInit[5])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                }
                else
                {
                    isInit[5] = false;
                }
            }
        }

        private string _my_username;
        public string _username
        {
            get => _my_username;
            set
            {
                if (_my_username == value) return;

                _my_username = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_username)));

                if (!isInit[6])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                }
                else
                {
                    isInit[6] = false;
                }
            }
        }

        private string _my_password_hash;
        public string _password_hash
        {
            get => _my_password_hash;
            set
            {
                if (_my_password_hash == value) return;

                _my_password_hash = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(_password_hash)));

                if (!isInit[7])
                {
                    // save info when 5 sec no activity
                    timer.Stop();
                    timer.Start();
                }
                else
                {
                    isInit[7] = false;
                }
            }
        }

        // Все отчёты пользователя
        public IList<Report> Reports { get; set; }

        public User() 
        {
            timer.Elapsed += new ElapsedEventHandler(OnSaveDbContext);
        }

        public User(string surname, string name, string lastname, int smena_number, string username, string password_hash)
        {
            _surname = surname;
            _name = name;
            _lastname = lastname;

            _smena_number = smena_number;

            _username = username;
            _password_hash = password_hash;

            _is_active = 1;
            _access_level = 0;

            timer.Elapsed += new ElapsedEventHandler(OnSaveDbContext);
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
