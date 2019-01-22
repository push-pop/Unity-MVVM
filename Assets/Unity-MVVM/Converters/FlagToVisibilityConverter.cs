using System;
using UnityEngine;
using UnityMVVM.View;

namespace UnityMVVM.Binding.Converters
{
    public class FlagToVisibilityConverter : FlagToBoolConverter
    {
        [SerializeField]
        bool _collapse;

        public override object Convert(object value, Type targetType, object parameter)
        {
            bool isTrue = (bool) base.Convert(value, typeof(bool), parameter);
           
            return isTrue ? Visibility.Visible : _collapse ? Visibility.Collapsed : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
