using System;
using UnityEngine;
using UnityMVVM.Binding.Converters;

namespace UnityMVVM.Samples.MVVMTest
{
    public class ColorToTextConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter)
        {
            return string.Format("The Color is: {0}", ((Color)value));
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
