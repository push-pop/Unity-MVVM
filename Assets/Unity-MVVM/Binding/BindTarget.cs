using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace UnityMVVM.Binding
{
    [System.Serializable]
    public class BindTarget
    {
        public object propertyOwner;

        public string propertyName;

        public string propertyPath;

        public PropertyInfo property;

        public BindTarget(object propOwner, string propName, string path = null, UnityEvent dstChangedEvent = null)
        {
            propertyOwner = propOwner;
            propertyName = propName;
            propertyPath = path;

            if (propertyOwner == null)
            {
                Debug.LogErrorFormat("Could not find ViewModel for Property {0}", propName);
            }

            property = propertyOwner.GetType().GetProperty(propertyName);//.ResolvePath(path);

            if (dstChangedEvent != null)
                dstChangedEvent.AddListener(new UnityAction(() =>
                {

                }));
        }

        public object GetValue()
        {
            return property != null ? property.GetValue(propertyOwner, null) : null;
        }

        public void SetValue(object src)
        {
            if (property == null) return;

            property.SetValue(propertyOwner, src, null);
        }
    }
}


