using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

using Spark.Common;

namespace Spark.ViewModels
{
    public abstract class ViewModelBase : Observable, IDisposable
    {
        bool isDisposed;
        string displayName;
        bool throwOnInvalidPropertyName;

        #region Properties
        public virtual string DisplayName
        {
            get { return displayName; }
            set { SetProperty(ref displayName, value); }
        }

        public virtual bool ThrowOnInvalidPropertyName
        {
            get { return throwOnInvalidPropertyName; }
            set { SetProperty(ref throwOnInvalidPropertyName, value); }
        }
        #endregion

        public ViewModelBase(string displayName)
        {
            this.DisplayName = displayName;
        }

        #region IDisposable Methods
        public void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose(bool isDisposing)
        {
            if (isDisposed)
                return;

            if (isDisposing)
            {
                // Dispose of managed resources here
            }

            // Dispose of unmanaged resources here

            isDisposed = true;
        }
        #endregion

        #region INotifyPropertyChanged Overrides
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            VerifyPropertyName(propertyName);
            base.OnPropertyChanged(propertyName);
        }
        #endregion

        #region INotifyPropertyChanging Overrides
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = "")
        {
            VerifyPropertyName(propertyName);
            base.OnPropertyChanging(propertyName);
        }
        #endregion

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            // Check if the property exists on this object
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                var message = string.Format("Invalid property name: {0}", propertyName);

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(message);
                else
                    Debug.Fail(message);
            }
        }
    }
}
