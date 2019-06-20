using UnityEngine;
using UnityMVVM.Util;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Binding
{

    public class DataBindingBase :
MonoBehaviour,
IDataBinding
    {
        public ViewModelBase ViewModelSrc
        {
            get
            {
                return _viewModel;
            }
        }

        [HideInInspector]
        public ViewModelBase _viewModel;


        [HideInInspector]
        public string ViewModelName = null;

        public virtual bool KeepConnectionAliveOnDisable { get { return _keepConnectionAliveOnDisable; } }
        protected bool _keepConnectionAliveOnDisable = false;

        public virtual void RegisterDataBinding()
        {

        }

        protected virtual void OnValidate()
        {
            UpdateBindings();
        }

        public virtual void UpdateBindings()
        {

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



        public virtual void UnregisterDataBinding()
        {
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
    }
}
