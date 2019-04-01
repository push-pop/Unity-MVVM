using System;
using UnityEngine;
using UnityMVVM.Model;

namespace UnityMVVM
{
    namespace Examples
    {
        [Serializable]
        public class DataModel : ModelBase
        {
            public string message;
            public Color color;
            
        }
    }
}
