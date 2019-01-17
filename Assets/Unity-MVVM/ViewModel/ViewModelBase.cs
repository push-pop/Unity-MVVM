using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityMVVM.ViewModel
{
    public class ViewModelBase :
        MonoBehaviour,
        IViewModel,
        INotifyPropertyChanged
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
    }
}