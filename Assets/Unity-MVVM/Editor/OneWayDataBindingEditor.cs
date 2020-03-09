using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Extensions;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(OneWayDataBinding), true)]
    public class OneWayDataBindingEditor
        : DataBindingBaseEditor
    {
        protected SerializedProperty _dstViewProp;
        protected SerializedProperty _converterProp;

        protected SerializedList _srcNames = new SerializedList("SrcPropertyName");
        protected SerializedList _srcPaths = new SerializedList("SrcPropertyPath");
        protected SerializedList _dstNames = new SerializedList("DstPropertyName");
        protected SerializedList _dstPaths = new SerializedList("DstPropertyPath");

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _srcNames.Init(serializedObject);
            _srcPaths.Init(serializedObject);
            _dstNames.Init(serializedObject);
            _dstPaths.Init(serializedObject);

            _dstViewProp = serializedObject.FindProperty("_dstView");
            _converterProp = serializedObject.FindProperty("_converter");
        }


        protected override void DrawChangeableElements()
        {
            GUIUtils.ObjectField("Dest View", _dstViewProp);

            base.DrawChangeableElements();

            GUIUtils.BindingField("Source Property", _srcNames, _srcPaths);

            GUIUtils.ObjectField("Converter", _converterProp);

            GUIUtils.BindingField("Destination Property", _dstNames, _dstPaths);
        }

        protected override void SetupDropdownIndices()
        {
            base.SetupDropdownIndices();

            _srcNames.SetupIndex();
            _srcPaths.SetupIndex();

            _dstNames.SetupIndex();
            _dstPaths.SetupIndex();
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();
            _srcNames.UpdateProperty();
            _srcPaths.UpdateProperty();

            _dstNames.UpdateProperty();
            _dstPaths.UpdateProperty();
        }

        protected override void CollectPropertyLists()
        {
            base.CollectPropertyLists();

            if (_viewModelChanged)
            {
                _srcNames.Value = null;
                _srcPaths.Value = null;
            }

            var view = _dstViewProp.objectReferenceValue as Component;

            _srcNames.Clear();
            _srcPaths.Clear();
            _dstNames.Clear();
            _dstPaths.Clear();


            if (view)
            {
                _dstNames.Values = view.GetBindablePropertyList(needsGetter: false);
                _dstPaths.Values = view?.GetPropertiesAndFieldsList(_dstNames.Value);
            }
            else
            {
                _dstNames.Value = null;
                _dstPaths.Value = null;
            }

            var vmType = ViewModelProvider.GetViewModelType(ViewModelName);

            _srcNames.Values = ViewModelProvider.GetViewModelPropertyList(ViewModelName);

            var propType = vmType.GetProperty(_srcNames.Value)?.PropertyType;
            _srcPaths.Values = propType?.GetNestedFields();
        }
    }
}
