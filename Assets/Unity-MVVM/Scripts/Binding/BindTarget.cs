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

        public string eventName;

        public BindTarget()
        {

        }

        public BindTarget Init()
        {
            if (propertyOwner == null)
            {
                Debug.LogErrorFormat("Could not find ViewModel for Property {0}", propertyName);
            }

            // Default value for setting just prop with no path
            propertyPath = propertyPath?.Replace("--", "");
            property = propertyOwner.GetType().GetProperty(propertyName);
            if (property == null)
                Debug.LogError($"Error finding property {propertyName} on object: {propertyOwner}");

            PropertyInfo prop2;
            FieldInfo Field2;

            if (!string.IsNullOrEmpty(propertyPath))
            {
                property.PropertyType.GetPropertyOrField(propertyPath, out prop2, out Field2);
                field = property.PropertyType.GetField(propertyPath);
            }

            return this;
        }

        public BindTarget(object propOwner, string propName, string path = null, UnityEvent dstChangedEvent = null)
        {

            propertyOwner = propOwner;
            propertyName = propName;
            propertyPath = path;

            Init();
        }

        public object GetValue()
        {
            if (string.IsNullOrEmpty(propertyPath))
            {
                return property?.GetValue(propertyOwner, null);
            }
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

        public void SetValue(object value, bool isToSource, IValueConverter converter = null)
        {

            if (property == null)
            {
                Debug.LogError("SetValue property NULL");
                return;
            }

            if (field != null)
            {
                var parentProp = property.GetValue(propertyOwner, null);

                if (converter != null)
                    if (isToSource)
                        field.SetValue(parentProp, converter.ConvertBack(value, field.GetType(), null));
                    else
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
                    if (isToSource)
                        property.SetValue(propertyOwner, converter.ConvertBack(value, property.PropertyType, null));
                    else
                        property.SetValue(propertyOwner, converter.Convert(value, property.PropertyType, null));
                else if (value is IConvertible)
                    property.SetValue(propertyOwner, Convert.ChangeType(value, property.PropertyType));
                else
                {
                    property.SetValue(propertyOwner, value, null);
                }


            }
        }
    }
}


