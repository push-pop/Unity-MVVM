﻿using System;
using UnityEngine;
using UnityMVVM.View;

namespace UnityMVVM.Binding.Converters
{
    [AddComponentMenu("Unity MVVM/Converters/Bool to Visibility")]
    public class BoolToVisibilityConverter : ValueConverterBase
    {
        [SerializeField]
        bool _collapse;

        [SerializeField]
        bool _invert;

        public override object Convert(object value, Type targetType, object parameter)
        {
            value = _invert ? !(bool)value : (bool)value;

            return (bool)value ? Visibility.Visible : _collapse ? Visibility.Collapsed : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
