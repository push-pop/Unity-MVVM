using System;
using UnityEngine;
using UnityMVVM.View;

namespace UnityMVVM.Binding.Converters
{
    public class FlagToVisibilityConverter : FlagToBoolConverter
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

        [SerializeField]
        bool _invert;

        [SerializeField]
        bool _collapse;

        public override object Convert(object value, Type targetType, object parameter)
        {
            bool isTrue = (bool) base.Convert(value, typeof(bool), parameter);
           
            isTrue = _invert ? !isTrue : isTrue;
            return isTrue ? Visibility.Visible : _collapse ? Visibility.Collapsed : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}