using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.View;

namespace UnityMVVM.Examples
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
            throw new NotImplementedException();
        }
    }
}

