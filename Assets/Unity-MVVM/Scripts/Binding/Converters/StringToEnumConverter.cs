using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    [AddComponentMenu("Unity-MVVM/Converters/StringToEnum")]
    public class StringToEnumConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter)
        {
            return Enum.Parse(targetType, value.ToString());
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
