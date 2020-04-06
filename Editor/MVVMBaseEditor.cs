using UnityEditor;
using UnityMVVM.Types;

[CustomEditor(typeof(MVVMBase))]
public abstract class MVVMBaseEditor : UnityEditor.Editor
{
    protected virtual void OnEnable()
    {
        CollectSerializedProperties();

        CollectPropertyLists();

        serializedObject.ApplyModifiedProperties();

        UpdateSerializedProperties();
    }

    protected abstract void CollectSerializedProperties();

    protected abstract void DrawChangeableElements();

    protected abstract void UpdateSerializedProperties();

    protected abstract void SetupDropdownIndices();

    protected abstract void CollectPropertyLists();

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SetupDropdownIndices();

        EditorGUI.BeginChangeCheck();

        DrawChangeableElements();

        if (EditorGUI.EndChangeCheck())
        {
            UpdateSerializedProperties();

            EditorUtility.SetDirty(target);

            CollectPropertyLists();

            serializedObject.ApplyModifiedProperties();
        }

    }

}
