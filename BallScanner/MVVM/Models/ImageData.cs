using BallScanner.MVVM.Base;

namespace BallScanner.MVVM.Models
{
    public class ImageData : BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id == value) return;

                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;

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
                if (_numberOfBlackPixels == value) return;

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
                if (_ballGrade == value) return;

                _ballGrade = value;
                OnPropertyChanged(nameof(BallGrade));
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath == value) return;

                _imagePath = value;
            }
        }
    }
}
