# Bindings

Bindings create the glue between ViewModels and Views. There are a set of MonoBehaviours to setup these bindings, as well as 3 types of Binding Modes.

## DataBinding
This is the most commonly used connection. By selecting your ViewModel, you will get a list of properties of which you can subscribe to changes for. If the property has nested fields, you will see those populate in another dropdown if you'd like to bind to one of them. Drag the destination component into the Dest View field, and select the destination for the property value.

There are 3 types of bindings that this components supports:
### `OneWay`
Value changes from ViewModel are set on the View

### `TwoWay`
Value changes from the ViewModel are set on the View, and changes on the View will get set on the ViewModel

### `OneWayToSource`
Value changes from the View will get set on the ViewModel

**Important: Defualt DataBindings disconnect when GameObjects are Disabled. Do not use OneWayDataBinding Or TwoWayDataBinding to turn GameObjects on and off, use the specialized types below**

### DataBoundActivator
This is a OneWayDataBinding which automatically targets the IsActive Property on the GameObject. It also does not disable when the GameObject is disabled. Use this to turn GameObjects on and off.

### VisibilityBinding
This is a OneWayDataBinding which automatically targets the Visibility Property on a Componenty inheriting from `ViewBase`. It also does not disable when the GameObject is disabled. Use this to trigger In/Out transitions in your custom Views

### CollectionViewSource
This is a OneWayDataBinding which binds to an `ObservableCollection` or `ObservableRangeCollection`. Use this with a `CollectionView` to create arrays of elements on the scene.

## Converters

Converters are powerful chunks of code that can convert from one type to another. Read more about how to use them [here](./Converters.md).