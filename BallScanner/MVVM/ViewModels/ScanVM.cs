using BallScanner.MVVM.Core;

namespace BallScanner.MVVM.ViewModels
{
    public class ScanVM : PageVM
    {
        public ScanVM()
        {

        }

        public void ChangePalette()
        {
            App.Palette = "Orange";
        }
    }
}
