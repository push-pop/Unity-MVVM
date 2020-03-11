using System;

namespace UnityMVVM.Binding.Converters
{
    public class BoolToNotBoolConverter : ValueConverterBase
    {

        public override object Convert(object value, Type targetType, object parameter)
        {
            var b = (bool)value;

            return !b;

        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
