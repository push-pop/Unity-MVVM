using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityMVVM
{
    namespace Binding
    {


        public class TwoWayDataBinding
            : OneWayDataBinding
        {
            [HideInInspector]
            public string _dstChangedEventName = null;

            [HideInInspector]
            public List<string> DstChangedEvents = new List<string>();

            public override void RegisterDataBinding()
            {
                base.RegisterDataBinding();

                var propInfo = _dstView.GetType().GetProperty(_dstChangedEventName);
                //dynamic evn = propInfo.GetValue(_dstView);
                var type = propInfo.PropertyType.BaseType;
                var argType = type.GetGenericArguments().FirstOrDefault();
                var evn = propInfo.GetValue(_dstView);

                UnityEventBinder.BindEvent(evn, () =>
                {
                    //Debug.Log("Change: " + dst.GetValue());
                    if (_converter != null)
                        src.SetValue(_converter.ConvertBack(dst.GetValue(), src.property.PropertyType, null));
                    else
                        src.SetValue(Convert.ChangeType(dst.GetValue(), src.property.PropertyType));
                });

            }

            public static Delegate CreateDelegate(Type delegateType, Action<object[]> target)
            {
                var sourceParameters = delegateType.GetMethod("Invoke").GetParameters();

                var parameters = sourceParameters.Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();

                var castParameters = parameters.Select(p => Expression.TypeAs(p, typeof(object))).ToArray();

                var createArray = Expression.NewArrayInit(typeof(object), castParameters);

                var invokeTarget = Expression.Invoke(Expression.Constant(target), createArray);

                var lambdaExpression = Expression.Lambda(delegateType, invokeTarget, parameters);

                return lambdaExpression.Compile();
            }



            private void BindEvent(object evn, Action handler)
            {
                var args = evn.GetType().GetGenericArguments();
                Debug.Log("Generic Args: ");
                foreach (var arg in args)
                {
                    Debug.Log(arg);
                }
                Type eventType = typeof(UnityEvent<>);
                Type genericType = eventType.MakeGenericType(args);

            }

            public void GetEventType<T>(dynamic even)
            {

            }

            public void OnChange(string change)
            {
                Debug.Log("Change: " + change);
                src.SetValue(dst.GetValue());
            }

            static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
            {
                while (toCheck != null && toCheck != typeof(object))
                {
                    var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                    if (generic == cur)
                    {
                        return true;
                    }
                    toCheck = toCheck.BaseType;
                }
                return false;
            }

            protected override void OnValidate()
            {
                UpdateBindings();
            }

            protected override void UpdateBindings()
            {
                base.UpdateBindings();
                if (_dstView != null)
                {
                    var props = _dstView.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    DstChangedEvents = props.Where(p => p.PropertyType.IsSubclassOf(typeof(UnityEventBase))
                                                   && !p.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any())
                                            .Select(p => p.Name).ToList();
                }
            }

#if UNITY_EDITOR

            [CustomEditor(typeof(TwoWayDataBinding), true)]
            public class TwoWayDataBindingEditor : OneWayDataBindingEditor
            {
                private void OnEnable()
                {
                    CollectSerializedProperties();
                }

                protected override void CollectSerializedProperties()
                {
                    base.CollectSerializedProperties();
                    _eventNameProp = serializedObject.FindProperty("_dstChangedEventName");
                }

                int _eventIdx = 0;
                SerializedProperty _eventNameProp;

                protected override void DrawChangeableElements()
                {
                    base.DrawChangeableElements();
                    var myClass = target as TwoWayDataBinding;

                    EditorGUILayout.LabelField("Destination Changed Event");
                    _eventIdx = EditorGUILayout.Popup(_eventIdx, myClass.DstChangedEvents.ToArray());
                }

                protected override void UpdateSerializedProperties()
                {
                    base.UpdateSerializedProperties();

                    var myClass = target as TwoWayDataBinding;

                    myClass._dstChangedEventName = _eventIdx > -1 ?
                        myClass.DstChangedEvents[_eventIdx] : null;
                }

                public override void OnInspectorGUI()
                {

                    var myClass = target as TwoWayDataBinding;

                    _eventIdx = myClass.DstChangedEvents.IndexOf(_eventNameProp.stringValue);

                    base.OnInspectorGUI();
                }
            }
#endif
        }
    }
}