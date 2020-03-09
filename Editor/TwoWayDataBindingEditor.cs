using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityMVVM.Binding;
using UnityMVVM.Extensions;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(TwoWayDataBinding), true)]
    public class TwoWayDataBindingEditor : OneWayDataBindingEditor
    {
        int _eventIdx = -1;

        SerializedList _eventNames = new SerializedList("_dstChangedEventName");

        protected override void SetupDropdownIndices()
        {
            base.SetupDropdownIndices();

            _eventNames.SetupIndex();
        }

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _eventNames.Init(serializedObject);
        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();

            GUIUtils.BindingField("Dest Changed Event", _eventNames);
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();

            _eventNames.UpdateProperty();
        }

        protected override void CollectPropertyLists()
        {
            base.CollectPropertyLists();

            var view = _dstViewProp.objectReferenceValue as UnityEngine.Component;

            _eventNames.Clear();

            if (view)
                _eventNames.Values = view.GetBindableEventsList();
            else
                _eventNames.Value = null;

        }
    }
}