using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityMVVM.Util;
using UnityMVVM.ViewModel;

namespace UnityMVVM
{
    namespace Binding
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
            void Awake()
            {
                UpdateBindings();
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

            private void UpdateBindings()
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
                    _dstViewModel = FindObjectOfType(ViewModelProvider.GetViewModelType(ViewModelName)) as ViewModelBase;
                    var methods = ViewModelProvider.GetViewModelMethods(ViewModelName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                    DstMethods = methods.Where(m => !m.IsSpecialName && !m.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()).Select(e => e.Name).ToList(); ;
                }

                if (_dstViewModel != null && !string.IsNullOrEmpty(DstMethodName))
                    _method = _dstViewModel.GetType().GetMethod(DstMethodName);


            }

#if UNITY_EDITOR
            [CustomEditor(typeof(EventBinding))]
            public class EventBindingEditor : Editor
            {
                SerializedProperty _dstMethodNameProp;
                SerializedProperty _eventNameProp;
                SerializedProperty _viewModelName;

                int _eventIdx = 0;
                int _dstMethodIdx = 0;
                int _viewModelIdx = 0;

                private void OnEnable()
                {
                    CollectSerializedProperties();
                }

                protected virtual void CollectSerializedProperties()
                {
                    _dstMethodNameProp = serializedObject.FindProperty("DstMethodName");
                    _eventNameProp = serializedObject.FindProperty("SrcEventName");
                    _viewModelName = serializedObject.FindProperty("ViewModelName");
                }

                public override void OnInspectorGUI()
                {
                    serializedObject.Update();

                    DrawDefaultInspector();

                    var myClass = target as EventBinding;

                    _dstMethodIdx = myClass.DstMethods.IndexOf(_dstMethodNameProp.stringValue);
                    _eventIdx = myClass.SrcEvents.IndexOf(_eventNameProp.stringValue);
                    _viewModelIdx = myClass.ViewModels.IndexOf(_viewModelName.stringValue);

                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.LabelField("Source Event");
                    _eventIdx = EditorGUILayout.Popup(_eventIdx, myClass.SrcEvents.ToArray());

                    EditorGUILayout.LabelField("Destination ViewModel");
                    _viewModelIdx = EditorGUILayout.Popup(_viewModelIdx, myClass.ViewModels.ToArray());

                    EditorGUILayout.LabelField("Destination Method");
                    _dstMethodIdx = EditorGUILayout.Popup(_dstMethodIdx, myClass.DstMethods.ToArray());


                    if (EditorGUI.EndChangeCheck())
                    {
                        myClass.ViewModelName = _viewModelIdx > -1 ?
                            myClass.ViewModels[_viewModelIdx] : null;

                        myClass.SrcEventName = _eventIdx > -1 ?
                            myClass.SrcEvents[_eventIdx] : null;

                        myClass.DstMethodName = _dstMethodIdx > -1 ?
                            myClass.DstMethods[_dstMethodIdx] : null;

                        EditorUtility.SetDirty(target);

                        serializedObject.ApplyModifiedProperties();

                        myClass.UpdateBindings();
                    }
                }

            }


#endif
        }
    }
}