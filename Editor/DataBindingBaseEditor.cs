using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityMVVM.Binding;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(DataBindingBase), true)]
    public class DataBindingBaseEditor : UnityEditor.Editor
    {

        public int _viewModelIdx = 0;
        List<string> _viewModels = new List<string>();

        SerializedProperty _viewmodelNameProp;

        private void OnEnable()
        {
            CollectSerializedProperties();
            (target as DataBindingBase).UpdateBindings();
        }

        protected virtual void CollectSerializedProperties()
        {
            _viewmodelNameProp = serializedObject.FindProperty("ViewModelName");
        }

        protected virtual void DrawChangeableElements()
        {
            var myClass = target as DataBindingBase;

            EditorGUILayout.LabelField("Source ViewModel");
            _viewModelIdx = EditorGUILayout.Popup(_viewModelIdx, _viewModels.ToArray());

        }

        protected virtual void UpdateSerializedProperties()
        {
            var myClass = target as DataBindingBase;

            myClass.ViewModelName = _viewModelIdx > -1 ?
                         _viewModels[_viewModelIdx] : null;
        }

        public override void OnInspectorGUI()
        {
            CollectSerializedProperties();

            _viewModels = ViewModelProvider.Viewmodels;

            serializedObject.Update();

            DrawDefaultInspector();

            var myClass = target as DataBindingBase;

            _viewModelIdx = _viewModels.ToList().IndexOf(_viewmodelNameProp.stringValue);

            if (_viewModelIdx < 0 && _viewModels.Count > 0)
            {
                _viewModelIdx = 0;
                myClass.ViewModelName = _viewModels.FirstOrDefault();
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
}
