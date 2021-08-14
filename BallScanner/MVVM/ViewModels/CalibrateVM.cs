using BallScanner.MVVM.Core;

namespace BallScanner.MVVM.ViewModels
{
    public class CalibrateVM : PageVM
    {
        public CalibrateVM()
        {

        }

        public void ChangePalette()
        {
            App.Palette = "Yellow";
        }
    }
}
