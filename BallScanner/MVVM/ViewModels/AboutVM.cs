using BallScanner.MVVM.Core;

namespace BallScanner.MVVM.ViewModels
{
    public class AboutVM : PageVM
    {
        public AboutVM()
        {

        }

        public void ChangePalette()
        {
            App.Palette = "Purple";
        }
    }
}
