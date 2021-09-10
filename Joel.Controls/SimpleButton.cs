using System.Windows;

namespace Joel.Controls
{
    public class SimpleButton : System.Windows.Controls.Button
    {
        static SimpleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleButton), 
                new FrameworkPropertyMetadata(typeof(SimpleButton)));
        }
    }
}
