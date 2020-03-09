using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Binding.Converters;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DataBoundActivator), true)]
    public class DataBoundActivatorEditor : OneWayDataBindingEditor
    {
        SerializedProperty _invertProp;

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _invertProp = serializedObject.FindProperty("Invert");
        }

        protected override void DrawChangeableElements()
        {
            DrawViewModelDrawer();

            GUIUtils.ObjectField("Converter", _converterProp, typeof(ValueConverterBase));
            GUIUtils.BindingField("Source Property", _srcNames, _srcPaths);

            GUIUtils.ToggleField("Invert", _invertProp);
        }

    }
}
