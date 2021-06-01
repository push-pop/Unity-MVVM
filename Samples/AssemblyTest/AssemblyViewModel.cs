using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Samples
{
    public class AssemblyViewModel : ViewModelBase
    {
        public string AssemblyName => Assembly.GetExecutingAssembly().FullName;

    }
} 