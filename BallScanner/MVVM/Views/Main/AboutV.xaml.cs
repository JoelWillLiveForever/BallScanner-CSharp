using System;
using System.Reflection;
using System.Windows.Controls;

namespace BallScanner.MVVM.Views.Main
{
    public partial class AboutV : UserControl
    {
        public AboutV()
        {
            InitializeComponent();

            var version = Assembly.GetEntryAssembly().GetName().Version;
            var buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
            TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
            TimeSpan.TicksPerSecond * 2 * version.Revision)); // seconds since midnight, (multiply by 2 to get original)

            Version_Field.Text = "Версия: " + version.ToString() + " beta";
            Build_Field.Text = "Сборка от: " + buildDateTime.ToString("dd-MM-yyyy");
        }
    }
}
