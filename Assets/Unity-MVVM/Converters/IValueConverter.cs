using System;

namespace UnityMVVM
{
    namespace Binding
    {
        namespace Converters
        {
            public interface IValueConverter
            {

                object Convert(object value, Type targetType, object parameter);
                object ConvertBack(object value, Type targetType, object parameter);
            }
        }
    }
}