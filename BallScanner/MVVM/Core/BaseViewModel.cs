using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BallScanner.MVVM.Core
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            if (propName != null) 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
