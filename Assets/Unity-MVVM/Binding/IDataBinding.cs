namespace UnityMVVM.Binding
{
    public interface IDataBinding
    {
        void RegisterDataBinding();
        void UnregisterDataBinding();

        bool IsBound { get; }
    }
}