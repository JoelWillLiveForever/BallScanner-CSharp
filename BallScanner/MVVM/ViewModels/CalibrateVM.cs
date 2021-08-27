using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Core;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BallScanner.MVVM.ViewModels
{
    public class CalibrateVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private ImageSource m_bitmapBind = null;
        public ImageSource M_bitmapBind 
        {
            get => m_bitmapBind;
            set
            {
                m_bitmapBind = value;
                OnPropertyChanged("M_bitmapBind");
            }
        }

        private int _sliderValue;
        public int SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                OnPropertyChanged("SliderValue");
            }
        }

        // commands
        public RelayCommand ButtonCommand { get; set; }

        public CalibrateVM()
        {
            Log.Info("Вызов конструктора класса");
            Console.WriteLine("CalibrateVM");
            ButtonCommand = new RelayCommand(OnButtonCommand);
            //CurrentModel = new CalibrateM();
        }

        public BitmapFrame BitmapToBitmapSource(Bitmap bitmap)
        {
            // Convert Bitmap to BitmapImage
            MemoryStream str = new MemoryStream();
            bitmap.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            return bdc.Frames[0];
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void OnButtonCommand(object param)
        {
            string sender = param as string;

            switch (sender)
            {
                case "Button_LoadImages":
                    try
                    {
                        OpenFileDialog openFile = new OpenFileDialog();
                        openFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                        if (openFile.ShowDialog() == true)
                        {

                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (UriFormatException ex)
                    {
                        MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;

                case "Button_PrevImage":



                    break;
                case "Button_NextImage":



                    break;

                case "Button_Execute":

                    break;
            }
        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;
            app.CurrentPalette = Palettes.Yellow;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Yellow/Dark.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Dark.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Yellow/Light.xaml", UriKind.Relative),
                                new Uri("Resources/Palettes/Light.xaml", UriKind.Relative));

            Properties.Settings.Default.Save();
        }

        public static class BetterEnumerable
        {
            public static IEnumerable<int> SteppedRange(int fromInclusive, int toExclusive, int step)
            {
                for (var i = fromInclusive; i < toExclusive; i += step)
                {
                    yield return i;
                }
            }
        }
    }
}

#region
//using (var context = new Context())
//{
//    // Priority search
//    Accelerator accelerator = null;
//    for (int i = 0; i < 3; i++)
//    {
//        foreach (var acceleratorId in Accelerator.Accelerators)
//        {
//            if (i == 0 && acceleratorId.AcceleratorType == AcceleratorType.Cuda ||
//                i == 1 && acceleratorId.AcceleratorType == AcceleratorType.OpenCL ||
//                i == 2 && acceleratorId.AcceleratorType == AcceleratorType.CPU)
//            {
//                accelerator = Accelerator.Create(context, acceleratorId);
//                goto Found;
//            }
//        }
//    }

//    if (accelerator == null)
//    {
//        _logger.LogError("Accelerator was null at: {time}", DateTimeOffset.UtcNow);
//        // error
//    }

//Found:
//    var kernel = accelerator.LoadAutoGroupedStreamKernel<Index1, ArrayView<int>, int>(MyKernel);

//    using (var buffer = accelerator.Allocate<int>(1024))
//    {
//        kernel(buffer.Length, buffer.View, 42);

//        accelerator.Synchronize();

//        var data = buffer.GetAsArray();
//        for (int i = 0; i < data.Length; i++)
//        {

//        }
//    }

//    break;
//}
#endregion