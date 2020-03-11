using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Extensions;
using UnityMVVM.Util;


namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EventBinding), true)]
    public class EventBindingEditor : DataBindingBaseEditor
    {
        SerializedList _eventNames = new SerializedList("SrcEventName");
        SerializedList _methodNames = new SerializedList("DstMethodName");

        SerializedProperty _srcViewProp;

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();

            GUIUtils.BindingField("Destination Method", _methodNames);
            GUIUtils.ObjectField("Source View", _srcViewProp, typeof(Component));
            GUIUtils.BindingField("Source Event", _eventNames);
        }

        protected override void SetupDropdownIndices()
        {
            base.SetupDropdownIndices();

            _eventNames.SetupIndex();
            _methodNames.SetupIndex();
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();

            _eventNames.UpdateProperty();
            _methodNames.UpdateProperty();
        }

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _eventNames.Init(serializedObject);
            _methodNames.Init(serializedObject);

            _srcViewProp = serializedObject.FindProperty("SrcView");
        }

        protected override void CollectPropertyLists()
        {
            base.CollectPropertyLists();

            _eventNames.Clear();
            _methodNames.Clear();

            var view = _srcViewProp.objectReferenceValue as Component;

            if (view)
                _eventNames.Values = view.GetBindableEventsList();

            else
                _eventNames.Value = null;


            _methodNames.Values = ViewModelProvider.GetViewModelMethodNames(ViewModelName);
        }
    }
}
