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

        public static PropertyInfo[] GetBindableProperties(this Type t, bool needsSetter = true, bool needsGetter = true)
        {

            var query =
                 t.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(prop =>
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
            return t.GetFields(BindingFlags.Instance | BindingFlags.Public).Where(field =>
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

        public static void GetNestedFields(this Type parentPropType, ref List<string> list)
        {
            var props = parentPropType.GetBindableProperties();
            var fields = parentPropType.GetBindableFieldNames();
            list.AddRange(fields);

            foreach (var prop in props)
            {
                var nestedFields = prop.PropertyType.GetBindableFieldNames();

                if (props.Length == 0) return;

                list.Add(prop.Name);
                //list.AddRange(nestedFields.Select(e => prop.Name + "/" + e));
            }
        }
    }
}