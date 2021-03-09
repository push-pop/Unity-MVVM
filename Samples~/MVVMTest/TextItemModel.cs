using System;
using UnityEngine;
using UnityMVVM.Model;

namespace UnityMVVM.Samples.MVVMTest
{
    [Serializable]
    public class TextItemModel : ModelBase
    {
        public string message { get => _message; set => _message = value; }
        public Color color { get => _color; set => _color = value; }

        [SerializeField]
        Color _color;

        [SerializeField]
        string _message;


        public override string ToString()
        {
            return string.Format("Message: {0} Color: {1}", message, color);
        }
    }
}
