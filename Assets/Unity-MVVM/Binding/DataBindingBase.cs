using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Util;
using UnityMVVM.ViewModel;

namespace UnityMVVM
{
    namespace Binding
    {

        public class DataBindingBase :
    MonoBehaviour,
    IDataBinding
        {
            public ViewModelBase ViewModelSrc
            {
                get
                {
                    return _viewModel;
                }
            }

            //[HideInInspector]
            public ViewModelBase _viewModel;


            //[HideInInspector]
            public string ViewModelName = null;

            protected List<string> ViewModels = new List<string>();

            public virtual void RegisterDataBinding()
            {

            }

            protected virtual void OnValidate()
            {
                UpdateBindings();
            }

            protected virtual void UpdateBindings()
            {
                if (!string.IsNullOrEmpty(ViewModelName))
                {
                    _viewModel = FindObjectOfType(ViewModelProvider.GetViewModel(ViewModelName)) as ViewModelBase;
                }
                ViewModels = ViewModelProvider.Viewmodels;
            }

            public virtual void OnSrcUpdated()
            {

            }

            public virtual void UnregisterDataBinding()
            {
            }

            protected virtual void Awake()
            {
                RegisterDataBinding();
            }

            protected virtual void OnDestroy()
            {
                UnregisterDataBinding();
            }

#if UNITY_EDITOR
            [CustomEditor(typeof(DataBindingBase), true)]
            public class DataBindingBaseEditor : Editor
            {

                public int _viewModelIdx = 0;


                SerializedProperty _viewmodelNameProp;

                private void OnEnable()
                {
                    CollectSerializedProperties();
                }

                protected virtual void CollectSerializedProperties()
                {
                    _viewmodelNameProp = serializedObject.FindProperty("ViewModelName");
                }

                protected virtual void DrawChangeableElements()
                {
                    var myClass = target as DataBindingBase;

                    EditorGUILayout.LabelField("Source ViewModel");
                    _viewModelIdx = EditorGUILayout.Popup(_viewModelIdx, myClass.ViewModels.ToArray());



                }

                protected virtual void UpdateSerializedProperties()
                {
                    var myClass = target as DataBindingBase;

                    myClass.ViewModelName = _viewModelIdx > -1 ?
                                 myClass.ViewModels[_viewModelIdx] : null;


                }

                public override void OnInspectorGUI()
                {

                    serializedObject.Update();

                    DrawDefaultInspector();

                    var myClass = target as DataBindingBase;

                    _viewModelIdx = myClass.ViewModels.IndexOf(_viewmodelNameProp.stringValue);

                    if (_viewModelIdx < 0 && myClass.ViewModels.Count > 0)
                    {
                        _viewModelIdx = 0;
                        myClass.ViewModelName = myClass.ViewModels.FirstOrDefault();
                    }

                    EditorGUI.BeginChangeCheck();

                    DrawChangeableElements();

                    if (EditorGUI.EndChangeCheck())
                    {
                        UpdateSerializedProperties();

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