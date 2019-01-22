using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityMVVM.Binding;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(OneWayDataBinding), true)]
    public class OneWayDataBindingEditor : DataBindingBaseEditor
    {
        public int _srcIndex = 0;
        public int _dstIndex = 0;

        SerializedProperty _srcNameProp;
        SerializedProperty _dstNameProp;

        SerializedProperty _srcProps;
        SerializedProperty _dstProps;

        List<string> _srcPropNames;
        List<string> _dstPropNames;

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();
            _srcNameProp = serializedObject.FindProperty("SrcPropertyName");
            _dstNameProp = serializedObject.FindProperty("DstPropertyName");

            _srcProps = serializedObject.FindProperty("SrcProps");
            _dstProps = serializedObject.FindProperty("DstProps");

            _srcPropNames = _srcProps.GetStringArray();
            _dstPropNames = _dstProps.GetStringArray();

        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();

            var myClass = target as OneWayDataBinding;

            EditorGUILayout.LabelField("Source Property");
            _srcIndex = EditorGUILayout.Popup(_srcIndex, _srcPropNames.ToArray());

            EditorGUILayout.LabelField("Destination Property");
            _dstIndex = EditorGUILayout.Popup(_dstIndex, _dstPropNames.ToArray());
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();
            var myClass = target as OneWayDataBinding;

            myClass.SrcPropertyName = _srcIndex > -1 ?
                   _srcPropNames[_srcIndex] : null;

            myClass.DstPropertyName = _dstIndex > -1 ?
                 _dstPropNames[_dstIndex] : null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var myClass = target as OneWayDataBinding;

            _srcIndex = _srcPropNames.IndexOf(_srcNameProp.stringValue);
            if (_srcIndex < 0 && _srcPropNames.Count > 0)
            {
                _srcIndex = 0;
                myClass.SrcPropertyName = _srcPropNames.FirstOrDefault();
            }

            _dstIndex = _dstPropNames.IndexOf(_dstNameProp.stringValue);
            if (_dstIndex < 0 && _dstPropNames.Count > 0)
            {
                _dstIndex = 0;
                myClass.DstPropertyName = _dstPropNames.FirstOrDefault();
            }
        }
    }
}

public static class SerializedPropertyExt
{

    public static List<string> GetStringArray(this SerializedProperty prop)
    {
        List<string> list = new List<string>(prop.arraySize);

        for (int i = 0; i < prop.arraySize; i++)
        {
            list.Add(prop.GetArrayElementAtIndex(i).stringValue);
        }

        return list;
    }
}