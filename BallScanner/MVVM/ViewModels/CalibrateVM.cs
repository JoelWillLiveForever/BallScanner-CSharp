using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Core;
using BallScanner.MVVM.Models;
using ILGPU;
using ILGPU.Runtime;
using Joel.Utils.Helpers;
using Microsoft.Win32;
using NLog;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace BallScanner.MVVM.ViewModels
{
    public class CalibrateVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static RelayCommand PerformAction { get; set; }

        private ImageData[] imagesArr = null;
        private int currentImageIndex = 0;

        private ImageSource _currentImageSource;
        public ImageSource CurrentImageSource
        {
            get => _currentImageSource;
            set
            {
                _currentImageSource = value;
                OnPropertyChanged(nameof(CurrentImageSource));
            }
        }

        private int _thresholdValue = 128;
        public int ThresholdValue
        {
            get => _thresholdValue;
            set
            {
                _thresholdValue = value;
                OnPropertyChanged(nameof(ThresholdValue));

                if (imagesArr != null)
                {
                    Bitmap bitmap = Threshold(new Bitmap(imagesArr[currentImageIndex].Bitmap), ThresholdValue);
                    CurrentImageSource = Converters.ImageSourceFromBitmap(bitmap);
                }
            }
        }

        public CalibrateVM()
        {
            Log.Info("Constructor called!");
            PerformAction = new RelayCommand(OnPerformAction);
        }

        private void OnPerformAction(object param)
        {
            string sender = param as string;

            switch (sender)
            {
                case "Button_LoadImages":
                    try
                    {
                        OpenFileDialog openFile = new OpenFileDialog();
                        openFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";
                        openFile.Multiselect = true;

                        if (openFile.ShowDialog() == true)
                        {
                            imagesArr = new ImageData[openFile.FileNames.Length];

                            for (int i = 0; i < imagesArr.Length; i++)
                            {
                                Bitmap bitmapImage = new Bitmap(openFile.FileNames[i]);

                                imagesArr[i] = new ImageData()
                                {
                                    Name = openFile.SafeFileNames[i],
                                    NumberOfBlackPixels = -1,
                                    BallGrade = BallGrade.INDEFINED,
                                    Bitmap = bitmapImage,
                                    ImageSource = Converters.ImageSourceFromBitmap(Threshold(new Bitmap(bitmapImage), ThresholdValue))
                                };
                            }

                            CurrentImageSource = imagesArr[currentImageIndex].ImageSource;

                            //var context = new Context();

                            //// Priority search
                            //Accelerator accelerator = null;
                            //for (int i = 0; i < 3; i++)
                            //{
                            //    foreach (var acceleratorId in Accelerator.Accelerators)
                            //    {
                            //        if (i == 0 && acceleratorId.AcceleratorType == AcceleratorType.Cuda ||
                            //            i == 1 && acceleratorId.AcceleratorType == AcceleratorType.OpenCL ||
                            //            i == 2 && acceleratorId.AcceleratorType == AcceleratorType.CPU)
                            //        {
                            //            accelerator = Accelerator.Create(context, acceleratorId);
                            //            goto Found;
                            //        }
                            //    }
                            //}

                            //if (accelerator == null)
                            //    Log.Error("Accelerator is null!");

                            //Found:
                            //var bitmapData = CurrentImage.Bitmap.LockBits(new Rectangle(0, 0, CurrentImage.Bitmap.Width, CurrentImage.Bitmap.Height),
                            //    ImageLockMode.ReadOnly,
                            //    PixelFormat.Format24bppRgb);

                            //// input data
                            //byte[] pixelData = new byte[bitmapData.Stride * bitmapData.Height];
                            //Marshal.Copy(bitmapData.Scan0, pixelData, 0, pixelData.Length);

                            //var kernel = accelerator.LoadAutoGroupedStreamKernel<Index1, ArrayView<int>, int>(MyKernel);
                            //var buffer = accelerator.Allocate<int>(1024);

                            //kernel(buffer.Length, buffer.View, ThresholdValue);

                            //accelerator.Synchronize();

                            //var data = buffer.GetAsArray();

                            //for (int i = 0; i < data.Length; i++)
                            //{

                            //}

                            break;
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

                    if (imagesArr != null && currentImageIndex > 0) 
                        currentImageIndex--;
                    break;
                case "Button_NextImage":

                    if (imagesArr != null && currentImageIndex < imagesArr.Length)
                        currentImageIndex++;
                    break;
                case "Button_Execute":

                    break;
            }
        }

        private Bitmap Threshold(Bitmap bitmap, float threshold)
        {
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetThreshold(threshold);

            System.Drawing.Point[] points =
            {
                new System.Drawing.Point(0,0),
                new System.Drawing.Point(bitmap.Width, 0),
                new System.Drawing.Point(0, bitmap.Height)
            };

            Rectangle rect =
                new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.DrawImage(bitmap, points, rect, GraphicsUnit.Pixel, attributes);
            }

            return bitmap;
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

        private static void MyKernel(Index1 index, ArrayView<int> dataView, int constant)
        {
            dataView[index] = dataView[index] + constant;
        }
    }
}