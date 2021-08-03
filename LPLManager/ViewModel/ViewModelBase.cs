using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPLManager.ViewModel
{
    public class ViewModelBase
    {
        // In ViewModelBase.cs
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (VerifyPropertyName(propertyName))
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    var e = new PropertyChangedEventArgs(propertyName);
                    handler(this, e);
                }
            }
        }

        public bool VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                //string msg = "Invalid property name: " + propertyName;

                //if (this.ThrowOnInvalidPropertyName)
                //    throw new Exception(msg);
                //else
                //    Debug.Fail(msg);
                return false;
            }
            return true;
        }
    }
}
