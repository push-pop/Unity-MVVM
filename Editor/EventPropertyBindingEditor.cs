using UnityEditor;
using UnityMVVM.Binding;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EventPropertyBinding), true)]
    public class EventPropertyBindingEditor : UnityEditor.Editor
    {
        SerializedProperty _dstMethodNameProp;
        SerializedProperty _eventNameProp;
        SerializedProperty _viewModelName;

        int _eventIdx = 0;
        int _dstMethodIdx = 0;
        int _viewModelIdx = 0;

        private void OnEnable()
        {
            CollectSerializedProperties();
        }

        protected void CollectSerializedProperties()
        {
            _dstMethodNameProp = serializedObject.FindProperty("DstPropName");
            _eventNameProp = serializedObject.FindProperty("SrcEventName");
            _viewModelName = serializedObject.FindProperty("ViewModelName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            var myClass = target as EventPropertyBinding;

            _dstMethodIdx = myClass.DstProps.IndexOf(_dstMethodNameProp.stringValue);
            _eventIdx = myClass.SrcEvents.IndexOf(_eventNameProp.stringValue);
            _viewModelIdx = myClass.ViewModels.IndexOf(_viewModelName.stringValue);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Source Event");
            _eventIdx = EditorGUILayout.Popup(_eventIdx, myClass.SrcEvents.ToArray());

            EditorGUILayout.LabelField("Destination ViewModel");
            _viewModelIdx = EditorGUILayout.Popup(_viewModelIdx, myClass.ViewModels.ToArray());

            EditorGUILayout.LabelField("Destination Property");
            _dstMethodIdx = EditorGUILayout.Popup(_dstMethodIdx, myClass.DstProps.ToArray());


            if (EditorGUI.EndChangeCheck())
            {
                myClass.ViewModelName = _viewModelIdx > -1 ?
                    myClass.ViewModels[_viewModelIdx] : null;

                myClass.SrcEventName = _eventIdx > -1 ?
                    myClass.SrcEvents[_eventIdx] : null;

                myClass.DstPropName = _dstMethodIdx > -1 ?
                    myClass.DstProps[_dstMethodIdx] : null;

                EditorUtility.SetDirty(target);

                serializedObject.ApplyModifiedProperties();

                myClass.UpdateBindings();
            }
        }

    }
}