using System;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    public class FlagToBoolConverter : ValueConverterBase
    {
        public enum Operation
        {
            AND,
            OR,
            EQUALS,
            NOR,
            XOR
        }

        public string ExpectedValue
        {
            set
            {
                _expectedValue = value;
            }
        }

        [SerializeField]
        protected string _expectedValue;

        [SerializeField]
        protected Operation _operation;

        [SerializeField]
        protected bool _invert;

        public override object Convert(object value, Type targetType, object parameter)
        {
            if (string.IsNullOrEmpty(value.ToString())) return false;

            var flagVal = (Enum)Enum.Parse(value.GetType(), _expectedValue.ToString());
            var result = false;
            switch (_operation)
            {
                case Operation.AND:
                    result = And(flagVal, (Enum)value);
                    break;
                case Operation.OR:
                    result = Or(flagVal, (Enum)value);
                    break;
                case Operation.EQUALS:
                    result = Equals(flagVal, (Enum)value);
                    break;
                case Operation.NOR:
                    throw new NotImplementedException();
                case Operation.XOR:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }

            return _invert ? !result : result;
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
