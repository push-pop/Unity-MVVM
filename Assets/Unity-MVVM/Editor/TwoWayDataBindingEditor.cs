using UnityEditor;
using UnityMVVM.Binding;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(TwoWayDataBinding), true)]
    public class TwoWayDataBindingEditor : OneWayDataBindingEditor
    {
        private void OnEnable()
        {
            CollectSerializedProperties();
        }

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();
            _eventNameProp = serializedObject.FindProperty("_dstChangedEventName");
        }

        int _eventIdx = 0;
        SerializedProperty _eventNameProp;

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();
            var myClass = target as TwoWayDataBinding;

            EditorGUILayout.LabelField("Destination Changed Event");
            _eventIdx = EditorGUILayout.Popup(_eventIdx, myClass.DstChangedEvents.ToArray());
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();

            var myClass = target as TwoWayDataBinding;

            myClass._dstChangedEventName = _eventIdx > -1 ?
                myClass.DstChangedEvents[_eventIdx] : null;
        }

        public override void OnInspectorGUI()
        {

            var myClass = target as TwoWayDataBinding;

            _eventIdx = myClass.DstChangedEvents.IndexOf(_eventNameProp.stringValue);

            base.OnInspectorGUI();
        }
    }
}