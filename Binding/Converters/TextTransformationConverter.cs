using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    public class TextTransformationConverter : ValueConverterBase
    {
        [SerializeField]
        Transformation _transformation;

        public override object Convert(object value, Type targetType, object parameter)
        {
            switch (_transformation)
            {
                case Transformation.ToLower:
                    return value.ToString().ToLower();
                case Transformation.ToUpper:
                    return value.ToString().ToUpper();
                default:
                    return value;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }

        enum Transformation
        {
            ToLower,
            ToUpper
        }
    }
}