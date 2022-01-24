using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Joel.Controls
{
    public class MenuButton : RadioButton
    {
        // Min state toggle
        public static readonly DependencyProperty IsMinStateProperty =
            DependencyProperty.Register(nameof(IsMinState), typeof(bool), typeof(MenuButton), new PropertyMetadata(default(bool)));

        public bool IsMinState
        {
            get { return (bool)GetValue(IsMinStateProperty); }
            set { SetValue(IsMinStateProperty, value); }
        }

        // Icon
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(MenuButton), new PropertyMetadata(default(Geometry)));

        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        
        // Icon Width & Height
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(MenuButton), new PropertyMetadata(default(double)));

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(MenuButton), new PropertyMetadata(default(double)));

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        // Icon Fill & Stroke
        public static readonly DependencyProperty IconFillProperty =
            DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(MenuButton), new PropertyMetadata(default(Brush)));

        public Brush IconFill
        {
            get { return (Brush)GetValue(IconFillProperty); }
            set { SetValue(IconFillProperty, value); }
        }

        //public static readonly DependencyProperty IconStrokeProperty =
        //    DependencyProperty.Register(nameof(IconStroke), typeof(Brush), typeof(MenuButton), new PropertyMetadata(default(Brush)));

        //public Brush IconStroke
        //{
        //    get { return (Brush)GetValue(IconStrokeProperty); }
        //    set { SetValue(IconStrokeProperty, value); }
        //}

        // Icon position
        public static readonly DependencyProperty IconPositionProperty =
            DependencyProperty.Register(nameof(IconPosition), typeof(Dock), typeof(MenuButton), new PropertyMetadata(default(Dock)));

        public Dock IconPosition
        {
            get { return (Dock)GetValue(IconPositionProperty); }
            set { SetValue(IconPositionProperty, value); }
        }

        // Text alingment
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(MenuButton), new PropertyMetadata(default(TextAlignment)));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        // Corner radius
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(MenuButton), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }


        static MenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuButton), new FrameworkPropertyMetadata(typeof(MenuButton)));
        }
    }
}
