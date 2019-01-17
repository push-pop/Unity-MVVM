using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityMVVM.Binding.Converters
{
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

