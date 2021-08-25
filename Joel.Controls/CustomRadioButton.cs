using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Joel.Controls
{
    public class CustomRadioButton : RadioButton
    {
        // Image
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(nameof(Image), typeof(UIElement), typeof(CustomRadioButton), new PropertyMetadata(default(UIElement)));

        public UIElement Image
        {
            get { return (UIElement)GetValue(ImageProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Image Width & Height
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(nameof(ImageWidth), typeof(double), typeof(CustomRadioButton), new PropertyMetadata(default(double)));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register(nameof(ImageHeight), typeof(double), typeof(CustomRadioButton), new PropertyMetadata(default(double)));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Icon
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(CustomRadioButton), new PropertyMetadata(default(Geometry)));

        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Icon Fill & Stroke
        public static readonly DependencyProperty IconFillProperty =
            DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(default(Brush)));

        public Brush IconFill
        {
            get { return (Brush)GetValue(IconFillProperty); }
            set { SetValue(IconFillProperty, value); }
        }

        // Icon Width & Height
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(CustomRadioButton), new PropertyMetadata(default(double)));

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(CustomRadioButton), new PropertyMetadata(default(double)));

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        static CustomRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomRadioButton),
                new FrameworkPropertyMetadata(typeof(CustomRadioButton)));
        }
    }
}
