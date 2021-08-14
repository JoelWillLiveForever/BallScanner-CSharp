using BallScanner.MVVM.Core;

namespace BallScanner.MVVM.ViewModels
{
    public class AccountVM : PageVM
    {
        public AccountVM()
        {

        }

        public void ChangePalette()
        {
            App.Palette = "Red";
        }
    }
}
