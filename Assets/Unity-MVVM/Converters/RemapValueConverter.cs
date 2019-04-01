using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    public class RemapValueConverter : ValueConverterBase
    {
        public float InputMax
        {
            set
            {
                InputRange.y = value;
            }
        }

        [SerializeField]
        Vector2 InputRange = new Vector2(0, 1);

        [SerializeField]
        Vector2 OutputRange = new Vector2(0, 1);

        [SerializeField]
        bool _floorToInt;

        public override object Convert(object value, Type targetType, object parameter)
        {
            if (value is Single)
            {
                var retVal = ((Single)value).Map(InputRange.x, InputRange.y, InputRange.x, OutputRange.y);

                return _floorToInt ? System.Convert.ChangeType(Mathf.FloorToInt(retVal), targetType) : retVal;
            }
            else if (value is Single?)
            {
                var retVal = (value as Single?).Map(InputRange.x, InputRange.y, InputRange.x, OutputRange.y);

                return retVal == null ? null : _floorToInt ? System.Convert.ChangeType(Mathf.FloorToInt(retVal.Value), targetType) : retVal;
            }

            else if (value is Double)
            {
                var retVal = ((Double)value).Map(InputRange.x, InputRange.y, InputRange.x, OutputRange.y);

                return _floorToInt ? System.Convert.ChangeType(Mathf.Floor((float)retVal), targetType) : retVal;
            }

            else if (value is Double?)
            {
                var retVal = (value as Double?).Map(InputRange.x, InputRange.y, InputRange.x, OutputRange.y);

                return retVal == null ? null : _floorToInt ? System.Convert.ChangeType(Mathf.FloorToInt((float)retVal.Value), targetType) : retVal;
            }

            else
                throw new NotImplementedException();
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }

    public static class RemapExt
    {
        public static float Map(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float? Map(this float? value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static double Map(this double value, double from1, double to1, double from2, double to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static double? Map(this double? value, double from1, double to1, double from2, double to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}