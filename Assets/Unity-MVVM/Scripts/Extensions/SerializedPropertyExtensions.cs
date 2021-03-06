﻿#if UNITY_EDITOR
using UnityEditor;

namespace UnityMVVM.Extensions
{
    public static class SerializedPropertyExt
    {
        public static string[] GetStringArray(this SerializedProperty prop)
        {
            string[] arr = new string[prop.arraySize];

            for (int i = 0; i < prop.arraySize; i++)
                arr[i] = (prop.GetArrayElementAtIndex(i).stringValue);

            return arr;
        }

        public static T GetEnumValue<T>(this SerializedProperty prop)
            where T : System.Enum
        {
            return (T)(object)prop.enumValueIndex;
        }
    }
}
#endif