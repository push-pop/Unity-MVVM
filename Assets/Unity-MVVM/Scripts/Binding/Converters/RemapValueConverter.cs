using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    [AddComponentMenu("Unity-MVVM/Converters/RemapValue")]
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
            switch (value)
            {
                case float number:
                    number = number.Map(InputRange, OutputRange);
                    return _floorToInt ? number.ToInt(targetType) : number;

                case double number:
                    number = number.Map(InputRange, OutputRange);
                    return _floorToInt ? number.ToInt(targetType) : number;

                case null:
                    return null;

                default:
                    Debug.LogWarning(
                        $"The property {value} is of an unsupported types. " +
                        $"Please use the float double or their nullable types");
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }

    public static class RemapExt
    {
        public static float Map(this float value, Vector2 input, Vector2 output)
        {
            return (value - input.x) / (input.y - input.x) * (output.y - output.x) + output.x;
        }

        public static double Map(this double value, Vector2 input, Vector2 output)
        {
            return (value - input.x) / (input.y - input.x) * (output.y - output.x) + output.x;
        }

        public static object ToInt(this float value, Type target)
        {
            return Convert.ChangeType(Mathf.FloorToInt(value), target);
        }

        public static object ToInt(this double value, Type target)
        {
            return Convert.ChangeType(Mathf.FloorToInt((float)value), target);
        }
    }
}