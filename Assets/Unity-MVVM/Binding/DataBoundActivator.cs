using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Binding;

namespace UnityMVVM.Binding
{
    public class DataBoundActivator : OneWayDataBinding
    {
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                gameObject.SetActive(_invert ? !_isActive : _isActive);
            }
        }

        bool _isActive;

        [SerializeField]
        bool _invert;

        public override bool KeepConnectionAliveOnDisable => true;

        protected override void OnValidate()
        {
            base.OnValidate();

            _dstView = this;
            DstPropertyName = nameof(IsActive);
        }
    }
}
