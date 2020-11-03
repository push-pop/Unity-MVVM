using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    public static class GUIUtils
    {
        public static GUILayoutOption[] messageOptions = { GUILayout.ExpandWidth(true) };
        public static GUILayoutOption[] labelOptions = { GUILayout.ExpandWidth(false), GUILayout.MaxWidth(120) };
        public static GUILayoutOption[] objectFieldOptions = { GUILayout.ExpandWidth(true) };


        public static void Message(string msg, MessageType msgType = MessageType.None)
        {

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.richText = true;
            labelStyle.wordWrap = true;

            switch (msgType)
            {
                case MessageType.Warning:
                    msg = msg.Color(Color.yellow).Bold();
                    break;
                case MessageType.Info:
                    msg = msg.Color(Color.blue).Bold();
                    break;
                case MessageType.Error:
                    msg = msg.Color(Color.red).Bold();
                    break;


            }
            EditorGUILayout.LabelField(msg, labelStyle);

        }

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

        public static bool ViewModelField(SerializedList viewModelList)
        {
            var changed = false;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("View Model", labelOptions);
            
            EditorGUI.BeginChangeCheck();
            viewModelList.Index = EditorGUILayout.Popup(viewModelList.Index, viewModelList.Values.ToArray());
            changed = EditorGUI.EndChangeCheck();

            if (UnityEngine.GUILayout.Button("Open"))
            {
                var type = ViewModelProvider.GetViewModelType(viewModelList.Value).Name;
                var str = AssetDatabase.FindAssets(type).FirstOrDefault();
                var path = AssetDatabase.GUIDToAssetPath(str);
                var asset = EditorGUIUtility.Load(path);
                AssetDatabase.OpenAsset(asset);
            }
            EditorGUILayout.EndHorizontal();

            return changed;
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

        internal static void EnumField<T>(string label, SerializedProperty prop)
            where T : Enum
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, labelOptions);
            int currIdx = prop.enumValueIndex;
            prop.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup((T)(object)currIdx));
            EditorGUILayout.EndHorizontal();
        }
    }
}
