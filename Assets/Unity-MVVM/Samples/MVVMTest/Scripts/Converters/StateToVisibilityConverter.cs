using System;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.View;

namespace UnityMVVM.Samples.MVVMTest
{
    public class StateToVisibilityConverter : ValueConverterBase
    {
        public ApplicationState VisibleState;
        public ApplicationState HiddenState;
        public ApplicationState CollapsedState;

        public override object Convert(object value, Type targetType, object parameter)
        {
            ApplicationState state = (ApplicationState)value;
            if (state == VisibleState)
                return Visibility.Visible;
            if (state == HiddenState)
                return Visibility.Hidden;
            if (state == CollapsedState)
                return Visibility.Collapsed;

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter)
        {
            Debug.LogError($"{GetType().Name} ConvertBack not implemented. Override this class if you wish to implement");
            return null;
        }
    }
}

