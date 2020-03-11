using System;
using System.Reflection;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Types;
using UnityMVVM.Util;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Binding
{
    public class EventPropertyBinding : DataBindingBase
    {
        #region SerializedProperties
        public string SrcEventName = null;
        public string SrcPropName = null;
        public string SrcPropPath = null;
        public string DstPropName = null;
        public string DstPath = null;

        public EventArgType ArgType;
        public float FloatArg = 0.0f;
        public int IntArg = 0;
        public string StringArg = "";
        public bool BoolArg = false;
        #endregion

        [HideInInspector]
        public Component SrcView;

        [HideInInspector]
        public ViewModelBase _dstViewModel;

        MethodInfo _method;
        PropertyInfo _srcEventProp;

        [SerializeField]
        ValueConverterBase converter;

        BindTarget dst;
        BindTarget src;

        Delegate d;

        public override bool IsBound { get => _isBound; protected set => _isBound = value; }
        bool _isBound;

        public override void RegisterDataBinding()
        {
            _dstViewModel = ViewModelProvider.Instance.GetViewModelBehaviour(ViewModelName);

            dst = new BindTarget(_dstViewModel, DstPropName);

            if (ArgType == EventArgType.Property)
                src = new BindTarget(SrcView, SrcPropName, SrcPropPath);

            BindEvent();
        }

        public override void UnregisterDataBinding()
        {
            if (!_isBound) return;

            var method = UnityEventBinder.GetRemoveListener(_srcEventProp.GetValue(SrcView));

            if (d == null || method == null) return;

            var p = new object[] { d };

            method.Invoke(_srcEventProp.GetValue(SrcView), p);
        }

        protected virtual void BindEvent()
        {
            if (_isBound) return;

            //TODO: Wrap PropertyInfo & MethodInfo in Serializable classes so we don't need reflection here
            try
            {
                if (_srcEventProp == null)
                    _srcEventProp = SrcView.GetType().GetProperty(SrcEventName);

                if (_method == null)
                    _method = this.GetType().GetMethod(nameof(SetProp));

                var method = UnityEventBinder.GetAddListener(_srcEventProp.GetValue(SrcView));

                var arg = method.GetParameters()[0];
                d = Delegate.CreateDelegate(arg.ParameterType, this, _method);

                var p = new object[] { d };

                method.Invoke(_srcEventProp.GetValue(SrcView), p);

                _isBound = true;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("EventPropertyBinding error in {0}. {1}", gameObject.name, e.Message);
            }
        }

        public void SetProp()
        {
            object toSet = null;

            try
            {
                switch (ArgType)
                {
                    case EventArgType.Property:
                        toSet = src.GetValue();
                        break;
                    case EventArgType.String:
                        toSet = StringArg;
                        break;
                    case EventArgType.Int:
                        toSet = IntArg;
                        break;
                    case EventArgType.Float:
                        toSet = FloatArg;
                        break;
                    case EventArgType.Bool:
                        toSet = BoolArg;
                        break;
                    default:
                        break;
                }

                if (converter != null)
                    dst.SetValue(converter.Convert(toSet, dst.property.PropertyType, null));

                else if (dst.property.PropertyType.IsEnum)
                    dst.SetValue(Enum.Parse(dst.property.PropertyType, toSet.ToString()));
                else
                    dst.SetValue(Convert.ChangeType(toSet, dst.property.PropertyType));
            }
            catch (Exception exc)
            {
                Debug.LogErrorFormat("[EventPropertyBinding Error] - {0} Can't convert {1} to type {2}", gameObject.name, toSet, dst.property.PropertyType.Name);

                if (exc.InnerException != null)
                    Debug.LogErrorFormat("Inner Exception: {0}", exc.InnerException.Message);
            }
        }
    }

}
