using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class UserAssemblyConfig : ScriptableObject
{
    [HideInInspector]
    public List<string> UserAssemblyNames;

#if UNITY_EDITOR
    public AssemblyDefinitionAsset[] UserAssemblies = new AssemblyDefinitionAsset[0];
#endif

    public static bool UserAssembliesChanged;

#if UNITY_EDITOR
    [MenuItem("Unity-MVVM/Create User Assembly Config")]
    public static void CreateObject()
    {
        var dir = $"Assets/Unity-MVVM/Resources";
        var fileName = "UserAssemblyConfig.asset";
        var path = $"{dir}/{fileName}";

        var asset = Resources.Load<UserAssemblyConfig>("UserAssemblyConfig");
        if (asset == null)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            asset = ScriptableObject.CreateInstance<UserAssemblyConfig>();
            AssetDatabase.CreateAsset(asset, path);
        }

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
#endif

#if UNITY_EDITOR
    private void OnValidate()
    {
        UserAssembliesChanged = true;
        UserAssemblyNames = UserAssemblies.Where(e => e != null).Select(e => e.name).ToList();
        AssetDatabase.Refresh();
    }

    private void OnDestroy()
    {
        UserAssembliesChanged = true;
    }
#endif

}