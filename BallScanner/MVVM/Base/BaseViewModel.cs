using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BallScanner.MVVM.Base
{
    public interface ISupportParentViewModel
    {
        object ParentViewModel { get; set; }
    }

    public abstract class BaseViewModel : INotifyPropertyChanged, ISupportParentViewModel
    {
        public object ParentViewModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
