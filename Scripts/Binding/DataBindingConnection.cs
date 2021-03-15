using System;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Enums;

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
        public GameObject _gameObject;

        public bool IsBound;

        public BindingMode _mode = BindingMode.OneWay;

        public string _dstChangeEventName;
        UnityEventBinder _eventBinder = new UnityEventBinder();
        Delegate changeDelegate;

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

        public DataBindingConnection(GameObject owner, BindTarget src, BindTarget dst, BindingMode bindingMode, string dstChangeEventName, IValueConverter converter = null)
        {
            _gameObject = owner;
            _src = src;
            _dst = dst;
            _converter = converter;

            PropertyChangedAction = OnSrcUpdated;
            _dstChangeEventName = dstChangeEventName;
            _mode = bindingMode;

            BindingMonitor.RegisterConnection(this);
        }


        public void DstUpdated()
        {
            try
            {
                _src.SetValue(_dst.GetValue(), true, _converter);
            }
            catch (Exception e)
            {
                Debug.LogError("Data binding error in: " + _gameObject.name);

                throw (e);
            }
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

                if (_mode != BindingMode.OneWayToSource)
                {
                    //Debug.Log($"Unbind {_src.propertyOwner}:{_src.propertyName}{(string.IsNullOrEmpty(_src.propertyPath) ? "" : ":" + _src.propertyPath)}");
                    (_src.propertyOwner as INotifyPropertyChanged).PropertyChanged -= PropertyChangedHandler;
                }

                if (_mode == BindingMode.TwoWay || _mode == BindingMode.OneWayToSource)
                    UnbindDstchangedHandler();

                IsBound = false;
            }

        }

        private void BindDstChangedHandler()
        {
            //Debug.Log($"Bind DstChangedHandler {_gameObject}");

            var owner = DstTarget.propertyOwner;
            var propInfo = owner.GetType().GetProperty(DstTarget.eventName);

            var type = propInfo.PropertyType.BaseType;
            var args = type.GetGenericArguments();

            var evn = propInfo.GetValue(owner);

            var addListenerMethod = UnityEventBinder.GetAddListener(propInfo.GetValue(owner));

            changeDelegate = UnityEventBinder.GetDelegate(_eventBinder, args);

            var p = new object[] { changeDelegate };

            _eventBinder.OnChange += DstUpdated;

            addListenerMethod.Invoke(propInfo.GetValue(owner), p);
        }

        private void UnbindDstchangedHandler()
        {
            //Debug.Log($"UnBind DstChangedHandler {_gameObject}");

            var owner = DstTarget.propertyOwner;
            var propInfo = owner.GetType().GetProperty(DstTarget.eventName);
            var removeListenerMethod = UnityEventBinder.GetRemoveListener(propInfo.GetValue(owner));

            var p = new object[] { changeDelegate };

            _eventBinder.OnChange -= DstUpdated;

            removeListenerMethod.Invoke(propInfo.GetValue(owner), p);
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
                if (_mode != BindingMode.OneWayToSource)
                {
                    var notifyPropChanged = (_src.propertyOwner as INotifyPropertyChanged);
                    if (notifyPropChanged == null)
                    {
                        Debug.LogError("Property Owner Doesn't Inherit from INotifyPropertyChanged - " + _src.propertyOwner);
                        return;
                    }

                    //     Debug.Log($"Bind {_src.propertyOwner}:{_src.propertyName}{(string.IsNullOrEmpty(_src.propertyPath) ? "" : ":" + _src.propertyPath)}");

                    notifyPropChanged.PropertyChanged += PropertyChangedHandler;
                }

                if (_mode == BindingMode.TwoWay || _mode == BindingMode.OneWayToSource)
                    BindDstChangedHandler();

                IsBound = true;
            }
        }

        public void OnSrcUpdated()
        {
            try
            {
                if (_mode != BindingMode.OneWayToSource)
                    _dst.SetValue(_src.GetValue(), false, _converter);
            }
            catch (Exception e)
            {
                Debug.LogError("Data binding error in: " + _gameObject.name);

                throw (e);
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
            {
                //    Debug.Log($"PropertyChanged: {e.PropertyName}");
                PropertyChangedAction?.Invoke();
            }
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
