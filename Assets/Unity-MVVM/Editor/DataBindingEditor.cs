using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Enums;
using UnityMVVM.Extensions;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DataBinding), true)]
    public class DataBindingEditor
        : DataBindingBaseEditor
    {
        protected SerializedProperty _dstViewProp;
        protected SerializedProperty _converterProp;

        protected SerializedProperty _bindingModeProp;

        protected SerializedList _srcNames = new SerializedList("SrcPropertyName");
        protected SerializedList _srcPaths = new SerializedList("SrcPropertyPath");
        protected SerializedList _dstNames = new SerializedList("DstPropertyName");
        protected SerializedList _dstPaths = new SerializedList("DstPropertyPath");

        protected SerializedList _dstChangeEvents = new SerializedList("DstChangedEventName");


        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _srcNames.Init(serializedObject);
            _srcPaths.Init(serializedObject);
            _dstNames.Init(serializedObject);
            _dstPaths.Init(serializedObject);
            _dstChangeEvents.Init(serializedObject);

            _dstViewProp = serializedObject.FindProperty("DstView");
            _converterProp = serializedObject.FindProperty("Converter");

            _bindingModeProp = serializedObject.FindProperty("BindingMode");
        }


        protected override void DrawChangeableElements()
        {

            if (string.IsNullOrEmpty(_viewModelProp.Value))
            {
                base.DrawChangeableElements();
                return;
            }

            base.DrawChangeableElements();
            GUIUtils.BindingField("Source Property", _srcNames, _srcPaths);

            GUIUtils.EnumField<BindingMode>("Mode", _bindingModeProp);


            GUIUtils.ObjectField("Dest View", _dstViewProp);
            GUIUtils.BindingField("Destination Property", _dstNames, _dstPaths);
            if (_bindingModeProp.GetEnumValue<BindingMode>() != BindingMode.OneWay)
                GUIUtils.BindingField("Dest Changed Event", _dstChangeEvents);



            GUIUtils.ObjectField("Converter", _converterProp);

        }

        protected override void SetupDropdownIndices()
        {
            base.SetupDropdownIndices();

            _srcNames.SetupIndex();
            _srcPaths.SetupIndex();

            _dstNames.SetupIndex();
            _dstPaths.SetupIndex();

            _dstChangeEvents.SetupIndex();
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();

            _srcNames.UpdateProperty();
            _srcPaths.UpdateProperty();

            _dstNames.UpdateProperty();
            _dstPaths.UpdateProperty();

            _dstChangeEvents.UpdateProperty();
        }

        protected override void CollectPropertyLists()
        {
            var bindingMode = (BindingMode)_bindingModeProp.enumValueIndex;

            base.CollectPropertyLists();

            if (_viewModelChanged)
            {
                _srcNames.Value = null;
                _srcPaths.Value = null;
            }

            if (string.IsNullOrEmpty(_viewModelProp.Value))
                return;

            var view = _dstViewProp.objectReferenceValue as Component;

            _srcNames.Clear();
            _srcPaths.Clear();
            _dstNames.Clear();
            _dstPaths.Clear();

            _dstChangeEvents.Clear();


            if (view)
            {
                var dstNeedsGetter = bindingMode != BindingMode.OneWay;
                var dstNeedsSetter = bindingMode != BindingMode.OneWayToSource;

                _dstNames.Values = view.GetBindablePropertyList(needsGetter: dstNeedsGetter, needsSetter: dstNeedsSetter);
                _dstPaths.Values = view.GetPropertiesAndFieldsList(_dstNames.Value);
                _dstChangeEvents.Values = view.GetBindableEventsList();
            }
            else
            {
                _dstNames.Value = null;
                _dstPaths.Value = null;
                _dstChangeEvents.Value = null;
            }

            var vmType = ViewModelProvider.GetViewModelType(ViewModelName);

            _srcNames.Values = ViewModelProvider.GetViewModelPropertyList(ViewModelName);

            var propType = vmType.GetProperty(_srcNames.Value)?.PropertyType;
            if (propType != null)
                _srcPaths.Values = propType.GetNestedFields();
        }
    }
}
