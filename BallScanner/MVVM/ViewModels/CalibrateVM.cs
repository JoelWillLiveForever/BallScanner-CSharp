using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Models;
using Microsoft.Win32;
using NLog;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BallScanner.MVVM.ViewModels
{
    public class CalibrateVM : PageVM
    {
        // Логгер
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static RelayCommand PerformAction { get; set; }

        // Декодирование изображения, если пикселей на экране меньше разрешения картинки
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
                if (Data != null && Data.Length != 0) UpdateImage(300);
            }
        }

        // Массив с изображениями
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

        // Только для чтения в дизайне
        public ImageData CurrentData
        {
            get
            {
                if (Data == null || Data.Length == 0) return null;
                return Data[currentImageIndex];
            }
        }

        // Индекс текущего изображения (ожидаемого)
        private int currentImageIndex;

        // Текущее изображение на экране
        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get => _currentImage;
            set
            {
                if (_currentImage == value) return;

                _currentImage = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        // Текущая чувствительность сканера (порог пикселя)
        private byte _thresholdValue = 128;
        public byte ThresholdValue
        {
            get => _thresholdValue;
            set
            {
                if (_thresholdValue == value) return;

                _thresholdValue = value;
                OnPropertyChanged(nameof(ThresholdValue));

                if (Data != null && Data.Length != 0 && !isDragging) UpdateImage(300);
            }
        }

        private bool isDragging;

        // Первое и второе пороговые значения
        private ulong _firstThreshold;
        public ulong FirstThreshold
        {
            get => _firstThreshold;
            set
            {
                if (_firstThreshold == value) return;

                _firstThreshold = value;
                OnPropertyChanged(nameof(FirstThreshold));
            }
        }

        private ulong _secondThreshold;
        public ulong SecondThreshold
        {
            get => _secondThreshold;
            set
            {
                if (_secondThreshold == value) return;

                _secondThreshold = value;
                OnPropertyChanged(nameof(SecondThreshold));
            }
        }

        private ulong _avgNumBlackPixels;
        public ulong AvgNumBlackPixels
        {
            get => _avgNumBlackPixels;
            set
            {
                if (_avgNumBlackPixels == value) return;

                _avgNumBlackPixels = value;
                OnPropertyChanged(nameof(AvgNumBlackPixels));
            }
        }

        private int _imagesCount;
        public int ImagesCount
        {
            get => _imagesCount;
            set
            {
                if (_imagesCount == value) return;

                _imagesCount = value;
                OnPropertyChanged(nameof(ImagesCount));
            }
        }

        private int _currentProgress;
        public int CurrentProgress
        {
            get => _currentProgress;
            private set
            {
                if (_currentProgress == value) return;

                _currentProgress = value;
                OnPropertyChanged(nameof(CurrentProgress));
            }
        }

        // Фоновые задачи
        private BackgroundWorker worker_DownloadExecute;

        // Конструктор
        public CalibrateVM()
        {
            Log.Info("Constructor called!");
            PerformAction = new RelayCommand(OnPerformAction);
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void OnPerformAction(object param)
        {
            Log.Info("Call function \"OnPerformAction(object param)\", param: " + param);
            Log.Info("Call function \"Task.Run()\"");

            switch (param)
            {
                case "Button_LoadImages":
                    if (worker_DownloadExecute == null || (worker_DownloadExecute != null && !worker_DownloadExecute.IsBusy))
                    {
                        worker_DownloadExecute = new BackgroundWorker();
                        worker_DownloadExecute.WorkerReportsProgress = false;
                        worker_DownloadExecute.WorkerSupportsCancellation = false;
                        //worker.ProgressChanged += ProgressChanged;
                        worker_DownloadExecute.DoWork += delegate
                        {
                            Log.Info("Button \"Load\" has been pressed");
                            try
                            {
                                OpenFileDialog openFile = new OpenFileDialog();
                                openFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";
                                openFile.Multiselect = true;

                                if (openFile.ShowDialog() == true)
                                {
                                    ImagesCount = openFile.FileNames.Length;
                                    Data = new ImageData[ImagesCount];
                                    currentImageIndex = 0;
                                    CurrentProgress = 0;

                                    int id = 1;
                                    for (int i = 0; i < Data.Length; i++)
                                    {
                                        Data[i] = new ImageData()
                                        {
                                            Id = id,
                                            Name = openFile.SafeFileNames[i],
                                            ImagePath = openFile.FileNames[i],
                                            NumberOfBlackPixels = 0,
                                            BallGrade = BallGrade.INDEFINED
                                        };

                                        if (i == 0) OnPropertyChanged(nameof(CurrentData));
                                        id++;
                                    }

                                    UpdateImage(0);
                                }
                            }
                            catch (FileNotFoundException ex)
                            {
                                Log.Error("Error \"" + nameof(FileNotFoundException) + "\"", ex.Message);
                                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            catch (ArgumentException ex)
                            {
                                Log.Error("Error \"" + nameof(ArgumentException) + "\"", ex.Message);
                                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            catch (UriFormatException ex)
                            {
                                Log.Error("Error \"" + nameof(IndexOutOfRangeException) + "\"", ex.Message);
                                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error \"" + nameof(Exception) + "\"", ex.Message);
                                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        };
                        worker_DownloadExecute.RunWorkerAsync();
                    }
                    break;
                case "Button_PrevImage":
                    Log.Info("Button \"Previous\" has been pressed");
                    if (Data != null && Data.Length > 0 && currentImageIndex > 0)
                    {
                        Interlocked.Decrement(ref currentImageIndex);
                        OnPropertyChanged(nameof(CurrentData));
                        UpdateImage(300);
                    }
                    break;
                case "Button_NextImage":
                    Log.Info("Button \"Next\" has been pressed");
                    if (Data != null && Data.Length > 0 && currentImageIndex < Data.Length - 1)
                    {
                        Interlocked.Increment(ref currentImageIndex);
                        OnPropertyChanged(nameof(CurrentData));
                        UpdateImage(300);
                    }
                    break;
                case "Button_Execute":
                    if (worker_DownloadExecute == null || (worker_DownloadExecute != null && !worker_DownloadExecute.IsBusy))
                    {
                        worker_DownloadExecute = new BackgroundWorker();
                        worker_DownloadExecute.WorkerReportsProgress = true;
                        worker_DownloadExecute.WorkerSupportsCancellation = false;
                        worker_DownloadExecute.ProgressChanged += ProgressChanged;
                        worker_DownloadExecute.DoWork += delegate {
                            Log.Info("Button \"Execute\" has been pressed");
                            if (Data != null && Data.Length > 0)
                            {
                                int progress = 0;
                                Parallel.For(0, Data.Length, i =>
                                {
                                    long numberOfBlackPixels = 0;
                                    using (var bitmap = GetThresholdBitmap(Data[i].ImagePath, _thresholdValue))
                                    {
                                        unsafe
                                        {
                                            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                                            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);

                                            int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                                            int heightInPixels = bitmapData.Height;
                                            int widthInBytes = bitmapData.Width * bytesPerPixel;
                                            byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                                            Parallel.For(0, heightInPixels, y =>
                                            {
                                                byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                                                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                                                {
                                                    // optimized
                                                    if (currentLine[x] == 0)
                                                        Interlocked.Increment(ref numberOfBlackPixels);
                                                }
                                            });

                                            bitmap.UnlockBits(bitmapData);
                                        }
                                    }

                                    // Определение сорта стеклошарика
                                    if (unchecked((ulong)numberOfBlackPixels) <= FirstThreshold)
                                        Data[i].BallGrade = BallGrade.FIRST;
                                    else if (unchecked((ulong)numberOfBlackPixels) <= SecondThreshold)
                                        Data[i].BallGrade = BallGrade.SECOND;
                                    else
                                        Data[i].BallGrade = BallGrade.DEFECTIVE;

                                    Data[i].NumberOfBlackPixels = unchecked((ulong)numberOfBlackPixels);

                                    Interlocked.Increment(ref progress);
                                    worker_DownloadExecute.ReportProgress(progress);
                                });

                                // Поиск среднего
                                foreach (var obj in Data)
                                {
                                    AvgNumBlackPixels += obj.NumberOfBlackPixels;
                                }
                                AvgNumBlackPixels /= (ulong)ImagesCount;
                            }
                        };
                        worker_DownloadExecute.RunWorkerAsync();
                    }
                    break;
                case "Slider_DragStarted":
                    isDragging = true;
                    break;
                case "Slider_DragCompleted":
                    Log.Info("Slider has been released");
                    isDragging = false;

                    if (Data != null && Data.Length > 0)
                        UpdateImage(0);
                    break;
            }

            Log.Info("End of function \"OnPerformAction\"");
        }

        private Bitmap GetThresholdBitmap(string path, short thresholdValue)
        {
            try
            {
                Bitmap original = new Bitmap(path);

                // gray bitmap
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
                    float threshold = (float)(ThresholdValue / 255.0d);

                    attributes.SetColorMatrix(new ColorMatrix(gray_matrix));
                    attributes.SetThreshold(threshold, ColorAdjustType.Default);

                    var rect = new Rectangle(0, 0, original.Width, original.Height);
                    g.DrawImage(original, rect, 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }

                return original;
            }
            catch (ArgumentException e)
            {
                Log.Error("Error \"ArgumentException\"", e.Message);
                MessageBox.Show(e.Message, nameof(ArgumentException), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Log.Error("Error \"Exception\"", e.Message);
                MessageBox.Show(e.Message, nameof(Exception), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        private async void UpdateImage(int delay)
        {
            int myImageIndex = currentImageIndex;
            int myThresholdValue = ThresholdValue;
            int myDecodePixelHeight = DecodePixelHeight;

            await Task.Delay(delay);

            if (delay == 0 || (myImageIndex == currentImageIndex && myDecodePixelHeight == DecodePixelHeight && myThresholdValue == ThresholdValue))
            {
                //if (Data == null || Data.Length == 0 || currentImageIndex < 0 || currentImageIndex > Data.Length - 1) return;
                await Task.Run(() =>
                {
                    try
                    {
                        Bitmap original = new Bitmap(Data[currentImageIndex].ImagePath);

                        // gray bitmap
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
                            float threshold = (float)(ThresholdValue / 255.0d);

                            attributes.SetColorMatrix(new ColorMatrix(gray_matrix));
                            attributes.SetThreshold(threshold, ColorAdjustType.Default);

                            var rect = new Rectangle(0, 0, original.Width, original.Height);
                            g.DrawImage(original, rect, 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                        }

                        // actually ?
                        if (myImageIndex != currentImageIndex) return;

                        // bitmap to bitmap image
                        using (MemoryStream memory = new MemoryStream())
                        {
                            original.Save(memory, ImageFormat.Bmp);
                            int imgHeight = original.Height;
                            original.Dispose();

                            memory.Position = 0;

                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memory;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                            if (imgHeight > DecodePixelHeight)
                                bitmapImage.DecodePixelHeight = DecodePixelHeight;

                            bitmapImage.EndInit();
                            bitmapImage.Freeze();

                            if (myImageIndex != currentImageIndex) return;

                            CurrentImage = bitmapImage;
                        }

                        original.Dispose();

                        // delete old image
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                    }
                    catch (ArgumentException e)
                    {
                        Log.Error("Error \"ArgumentException\"", e.Message);
                        MessageBox.Show(e.Message, nameof(ArgumentException), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error \"Exception\"", e.Message);
                        MessageBox.Show(e.Message, nameof(Exception), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public override void ChangePalette()
        {
            Log.Info("Вызов функции \"ChangePalette()\" - смена цвета для текущей страницы");

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

        public void ChangeImage(object item)
        {
            ImageData data = item as ImageData;
            currentImageIndex = data.Id - 1;

            OnPropertyChanged(nameof(CurrentData));
            UpdateImage(0);
        }
    }
}