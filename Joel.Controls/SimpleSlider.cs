using System.Windows;

namespace Joel.Controls
{
    public class SimpleSlider : System.Windows.Controls.Slider
    {
        static SimpleSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleSlider),
                new FrameworkPropertyMetadata(typeof(SimpleSlider)));
        }
    }
}
