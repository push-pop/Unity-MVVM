using System;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;
using UnityMVVM.Binding.Converters;

namespace UnityMVVM.Binding
{
    [Serializable]
    public class DataBindingConnection : IDisposable
    {
        //object PropertyOwner;

        //public string PropertyName;

        public Action PropertyChangedAction;

        public bool isDisposed = false;

        BindTarget _src;
        BindTarget _dst;
        IValueConverter _converter;
        GameObject _gameObject;

        public DataBindingConnection()
        { }

        public DataBindingConnection(GameObject owner, BindTarget src, BindTarget dst, IValueConverter converter = null)
        {
            _gameObject = owner;
            _src = src;
            _dst = dst;
            _converter = converter;

            var notifyChange = _src.propertyOwner as INotifyPropertyChanged;

            if (notifyChange != null)
                notifyChange.PropertyChanged += PropertyChangedHandler;
        }

        public void AddHandler(Action action)
        {
            PropertyChangedAction = action;
        }

        public void DstUpdated()
        {
            if (_converter != null)
                _src.SetValue(_converter.ConvertBack(_dst.GetValue(), _src.property.PropertyType, null));
            else
                _src.SetValue(Convert.ChangeType(_dst.GetValue(), _src.property.PropertyType));
        }

        internal void ClearHandler()
        {
            PropertyChangedAction = null;
        }

        public static string GetName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        internal void Bind()
        {
            PropertyChangedAction = OnSrcUpdated;
        }

        private void OnSrcUpdated()
        {
            try
            {
                if (_converter != null)
                    _dst.SetValue(_converter.Convert(_src.GetValue(), _src.property.PropertyType, null));
                else
                    _dst.SetValue(Convert.ChangeType(_src.GetValue(), _src.property.PropertyType));
            }
            catch (Exception e)
            {
                Debug.LogError("Data binding error in: " + _gameObject.name + ": " + e.Message);
            }
        }

        public void SetHandler(Action handler)
        {
            PropertyChangedAction = handler;
        }

        public static object GetOwner<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Expression.Type;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(_src.propertyName))
                PropertyChangedAction?.Invoke();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing && _src.propertyOwner != null)
            {
                var notifyPropertyChanged = _src.propertyOwner as INotifyPropertyChanged;
                if (notifyPropertyChanged != null)
                {
                    notifyPropertyChanged.PropertyChanged -= PropertyChangedHandler;
                }
            }

            isDisposed = true;
        }

    }
}