using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;

public class BindingMonitor : EditorWindow
{
    [SerializeField]
    BindTarget src = new BindTarget(new object(), "testProp");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [MenuItem("Unity-MVVM/Binding Monitor")]
    static void Init()
    {
        var window = (BindingMonitor)EditorWindow.GetWindow(typeof(BindingMonitor));

        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Bindings", EditorStyles.boldLabel);

        EditorGUILayout.TextField(string.Format("Binding: {0} {1}", src.propertyOwner, src.propertyName));
    }
}
