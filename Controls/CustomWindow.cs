using System.Windows;
using System.Windows.Controls;

namespace Controls
{
    public class CustomWindow : Control
    {
        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }
    }
}
