using System.Drawing;

namespace BallScanner.MVVM.Models
{
    public struct ImageData
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private ulong _numberOfBlackPixels;
        public ulong NumberOfBlackPixels
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

        //private int _width;
        //public int Width
        //{
        //    get => _width;
        //    set => _width = value;
        //}

        //private int _height;
        //public int Height
        //{
        //    get => _height;
        //    set => _height = value;
        //}

        //private byte[] _data;
        //public byte[] Data
        //{
        //    get => _data;
        //    set => _data = value;
        //}

        private Bitmap _bitmap;
        public Bitmap Bitmap
        {
            get => _bitmap;
            set => _bitmap = value;
        }
    }
}
