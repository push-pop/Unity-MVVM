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

        Delegate d;

        // Use this for initialization
        protected virtual void Awake()
        {
            BindEvent();
        }

        protected virtual void BindEvent()
        {
            _dstViewModel = ViewModelProvider.Instance.GetViewModelBehaviour(ViewModelName);

            //TODO: Wrap PropertyInfo & MethodInfo in Serializable classes so we don't need reflection here
            
            if (_srcEventProp == null)
                _srcEventProp = _srcView.GetType().GetProperty(SrcEventName);

            if (_method == null)
                _method = ViewModelProvider.GetViewModelType(ViewModelName).GetMethod(DstMethodName);

            if (_method == null)
            {
                Debug.LogErrorFormat("EventBinding error in {0}. No method found in {1} with name {2}", gameObject.name, ViewModelName, DstMethodName);

                return;
            }

            var method = UnityEventBinder.GetAddListener(_srcEventProp.GetValue(_srcView));

            var arg = method.GetParameters()[0];
            d = Delegate.CreateDelegate(arg.ParameterType, _dstViewModel, _method);

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

            if (d == null || method == null)
                return;

            var p = new object[] { d };

            method.Invoke(_srcEventProp.GetValue(_srcView), p);
        }

        public void UpdateBindings()
        {
            ViewModels = ViewModelProvider.Viewmodels;

            if (_srcView != null)
            {
                var props = _srcView.GetType().GetProperties(/*BindingFlags.DeclaredOnly |*/ BindingFlags.Instance | BindingFlags.Public);

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
