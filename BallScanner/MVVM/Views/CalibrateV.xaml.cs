using BallScanner.MVVM.ViewModels;
using Joel.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BallScanner.MVVM.Views
{
    public partial class CalibrateV : UserControl
    {
        private DataTemplate DefaultState;
        private DataTemplate MinState;

        public CalibrateV()
        {
            InitializeComponent();

            DefaultState = FindResource("DefaultState") as DataTemplate;
            MinState = FindResource("MinState") as DataTemplate;
        }

        private void BlockHeader_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            BlockHeader blockHeader = sender as BlockHeader;

            var formattedText = new FormattedText(blockHeader.Text,
                CultureInfo.CurrentCulture, 
                blockHeader.FlowDirection,
                new Typeface(blockHeader.FontFamily, blockHeader.FontStyle, blockHeader.FontWeight, blockHeader.FontStretch),
                blockHeader.FontSize,
                blockHeader.Foreground,
                new NumberSubstitution(), 
                1);

            if (blockHeader.ActualWidth >= formattedText.Width && blockHeader.FontSize == 14) return;

            if (blockHeader.ActualWidth >= formattedText.Width && blockHeader.FontSize < 14)
            {
                blockHeader.FontSize += 0.5;
            }
            
            blockHeader.FontSize -= 0.5;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            SetMinWidths(sender);
        }

        private void SetMinWidths(object source)
        {
            foreach (var column in (source as DataGrid).Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void Slider_KeyDown(object sender, KeyEventArgs e)
        {
            Slider slider = sender as Slider;

            switch (e.Key)
            {
                case Key.Left:
                    slider.Value -= 1;
                    break;
                case Key.Right:
                    slider.Value += 1;
                    break;
            }
        }

        private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (CalibrateVM.PerformAction.CanExecute("Slider_DragStarted"))
                CalibrateVM.PerformAction.Execute("Slider_DragStarted");
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (CalibrateVM.PerformAction.CanExecute("Slider_DragCompeled"))
                CalibrateVM.PerformAction.Execute("Slider_DragCompleted");
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DefaultState == null || MinState == null) return;

            if (e.NewSize.Width < 1007)
            {
                if (MyContentControl.ContentTemplate == MinState) return; 
                MyContentControl.ContentTemplate = MinState;
            }
            else
            {
                if (MyContentControl.ContentTemplate == DefaultState) return;
                MyContentControl.ContentTemplate = DefaultState;
            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // execute some code
            Console.WriteLine("Double Ckick");
        }

        //public System.Windows.Size GetElementPixelSize(UIElement element)
        //{
        //    Matrix transformToDevice;
        //    var source = PresentationSource.FromVisual(element);
        //    if (source != null)
        //        transformToDevice = source.CompositionTarget.TransformToDevice;
        //    else
        //        using (var local_source = new HwndSource(new HwndSourceParameters()))
        //            transformToDevice = local_source.CompositionTarget.TransformToDevice;

        //    if (element.DesiredSize == new System.Windows.Size())
        //        element.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));

        //    return (System.Windows.Size)transformToDevice.Transform((Vector)element.DesiredSize);
        //}

        //private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    Grid grid = sender as Grid;
        //    if (e.NewSize.Width < 640)
        //    {
        //        grid.RowDefinitions.Clear();
        //        grid.ColumnDefinitions.Clear();

        //        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        //        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

        //        Grid.SetColumn(FirstGrid, 0);
        //        Grid.SetColumn(SecondGrid, 0);

        //        Grid.SetRow(FirstGrid, 0);
        //        Grid.SetRow(SecondGrid, 1);
        //    }
        //    else
        //    {
        //        grid.RowDefinitions.Clear();
        //        grid.ColumnDefinitions.Clear();

        //        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.315, GridUnitType.Star) });
        //        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

        //        Grid.SetColumn(FirstGrid, 0);
        //        Grid.SetColumn(SecondGrid, 1);

        //        Grid.SetRow(FirstGrid, 0);
        //        Grid.SetRow(SecondGrid, 0);
        //    }
        //}
    }
}
