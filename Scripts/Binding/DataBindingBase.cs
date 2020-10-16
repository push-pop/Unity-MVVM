using UnityEngine;
using UnityMVVM.Util;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Binding
{
    public abstract class DataBindingBase :
        MonoBehaviour,
        IDataBinding
    {

        public ViewModelBase ViewModelSrc
        {
            get
            {
                if (_viewModel == null && !string.IsNullOrEmpty(ViewModelName))
                    _viewModel = ViewModelProvider.Instance.GetViewModelBehaviour(ViewModelName);

                return _viewModel;
            }
        }

        protected ViewModelBase _viewModel;

        public string ViewModelName = null;

        public virtual bool KeepConnectionAliveOnDisable { get { return _keepConnectionAliveOnDisable; } }

        protected bool _keepConnectionAliveOnDisable = false;

        [ContextMenu("UpdateComponent")]
        public bool UpdateComponent()
        {
            bool result = true;

            try
            {
                var db = gameObject.AddComponent<DataBinding>();
                db.ViewModelName = ViewModelName;

                var oneWay = this as OneWayDataBinding;

                if (oneWay != null)
                {
                    db.SrcPropertyName = oneWay.SrcPropertyName;
                    db.SrcPropertyPath = oneWay.SrcPropertyPath;
                    db.DstPropertyName = oneWay.DstPropertyName;
                    db.DstPropertyPath = oneWay.DstPropertyPath;

                    db.DstView = oneWay._dstView;
                    db.Converter = oneWay._converter;

                    db.BindingMode = Enums.BindingMode.OneWay;
                }

                var twoWay = this as TwoWayDataBinding;

                if (twoWay != null)
                {
                    db.DstChangedEventName = twoWay._dstChangedEventName;

                    db.BindingMode = Enums.BindingMode.TwoWay;
                }
                DestroyImmediate(this);
            }
            catch(System.Exception e)
            {
                result = false;
            }

            return result;
        }

        protected void FindViewModel()
        {
            if (!string.IsNullOrEmpty(ViewModelName))
            {
                _viewModel = ViewModelProvider.Instance.GetViewModelBehaviour(ViewModelName);
            }

            if (_viewModel == null)
                Debug.LogErrorFormat("ViewModel Null: {0}", gameObject.name);
        }


        protected virtual void OnEnable()
        {
            if (!_keepConnectionAliveOnDisable)
                RegisterDataBinding();
        }

        protected virtual void OnDisable()
        {
            if (!KeepConnectionAliveOnDisable)
                UnregisterDataBinding();
        }

        protected virtual void Awake()
        {
            FindViewModel();
            RegisterDataBinding();
        }

        protected virtual void OnDestroy()
        {
            UnregisterDataBinding();
        }

        #region IDataBinding Abstract Implementation
        public abstract bool IsBound { get; }
        public abstract void RegisterDataBinding();
        public abstract void UnregisterDataBinding();
        #endregion
    }
}
