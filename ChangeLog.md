# 1.1.0
- Refactor OneWayDataBinding and TwoWayDataBinding into single class called DataBinding.
- Deprecates OneWayDataBinding and TwoWayDataBinding. Please use DataBinding class from now on
- Adds support for "OneWayToSource" bindings
- rethrows exceptions in DataBindingConnection when setter fails

# 1.0.1
- Fixes issues with making builds due to asmdef
