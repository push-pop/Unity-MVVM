using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Model;
using UnityMVVM.View;

namespace UnityMVVM.Binding
{
    public class CollectionItemBinding : MonoBehaviour, IDataBinding
    {
        public string _srcProp;
        public string _srcPath;
        public string _dstPath;
        public string _dstProp;
        public Component _dstView;
        public ValueConverterBase _converter;

        public bool IsBound => _isBound;

        bool _isBound = false;
        bool _isStartup = true;

        public string SrcViewName;
        protected DataBindingConnection _connection;


        public void RegisterDataBinding(IModel model)
        {
            if (model == null)
            {
                Debug.LogError("No CollectionViewItem on parent. Where did this get spawned");
                enabled = false;
                return;
            }

            if (_connection == null)
            {
                _connection = new DataBindingConnection(gameObject, new BindTarget(model, _srcProp, _srcPath), new BindTarget(_dstView, _dstProp, _dstPath), _converter);
            }

            if (isActiveAndEnabled && !_isBound)
                _connection.Bind();

            _isBound = true;
        }

        public void UnregisterDataBinding()
        {

            if (_connection != null)
                _connection.Unbind();

            _isBound = false;
        }

        private void Awake()
        {
            RegisterDataBinding();
        }

        private void Start()
        {
            if (_connection != null)
                _connection.OnSrcUpdated();
            _isStartup = false;
        }

        protected void OnEnable()
        {
            RegisterDataBinding();

            if (!_isStartup)
                _connection.OnSrcUpdated();
        }

        private void OnDisable()
        {
            UnregisterDataBinding();
        }

        protected void OnDestroy()
        {
            if (_connection != null)
                _connection.Dispose();
        }

        public void RegisterDataBinding()
        {
            if (!_isBound && _connection != null)
                _connection.Bind();
        }
    }
}
