using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Joel.Controls
{
    public partial class CustomWindow : Window
    {
        private bool isMouseButtonDown;
        private Point mouseDownPosition;
        private Point positionBeforeDrag;

        public Grid WindowRoot { get; private set; }
        public Grid LayoutRoot { get; private set; }

        // Buttons
        public Button MinimizeButton { get; private set; }
        public Button MaximizeButton { get; private set; }
        public Button RestoreButton { get; private set; }
        public Button CloseButton { get; private set; }

        // Header
        public Grid HeaderBar { get; private set; }

        public T GetRequiredTemplateChild<T>(string childName) where T : DependencyObject
        {
            return (T)base.GetTemplateChild(childName);
        }

        // Set links to UI elements before showing them to the user
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.WindowRoot = this.GetRequiredTemplateChild<Grid>("WindowRoot");
            this.LayoutRoot = this.GetRequiredTemplateChild<Grid>("LayoutRoot");

            this.MinimizeButton = this.GetRequiredTemplateChild<Button>("MinimizeButton");
            this.MaximizeButton = this.GetRequiredTemplateChild<Button>("MaximizeButton");
            this.RestoreButton = this.GetRequiredTemplateChild<Button>("RestoreButton");
            this.CloseButton = this.GetRequiredTemplateChild<Button>("CloseButton");

            this.HeaderBar = this.GetRequiredTemplateChild<Grid>("PART_HeaderBar");

            if (this.CloseButton != null)
            {
                this.CloseButton.Click += CloseButton_Click;
            }

            if (this.MinimizeButton != null)
            {
                this.MinimizeButton.Click += MinimizeButton_Click;
            }

            if (this.RestoreButton != null)
            {
                this.RestoreButton.Click += RestoreButton_Click;
            }

            if (this.MaximizeButton != null)
            {
                this.MaximizeButton.Click += MaximizeButton_Click;
            }

            if (this.HeaderBar != null)
            {
                this.HeaderBar.AddHandler(Grid.MouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(this.OnHeaderBarMouseLeftButtonDown));
            }
        }

        protected virtual void OnHeaderBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(this);
            int headerBarHeight = 36;
            int leftmostClickableOffset = 50;

            if (position.X - this.LayoutRoot.Margin.Left <= leftmostClickableOffset && position.Y <= headerBarHeight)
            {
                if (e.ClickCount != 2)
                {

                }
                else
                {
                    base.Close();
                }

                e.Handled = true;
                return;
            }

            if (e.ClickCount == 2 && base.ResizeMode == ResizeMode.CanResize)
            {
                this.ToggleWindowState();
                return;
            }

            if (base.WindowState == WindowState.Maximized)
            {
                this.isMouseButtonDown = true;
                this.mouseDownPosition = position;
            }
            else
            {
                try
                {
                    this.positionBeforeDrag = new Point(base.Left, base.Top);
                    base.DragMove();
                } 
                catch
                {

                }
            }
        }

        protected void ToggleWindowState()
        {
            if (base.WindowState != WindowState.Maximized)
            {
                base.WindowState = WindowState.Maximized;
            }
            else
            {
                base.WindowState = WindowState.Normal;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleWindowState();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleWindowState();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }
    }
}
