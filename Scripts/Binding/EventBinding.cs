using System;
using System.Reflection;
using UnityEngine;
using UnityMVVM.Util;

namespace UnityMVVM.Binding
{
    public class EventBinding : DataBindingBase
    {
        [HideInInspector]
        public string SrcEventName = null;

        [HideInInspector]
        public string DstMethodName = null;

        [HideInInspector]
        public Component SrcView;

        MethodInfo _method;
        PropertyInfo _srcEventProp;

        Delegate d;

        bool _isEventBound = false;

        public override bool IsBound
        {
            get => _isEventBound;
            protected set => _isEventBound = value;
        }

        public override void RegisterDataBinding()
        {
            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for Event {1}", ViewModelName, SrcEventName);

                return;
            }

            if (!_isEventBound)
                BindEvent();
        }

        public override void UnregisterDataBinding()
        {
            var method = UnityEventBinder.GetRemoveListener(_srcEventProp.GetValue(SrcView));

            if (d == null || method == null)
                return;

            var p = new object[] { d };

            method.Invoke(_srcEventProp.GetValue(SrcView), p);

            _isEventBound = false;
        }

        protected virtual void BindEvent()
        {
            var vm = ViewModelProvider.Instance.GetViewModelBehaviour(ViewModelName);

            //TODO: Wrap PropertyInfo & MethodInfo in Serializable classes so we don't need reflection here

            if (_srcEventProp == null)
                _srcEventProp = SrcView.GetType().GetProperty(SrcEventName);

            if (_method == null)
                _method = ViewModelProvider.GetViewModelType(ViewModelName).GetMethod(DstMethodName);

            if (_method == null)
            {
                Debug.LogErrorFormat("EventBinding error in {0}. No method found in {1} with name {2}", gameObject.name, ViewModelName, DstMethodName);

                return;
            }

            var method = UnityEventBinder.GetAddListener(_srcEventProp.GetValue(SrcView));

            var arg = method.GetParameters()[0];
            d = Delegate.CreateDelegate(arg.ParameterType, vm, _method);

            var p = new object[] { d };

            method.Invoke(_srcEventProp.GetValue(SrcView), p);

            _isEventBound = true;
        }
    }
}
