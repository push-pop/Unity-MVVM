using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Util;

namespace UnityMVVM.Binding
{
    public class OneWayDataBinding
        : DataBindingBase
    {
        protected BindTarget src;

        protected BindTarget dst;

        DataBindingConnection _connection;

        protected List<string> SrcProps = new List<string>();
        protected List<string> DstProps = new List<string>();

        [HideInInspector]
        public string SrcPropertyName = null;

        [HideInInspector]
        public string DstPropertyName = null;

        [SerializeField]
        protected UnityEngine.Component _dstView;

        [SerializeField]
        protected ValueConverterBase _converter;

        [HideInInspector]
        protected string PropertyPath = null;

        public virtual bool KeepConnectionAliveOnDisable { get { return _keepConnectionAliveOnDisable; } }
        protected bool _keepConnectionAliveOnDisable = false;

        public override void RegisterDataBinding()
        {
            base.RegisterDataBinding();

            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for Property {1}", ViewModelName, SrcPropertyName);

                return;
            }
            if (_connection == null)
            {
                src = new BindTarget(_viewModel, SrcPropertyName, path: PropertyPath);
                dst = new BindTarget(_dstView, DstPropertyName);

                _connection = new DataBindingConnection(src, OnSrcUpdated);
            }
            else
                _connection.SetHandler(OnSrcUpdated);

        }

        public override void UnregisterDataBinding()
        {
            base.UnregisterDataBinding();
            if (_connection != null)
                _connection.ClearHandler();
        }

        protected override void UpdateBindings()
        {
            base.UpdateBindings();

            if (_dstView != null)
            {
                var props = _dstView.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                DstProps = props.Where(prop => prop.GetSetMethod(false) != null
                       && prop.GetSetMethod(false) != null
                       && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                    ).Select(e => e.Name).ToList(); ;
            }

            if (!string.IsNullOrEmpty(ViewModelName))
            {
                var props = ViewModelProvider.GetViewModelProperties(ViewModelName);
                SrcProps = props.Where(prop => prop.GetGetMethod(false) != null
                       && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                    ).Select(e => e.Name).ToList();
            }
        }

        public override void OnSrcUpdated()
        {
            base.OnSrcUpdated();

            try
            {
                if (_converter)
                    dst.SetValue(_converter.Convert(src.GetValue(), dst.property.PropertyType, null));
                else
                    dst.SetValue(Convert.ChangeType(src.GetValue(), dst.property.PropertyType));
            }
            catch (Exception e)
            {
                Debug.LogError("Data binding error in: " + gameObject.name + ": " + e.Message);

            }
        }

        private void OnEnable()
        {
            RegisterDataBinding();
            OnSrcUpdated();
        }

        private void OnDisable()
        {
            if (!KeepConnectionAliveOnDisable)
                UnregisterDataBinding();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_connection != null)
                _connection.Dispose();
        }
#if UNITY_EDITOR
        [CanEditMultipleObjects]
        [CustomEditor(typeof(OneWayDataBinding), true)]
        public class OneWayDataBindingEditor : DataBindingBaseEditor
        {
            public int _srcIndex = 0;
            public int _dstIndex = 0;

            SerializedProperty _srcNameProp;
            SerializedProperty _dstNameProp;

            protected override void CollectSerializedProperties()
            {
                base.CollectSerializedProperties();
                _srcNameProp = serializedObject.FindProperty("SrcPropertyName");
                _dstNameProp = serializedObject.FindProperty("DstPropertyName");
            }

            protected override void DrawChangeableElements()
            {
                base.DrawChangeableElements();

                var myClass = target as OneWayDataBinding;

                EditorGUILayout.LabelField("Source Property");
                _srcIndex = EditorGUILayout.Popup(_srcIndex, myClass.SrcProps.ToArray());

                EditorGUILayout.LabelField("Destination Property");
                _dstIndex = EditorGUILayout.Popup(_dstIndex, myClass.DstProps.ToArray());
            }

            protected override void UpdateSerializedProperties()
            {
                base.UpdateSerializedProperties();
                var myClass = target as OneWayDataBinding;

                myClass.SrcPropertyName = _srcIndex > -1 ?
                       myClass.SrcProps[_srcIndex] : null;

                myClass.DstPropertyName = _dstIndex > -1 ?
                     myClass.DstProps[_dstIndex] : null;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                var myClass = target as OneWayDataBinding;

                _srcIndex = myClass.SrcProps.IndexOf(_srcNameProp.stringValue);
                if (_srcIndex < 0 && myClass.SrcProps.Count > 0)
                {
                    _srcIndex = 0;
                    myClass.SrcPropertyName = myClass.SrcProps.FirstOrDefault();
                }

                _dstIndex = myClass.DstProps.IndexOf(_dstNameProp.stringValue);
                if (_dstIndex < 0 && myClass.DstProps.Count > 0)
                {
                    _dstIndex = 0;
                    myClass.DstPropertyName = myClass.DstProps.FirstOrDefault();
                }
            }
        }
#endif
    }
}
