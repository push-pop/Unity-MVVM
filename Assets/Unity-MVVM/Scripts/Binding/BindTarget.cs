using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Extensions;

namespace UnityMVVM.Binding
{
    [System.Serializable]
    public class BindTarget
    {
        public object propertyOwner;

        public string propertyName;

        public string propertyPath;

        public PropertyInfo property;

        public FieldInfo field;

        public BindTarget(object propOwner, string propName, string path = null, UnityEvent dstChangedEvent = null)
        {
            // Default value for setting just prop with no path
            path = path?.Replace("--", "");

            propertyOwner = propOwner;
            propertyName = propName;
            propertyPath = path;

            if (propertyOwner == null)
            {
                Debug.LogErrorFormat("Could not find ViewModel for Property {0}", propName);
            }

            property = propertyOwner.GetType().GetProperty(propertyName);

            PropertyInfo prop2;
            FieldInfo Field2;

            if (!string.IsNullOrEmpty(path))
            {
                property.PropertyType.GetPropertyOrField(path, out prop2, out Field2);
                field = property.PropertyType.GetField(path);
            }
        }

        public object GetValue()
        {
            if (string.IsNullOrEmpty(propertyPath))
                return property?.GetValue(propertyOwner, null);
            else
            {
                var parentProp = property.GetValue(propertyOwner, null);
                var parts = propertyPath.Split('.');

                FieldInfo field = null;
                PropertyInfo prop = null;

                foreach (var part in parts)
                {
                    parentProp.GetType().GetPropertyOrField(propertyPath, out prop, out field);
                    if (prop != null)
                        return prop.GetValue(parentProp);
                    else if (field != null)
                    {
                        var val = field.GetValue(parentProp);
                        return val;
                    }
                }

                return null;
            }
        }

        public void SetValue(object value, IValueConverter converter = null)
        {

            if (property == null) return;

            if (field != null)
            {
                var parentProp = property.GetValue(propertyOwner, null);

                if (converter != null)
                    field.SetValue(parentProp, converter.Convert(value, field.GetType(), null));
                else if (value is IConvertible)
                    field.SetValue(parentProp, Convert.ChangeType(value, field.FieldType));
                else
                    field.SetValue(parentProp, value);

                property.SetValue(propertyOwner, parentProp);
            }

            else
            {
                if (converter != null)
                    property.SetValue(propertyOwner, converter.Convert(value, property.PropertyType, null));
                else if (value is IConvertible)
                    property.SetValue(propertyOwner, Convert.ChangeType(value, property.PropertyType));
                else
                    property.SetValue(propertyOwner, value, null);

            }
        }
    }
}


