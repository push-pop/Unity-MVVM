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
        protected bool _viewModelChanged = true;


        private void OnEnable()
        {
            _viewModels = ViewModelProvider.Viewmodels;

            CollectSerializedProperties();
            //(target as DataBindingBase).UpdateBindings(); 
            CollectPropertyLists();
            UpdateSerializedProperties();
        }

        protected virtual void CollectSerializedProperties()
        {
            UnityEngine.Debug.Log("CollectSerializedProperties");
            _viewmodelNameProp = serializedObject.FindProperty("ViewModelName");
        }

        protected virtual void DrawChangeableElements()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Source ViewModel");
            _viewModelIdx = EditorGUILayout.Popup(_viewModelIdx, _viewModels.ToArray());

            _viewModelChanged = EditorGUI.EndChangeCheck();
        }

        protected virtual void UpdateSerializedProperties()
        {
            var myClass = target as DataBindingBase;

            myClass.ViewModelName = _viewModelIdx > -1 ?
                         _viewModels[_viewModelIdx] : null;
        }

        public override void OnInspectorGUI()
        {
            //CollectSerializedProperties();
            DrawDefaultInspector();

            //_viewModels = ViewModelProvider.Viewmodels;


            var myClass = target as DataBindingBase;

            serializedObject.Update();


            _viewModelIdx = _viewModels.ToList().IndexOf(myClass.ViewModelName);

            UnityEngine.Debug.Log("VM Index: " + _viewModelIdx);

            //if (_viewModelIdx < 0 && _viewModels.Count > 0)
            //{
            //    _viewModelIdx = 0;
            //    myClass.ViewModelName = _viewModels.FirstOrDefault();
            //}

            EditorGUI.BeginChangeCheck();

            DrawChangeableElements();

            if (EditorGUI.EndChangeCheck() || UnityEngine.GUILayout.Button("Test Change"))
            {
                UnityEngine.Debug.Log("Change");

                UpdateSerializedProperties();

                //myClass.UpdateBindings();
                CollectPropertyLists();
                serializedObject.ApplyModifiedProperties();

                EditorUtility.SetDirty(target);
            }
        }

        protected virtual void CollectPropertyLists()
        {

        }
    }
}
