using System.Windows;

namespace Millennium.Controls
{
    public partial class MillenniumWindow : Window
    {
        static MillenniumWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MillenniumWindow),
                new FrameworkPropertyMetadata(typeof(MillenniumWindow)));
        }
    }
}
