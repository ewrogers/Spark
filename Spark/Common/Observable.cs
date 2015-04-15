using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Spark.Common
{
    public abstract class Observable : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = this.PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyPropertyChanging
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = "")
        {
            var handler = this.PropertyChanging;

            if (handler!=null)
                handler(this, new PropertyChangingEventArgs(propertyName));
        }
        #endregion

        protected Observable() { }

        protected virtual bool SetProperty<T>(ref T backingStore, T newValue, [CallerMemberName] string propertyName = "", Action onChanged = null, Action<T> onChanging = null)
        {
            // Check if the new value is the same as the existing value
            if (EqualityComparer<T>.Default.Equals(newValue, backingStore))
                return false;

            // OnChanging acton invoked prior to OnPropertyChanging
            if (onChanging != null)
                onChanging(newValue);

            OnPropertyChanging(propertyName);
            
            // Replace the existing value with the new value
            backingStore = newValue;

            // OnChanged action invoked prior to OnPropertyChanged
            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
