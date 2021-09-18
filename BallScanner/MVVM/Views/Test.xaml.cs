using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BallScanner.MVVM.Views
{
    class Person
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public byte Age { get; set; }
    }

    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();

            Person[] persons = new Person[10];
            for (int i = 0; i < persons.Length; i++)
                persons[i] = new Person()
                {
                    Name = "Name",
                    Surname = "Surname",
                    Age = 10
                };

            TestDataGrid.ItemsSource = persons;
        }
    }
}
