using BallScanner.MVVM.Base;
using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Models.Main;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BallScanner.MVVM.ViewModels.Main
{
    public class CalibrateVM : PageVM
    {
        // Команды
        public static RelayCommand PerformAction { get; set; }

        public RelayCommand ChangeImageCommand { get; set; }

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
        private byte _thresholdValue;
        public byte ThresholdValue
        {
            get => _thresholdValue;
            set
            {
                if (_thresholdValue == value) return;

                _thresholdValue = value;
                OnPropertyChanged(nameof(ThresholdValue));

                Properties.Settings.Default.Threshold = ThresholdValue;
                Properties.Settings.Default.Save();

                if (Data != null && Data.Length != 0 && !isDragging) UpdateImage(300);
            }
        }

        private bool isDragging;

        // Первое и второе пороговые значения
        private int _firstThreshold;
        public int FirstThreshold
        {
            get => _firstThreshold;
            set
            {
                if (_firstThreshold == value) return;

                _firstThreshold = value;
                OnPropertyChanged(nameof(FirstThreshold));

                Properties.Settings.Default.FirstThreshold = FirstThreshold;
                Properties.Settings.Default.Save();
            }
        }

        private int _secondThreshold;
        public int SecondThreshold
        {
            get => _secondThreshold;
            set
            {
                if (_secondThreshold == value) return;

                _secondThreshold = value;
                OnPropertyChanged(nameof(SecondThreshold));

                Properties.Settings.Default.SecondThreshold = SecondThreshold;
                Properties.Settings.Default.Save();
            }
        }

        private long _avgNumBlackPixels;
        public long AvgNumBlackPixels
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
            PerformAction = new RelayCommand(OnPerformAction);

            // Подгрузить настройки
            ThresholdValue = Properties.Settings.Default.Threshold;

            FirstThreshold = Properties.Settings.Default.FirstThreshold;
            SecondThreshold = Properties.Settings.Default.SecondThreshold;

            //Fraction = Properties.Settings.Default.Fraction;
            //Partia_Number = Properties.Settings.Default.Partia_Number;

            ChangeImageCommand = new RelayCommand(ChangeImage);
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void OnPerformAction(object param)
        {
            switch (param)
            {
                case "Button_LoadImages":
                    App.WriteMsg2Log("Нажатие на кнопку \"Загрузка изображений\" на странице \"Калибровка\"", LoggerTypes.INFO);

                    if (worker_DownloadExecute == null || (worker_DownloadExecute != null && !worker_DownloadExecute.IsBusy))
                    {
                        worker_DownloadExecute = new BackgroundWorker();
                        worker_DownloadExecute.WorkerReportsProgress = false;
                        worker_DownloadExecute.WorkerSupportsCancellation = false;
                        //worker.ProgressChanged += ProgressChanged;
                        worker_DownloadExecute.DoWork += delegate
                        {
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
                                App.WriteMsg2Log("Ошибка \"" + nameof(FileNotFoundException) + "\"! Текст ошибки: " + ex.Message, LoggerTypes.ERROR);
                                MessageBox.Show("Текст ошибки: " + ex.Message, "Ошибка \"" + nameof(FileNotFoundException) + "\"!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            catch (ArgumentException ex)
                            {
                                App.WriteMsg2Log("Ошибка \"" + nameof(ArgumentException) + "\"! Текст ошибки: " + ex.Message, LoggerTypes.ERROR);
                                MessageBox.Show("Текст ошибки: " + ex.Message, "Ошибка \"" + nameof(ArgumentException) + "\"!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            catch (UriFormatException ex)
                            {
                                App.WriteMsg2Log("Ошибка \"" + nameof(UriFormatException) + "\"! Текст ошибки: " + ex.Message, LoggerTypes.ERROR);
                                MessageBox.Show("Текст ошибки: " + ex.Message, "Ошибка \"" + nameof(UriFormatException) + "\"!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            catch (Exception ex)
                            {
                                App.WriteMsg2Log("Непредвиденная ошибка во время выполнения! Текст ошибки: " + ex.Message, LoggerTypes.FATAL);
                                MessageBox.Show("Текст ошибки: " + ex.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        };
                        worker_DownloadExecute.RunWorkerAsync();
                    }
                    break;
                case "Button_PrevImage":
                    App.WriteMsg2Log("Нажатие на кнопку \"Предыдущее изображение\" на странице \"Калибровка\"", LoggerTypes.INFO);

                    if (Data != null && Data.Length > 0 && currentImageIndex > 0)
                    {
                        Interlocked.Decrement(ref currentImageIndex);
                        OnPropertyChanged(nameof(CurrentData));
                        UpdateImage(300);
                    }
                    break;
                case "Button_NextImage":
                    App.WriteMsg2Log("Нажатие на кнопку \"Следующее изображение\" на странице \"Калибровка\"", LoggerTypes.INFO);

                    if (Data != null && Data.Length > 0 && currentImageIndex < Data.Length - 1)
                    {
                        Interlocked.Increment(ref currentImageIndex);
                        OnPropertyChanged(nameof(CurrentData));
                        UpdateImage(300);
                    }
                    break;
                case "Button_Execute":
                    App.WriteMsg2Log("Нажатие на кнопку \"Выполнить\" на странице \"Калибровка\"", LoggerTypes.INFO);

                    if (worker_DownloadExecute == null || (worker_DownloadExecute != null && !worker_DownloadExecute.IsBusy))
                    {
                        worker_DownloadExecute = new BackgroundWorker();
                        worker_DownloadExecute.WorkerReportsProgress = true;
                        worker_DownloadExecute.WorkerSupportsCancellation = false;
                        worker_DownloadExecute.ProgressChanged += ProgressChanged;
                        worker_DownloadExecute.DoWork += delegate {
                            if (Data != null && Data.Length > 0)
                            {
                                int progress = 0;
                                AvgNumBlackPixels = 0;
                                object locker = new object();

                                Parallel.For(0, Data.Length, i =>
                                {
                                    int numberOfBlackPixels = 0;
                                    using (var bitmap = GetThresholdBitmap(Data[i].ImagePath, ThresholdValue))
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
                                    if (numberOfBlackPixels <= FirstThreshold)
                                        Data[i].BallGrade = BallGrade.FIRST;
                                    else if (numberOfBlackPixels <= SecondThreshold)
                                        Data[i].BallGrade = BallGrade.SECOND;
                                    else
                                        Data[i].BallGrade = BallGrade.DEFECTIVE;

                                    Data[i].NumberOfBlackPixels = numberOfBlackPixels;

                                    Interlocked.Increment(ref progress);
                                    lock(locker)worker_DownloadExecute.ReportProgress(progress);
                                });

                                // Поиск среднего
                                foreach (var obj in Data)
                                {
                                    AvgNumBlackPixels += obj.NumberOfBlackPixels;
                                }
                                AvgNumBlackPixels /= ImagesCount;

                                App.WriteMsg2Log("Завершено сканирование " + Data.Length + " изображений на странице \"Калибровка\"! Среднее число чёрных пикселей = " + AvgNumBlackPixels + ". Порогое значение ползунка = " + ThresholdValue + ". Первое порогое значение = " + FirstThreshold + ". Второе пороговое значение = " + SecondThreshold + ".", LoggerTypes.INFO);
                            }
                        };
                        worker_DownloadExecute.RunWorkerAsync();
                    }
                    break;
                case "Slider_DragStarted":
                    isDragging = true;
                    break;
                case "Slider_DragCompleted":
                    isDragging = false;
                    App.WriteMsg2Log("Изменено положение ползунка \"Пороговое значение\" на странице \"Калибровка\"! Новое значение = " + ThresholdValue, LoggerTypes.INFO);

                    if (Data != null && Data.Length > 0)
                        UpdateImage(0);
                    break;
            }
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
                App.WriteMsg2Log("Ошибка \"" + nameof(ArgumentException) + "\" в методе \"" + nameof(GetThresholdBitmap) + "\". Активная страница \"Калибровка\". Текст ошибки: " + e.Message, LoggerTypes.ERROR);
                MessageBox.Show("Текст ошибки: " + e.Message, "Ошибка \"" + nameof(ArgumentException) + "\"!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                App.WriteMsg2Log("Непредвиденная ошибка в методе \"" + nameof(GetThresholdBitmap) + "\". Активная страница \"Калибровка\". Текст ошибки: " + e.Message, LoggerTypes.FATAL);
                MessageBox.Show("Текст ошибки: " + e.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        App.WriteMsg2Log("Ошибка \"" + nameof(ArgumentException) + "\" в методе \"" + nameof(UpdateImage) + "\". Активная страница \"Калибровка\". Текст ошибки: " + e.Message, LoggerTypes.ERROR);
                        MessageBox.Show("Текст ошибки: " + e.Message, "Ошибка \"" + nameof(ArgumentException) + "\"!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        App.WriteMsg2Log("Непредвиденная ошибка в методе \"" + nameof(UpdateImage) + "\". Активная страница \"Калибровка\". Текст ошибки: " + e.Message, LoggerTypes.FATAL);
                        MessageBox.Show("Текст ошибки: " + e.Message, "Непредвиденная ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public void ChangeImage(object item)
        {
            if (item != null)
            {
                ImageData data = item as ImageData;
                currentImageIndex = data.Id - 1;

                OnPropertyChanged(nameof(CurrentData));
                UpdateImage(0);

                App.WriteMsg2Log("Изменено текущее изображение. Информация о новом изображении: номер в списке = " + data.Id + "; имя файла = " + data.Name + "; путь к файлу = " + data.ImagePath + "; число чёрных пикселей = " + data.NumberOfBlackPixels + "; cорт шарика = " + data.BallGrade_Text + ";", LoggerTypes.INFO);
            }
        }

        public override void ChangePalette()
        {
            var app = (App)Application.Current;

            if (Properties.Settings.Default.IsDarkTheme)
                app.ChangeTheme(new Uri("Resources/Palettes/Dark/Yellow.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/Palettes/Light/Yellow.xaml", UriKind.Relative));

            Properties.Settings.Default.SelectedPage = 3;
            Properties.Settings.Default.Save();
        }
    }
}