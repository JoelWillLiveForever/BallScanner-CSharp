using BallScanner.MVVM.Core;

namespace BallScanner.MVVM.ViewModels
{
    public class SettingsVM : PageVM
    {
        public SettingsVM()
        {

        }

        public void ChangePalette()
        {
            App.Palette = "Blue";
        }
    }
}
