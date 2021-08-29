namespace BallScanner.MVVM.Models
{
    public struct BrightnessMap
    {
        private int _width;
        public int Width
        {
            get => _width;
        }

        private int _height;
        public int Height
        {
            get => _height;
        }

        private byte[][] _map;
        public byte[][] Map
        {
            get => _map;
            set
            {
                _map = value;

                _height = _map.Length;
                _width = _map[0].Length;
            }
        }

        //public byte GetPixelBrightness(int x, int y)
        //{
        //    return _map[y][x];
        //}

        //public void SetPixelBrightness(int x, int y, byte brightness)
        //{
        //    _map[y][x] = brightness;
        //}
    }
}
