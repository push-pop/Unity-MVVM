using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityMVVM.Extensions
{
    public static class TypeExtensions
    {
        public static List<string> GetBindablePropertyNames(this Type t, bool needsGetter = true, bool needsSetter = true)
        {
            return t.GetBindableProperties(needsGetter, needsSetter).Select(e => e.Name).ToList();
        }

        public static PropertyInfo[] GetBindableProperties(this Type t, bool needsGetter = true, bool needsSetter = true)
        {
            var query =
                 t.GetProperties( BindingFlags.Instance | BindingFlags.Public).Where(prop =>
                  !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any());

            if (needsSetter)
                query = query.Where(prop => prop.GetSetMethod(false) != null);

            if (needsGetter)
                query = query.Where(prop => prop.GetGetMethod(false) != null);

            return query.ToArray();
        }

        public static List<string> GetBindableFieldNames(this Type t)
        {
            return t.GetBindableFields().Select(e => e.Name).ToList();
        }

        public static FieldInfo[] GetBindableFields(this Type t)
        {
            return t.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).Where(field =>
                  !field.GetCustomAttributes(typeof(ObsoleteAttribute), true)
                  .Any()).ToArray();
        }

        public static void GetPropertyOrField(this Type t, string name, out PropertyInfo propInfo, out FieldInfo fieldInfo)
        {
            propInfo = null;
            fieldInfo = null;

            propInfo = t.GetProperty(name);

            if (propInfo == null)
                fieldInfo = t.GetField(name);

        }

        public static List<string> GetNestedFields(this Type parentPropType)
        {
            var list = new List<string>();

            parentPropType.GetNestedFields(ref list);

            return list;
        }

        public static void GetNestedFields(this Type parentPropType, ref List<string> list)
        {
            var props = parentPropType.GetBindableProperties();
            var fields = parentPropType.GetBindableFieldNames();

            // Special case don't want to get value field from Enum
            if (parentPropType.IsEnum)
                return;

            if (fields.Count > 0)
                list.Add("--");
            list.AddRange(fields);
        }

    }
}