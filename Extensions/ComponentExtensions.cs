
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityMVVM.Extensions
{
    public static class ComponentExtensions
    {
        public static List<string> GetBindablePropertyList(this Component component, bool needsGetter = true, bool needsSetter = true)
        {
            return component.GetType().GetBindablePropertyNames(needsGetter, needsSetter);
        }

       public static List<string> GetBindableEventsList(this Component component)
        {
            return component.GetType().GetBindableProperties(false, false).Where(p => p.PropertyType.IsSubclassOf(typeof(UnityEngine.Events.UnityEventBase))
                                               && !p.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any())
                                        .Select(p => p.Name).ToList();
        }


        public static void GetPropertiesAndFieldsListRecursive(this Component component, ref List<string> list, Type parentProp)
        {

        }

        public static void GetPropertiesAndFieldsList(this Component component, string parentPropName, ref List<string> list)
        {
            if (string.IsNullOrEmpty(parentPropName))
                return;

            var parentPropType = component.GetType().GetProperty(parentPropName).PropertyType;

            parentPropType.GetNestedFields(ref list);

        }
    }
}