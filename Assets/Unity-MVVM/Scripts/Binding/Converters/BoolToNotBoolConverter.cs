using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    [AddComponentMenu("Unity-MVVM/Converters/BoolToNotBool")]
    public class BoolToNotBoolConverter : ValueConverterBase
    {

        public override object Convert(object value, Type targetType, object parameter)
        {
            var b = (bool)value;

            return !b;

        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
