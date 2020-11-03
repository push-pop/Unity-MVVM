using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityMVVM.Editor
{
    public class PackageUpdater
    {
        [MenuItem("Unity-MVVM/Update Package")]
        public static void Update()
        {
            UnityEditor.PackageManager.Client.Add("https://github.com/push-pop/Unity-MVVM.git#upm");
        }
    }
}