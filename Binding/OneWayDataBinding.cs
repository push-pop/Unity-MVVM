using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Util;

namespace UnityMVVM.Binding
{
    public class OneWayDataBinding
        : DataBindingBase
    {
        protected DataBindingConnection _connection;

        [HideInInspector]
        public List<string> SrcProps = new List<string>();
        [HideInInspector]
        public List<string> DstProps = new List<string>();

        [HideInInspector]
        public string SrcPropertyName = null;

        [HideInInspector]
        public string DstPropertyName = null;

        [SerializeField]
        protected UnityEngine.Component _dstView;

        [SerializeField]
        protected ValueConverterBase _converter;

        [HideInInspector]
        protected string PropertyPath = null;

        public virtual bool KeepConnectionAliveOnDisable { get { return _keepConnectionAliveOnDisable; } }
        protected bool _keepConnectionAliveOnDisable = false;

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
                _connection = new DataBindingConnection(gameObject, new BindTarget(_viewModel, SrcPropertyName, path: PropertyPath), new BindTarget(_dstView, DstPropertyName), _converter);
            }

            _connection.Bind();
        }

        public override void UnregisterDataBinding()
        {
            base.UnregisterDataBinding();
            if (_connection != null)
                _connection.ClearHandler();
        }

        protected override void UpdateBindings()
        {
            base.UpdateBindings();

            if (_dstView != null)
            {
                var props = _dstView.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                DstProps = props.Where(prop => prop.GetSetMethod(false) != null
                       && prop.GetSetMethod(false) != null
                       && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                    ).Select(e => e.Name).ToList(); ;
            }

            if (!string.IsNullOrEmpty(ViewModelName))
            {
                var props = ViewModelProvider.GetViewModelProperties(ViewModelName);
                SrcProps = props.Where(prop => prop.GetGetMethod(false) != null
                       && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                    ).Select(e => e.Name).ToList();
            }
        }

        private void Start()
        {
            _connection.OnSrcUpdated();
            _isStartup = false;
        }

        protected virtual void OnEnable()
        {
            if (!_isStartup)
                _connection.OnSrcUpdated();
        }

        private void OnDisable()
        {
            if (!KeepConnectionAliveOnDisable)
                UnregisterDataBinding();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_connection != null)
                _connection.Dispose();
        }
    }
}
