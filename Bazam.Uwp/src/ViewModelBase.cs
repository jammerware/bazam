using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bazam.Uwp
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string property = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(property);
            return true;
        }
        
        protected bool SetProperty<T>(T currentValue, T newValue, Action DoSet, [CallerMemberName] string property = null)
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue)) return false;
            DoSet.Invoke();
            RaisePropertyChanged(property);
            return true;
        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public class ViewModelBase<T> : ViewModelBase where T : class, new()
    {
        protected T _This;

        public static implicit operator T(ViewModelBase<T> thing) { return thing._This; }

        public ViewModelBase(T thing = null)
        {
            _This = (thing == null) ? new T() : thing;
        }
    }
}