
# Converters
Sometimes you want to bind to a property in the ViewModel, but you want that to be altered before it is set on the View Property. You can use a Converter to make a string all lowecase/uppercase with [`TextTransformationConverter`](../Assets/Unity-MVVM/Scripts/Binding/Converters/TextTransformationConverter.cs) or check if an enum equals a certain value with [`EnumToBoolConverter`](../Assets/Unity-MVVM/Scripts/Binding/Converters/EnumToBoolConverter.cs). 

Some Converters have parameters so you can tailor the output to a specific Property.

### Writing your own Converter
You can write your own converters by inheriting from [`ValueConverterBase`](../Assets/Unity-MVVM/Scripts/Binding/Converters/ValueConverterBase.cs). You then need to implement abstract methods `Convert` and `ConvertBack` (ConvertBack is only used by TwoWayDataBindings)