using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LeseEulenBibliothek.Core
{
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool Set<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (object.Equals(field,value))
                return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
