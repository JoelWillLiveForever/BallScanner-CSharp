using BallScanner.MVVM.Base;

namespace BallScanner.MVVM.Models
{
    public class ImageData : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private ulong _numberOfBlackPixels;
        public ulong NumberOfBlackPixels
        {
            get => _numberOfBlackPixels;
            set
            {
                _numberOfBlackPixels = value;
                OnPropertyChanged(nameof(NumberOfBlackPixels));
            }
        }

        private BallGrade _ballGrade;
        public BallGrade BallGrade
        {
            get => _ballGrade;
            set
            {
                _ballGrade = value;
                OnPropertyChanged(nameof(BallGrade));
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set => _imagePath = value;
        }
    }
}
