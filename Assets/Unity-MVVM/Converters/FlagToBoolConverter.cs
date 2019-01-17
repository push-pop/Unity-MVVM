using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    public class FlagToBoolConverter : ValueConverterBase
    {
        enum Operation
        {
            AND,
            OR,
            EQUALS,
            NOR,
            XOR
        }

        [SerializeField]
        string _expectedValue;
        [SerializeField]
        Operation _operation;


        public override object Convert(object value, Type targetType, object parameter)
        {
            var flagVal = (Enum)Enum.Parse(value.GetType(), _expectedValue.ToString());

            switch (_operation)
            {
                case Operation.AND:
                    return And(flagVal, (Enum)value);
                case Operation.OR:
                    return Or(flagVal, (Enum)value);
                case Operation.EQUALS:
                    return Equals(flagVal, (Enum)value);
                case Operation.NOR:
                    throw new NotImplementedException();
                case Operation.XOR:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }



        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }

        private bool Or(Enum a, Enum b)
        {
            if (Enum.GetUnderlyingType(a.GetType()) != typeof(ulong))
                return (System.Convert.ToInt64(a) | System.Convert.ToInt64(b)) != 0;
            else
                return (System.Convert.ToUInt64(a) | System.Convert.ToUInt64(b)) != 0;
        }

        private bool And(Enum a, Enum b)
        {
            if (Enum.GetUnderlyingType(a.GetType()) != typeof(ulong))
                return (System.Convert.ToInt64(a) & System.Convert.ToInt64(b)) != 0;
            else
                return (System.Convert.ToUInt64(a) & System.Convert.ToUInt64(b)) != 0;
        }

        static bool Equals(Enum a, Enum b)
        {
            // consider adding argument validation here

            if (Enum.GetUnderlyingType(a.GetType()) != typeof(ulong))
                return System.Convert.ToInt64(a) == System.Convert.ToInt64(b);
            else
                return System.Convert.ToUInt64(a) == System.Convert.ToUInt64(b);
        }
    }
}