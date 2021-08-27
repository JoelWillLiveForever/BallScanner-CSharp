using Joel.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BallScanner.MVVM.Views
{
    public partial class CalibrateV : UserControl
    {
        public CalibrateV()
        {
            InitializeComponent();
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
    }
}
