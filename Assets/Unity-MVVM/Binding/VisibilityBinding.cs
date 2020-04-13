using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.View;

namespace UnityMVVM.Binding
{
    public class VisibilityBinding : OneWayDataBinding
    {
        public override bool KeepConnectionAliveOnDisable => true;

        private void OnValidate()
        {
            DstPropertyName = "ElementVisibility";
            _dstView = (Component)GetComponent<IView>();
        }
    }
}
