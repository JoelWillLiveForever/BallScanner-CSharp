using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BallScanner.MVVM.Views.Main
{
    public partial class ScanV : UserControl
    {
        private DataTemplate DefaultState;
        private DataTemplate MinState;

        private bool isMinState;

        public ScanV()
        {
            InitializeComponent();

            DefaultState = FindResource("DefaultState") as DataTemplate;
            MinState = FindResource("MinState") as DataTemplate;
        }

        private void Border_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (DefaultState == null || MinState == null) return;

            if (e.NewSize.Width < 1007)
            {
                if (MyContentControl.ContentTemplate == MinState) return;
                MyContentControl.ContentTemplate = MinState;

                isMinState = true;
                App.WriteMsg2Log("Изменено состояние страницы \"Сканирование\" на \"Компактное состояние\"", LoggerTypes.INFO);
            }
            else
            {
                if (MyContentControl.ContentTemplate == DefaultState) return;
                MyContentControl.ContentTemplate = DefaultState;

                isMinState = false;
                App.WriteMsg2Log("Изменено состояние страницы \"Сканирование\" на \"Обычное состояние\"", LoggerTypes.INFO);
            }
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (isMinState)
            {
                foreach (var sv in FindVisualChildren<ScrollViewer>(this))
                {
                    if (sv.Name == "sv")
                    {
                        sv.ScrollToVerticalOffset(sv.ContentVerticalOffset - e.Delta);
                    }
                }
            }
        }

        // Поиск элемента в DataTemplate
        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }
}
