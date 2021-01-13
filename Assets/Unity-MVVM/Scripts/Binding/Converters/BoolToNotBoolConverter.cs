using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    [AddComponentMenu("Unity MVVM/Converters/Bool to Not Bool")]
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
