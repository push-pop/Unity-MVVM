using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    public class EnumToColorConverter : ValueConverterBase
    {
        [SerializeField]
        protected string _expectedValue;

        [SerializeField] Color _trueColor = Color.green;
        [SerializeField] Color _falseColor = Color.red;

        public override object Convert(object value, Type targetType, object parameter)
        {
            var equals = value.ToString().Equals(_expectedValue);

            return equals ? _trueColor : _falseColor;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
