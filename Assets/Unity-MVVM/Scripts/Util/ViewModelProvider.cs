﻿using System;
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
        public static Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();

        public static Assembly UserAssembly
        {
            get
            {
                if (_userAssembly == null)
                {
                    try
                    {
                        _userAssembly = Assembly.Load("Assembly-CSharp");
                    }
                    catch
                    {

                    }
                }

                return _userAssembly;
            }
        }
        static Assembly _userAssembly;



        public static List<string> Viewmodels
        {
            get
            {
                if (_viewModels == null)
                {
                    _viewModels = GetViewModels(Assembly.GetExecutingAssembly()); // Samples

                    if (UserAssembly != null)
                        _viewModels = _viewModels.Concat(GetViewModels(UserAssembly)).ToList(); // User Assembly
                }

                return _viewModels;

            }
        }
        static List<string> _viewModels = null;

        public static List<string> GetViewModels(Assembly asm)
        {
            if (asm == null) return new List<string>();
            return asm.GetTypes().Where(e => e.IsSubclassOf(ViewModelBaseType)).Select(e => e.ToString()).ToList();
        }

        public static Type GetViewModelType(string typeString)
        {
            Type t = null;

            t = UserAssembly?.GetType(typeString);

            if (t == null)
                t = ExecutingAssembly.GetType(typeString);

            if (t == null)
                Debug.LogError($"ViewModel type {typeString} not found. Is it in a different Assembly?");

            return t;
        }

        public ViewModelBase GetViewModelBehaviour(string viewModelName)
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

        public static PropertyInfo[] GetViewModelProperties(string typeString, BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
        {
            return GetViewModelType(typeString).GetProperties(bindingFlags);
        }

        internal static MethodInfo[] GetViewModelMethods(string typeString, BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
        {
            return GetViewModelType(typeString).GetMethods(bindingFlags);
        }
    }
}
