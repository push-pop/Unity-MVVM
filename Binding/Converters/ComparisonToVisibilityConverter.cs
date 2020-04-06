using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.View;

namespace UnityMVVM.Binding.Converters
{
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