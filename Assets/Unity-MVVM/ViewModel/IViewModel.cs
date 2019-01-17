using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnityMVVM.ViewModel
{
    public interface IViewModel
    {
        void NotifyPropertyChanged<T>(Expression<Func<T>> memberExpr);
    }
}


