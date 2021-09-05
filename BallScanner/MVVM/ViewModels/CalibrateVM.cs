using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Core;
using BallScanner.MVVM.Models;
using Microsoft.Win32;
using NLog;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BallScanner.MVVM.ViewModels
{
    public class CalibrateVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static RelayCommand PerformAction { get; set; }
        private System.Timers.Timer resizeTimer = new System.Timers.Timer(100) { Enabled = false };
        //public static RelayCommand UpdateDecodeImageHeight { get; set; }

        private int _decodePixelHeight;
        public int DecodePixelHeight
        {
            get => _decodePixelHeight;
            set
            {
                _decodePixelHeight = value;

                PresentationSource source = PresentationSource.FromVisual(Application.Current.MainWindow);

                double scaleY = 0;
                if (source != null)
                {
                    scaleY = source.CompositionTarget.TransformToDevice.M22;
                }

                _decodePixelHeight = (int)(_decodePixelHeight * scaleY);

                resizeTimer.Stop();
                resizeTimer.Start();
            }
        }

        private void ResizingDone(object sender, ElapsedEventArgs e)
        {
            resizeTimer.Stop();

            if (Data != null)
            {
                UpdateImage();
            }
        }

        private ImageData[] _data;
        public ImageData[] Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private int currentImageIndex;

        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        //private BitmapImage _currentImageSource;
        //public BitmapImage CurrentImageSource
        //{
        //    get => _currentImageSource;
        //    set
        //    {
        //        _currentImageSource = value;
        //        OnPropertyChanged(nameof(CurrentImageSource));
        //    }
        //}

        //private int _secondThreshold = 128;
        //private int SecondThreshold
        //{
        //    get => _secondThreshold;
        //    set => _secondThreshold = 256 - value;
        //}

        private byte _thresholdValue = 128;
        public byte ThresholdValue
        {
            get => _thresholdValue;
            set
            {
                _thresholdValue = value;
                OnPropertyChanged(nameof(ThresholdValue));
            }
        }

        public CalibrateVM()
        {
            Log.Info("Constructor called!");
            PerformAction = new RelayCommand(OnPerformAction);
            resizeTimer.Elapsed += new ElapsedEventHandler(ResizingDone);
            //UpdateDecodeImageHeight = new RelayCommand(OnUpdateDecodeImageHeight);
        }

        //private void OnUpdateDecodeImageHeight(object param)
        //{
        //    if (param == null) return;
        //    if (!double.TryParse(param as string, out double actualHeight)) return;

        //    using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
        //    {
        //        decodeImageHeight = (int)(g.DpiY / 96.0d * actualHeight);
        //    }

        //    Console.WriteLine("decodeImageHeight = " + decodeImageHeight);
        //}

        private void OnPerformAction(object param)
        {
            string sender = param as string;

            switch (sender)
            {
                case "Button_LoadImages":
                    //try
                    //{
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";
                    openFile.Multiselect = true;

                    if (openFile.ShowDialog() == true)
                    {
                        Data = new ImageData[openFile.FileNames.Length];
                        currentImageIndex = 0;

                        for (int i = 0; i < Data.Length; i++)
                        {
                            //Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                            //bmp = bmp.Clone(rect, PixelFormat.Format16bppGrayScale);

                            Data[i] = new ImageData()
                            {
                                Name = openFile.SafeFileNames[i],
                                ImagePath = openFile.FileNames[i],
                                NumberOfBlackPixels = 0,
                                BallGrade = BallGrade.INDEFINED,
                                //Width = bmp.Width,
                                //Height = bmp.Height,
                                //Data = BitmapToArray(bmp)
                                //BitmapImage = BitmapToBitmapImage(bmp)
                            };
                        }

                        UpdateImage();

                        //byte[] data = imagesArr[currentImageIndex].Data;
                        //Bitmap threshold = ThresholdBitmap(ArrayToBitmap(data));
                        //CurrentImageSource = BitmapToBitmapImage(threshold);

                        //using (var newBitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap))
                        //{
                        //    CurrentImageSource = BitmapToBitmapImage(Threshold(newBitmap, ThresholdValue));
                        //    newBitmap.Dispose();
                        //}

                        break;
                    }
                    //}
                    //catch (FileNotFoundException ex)
                    //{
                    //    MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //catch (ArgumentException ex)
                    //{
                    //    MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //catch (UriFormatException ex)
                    //{
                    //    MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}

                    break;
                case "Button_PrevImage":

                    if (Data != null & currentImageIndex > 0)
                    {
                        currentImageIndex--;
                        UpdateImage();
                        //byte[] data = imagesArr[currentImageIndex].Data;
                        //Bitmap threshold = ThresholdBitmap(ArrayToBitmap(data));
                        //CurrentImageSource = BitmapToBitmapImage(threshold);

                        //using (var newBitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap))
                        //{
                        //    CurrentImageSource = BitmapToBitmapImage(Threshold(newBitmap, ThresholdValue));
                        //    newBitmap.Dispose();
                        //}
                    }
                    break;
                case "Button_NextImage":

                    if (Data != null & currentImageIndex < Data.Length - 1)
                    {
                        currentImageIndex++;
                        UpdateImage();
                        //byte[] data = imagesArr[currentImageIndex].Data;
                        //Bitmap threshold = ThresholdBitmap(ArrayToBitmap(data));
                        //CurrentImageSource = BitmapToBitmapImage(threshold);

                        //using (var newBitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap))
                        //{
                        //    CurrentImageSource = BitmapToBitmapImage(Threshold(newBitmap, ThresholdValue));
                        //    newBitmap.Dispose();
                        //}
                    }
                    break;
                case "Button_Execute":

                    if (Data != null)
                    {
                        Task.Run(() =>
                        {
                            for (int i = 0; i < Data.Length; i++)
                            {
                                ulong numberOfBlackPixels = 0;
                                using (var bitmap = ThresholdBitmap(ConvertToGrayBitmap(new Bitmap(Data[i].ImagePath)), ThresholdValue))
                                {
                                    unsafe
                                    {
                                        Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                                        BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);

                                        int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                                        int heightInPixels = bitmapData.Height;
                                        int widthInBytes = bitmapData.Width * bytesPerPixel;
                                        byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                                        object locker = new object();
                                        Parallel.For(0, heightInPixels, y =>
                                        {
                                            byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                                            for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                                            {
                                                // optimized
                                                if (currentLine[x] == 0)
                                                    lock (locker)
                                                        numberOfBlackPixels++;

                                                //int blue = currentLine[x];
                                                //int green = currentLine[x + 1];
                                                //int red = currentLine[x + 2];

                                                //if (currentLine[x] == 0 & currentLine[x + 1] == 0 & currentLine[x + 2] == 0)
                                                //{
                                                //    lock(locker)
                                                //    {
                                                //        numberOfBlackPixels++;
                                                //    }
                                                //}
                                            }
                                        });

                                        bitmap.UnlockBits(bitmapData);
                                    }


                                    //for (int y = 0; y < bitmap.Height; y++)
                                    //    for (int x = 0; x < bitmap.Width; x++)
                                    //    {
                                    //        System.Drawing.Color pixel = bitmap.GetPixel(x, y);
                                    //        if (pixel == System.Drawing.Color.FromArgb(255, 0, 0, 0)) numberOfBlackPixels++;
                                    //    }

                                }

                                if (numberOfBlackPixels < 11000)
                                    Data[i].BallGrade = BallGrade.FIRST;
                                else if (numberOfBlackPixels < 17000)
                                    Data[i].BallGrade = BallGrade.SECOND;
                                else
                                    Data[i].BallGrade = BallGrade.DEFECTIVE;

                                Data[i].NumberOfBlackPixels = numberOfBlackPixels;
                            }
                        });
                    }

                    //SaveFileDialog dialog = new SaveFileDialog();
                    //dialog.FileName = imagesArr[currentImageIndex].Name;

                    //if (dialog.ShowDialog() == true)
                    //{
                    //    Bitmap bmp = new Bitmap(imagesArr[currentImageIndex].Path);

                    //    bmp = ConvertToGrayBitmap(bmp);
                    //    bmp = ThresholdBitmap(bmp, ThresholdValue);

                    //    bmp.Save(dialog.FileName, ImageFormat.Bmp);
                    //}
                    //CurrentImageSource = BitmapToImageSource(imagesArr[currentImageIndex].Bitmap);

                    //using (var bmp = BitmapImageToBitmap(CurrentImageSource))
                    //{
                    //    long count = 0;
                    //    for (int y = 0; y < bmp.Height; y++)
                    //    {
                    //        for (int x = 0; x < bmp.Width; x++)
                    //        {
                    //            if (bmp.GetPixel(x, y).GetBrightness() == 0) count++;
                    //        }
                    //    }
                    //    Console.WriteLine("Count = " + count);
                    //}

                    break;

                case "DragCompleted":

                    if (Data != null)
                    {
                        UpdateImage();
                        //byte[] data = imagesArr[currentImageIndex].Data;
                        //Bitmap threshold = ThresholdBitmap(ArrayToBitmap(data));
                        //CurrentImageSource = BitmapToBitmapImage(threshold);

                        //using (var newBitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap))
                        //{
                        //    CurrentImageSource = BitmapToBitmapImage(Threshold(newBitmap, ThresholdValue));
                        //    newBitmap.Dispose();
                        //}
                    }

                    break;
            }
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);
                return stream.ToArray();
            }
        }

        private Bitmap BitmapTo1BppIndexed(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            return bitmap.Clone(rect, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
        }

        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);

                return new Bitmap(outStream);
            }
        }

        private Bitmap ConvertToGrayBitmap(Bitmap original)
        {
            using (Graphics g = Graphics.FromImage(original))
            {
                var gray_matrix = new float[][] {
                    new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                    new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                    new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                    new float[] { 0,      0,      0,      1, 0 },
                    new float[] { 0,      0,      0,      0, 1 }
                };

                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(new ColorMatrix(gray_matrix));

                var rect = new Rectangle(0, 0, original.Width, original.Height);
                g.DrawImage(original, rect, 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

                return original;
            }
        }

        private Bitmap ThresholdBitmap(Bitmap original, short thresholdValue)
        {
            Bitmap result = new Bitmap(original);
            using (Graphics g = Graphics.FromImage(result))
            {
                var attributes = new ImageAttributes();

                float threshold = (float)(thresholdValue / 255.0d);
                attributes.SetThreshold(threshold, ColorAdjustType.Bitmap);

                var rect = new Rectangle(0, 0, result.Width, result.Height);
                g.DrawImage(result, rect, 0, 0, result.Width, result.Height, GraphicsUnit.Pixel, attributes);

                return result;
            }
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                int imgHeight = bitmap.Height;
                bitmap.Dispose();

                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                if (imgHeight > DecodePixelHeight)
                {
                    bitmapImage.DecodePixelHeight = DecodePixelHeight;
                }

                bitmapImage.EndInit();

                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        private void UpdateImage()
        {
            try
            {
                Bitmap temp = new Bitmap(Data[currentImageIndex].ImagePath);

                temp = ConvertToGrayBitmap(temp);
                temp = ThresholdBitmap(temp, ThresholdValue);

                CurrentImage = BitmapToBitmapImage(temp);

                temp.Dispose();

                // delete old image
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, nameof(ArgumentException), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, nameof(Exception), MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}