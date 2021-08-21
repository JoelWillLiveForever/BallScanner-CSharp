using System.Windows;
using System.Windows.Media;

namespace BallScanner.Utils
{
    public class Extensions
    {
        public static readonly DependencyProperty IconBackground =
            DependencyProperty.RegisterAttached("IconBackground", typeof(Brush), typeof(Extensions), new PropertyMetadata(default(Brush)));
        public static readonly DependencyProperty IconForeground =
            DependencyProperty.RegisterAttached("IconForeground", typeof(Brush), typeof(Extensions), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty OverWidthProperty =
            DependencyProperty.RegisterAttached("OverWidth", typeof(double), typeof(Extensions), new PropertyMetadata(default(double)));
    
        public static void SetIconBackground(UIElement element, Brush brush)
        {
            element.SetValue(IconBackground, brush);
        }

        public static Brush GetIconBackground(UIElement element)
        {
            return (Brush)element.GetValue(IconBackground);
        }

        public static void SetIconForeground(UIElement element, Brush brush)
        {
            element.SetValue(IconForeground, brush);
        }

        public static Brush GetIconForeground(UIElement element)
        {
            return (Brush)element.GetValue(IconForeground);
        }

        public static void SetOverWidth(UIElement element, double value)
        {
            element.SetValue(OverWidthProperty, value);
        }

        public static double GetOverWidth(UIElement element)
        {
            return (double)element.GetValue(OverWidthProperty);
        }
    }
}
