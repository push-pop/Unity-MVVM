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
        public BindTarget SrcTarget { get { return _src; } }
        public BindTarget DstTarget { get { return _dst; } }
        public string Owner { get { return _gameObject?.name; } }

        public Action PropertyChangedAction;

        public bool isDisposed = false;

        readonly BindTarget _src;
        readonly BindTarget _dst;
        IValueConverter _converter;
        GameObject _gameObject;

        public bool IsBound;

        public DataBindingConnection()
        { }

        public DataBindingConnection(GameObject owner, BindTarget src, BindTarget dst, IValueConverter converter = null)
        {
            _gameObject = owner;
            _src = src;
            _dst = dst;
            _converter = converter;

            PropertyChangedAction = OnSrcUpdated;

            BindingMonitor.RegisterConnection(this);
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
            IsBound = false;
        }

        internal void Unbind()
        {
            if (IsBound)
            {
                (_src.propertyOwner as INotifyPropertyChanged).PropertyChanged -= PropertyChangedHandler;
                IsBound = false;
            }

        }

        public static string GetName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        internal void Bind()
        {
            if (!IsBound)
            {
                var notifyPropChanged = (_src.propertyOwner as INotifyPropertyChanged);
                if (notifyPropChanged == null)
                {
                    Debug.LogError("Property Owner Doesn't Inherit from INotifyPropertyChanged - " + _src.propertyOwner);
                    return;
                }
                notifyPropChanged.PropertyChanged += PropertyChangedHandler;
                IsBound = true;
            }
        }

        public void OnSrcUpdated()
        {
            try
            {
                _dst.SetValue(_src.GetValue(), _converter);
            }
            catch (Exception e)
            {
                Debug.LogError("Data binding error in: " + _gameObject.name + ": " + e.Message);

                if (e.InnerException != null)
                    Debug.LogErrorFormat("Inner Exception: {0}", e.InnerException.Message);
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
            BindingMonitor.UnRegisterConnection(this);

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
