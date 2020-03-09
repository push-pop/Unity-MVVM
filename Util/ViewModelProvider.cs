using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Util
{
    public class ViewModelProvider : Singleton<ViewModelProvider>
    {
        public static List<string> Viewmodels
        {
            get
            {
                if (_viewModels == null)
                    _viewModels = GetViewModels();

                return _viewModels;

            }
        }
        static List<string> _viewModels = null;

        public static List<string> GetViewModels()
        {
            Assembly mscorlib = Assembly.GetExecutingAssembly();
            Type t = typeof(ViewModelBase);

            return mscorlib.GetTypes().Where(e => e.IsSubclassOf(t)).Select(e => e.ToString()).ToList();
        }

        public static Type GetViewModelType(string typeString)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return asm.GetType(typeString);
        }

        internal ViewModelBase GetViewModelBehaviour(string viewModelName)
        {

            var vm = GetComponent(ViewModelProvider.GetViewModelType(viewModelName));
            if (vm == null)
                vm = FindObjectOfType(ViewModelProvider.GetViewModelType(viewModelName)) as ViewModelBase;

            if (vm == null)
                return gameObject.AddComponent(ViewModelProvider.GetViewModelType(viewModelName)) as ViewModelBase;

            return vm as ViewModelBase;

        }

        public static List<string> GetViewModelPropertyList<T>(string viewModelTypeString)
        {
            return GetViewModelPropertyList(viewModelTypeString, typeof(T));
        }

        public static List<string> GetViewModelPropertyList(string viewModelTypeString, Type t = null)
        {
            var query = GetViewModelProperties(viewModelTypeString)
                .Where(prop =>
                        prop.GetGetMethod(false) != null
                        && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                    );
            if (t != null)
                query = query.Where(prop => t.IsAssignableFrom(prop.PropertyType));

            return query.Select(e => e.Name).ToList();
        }

        public static List<string> GetViewModelMethodNames(string viewModelTypeString)
        {
            return GetViewModelMethods(viewModelTypeString)
                .Where(m =>
                        !m.IsSpecialName &&
                        !m.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any())
                .Select(e => e.Name).ToList();
        }

        public static PropertyInfo[] GetViewModelProperties(string viewModelTypeString, BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return asm.GetType(viewModelTypeString).GetProperties(bindingFlags);
        }

        internal static MethodInfo[] GetViewModelMethods(string viewModelName, BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return asm.GetType(viewModelName).GetMethods(bindingFlags);
        }
    }
}
