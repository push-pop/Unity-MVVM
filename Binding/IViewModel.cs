using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnityMVVM
{
    namespace ViewModel
    {
        public interface IViewModel
        {
            void NotifyPropertyChanged<T>(Expression<Func<T>> memberExpr);
        }
    }
}


