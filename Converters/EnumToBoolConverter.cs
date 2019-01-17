using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    public class EnumToBoolConverter : ValueConverterBase
    {
        [SerializeField]
        string _expectedValue;

        [SerializeField]
        bool _invert;

        public override object Convert(object value, Type targetType, object parameter)
        {
            var equals = value.ToString().Equals(_expectedValue);

            return _invert ? !equals : equals;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}