using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Util
{
    public class ViewModelProvider : Singleton<ViewModelProvider>
    {
        public static Type ViewModelBaseType => typeof(ViewModelBase);

        public static Assembly UnityAssembly
        {
            get
            {
                if (_unityAssembly == null)
                {
                    _unityAssembly = Assembly.Load("Assembly-CSharp");

                    if (_unityAssembly != null)
                        Debug.Log("[Unity-MVVM] - Successfully loaded assembly " + _unityAssembly.GetName().Name + " for reflection");
                }

                return _unityAssembly;
            }
        }
        static Assembly _unityAssembly;

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

        public static List<string> GetViewModels(Assembly asm)
        {
            return asm.GetTypes().Where(e => e.IsSubclassOf(ViewModelBaseType)).Select(e => e.ToString()).ToList();
        }

        public static List<string> GetViewModels()
        {
            return GetViewModels(UnityAssembly);
        }

        public static Type GetViewModelType(string typeString)
        {
            return UnityAssembly.GetType(typeString);
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
            return UnityAssembly.GetType(viewModelTypeString).GetProperties(bindingFlags);
        }

        internal static MethodInfo[] GetViewModelMethods(string viewModelName, BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
        {
            return UnityAssembly.GetType(viewModelName).GetMethods(bindingFlags);
        }
    }
}
