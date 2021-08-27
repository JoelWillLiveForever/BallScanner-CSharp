using BallScanner.MVVM.Commands;
using BallScanner.MVVM.Core;
using System;

namespace BallScanner.MVVM.ViewModels
{
    public class MainVM : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public MainVM()
        {
            Console.WriteLine("MainVM");
            SelectedViewModel = new MenuVM();
        }
    }
}
