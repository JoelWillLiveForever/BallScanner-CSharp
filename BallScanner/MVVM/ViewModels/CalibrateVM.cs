using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Core;
using BallScanner.MVVM.Models;
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

        //private int _secondThreshold = 128;
        //private int SecondThreshold
        //{
        //    get => _secondThreshold;
        //    set => _secondThreshold = 256 - value;
        //}

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
                    Bitmap bitmap;
                    if (ThresholdValue == 0)
                    {
                        bitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap.Width,
                            imagesArr[currentImageIndex].Bitmap.Height,
                            System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
                    }
                    else
                    {
                        //SecondThreshold = ThresholdValue;
                        bitmap = Threshold(new Bitmap(imagesArr[currentImageIndex].Bitmap), ThresholdValue);
                    }

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
                                //SecondThreshold = ThresholdValue

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

        private Bitmap Threshold(Bitmap bitmap, int threshold)
        {
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetThreshold(threshold, ColorAdjustType.Bitmap);

            System.Drawing.Point[] points =
            {
                new System.Drawing.Point(0, 0),
                new System.Drawing.Point(bitmap.Width, 0),
                new System.Drawing.Point(0, bitmap.Height)
            };

            Rectangle rect =
                new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.DrawImage(bitmap, points, rect, GraphicsUnit.Pixel, attributes);
            }

            bitmap = BitmapTo1Bpp(bitmap);

            return bitmap;
        }

        public static Bitmap BitmapTo1Bpp(Bitmap original)
        {
            var rectangle = new Rectangle(0, 0, original.Width, original.Height);
            return original.Clone(rectangle, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
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
    }
}