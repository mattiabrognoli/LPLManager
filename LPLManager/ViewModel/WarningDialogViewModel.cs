﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPLManager.ViewModel
{
    public class WarningDialogViewModel : ViewModelBase
    {
        string _description;

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public WarningDialogViewModel()
        {
        }
    }
}