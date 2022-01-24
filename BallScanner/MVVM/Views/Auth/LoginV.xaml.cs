using System;
using System.Reflection;
using System.Windows.Controls;

namespace BallScanner.MVVM.Views.Auth
{
    public partial class LoginV : UserControl
    {
        public LoginV()
        {
            InitializeComponent();

            var version = Assembly.GetEntryAssembly().GetName().Version;
            var buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
            TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
            TimeSpan.TicksPerSecond * 2 * version.Revision)); // seconds since midnight, (multiply by 2 to get original)

            Version_Field.Text = "v." + version.ToString() + " beta";
            Build_Field.Text = "Build " + buildDateTime.ToString("dd-MM-yyyy");
        }

        //private void Login_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    Console.WriteLine("ВВОД ТЕКСТА!");

        //    Login.SelectionLength = Login.Text.Length;
        //    Login.SelectionStart = Login.Text.Length;

        //    Login.CaretIndex = Login.Text.Length;
        //    var rect = Login.GetRectFromCharacterIndex(Login.CaretIndex);
        //    Login.ScrollToHorizontalOffset(rect.Right);
        //    Login.ScrollToHome();
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //Application.Current.MainWindow = null;
        //    //RootV rootV = new RootV();
        //    //rootV.Show();
        //    //var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        //    //currentWindow.Close();
        //}
    }
}
