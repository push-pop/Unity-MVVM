using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Extensions;
using UnityMVVM.Util;

namespace UnityMVVM.Binding
{
    public class OneWayDataBinding
        : DataBindingBase
    {
        public DataBindingConnection Connection { get { return _connection; } }

        protected DataBindingConnection _connection;

        #region Binding Properties
        [HideInInspector]
        public List<string> SrcProps = new List<string>();

        [HideInInspector]
        public List<string> DstProps = new List<string>();

        [HideInInspector]
        public List<string> SrcPaths = new List<string>();

        [HideInInspector]
        public List<string> DstPaths = new List<string>();

        [HideInInspector]
        public string SrcPropertyName = null;

        [HideInInspector]
        public string DstPropertyName = null;

        [HideInInspector]
        public string SrcPropertyPath = null;

        [HideInInspector]
        public string DstPropertyPath = null;

        #endregion

        [SerializeField]
        public Component _dstView;

        [SerializeField]
        protected ValueConverterBase _converter;

        bool _isStartup = true;

        public override void RegisterDataBinding()
        {
            base.RegisterDataBinding();

            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for Property {1}", ViewModelName, SrcPropertyName);

                return;
            }
            if (_connection == null)
            {
                _connection = new DataBindingConnection(gameObject, new BindTarget(_viewModel, SrcPropertyName, SrcPropertyPath), new BindTarget(_dstView, DstPropertyName, DstPropertyPath), _converter);
            }

            if (KeepConnectionAliveOnDisable || isActiveAndEnabled)
                _connection.Bind();
        }

        public override void UnregisterDataBinding()
        {
            base.UnregisterDataBinding();

            if (_connection != null)
                _connection.Unbind();
        }

        public override void UpdateBindings()
        {
            base.UpdateBindings();

            DstProps = _dstView?.GetBindablePropertyList(false, true);

            DstPaths.Clear();
            _dstView?.GetPropertiesAndFieldsList(DstPropertyName, ref DstPaths);

            SrcPaths.Clear();
            if (!string.IsNullOrEmpty(ViewModelName))
            {
                var vmType = ViewModelProvider.GetViewModelType(ViewModelName);
                var propType = vmType.GetProperty(SrcPropertyName).PropertyType;

                SrcProps = ViewModelProvider.GetViewModelPropertyList(ViewModelName);
                propType.GetNestedFields(ref SrcPaths);

            }

        }

        private void Start()
        {
            _connection.OnSrcUpdated();
            _isStartup = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!_isStartup)
                _connection.OnSrcUpdated();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_connection != null)
                _connection.Dispose();
        }
    }
}
