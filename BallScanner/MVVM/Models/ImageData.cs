using System.Drawing;
using System.Windows.Media;

namespace BallScanner.MVVM.Models
{
    public class ImageData
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private long _numberOfBlackPixels;
        public long NumberOfBlackPixels
        {
            get => _numberOfBlackPixels;
            set => _numberOfBlackPixels = value;
        }

        private BallGrade _ballGrade;
        public BallGrade BallGrade
        {
            get => _ballGrade;
            set => _ballGrade = value;
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
