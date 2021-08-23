using System.Windows;

namespace Joel.Controls
{
    public class RadioButton : System.Windows.Controls.RadioButton
    {
        // Icon
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(UIElement), typeof(RadioButton), new PropertyMetadata(default(UIElement)));

        public UIElement Icon
        {
            get { return (UIElement)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        static RadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButton),
                new FrameworkPropertyMetadata(typeof(RadioButton)));
        }
    }
}
