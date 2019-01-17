using System;
using UnityEngine;
using UnityMVVM.View;

namespace UnityMVVM.Binding.Converters
{
    public class BoolToVisibilityConverter : ValueConverterBase
    {
        [SerializeField]
        bool _collapse;

        public override object Convert(object value, Type targetType, object parameter)
        {
            return (bool)value ? Visibility.Visible : _collapse ? Visibility.Collapsed : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
