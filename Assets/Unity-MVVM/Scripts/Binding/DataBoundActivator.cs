
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
                gameObject.SetActive(Invert ? !_isActive : _isActive);
            }
        }

        bool _isActive;

        public bool Invert;

        public override bool KeepConnectionAliveOnDisable => true;

        protected void OnValidate()
        {
            _dstView = this;
            DstPropertyName = nameof(IsActive);
        }
    }
}
