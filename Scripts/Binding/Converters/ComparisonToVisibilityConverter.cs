using System;
using UnityEngine;
using UnityMVVM.View;

namespace UnityMVVM.Binding.Converters
{
    [AddComponentMenu("Unity-MVVM/Converters/ComparisonToVisibility")]
    public class ComparisonToVisibilityConverter : ComparisonToBoolConverter
    {
        [SerializeField]
        bool _collapse;

        public override object Convert(object value, Type targetType, object parameter)
        {
            var result = (bool)base.Convert(value, typeof(bool), parameter);

            return result ? Visibility.Visible : _collapse ? Visibility.Collapsed : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}