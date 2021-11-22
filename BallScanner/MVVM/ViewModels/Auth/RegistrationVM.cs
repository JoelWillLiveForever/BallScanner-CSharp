using BallScanner.MVVM.Base;
using NLog;

namespace BallScanner.MVVM.ViewModels.Auth
{
    public class RegistrationVM : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RegistrationVM()
        {
            Log.Info("Constructor called!");
        }
    }
}
