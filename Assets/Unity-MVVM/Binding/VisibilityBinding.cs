using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Binding;

namespace UnityMVVM.Binding
{
    public class VisibilityBinding : OneWayDataBinding
    {
        public override bool KeepConnectionAliveOnDisable => true;
    }
}
