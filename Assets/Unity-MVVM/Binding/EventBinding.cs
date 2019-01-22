using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityMVVM.Util;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Binding
{
    public class EventBinding : MonoBehaviour
    {
        [HideInInspector]
        public string SrcEventName = null;
        [HideInInspector]
        public string ViewModelName = null;
        [HideInInspector]
        public string DstMethodName = null;

        [HideInInspector]
        public List<string> SrcEvents = new List<string>();
        [HideInInspector]
        public List<string> DstMethods = new List<string>();
        [HideInInspector]
        public List<string> ViewModels = new List<string>();

        public Component _srcView;
        [HideInInspector]
        public ViewModelBase _dstViewModel;

        MethodInfo _method;
        PropertyInfo _srcEventProp;

        // Use this for initialization
        protected virtual void Awake()
        {

            UpdateBindings();

            BindEvent();

        }
        protected virtual void BindEvent()
        {
            _dstViewModel = ViewModelProvider.Instance.GetViewModelBehaviour(ViewModelName);

            var method = UnityEventBinder.GetAddListener(_srcEventProp.GetValue(_srcView));

            var arg = method.GetParameters()[0];
            var d = Delegate.CreateDelegate(arg.ParameterType, _dstViewModel, _method);

            var p = new object[] { d };

            method.Invoke(_srcEventProp.GetValue(_srcView), p);
        }

        public void handler(object caller, params object[] args)
        {
            var newArgs = new object[args.Length + 1];
            newArgs[0] = _srcView;
            args.CopyTo(newArgs, 1);

            _method.Invoke(_dstViewModel, newArgs);
        }

        private void OnValidate()
        {
            UpdateBindings();
        }

        private void OnDestroy()
        {
            var method = UnityEventBinder.GetRemoveListener(_srcEventProp.GetValue(_srcView));

            var arg = method.GetParameters()[0];
            var d = Delegate.CreateDelegate(arg.ParameterType, _dstViewModel, _method);

            var p = new object[] { d };

            method.Invoke(_srcEventProp.GetValue(_srcView), p);
        }

        public void UpdateBindings()
        {
            ViewModels = ViewModelProvider.Viewmodels;

            if (_srcView != null)
            {
                var props = _srcView.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                SrcEvents = props
                    .Where(p => p.PropertyType.IsSubclassOf(typeof(UnityEventBase))
                                   && !p.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any())
                                        .Select(p => p.Name).ToList();

                if (!string.IsNullOrEmpty(SrcEventName))
                    _srcEventProp = _srcView.GetType().GetProperty(SrcEventName);
            }

            if (!string.IsNullOrEmpty(ViewModelName))
            {
                var methods = ViewModelProvider.GetViewModelMethods(ViewModelName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                DstMethods = methods.Where(m => !m.IsSpecialName && !m.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()).Select(e => e.Name).ToList(); ;
            }


            if (!string.IsNullOrEmpty(DstMethodName))
                _method = ViewModelProvider.GetViewModelType(ViewModelName).GetMethod(DstMethodName);
        }

    }
}
