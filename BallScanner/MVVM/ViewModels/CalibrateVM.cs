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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BallScanner.MVVM.ViewModels
{
    public class CalibrateVM : PageVM
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static RelayCommand PerformAction { get; set; }

        private ImageData[] imagesArr = null;
        private int currentImageIndex = 0;

        private BitmapImage _currentImageSource;
        public BitmapImage CurrentImageSource
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
        }

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
                        imagesArr = new ImageData[openFile.FileNames.Length];

                        for (int i = 0; i < imagesArr.Length; i++)
                        {
                            Bitmap bmp = new Bitmap(openFile.FileNames[i]);
                            //MakeGrayBitmap(ref bmp);

                            imagesArr[i] = new ImageData()
                            {
                                Name = openFile.SafeFileNames[i],
                                NumberOfBlackPixels = 0,
                                BallGrade = BallGrade.INDEFINED,
                                //Width = bmp.Width,
                                //Height = bmp.Height,
                                //Data = BitmapToArray(bmp)
                                Bitmap = bmp
                            };
                        }


                        //byte[] data = imagesArr[currentImageIndex].Data;
                        //Bitmap threshold = ThresholdBitmap(ArrayToBitmap(data));
                        //CurrentImageSource = BitmapToBitmapImage(threshold);
                        using (var newBitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap))
                        {
                            CurrentImageSource = BitmapToBitmapImage(Threshold(newBitmap, ThresholdValue));
                            newBitmap.Dispose();
                        }

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

                    if (imagesArr != null & currentImageIndex > 0)
                    {
                        currentImageIndex--;

                        //byte[] data = imagesArr[currentImageIndex].Data;
                        //Bitmap threshold = ThresholdBitmap(ArrayToBitmap(data));
                        //CurrentImageSource = BitmapToBitmapImage(threshold);

                        using (var newBitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap))
                        {
                            CurrentImageSource = BitmapToBitmapImage(Threshold(newBitmap, ThresholdValue));
                            newBitmap.Dispose();
                        }
                    }
                    break;
                case "Button_NextImage":

                    if (imagesArr != null & currentImageIndex < imagesArr.Length - 1)
                    {
                        currentImageIndex++;

                        //byte[] data = imagesArr[currentImageIndex].Data;
                        //Bitmap threshold = ThresholdBitmap(ArrayToBitmap(data));
                        //CurrentImageSource = BitmapToBitmapImage(threshold);

                        using (var newBitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap))
                        {
                            CurrentImageSource = BitmapToBitmapImage(Threshold(newBitmap, ThresholdValue));
                            newBitmap.Dispose();
                        }
                    }
                    break;
                case "Button_Execute":

                    //CurrentImageSource = BitmapToImageSource(imagesArr[currentImageIndex].Bitmap);

                    using (var bmp = BitmapImageToBitmap(CurrentImageSource))
                    {
                        long count = 0;
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            for (int x = 0; x < bmp.Width; x++)
                            {
                                if (bmp.GetPixel(x, y).GetBrightness() == 0) count++;
                            }
                        }
                        Console.WriteLine("Count = " + count);
                    }

                    break;

                case "DragCompleted":

                    if (imagesArr != null)
                    {
                        //byte[] data = imagesArr[currentImageIndex].Data;
                        //Bitmap threshold = ThresholdBitmap(ArrayToBitmap(data));
                        //CurrentImageSource = BitmapToBitmapImage(threshold);

                        using (var newBitmap = new Bitmap(imagesArr[currentImageIndex].Bitmap))
                        {
                            CurrentImageSource = BitmapToBitmapImage(Threshold(newBitmap, ThresholdValue));
                            newBitmap.Dispose();
                        }
                    }

                    break;
            }
        }

        private Bitmap Threshold(Bitmap original, int threshold)
        {
            using (var temp = original)
            {
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetThreshold(threshold * 1.0f / 255.0f);

                System.Drawing.Point[] points =
                {
                    new System.Drawing.Point(0, 0),
                    new System.Drawing.Point(temp.Width, 0),
                    new System.Drawing.Point(0, temp.Height)
                };

                Rectangle rect = new Rectangle(0, 0, temp.Width, temp.Height);

                using (Graphics gr = Graphics.FromImage(temp))
                {
                    gr.DrawImage(temp, points, rect, GraphicsUnit.Pixel, attributes);
                }

                return BitmapTo1Bpp(temp);
            }
        }

        private Bitmap BitmapTo1Bpp(Bitmap original)
        {
            var rectangle = new Rectangle(0, 0, original.Width, original.Height);
            return original.Clone(rectangle, PixelFormat.Format1bppIndexed);
        }

        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        //private Bitmap BitmapToGrayScale(Bitmap original)
        //{
        //    using (Graphics gr = Graphics.FromImage(original)) // SourceImage is a Bitmap object
        //    {
        //        var gray_matrix = new float[][] {
        //        new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
        //        new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
        //        new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
        //        new float[] { 0,      0,      0,      1, 0 },
        //        new float[] { 0,      0,      0,      0, 1 }
        //    };

        //        var ia = new ImageAttributes();
        //        ia.SetColorMatrix(new ColorMatrix(gray_matrix));
        //        //ia.SetThreshold(0.8f); // Change this threshold as needed

        //        var rc = new Rectangle(0, 0, original.Width, original.Height);

        //        gr.DrawImage(original, rc, 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, ia);

        //        return original;
        //    }
        //}

        //private unsafe byte[][] GrayBitmapToByteArray(Bitmap original)
        //{
        //    //Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
        //    //BitmapData bmpData = original.LockBits(rect, ImageLockMode.ReadOnly, original.PixelFormat);

        //    //byte[] pixels = new byte[bmpData.Stride * bmpData.Height];

        //    //IntPtr ptrFirstPixel = bmpData.Scan0;
        //    //Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

        //    //original.UnlockBits(bmpData);

        //    Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
        //    BitmapData bitmapData = original.LockBits(rect, ImageLockMode.ReadWrite, original.PixelFormat);

        //    int width = original.Width;
        //    int height = original.Height;
        //    int stride = bitmapData.Stride;

        //    int bytesPerPixel = Image.GetPixelFormatSize(original.PixelFormat) / 8;
        //    int heightInPixels = bitmapData.Height;
        //    int widthInBytes = bitmapData.Width * bytesPerPixel;

        //    byte* ptrFirstPixel = (byte*)bitmapData.Scan0;
        //    byte[][] result = new byte[original.Height][];

        //    original.UnlockBits(bitmapData);

        //    Parallel.For(0, height, y =>
        //    {
        //        int index = 0;
        //        byte[] resultLine = new byte[width];
        //        byte* currentLine = ptrFirstPixel + y * stride;

        //        for (int x = 0; x < widthInBytes; x = x + stride)
        //        {
        //                //int b = currentLine[x];
        //                //int g = currentLine[x + 1];
        //                //int r = currentLine[x + 2];

        //                //resultLine[index] = (byte)(r * 0.299d + g * 0.587d + b * 0.114d);
        //             resultLine[index] = currentLine[x];
        //            index++;
        //        }

        //        result[y] = resultLine;
        //    });

        //    return result;
        //    //using (var stream = new MemoryStream())
        //    //{
        //    //    source.Save(stream, ImageFormat.Png);
        //    //    return stream.ToArray();
        //    //}
        //}

        //private byte[] BitmapToByteArray(Bitmap original)
        //{
        //    Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
        //    BitmapData bitmapData = original.LockBits(rect, ImageLockMode.ReadOnly, original.PixelFormat);

        //    byte[] rgb = new byte[bitmapData.Stride * bitmapData.Height];

        //    IntPtr ptr = bitmapData.Scan0;
        //    Marshal.Copy(ptr, rgb, 0, rgb.Length);

        //    original.UnlockBits(bitmapData);

        //    return rgb;
        //}

        //public static Bitmap MakeGrayscale3(Bitmap original)
        //{
        //    //create a blank bitmap the same size as original
        //    Bitmap newBitmap = new Bitmap(original.Width, original.Height);

        //    //get a graphics object from the new image
        //    using (Graphics g = Graphics.FromImage(newBitmap))
        //    {

        //        //create the grayscale ColorMatrix
        //        ColorMatrix colorMatrix = new ColorMatrix(
        //           new float[][]
        //           {
        //               new float[] {.3f, .3f, .3f, 0, 0},
        //               new float[] {.59f, .59f, .59f, 0, 0},
        //               new float[] {.11f, .11f, .11f, 0, 0},
        //               new float[] {0, 0, 0, 1, 0},
        //               new float[] {0, 0, 0, 0, 1}
        //           });

        //        //create some image attributes
        //        using (ImageAttributes attributes = new ImageAttributes())
        //        {

        //            //set the color matrix attribute
        //            attributes.SetColorMatrix(colorMatrix);

        //            //draw the original image on the new image
        //            //using the grayscale color matrix
        //            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
        //                        0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
        //        }
        //    }
        //    return newBitmap;
        //}

        private byte[] BitmapToArray(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                return memory.ToArray();
            }
        }

        private Bitmap ArrayToBitmap(byte[] data)
        {
            using (var memory = new MemoryStream(data))
            {
                return new Bitmap(memory);
            }
        }

        private void MakeGrayBitmap(ref Bitmap original)
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
            }
        }

        private Bitmap ThresholdBitmap(Bitmap original)
        {
            Bitmap result = new Bitmap(original);
            using (Graphics g = Graphics.FromImage(result))
            {
                var attributes = new ImageAttributes();

                float threshold = (float)(ThresholdValue * 1.0d / 255.0d);
                Console.WriteLine("Threshold = " + threshold);
                attributes.SetThreshold(threshold);

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
                bitmap.Dispose();

                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
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