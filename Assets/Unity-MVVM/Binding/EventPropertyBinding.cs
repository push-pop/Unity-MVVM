using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Util;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Binding
{
    public class EventPropertyBinding : MonoBehaviour
    {
        [HideInInspector]
        public string SrcEventName = null;
        [HideInInspector]
        public string ViewModelName = null;
        [HideInInspector]
        public string DstPropName = null;

        [HideInInspector]
        public List<string> SrcEvents = new List<string>();
        [HideInInspector]
        public List<string> DstProps = new List<string>();
        [HideInInspector]
        public List<string> ViewModels = new List<string>();

        public Component _srcView;
        [HideInInspector]
        public ViewModelBase _dstViewModel;

        MethodInfo _method;
        PropertyInfo _srcEventProp;

        [SerializeField]
        string value;

        [SerializeField]
        bool isProperty;

        [SerializeField]
        ValueConverterBase converter;

        BindTarget dst;
        BindTarget src;

        // Use this for initialization
        protected virtual void Awake()
        {
            UpdateBindings();

            _dstViewModel = ViewModelProvider.Instance.GetViewModelBehaviour(ViewModelName);

            dst = new BindTarget(_dstViewModel, DstPropName);
            if (isProperty)
                src = new BindTarget(_srcView, value);

            BindEvent();
        }
        protected virtual void BindEvent()
        {
            var method = UnityEventBinder.GetAddListener(_srcEventProp.GetValue(_srcView));

            var arg = method.GetParameters()[0];
            var d = Delegate.CreateDelegate(arg.ParameterType, this, _method);

            var p = new object[] { d };

            method.Invoke(_srcEventProp.GetValue(_srcView), p);
        }

        public void SetProp()
        {
            try
            {
                var toSet = isProperty ? src.GetValue() : value;

                if (converter != null)
                    dst.SetValue(converter.Convert(toSet, dst.property.PropertyType, null));

                else if (dst.property.PropertyType.IsEnum)
                    dst.SetValue(Enum.Parse(dst.property.PropertyType, toSet.ToString()));
                else
                    dst.SetValue(Convert.ChangeType(toSet, dst.property.PropertyType));
            }
            catch (Exception exc)
            {
                Debug.LogErrorFormat("[EventPropertyBinding Error] - {0} Can't convert {1} to type {2}", gameObject.name, value, dst.property.PropertyType.Name);
            }
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
            var d = Delegate.CreateDelegate(arg.ParameterType, this, _method);

            var p = new object[] { d };

            method.Invoke(_srcEventProp.GetValue(_srcView), p);
        }

        public void UpdateBindings()
        {
            try
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
                    var props = ViewModelProvider.GetViewModelProperties(ViewModelName, BindingFlags.Instance | BindingFlags.Public);
                    DstProps = props.Where(prop => prop.GetSetMethod(false) != null
                               && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                            ).Select(e => e.Name).ToList();
                }

                if (!string.IsNullOrEmpty(DstPropName))
                    _method = this.GetType().GetMethod(nameof(SetProp));

            }
            catch (Exception exc)
            {
                Debug.LogErrorFormat("Error creating bindings in {0}. Exception: {1}", gameObject.name, exc.Message);

                if (exc.InnerException != null)
                    Debug.LogErrorFormat("InnerException: {0}" + exc.InnerException.Message);
            }

        }

    }

}
