using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Binding.Converters;

namespace UnityMVVM.Samples.BasicUsage
{
    public class BoolToColorConverter : ValueConverterBase
    {
        [SerializeField]
        Color _trueColor = Color.green;

        [SerializeField]
        Color _falseColor = Color.red;

        public override object Convert(object value, Type targetType, object parameter)
        {
            return (bool)value ? _trueColor : _falseColor;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}