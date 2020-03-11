using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Extensions;
using UnityMVVM.Types;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EventPropertyBinding), true)]
    public class EventPropertyBindingEditor : DataBindingBaseEditor
    {
        EventArgType _argTypeIdx = EventArgType.Int;

        SerializedList _dstNames = new SerializedList("DstPropName");
        SerializedList _dstPaths = new SerializedList("DstPath");
        SerializedList _eventNames = new SerializedList("SrcEventName");
        SerializedList _srcPropNames = new SerializedList("SrcPropName");
        SerializedList _srcPathNames = new SerializedList("SrcPropPath");

        SerializedProperty _srcViewProp;
        SerializedProperty _argTypeProp;

        SerializedProperty _floatArgProp;
        SerializedProperty _intArgProp;
        SerializedProperty _stringArgProp;
        SerializedProperty _boolArgProp;


        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _dstNames.Init(serializedObject);
            _dstPaths.Init(serializedObject);
            _eventNames.Init(serializedObject);
            _srcPropNames.Init(serializedObject);
            _srcPathNames.Init(serializedObject);

            _srcViewProp = serializedObject.FindProperty("SrcView");
            _argTypeProp = serializedObject.FindProperty("ArgType");

            _floatArgProp = serializedObject.FindProperty("FloatArg");
            _intArgProp = serializedObject.FindProperty("IntArg");
            _stringArgProp = serializedObject.FindProperty("StringArg");
            _boolArgProp = serializedObject.FindProperty("BoolArg");

        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();

            GUIUtils.BindingField("Destination property", _dstNames, _dstPaths);

            GUIUtils.ObjectField("Source View", _srcViewProp, typeof(Component));

            GUIUtils.BindingField("Source Event", _eventNames);

            EditorGUILayout.BeginHorizontal();
            GUIUtils.EnumField("Argument", ref _argTypeIdx);

            switch (_argTypeIdx)
            {
                case EventArgType.Property:
                    GUIUtils.BindingField(null, _srcPropNames);
                    break;
                case EventArgType.String:
                    _stringArgProp.stringValue = EditorGUILayout.TextField(_stringArgProp.stringValue);
                    break;
                case EventArgType.Int:
                    _intArgProp.intValue = EditorGUILayout.IntField(_intArgProp.intValue);
                    break;
                case EventArgType.Float:
                    _floatArgProp.floatValue = EditorGUILayout.FloatField(_floatArgProp.floatValue);
                    break;
                case EventArgType.Bool:
                    _boolArgProp.boolValue = EditorGUILayout.Toggle(_boolArgProp.boolValue);
                    break;
                default:
                    break;
            }

            EditorGUILayout.EndHorizontal();
        }



        protected override void SetupDropdownIndices()
        {
            base.SetupDropdownIndices();

            _dstNames.SetupIndex();
            _dstPaths.SetupIndex();
            _eventNames.SetupIndex();
            _srcPropNames.SetupIndex();
            _srcPathNames.SetupIndex();

            _argTypeIdx = (EventArgType)_argTypeProp.enumValueIndex;
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();

            _dstNames.UpdateProperty();
            _dstPaths.UpdateProperty();
            _eventNames.UpdateProperty();
            _srcPathNames.UpdateProperty();
            _srcPropNames.UpdateProperty();

            _argTypeProp.enumValueIndex = (int)_argTypeIdx;
        }

        protected override void CollectPropertyLists()
        {
            base.CollectPropertyLists();

            if (_viewModelChanged)
            {
                _dstNames.Value = null;
                _dstPaths.Value = null;
            }

            _dstNames.Clear();
            _dstPaths.Clear();
            _eventNames.Clear();
            _srcPropNames.Clear();
            _srcPathNames.Clear();

            var view = _srcViewProp.objectReferenceValue as Component;

            if (view)
            {
                _eventNames.Values = view.GetBindableEventsList();
                _srcPropNames.Values = view.GetBindablePropertyList(needsSetter: false);

                _srcPathNames.Values = view.GetPropertiesAndFieldsList(_srcPropNames.Value);
            }
            else
                _eventNames.Value = null;

            _dstNames.Values = ViewModelProvider.GetViewModelPropertyList(ViewModelName);

            var vmType = ViewModelProvider.GetViewModelType(ViewModelName);

            var propType = vmType.GetProperty(_dstNames.Value)?.PropertyType;
           _dstPaths.Values =  propType?.GetNestedFields();
        }

    }
}