using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace UnityMVVM.Model
{
    [Serializable]
    public abstract class ModelBase :
        IModel, INotifyPropertyChanged, IEquatable<ModelBase>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged<T>(Expression<Func<T>> memberExpr)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(GetName(memberExpr)));
        }

        public void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static string GetName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        public virtual bool Equals(ModelBase other)
        {
            return Object.ReferenceEquals(this, other);
        }
    }
}