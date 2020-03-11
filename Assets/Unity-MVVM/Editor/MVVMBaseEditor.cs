using UnityEditor;
using UnityMVVM.Types;

[CustomEditor(typeof(MVVMBase))]
public class MVVMBaseEditor : UnityEditor.Editor
{
    protected virtual void OnEnable()
    {
        CollectSerializedProperties();

        CollectPropertyLists();

        serializedObject.ApplyModifiedProperties();

        UpdateSerializedProperties();
    }

    protected virtual void CollectSerializedProperties() { }

    protected virtual void DrawChangeableElements() { }

    protected virtual void UpdateSerializedProperties() { }

    protected virtual void SetupDropdownIndices() { }

    protected virtual void CollectPropertyLists() { }

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
        }

    }

}
