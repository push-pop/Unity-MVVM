using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace UnityMVVM.Editor
{
    public class SerializedList
    {
        public List<string> Values
        {
            get => values;
            set
            {
                values = value;
                if (string.IsNullOrEmpty(Value))
                    Value = values.FirstOrDefault();
            }
        }

        List<string> values = new List<string>();
        public string Value
        {
            get
            {
                return _backingProp.stringValue;
            }
            set
            {
                _backingProp.stringValue = value;
            }
        }

        public int Index { get => _idx; set => _idx = value; }
        SerializedProperty _backingProp;
        string propertyName;
        int _idx = -1;

        public SerializedList(string propName)
        {
            propertyName = propName;
        }

        public void Init(SerializedObject serializedObject)
        {
            _backingProp = serializedObject.FindProperty(propertyName);
        }

        public void SetupIndex()
        {
            _idx = values.IndexOf(_backingProp.stringValue);
            if (_idx < 0 && values.Count > 0)
            {
                _idx = 0;
                _backingProp.stringValue = values.FirstOrDefault();
            }
        }

        public void UpdateProperty()
        {
            _backingProp.stringValue = _idx > -1 ? values[_idx] : null;
        }

        public void Clear()
        {
            values.Clear();
        }
    }
}