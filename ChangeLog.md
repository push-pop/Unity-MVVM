# Changelog


## 1.1.3
- Fixes indexing with CollectionViewBase and AddRange
- Adds component menus
- Fixes RemapValueConverter bug and cleans up code

## 1.1.2
- Fixes OnewWayToSource bug that fires OnSrcUpdated on start
- Fixes not checking if type is IConvertable in DstUpdated
- Fixes change check on ViewModel resetting src property in editor
- Fixes issue with nested views not able to start coroutines

## 1.1.1
- Fix filtering lowercase invariant in BindingMonitorEditorWindow

## 1.1.0
- Refactor OneWayDataBinding and TwoWayDataBinding into single class called DataBinding.
- Deprecates OneWayDataBinding and TwoWayDataBinding. Please use DataBinding class from now on
- Adds support for "OneWayToSource" bindings
- rethrows exceptions in DataBindingConnection when setter fails

## 1.0.1
- Fixes issues with making builds due to asmdef


