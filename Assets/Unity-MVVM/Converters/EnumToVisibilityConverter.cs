using System;
using UnityEngine;
using UnityMVVM.View;

namespace UnityMVVM.Binding.Converters
{
    public class EnumToVisibilityConverter : EnumToBoolConverter
    {
        [SerializeField]
        string _expectedValue;

        [SerializeField]
        bool _collapse;

        [SerializeField]
        bool _invert;

        public override object Convert(object value, Type targetType, object parameter)
        {
            var equals = (bool) base.Convert(value, typeof(bool), parameter);

            equals = _invert ? !equals : equals;

            return equals ?
                Visibility.Visible : _collapse ? Visibility.Collapsed : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
