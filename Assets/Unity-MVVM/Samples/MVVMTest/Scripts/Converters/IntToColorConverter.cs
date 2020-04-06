using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Binding.Converters;

namespace UnityMVVM.Samples.MVVMTest { 
        public class IntToColorConverter : ValueConverterBase
        {

            public override object Convert(object value, Type targetType, object parameter)
            {
                var num = (int)value;

                switch (num % 5)
                {
                    case 0:
                        return Color.black;
                    case 1:
                        return Color.blue;
                    case 3:
                        return Color.red;
                    case 4:
                        return Color.yellow;
                    default:
                        return Color.white;
                }


            }

            public override object ConvertBack(object value, Type targetType, object parameter)
            {
                throw new NotImplementedException();
            }
        }
    }