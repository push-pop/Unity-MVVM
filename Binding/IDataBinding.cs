using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityMVVM
{
    namespace Binding
    {
        public interface IDataBinding
        {
            void RegisterDataBinding();
            void UnregisterDataBinding();
        }
    }
}