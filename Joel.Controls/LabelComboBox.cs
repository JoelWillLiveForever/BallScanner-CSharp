using System.Windows;
using System.Windows.Controls;

namespace Joel.Controls
{
    public class LabelComboBox : ComboBox
    {
        // label text
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(LabelComboBox), new PropertyMetadata(default(string)));

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        static LabelComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelComboBox),
                new FrameworkPropertyMetadata(typeof(LabelComboBox)));
        }
    }
}
