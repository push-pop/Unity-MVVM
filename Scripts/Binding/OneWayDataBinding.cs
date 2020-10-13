using UnityEngine;
using UnityMVVM.Binding.Converters;

namespace UnityMVVM.Binding
{
    public class OneWayDataBinding
        : DataBindingBase
    {
        public DataBindingConnection Connection { get { return _connection; } }

        public override bool IsBound { get => _isBound; protected set => _isBound = value; }


        protected DataBindingConnection _connection;

        #region Serialized Properties
        [HideInInspector]
        public string SrcPropertyName = null;

        [HideInInspector]
        public string DstPropertyName = null;

        [HideInInspector]
        public string SrcPropertyPath = null;

        [HideInInspector]
        public string DstPropertyPath = null;

        [HideInInspector]
        public Component _dstView;

        [HideInInspector]
        public ValueConverterBase _converter;

        #endregion
        private bool _isBound;

        bool _isStartup = true;

        public override void RegisterDataBinding()
        {
            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for Property {1}", ViewModelName, SrcPropertyName);

                return;
            }
            if (_connection == null)
            {
                _connection = new DataBindingConnection(gameObject, new BindTarget(_viewModel, SrcPropertyName, SrcPropertyPath), new BindTarget(_dstView, DstPropertyName, DstPropertyPath), _converter);
            }

            if ((KeepConnectionAliveOnDisable || isActiveAndEnabled) && !_isBound)
                _connection.Bind();

            _isBound = true;
        }

        public override void UnregisterDataBinding()
        {
            if (_connection != null)
                _connection.Unbind();

            _isBound = false;
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
