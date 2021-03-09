using System;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Enums;

namespace UnityMVVM.Binding
{
    [AddComponentMenu("Unity-MVVM/Bindings/DataBinding")]
    public class DataBinding
        : DataBindingBase
    {
        public DataBindingConnection Connection { get { return _connection; } }

        public override bool IsBound { get => _connection != null && _connection.IsBound; }


        protected DataBindingConnection _connection;

        #region Serialized Properties
        [HideInInspector]
        public BindingMode BindingMode = BindingMode.OneWay;

        [HideInInspector]
        public string SrcPropertyName = null;

        [HideInInspector]
        public string DstPropertyName = null;

        [HideInInspector]
        public string SrcPropertyPath = null;

        [HideInInspector]
        public string DstPropertyPath = null;

        [HideInInspector]
        public Component DstView;

        [HideInInspector]
        public ValueConverterBase Converter;

        [HideInInspector]
        public string DstChangedEventName = null;

        UnityEventBinder _eventBinder = new UnityEventBinder();
        Delegate changeDelegate;

        #endregion

        bool _isStartup = true;

        public override void RegisterDataBinding()
        {
            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for Property {1}", ViewModelName, SrcPropertyName);

                return;
            }
            if (_connection == null)
            {
                var srcTarget = new BindTarget()
                {
                    propertyOwner = _viewModel,
                    propertyName = SrcPropertyName,
                    propertyPath= SrcPropertyPath
                }.Init();

                var dstTarget = new BindTarget()
                {
                    propertyOwner = DstView,
                    propertyName = DstPropertyName,
                    propertyPath = DstPropertyPath,
                    eventName = DstChangedEventName
                }.Init();

                _connection = new DataBindingConnection(gameObject, srcTarget, dstTarget, BindingMode, DstChangedEventName, Converter);
            }


            if ((KeepConnectionAliveOnDisable || isActiveAndEnabled) && !IsBound)
            {
                _connection.Bind();

                if (BindingMode == BindingMode.TwoWay || BindingMode == BindingMode.OneWayToSource)
                    BindChangeEvent();
            }
        }

        public override void UnregisterDataBinding()
        {
            if (IsBound)
            {
                _connection.Unbind();

                if(BindingMode == BindingMode.TwoWay || BindingMode == BindingMode.OneWayToSource)
                UnbindChangeEvent();
            }
        }

        private void BindChangeEvent()
        {
            var propInfo = DstView.GetType().GetProperty(DstChangedEventName);

            var type = propInfo.PropertyType.BaseType;
            var args = type.GetGenericArguments();

            var evn = propInfo.GetValue(DstView);

            var addListenerMethod = UnityEventBinder.GetAddListener(propInfo.GetValue(DstView));

            changeDelegate = UnityEventBinder.GetDelegate(_eventBinder, args);

            var p = new object[] { changeDelegate };

            _eventBinder.OnChange += _connection.DstUpdated;

            addListenerMethod.Invoke(propInfo.GetValue(DstView), p);
        }

        private void UnbindChangeEvent()
        {
            var propInfo = DstView.GetType().GetProperty(DstChangedEventName);
            var removeListenerMethod = UnityEventBinder.GetRemoveListener(propInfo.GetValue(DstView));

            var p = new object[] { changeDelegate };

            _eventBinder.OnChange -= _connection.DstUpdated;

            removeListenerMethod.Invoke(propInfo.GetValue(DstView), p);
        }



        private void Start()
        {
            _connection.OnSrcUpdated();
            _isStartup = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!_isStartup)
                _connection.OnSrcUpdated();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_connection != null)
                _connection.Dispose();
        }
    }
}
