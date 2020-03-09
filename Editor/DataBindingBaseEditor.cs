using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    #region GUI Utilities
    public static class GUIUtils
    {
        public static GUILayoutOption[] labelOptions = { GUILayout.ExpandWidth(false), GUILayout.MaxWidth(120) };
        public static GUILayoutOption[] objectFieldOptions = { GUILayout.ExpandWidth(true) };

        public static void ObjectField(string label, SerializedProperty prop, Type t = null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, labelOptions);
            EditorGUILayout.ObjectField(prop, t, GUIContent.none, objectFieldOptions);
            EditorGUILayout.EndHorizontal();
        }

        public static void BindingField(string label, SerializedList propsList, SerializedList pathsList = null)
        {

            bool hasPaths = pathsList != null && pathsList.Values.Count > 0;

            EditorGUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(label))
                EditorGUILayout.LabelField(label, labelOptions);

            propsList.Index = EditorGUILayout.Popup(propsList.Index, propsList.Values.ToArray());

            if (hasPaths)
                pathsList.Index = EditorGUILayout.Popup(pathsList.Index, pathsList.Values.ToArray());

            EditorGUILayout.EndHorizontal();
        }


        public static void BindingField(string label, ref int propIdx, List<string> props)
        {
            int idx = 0;
            BindingField(label, ref propIdx, props, ref idx, null);
        }

        public static void BindingField(string label, ref int propIdx, List<string> props, ref int pathIdx, List<string> paths)
        {
            bool hasPaths = paths != null && paths.Count > 0;

            EditorGUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(label))
                EditorGUILayout.LabelField(label, labelOptions);

            propIdx = EditorGUILayout.Popup(propIdx, props.ToArray());

            if (hasPaths)
                pathIdx = EditorGUILayout.Popup(pathIdx, paths.ToArray());

            EditorGUILayout.EndHorizontal();
        }

        public static void ViewModelField(SerializedList viewModelList)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("View Model", labelOptions);
            viewModelList.Index = EditorGUILayout.Popup(viewModelList.Index, viewModelList.Values.ToArray());
            if (UnityEngine.GUILayout.Button("Open"))
            {
                var type = ViewModelProvider.GetViewModelType(viewModelList.Value).Name;
                var str = AssetDatabase.FindAssets(type).FirstOrDefault();
                var path = AssetDatabase.GUIDToAssetPath(str);
                var asset = EditorGUIUtility.Load(path);
                AssetDatabase.OpenAsset(asset);
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void ViewModelField(ref int viewModelIdx, List<string> viewModels, SerializedProperty selectedViewModel)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("View Model", labelOptions);
            viewModelIdx = EditorGUILayout.Popup(viewModelIdx, viewModels.ToArray());
            if (UnityEngine.GUILayout.Button("Open"))
            {
                var type = ViewModelProvider.GetViewModelType(selectedViewModel.stringValue).Name;
                var str = AssetDatabase.FindAssets(type).FirstOrDefault();
                var path = AssetDatabase.GUIDToAssetPath(str);
                var asset = EditorGUIUtility.Load(path);
                AssetDatabase.OpenAsset(asset);
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void ToggleField(string label, SerializedProperty prop)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, labelOptions);

            prop.boolValue = EditorGUILayout.Toggle(prop.boolValue);

            EditorGUILayout.EndHorizontal();
        }

        internal static void EnumField<T>(string label, ref T idx)
            where T : Enum
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, labelOptions);
            idx = (T)EditorGUILayout.EnumPopup((T)idx);
            EditorGUILayout.EndHorizontal();
        }
    }
    #endregion

    [CustomEditor(typeof(DataBindingBase), true)]
    public class DataBindingBaseEditor : UnityEditor.Editor
    {
        public string ViewModelName { get => _viewModelProp.Value; }

        public int _viewModelIdx = 0;
        protected List<string> _viewModels = new List<string>();

        protected SerializedList _viewModelProp = new SerializedList("ViewModelName");

        protected bool _viewModelChanged { get; set; }

        protected virtual void OnEnable()
        {
            _viewModels = ViewModelProvider.Viewmodels;

            CollectSerializedProperties();

            CollectPropertyLists();

            serializedObject.ApplyModifiedProperties();

            UpdateSerializedProperties();
        }

        protected virtual void CollectSerializedProperties()
        {
            _viewModelProp.Init(serializedObject);
        }

        protected void DrawViewModelDrawer()
        {
            EditorGUI.BeginChangeCheck();

            GUIUtils.ViewModelField(_viewModelProp);

            _viewModelChanged = EditorGUI.EndChangeCheck();
        }

        protected virtual void DrawChangeableElements()
        {
            DrawViewModelDrawer();
        }

        protected virtual void UpdateSerializedProperties()
        {
            _viewModelProp.UpdateProperty();
        }

        protected virtual void SetupDropdownIndices()
        {
            _viewModelProp.SetupIndex();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SetupDropdownIndices();

            EditorGUI.BeginChangeCheck();

            DrawChangeableElements();

            if (EditorGUI.EndChangeCheck())
            {
                UpdateSerializedProperties();

                serializedObject.ApplyModifiedProperties();


                EditorUtility.SetDirty(target);

                CollectPropertyLists();

                _viewModelChanged = false;
            }

        }

        protected virtual void CollectPropertyLists()
        {
            _viewModelProp.Values = ViewModelProvider.GetViewModels();
        }

        protected void SetupIndex(ref int idx, SerializedProperty prop, List<string> values)
        {
            idx = values.IndexOf(prop.stringValue);
            if (idx < 0 && values.Count > 0)
            {
                idx = 0;
                prop.stringValue = values.FirstOrDefault();
            }
        }

        protected void UpdateListProperty(SerializedProperty prop, int idx, List<string> values)
        {
            prop.stringValue = idx > -1 ? values[idx] : null;
        }


    }
}
