using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityMVVM.Binding.Converters
{
    public abstract class ValueConverterBase : MonoBehaviour, IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter);

        public abstract object ConvertBack(object value, Type targetType, object parameter);
    }
}