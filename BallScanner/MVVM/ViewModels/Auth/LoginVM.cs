using BallScanner.MVVM.Base;
using NLog;

namespace BallScanner.MVVM.ViewModels.Auth
{
    public class LoginVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public LoginVM()
        {
            Log.Info("Constructor called!");
        }
    }
}

