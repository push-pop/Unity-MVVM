using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityMVVM.Binding;
using UnityMVVM.Extensions;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(OneWayDataBinding), true)]
    public class OneWayDataBindingEditor : DataBindingBaseEditor
    {
        public int _srcIndex = 0;
        public int _dstIndex = 0;
        public int _dstPathIndex = 0;
        public int _srcPathIndex = 0;

        SerializedProperty _srcNameProp;
        SerializedProperty _dstNameProp;
        SerializedProperty _srcPathProp;
        SerializedProperty _dstPathProp;

        SerializedProperty _srcProps;
        SerializedProperty _dstProps;
        SerializedProperty _srcPaths;
        SerializedProperty _dstPaths;

        string[] _srcPropNames;
        string[] _srcPathNames;
        string[] _dstPropNames;
        string[] _dstPathNames;

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _srcNameProp = serializedObject.FindProperty("SrcPropertyName");
            _dstNameProp = serializedObject.FindProperty("DstPropertyName");
            _srcPathProp = serializedObject.FindProperty("SrcPropertyPath");
            _dstPathProp = serializedObject.FindProperty("DstPropertyPath");

            _srcProps = serializedObject.FindProperty("SrcProps");
            _dstProps = serializedObject.FindProperty("DstProps");
            _srcPaths = serializedObject.FindProperty("SrcPaths");
            _dstPaths = serializedObject.FindProperty("DstPaths");

            _srcPropNames = _srcProps.GetStringArray();
            _dstPropNames = _dstProps.GetStringArray();
            _srcPathNames = _srcPaths.GetStringArray();
            _dstPathNames = _dstPaths.GetStringArray();
        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();


            bool srcHasPaths = _srcPathNames.Length > 0;

            EditorGUILayout.LabelField("Source Property");
            if (srcHasPaths)
                EditorGUILayout.BeginHorizontal();

            _srcIndex = EditorGUILayout.Popup(_srcIndex, _srcPropNames);
            if (srcHasPaths)
            {
                _srcPathIndex = EditorGUILayout.Popup(_srcPathIndex, _srcPathNames);
                EditorGUILayout.EndHorizontal();
            }




            EditorGUILayout.LabelField("Destination Property");

            bool dstHasPaths = _dstPathNames.Length > 0;
            if (dstHasPaths)
                EditorGUILayout.BeginHorizontal();

            _dstIndex = EditorGUILayout.Popup(_dstIndex, _dstPropNames);
            if (dstHasPaths)
            {
                _dstPathIndex = EditorGUILayout.Popup(_dstPathIndex, _dstPathNames);
                EditorGUILayout.EndHorizontal();
            }
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();
            var myClass = target as OneWayDataBinding;

            myClass.SrcPropertyName = _srcIndex > -1 ?
                   _srcPropNames[_srcIndex] : null;

            myClass.DstPropertyName = _dstIndex > -1 ?
                 _dstPropNames[_dstIndex] : null;

            myClass.DstPropertyPath = _dstPathIndex > -1 ?
                _dstPathNames[_dstPathIndex] : null;

            myClass.SrcPropertyPath = _srcPathIndex > -1 ?
                _srcPathNames[_srcPathIndex] : null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var myClass = target as OneWayDataBinding;

            _srcIndex = Array.IndexOf(_srcPropNames, _srcNameProp.stringValue);
            if (_srcIndex < 0 && _srcPropNames.Length > 0)
            {
                _srcIndex = 0;
                myClass.SrcPropertyName = _srcPropNames.FirstOrDefault();
            }

            _dstIndex = Array.IndexOf(_dstPropNames, _dstNameProp.stringValue);
            if (_dstIndex < 0 && _dstPropNames.Length > 0)
            {
                _dstIndex = 0;
                myClass.DstPropertyName = _dstPropNames.FirstOrDefault();
            }

            _dstPathIndex = Array.IndexOf(_dstPathNames, _dstPathProp.stringValue);
            if (_dstPathIndex < 0 && _dstPathNames.Length > 0)
            {
                _dstPathIndex = 0;
                myClass.DstPropertyPath = _dstPathNames.FirstOrDefault();
            }

            _srcPathIndex = Array.IndexOf(_srcPathNames, _srcPathProp.stringValue);
            if(_srcPathIndex < 0 && _srcPathNames.Length > 0)
            {
                _srcPathIndex = 0;
                myClass.SrcPropertyPath = _srcPathNames.FirstOrDefault();
            }
        }
    }
}
