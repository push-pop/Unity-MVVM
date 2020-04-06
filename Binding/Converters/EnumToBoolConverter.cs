using System;
using System.Linq;
using UnityEngine;

namespace UnityMVVM.Binding.Converters
{
    public class EnumToBoolConverter : ValueConverterBase
    {
        [SerializeField]
        protected string _expectedValue;

        [SerializeField]
        protected bool _invert;

        public override object Convert(object value, Type targetType, object parameter)
        {
            var values = _expectedValue.Split('|').Select(p => p.Trim());

            var equals = false;

            foreach (var item in values)
            {
                equals |= value.ToString().Equals(item);
            }

            return _invert ? !equals : equals;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
