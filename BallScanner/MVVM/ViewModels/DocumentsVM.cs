using BallScanner.MVVM.Core;

namespace BallScanner.MVVM.ViewModels
{
    public class DocumentsVM : PageVM
    {
        public DocumentsVM()
        {

        }

        public void ChangePalette()
        {
            App.Palette = "Green";
        }
    }
}
