using System;
using UnityEngine;

namespace UnityMVVM.Binding
{
    public class TwoWayDataBinding
        : OneWayDataBinding
    {
        [HideInInspector]
        public string _dstChangedEventName = null;

        UnityEventBinder _binder = new UnityEventBinder();
        Delegate changeDelegate;

        public override void RegisterDataBinding()
        {
            base.RegisterDataBinding();

            var propInfo = _dstView.GetType().GetProperty(_dstChangedEventName);

            var type = propInfo.PropertyType.BaseType;
            var args = type.GetGenericArguments();

            var evn = propInfo.GetValue(_dstView);

            var addListenerMethod = UnityEventBinder.GetAddListener(propInfo.GetValue(_dstView));

            changeDelegate = UnityEventBinder.GetDelegate(_binder, args);

            var p = new object[] { changeDelegate };

            _binder.OnChange += _connection.DstUpdated;

            addListenerMethod.Invoke(propInfo.GetValue(_dstView), p);

            IsBound = true;
        }

        public override void UnregisterDataBinding()
        {
            base.UnregisterDataBinding();

            var propInfo = _dstView.GetType().GetProperty(_dstChangedEventName);
            var removeListenerMethod = UnityEventBinder.GetRemoveListener(propInfo.GetValue(_dstView));

            var p = new object[] { changeDelegate };

            _binder.OnChange -= _connection.DstUpdated;

            removeListenerMethod.Invoke(propInfo.GetValue(_dstView), p);

            IsBound = false;
        }
    }
}
