using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Editor;

public class UpdateComponentsEditorWindow : EditorWindow
{
    Vector2 scrollPos = Vector2.zero;

    [MenuItem("Unity-MVVM/Component Updater")]
    static void Init()
    {
        var window = (UpdateComponentsEditorWindow)GetWindow<UpdateComponentsEditorWindow>();
        window.titleContent = new UnityEngine.GUIContent("Update Components");
        window.Show();
    }

    private void OnGUI()
    {
        GUIUtils.Message("Use this utility to replace all deprecated components with their new counterparts.");
        GUIUtils.Message("WARNING: THIS IS A DESTRUCTIVE OPERATION. PLEASE MAKE SURE YOU HAVE A BACKUP BEFORE ATTEMPTING!!!", MessageType.Error);

        EditorGUILayout.Space();
        GUIUtils.Message("The following components will be updated:");

        var obsoleteComponents = FindObjectsOfType<DataBindingBase>()
            .Where(e=>e.GetType().GetCustomAttributes(typeof(ObsoleteAttribute), true).FirstOrDefault() != null);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxHeight(600));


        foreach (var item in obsoleteComponents)
        {
            GUIUtils.Message($"{item.gameObject} - {item.GetType().Name} -> DataBinding");
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Update Components"))
        {
            foreach (var item in obsoleteComponents)
            {
                var isObsolete = item.GetType().GetCustomAttributes(typeof(ObsoleteAttribute),false).FirstOrDefault();

                if (!item.UpdateComponent())
                    Debug.LogError($"Failed to update component {item} on {item.gameObject}");
            }
        }
    }
}
