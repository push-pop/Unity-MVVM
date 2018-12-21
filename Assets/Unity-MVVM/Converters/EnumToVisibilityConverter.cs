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
            public class EnumToVisibilityConverter : ValueConverterBase
            {
                [SerializeField]
                string _expectedValue;

                [SerializeField]
                bool _collapse;

                [SerializeField]
                bool _invert;

                public override object Convert(object value, Type targetType, object parameter)
                {
                    var equals = value.ToString().Equals(_expectedValue);
                    equals = _invert ? !equals : equals;
                    return equals ?
                        Visibility.Visible : _collapse ? Visibility.Collapsed : Visibility.Hidden;
                }

                public override object ConvertBack(object value, Type targetType, object parameter)
                {
                    throw new NotImplementedException();
                }

                private void OnValidate()
                {

                }
            }
        }
    }
}
