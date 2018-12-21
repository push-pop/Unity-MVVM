using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVVM
{
    namespace Binding
    {
        namespace Converters
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
    }
}
