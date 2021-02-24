using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    [AddComponentMenu("Unity-MVVM/Converters/FormatString")]
    public class FormatStringConverter : ValueConverterBase
    {
        [SerializeField]
        string _format = "{0}";

        public override object Convert(object value, Type targetType, object parameter)
        {
            return string.Format(_format, value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }

    }
}

