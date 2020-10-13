using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Binding.Converters;

public class ComparisonToBoolConverter : ValueConverterBase
{
    enum ComparisonType
    {
        GreaterThan,
        LessThan,
        EqualTo,
        NotEqualTo,
        InRange
    }

    [SerializeField]
    ComparisonType _comparisonType;

    [SerializeField]
    string _compareTo;

    [SerializeField]
    bool _invert;

    public override object Convert(object value, Type targetType, object parameter)
    {
        var val = System.Convert.ToDouble(value);
        var result = false;

        switch (_comparisonType)
        {
            case ComparisonType.LessThan:
                result = val < double.Parse(_compareTo);
                break;
            case ComparisonType.GreaterThan:
                result = val > double.Parse(_compareTo);
                break;
            case ComparisonType.EqualTo:
                result = val.Equals(double.Parse(_compareTo));
                break;
            case ComparisonType.NotEqualTo:
                result = !val.Equals(double.Parse(_compareTo));
                break;
            case ComparisonType.InRange:
                var minMax = _compareTo.ToString().Split(',');
                result = val < double.Parse(minMax[1]) && val > double.Parse(minMax[0]);
                break;
            default:
                break;
        }

        return _invert ? !result : result;
    }

    public override object ConvertBack(object value, Type targetType, object parameter)
    {
        throw new NotImplementedException();
    }


}
