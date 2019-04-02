using System;

namespace UnityMVVM.Model
{
    public abstract class ModelBase : IModel
    {
    }

    public abstract class ModelBase<T> : IModel, IEquatable<T>
        where T : class
    {
        public abstract bool Equals(T other);
        public override bool Equals(object obj) => Equals(obj as T);
        public override abstract int GetHashCode();

        T GetUnderlyingValue()
        {
            return this as T;
        }
    }
}