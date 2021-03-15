using System;
using System.Linq;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    [AddComponentMenu("Unity-MVVM/Converters/EnumToBool")]
    public class EnumToBoolConverter : ValueConverterBase
    {
        [SerializeField]
        protected string _expectedValue;

        [SerializeField]
        protected bool _invert;

        public override object Convert(object value, Type targetType, object parameter)
        {
            var values = _expectedValue.Split('|').Select(p => p.Trim());

            var equals = false;

            foreach (var item in values)
            {
                equals |= value.ToString().Equals(item);
            }

            return _invert ? !equals : equals;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            Debug.LogError($"{GetType().Name} ConvertBack not implemented. Override this class if you wish to implement");
            return null;
        }
    }
}
