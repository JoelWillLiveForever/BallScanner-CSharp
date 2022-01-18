using System.Windows;
using System.Windows.Controls;

namespace Joel.Controls
{
    public class LabelTextBox : TextBox
    {
        // label text
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(LabelTextBox), new PropertyMetadata(default(string)));

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        static LabelTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelTextBox),
                new FrameworkPropertyMetadata(typeof(LabelTextBox)));
        }
    }
}
