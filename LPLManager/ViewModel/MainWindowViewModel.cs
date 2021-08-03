using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPLManager.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        string _selectedChoice;


        public string SelectedChoice
        {
            get
            {
                return _selectedChoice;
            }
            set
            {
                _selectedChoice = value;
                OnPropertyChanged(nameof(SelectedChoice));
            }
        }


        public MainWindowViewModel()
        {
        }
    }
}
