using UnityEngine;
using UnityMVVM.View;

namespace UnityMVVM.Binding
{
    [AddComponentMenu("Unity MVVM/Bindings/Visibility")]
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
