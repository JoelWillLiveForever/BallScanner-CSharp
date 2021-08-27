using System.Drawing;
using System.Windows.Media;

namespace BallScanner.MVVM.Models
{
    public enum Grade
    {
        FIRST,
        SECOND,
        DEFECTIVE
    }

    public class CalibrateM
    {
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => _fileName = value;
        }

        private long _numberOfBlackPixels;
        public long NumberOfBlackPixels
        {
            get => _numberOfBlackPixels;
            set => _numberOfBlackPixels = value;
        }

        private Grade _grade;
        public Grade Grade
        {
            get => _grade;
            set => _grade = value;
        }

        private Bitmap _bitmap;
        public Bitmap Bitmap
        {
            get => _bitmap;
            set => _bitmap = value;
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => _imageSource = value;
        }
    }
}
