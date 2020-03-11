using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    public class BoolToColorConverter : ValueConverterBase
    {
        [SerializeField] Color _trueColor = Color.green;
        [SerializeField] Color _falseColor = Color.red;

        public override object Convert(object value, Type targetType, object parameter)
        {
            var b = (bool)value;

            return b ? _trueColor : _falseColor;

        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
