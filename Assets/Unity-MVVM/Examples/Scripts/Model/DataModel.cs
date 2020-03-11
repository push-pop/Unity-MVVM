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

            public override string ToString()
            {
                return string.Format("Message: {0} Color: {1}", message, color);
            }
        }
    }
}
