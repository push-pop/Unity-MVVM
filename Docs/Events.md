# Events

Sometimes you want to bind an event on the View to a method or property on your ViewModel. An example would be when you click a button it sets a Property value on your VM. Unity-MVVM provides two ways to do this

### EventBinding
This allows you to bind a UnityEvent from a view to a method on your ViewModel. The binding expects a method with the same signature as the UnityEvent. i.e. if the event is a `UnityEvent<string>` you need to bind to a `void MethodName(sting argName)` in your VM.

### EventPropertyBinding
This allows you to bind a UnityEvent from a view to a property on your ViewModel. In the Value field, you can specify a value to set the property to **OR** the name of a property on your view to set from. In the latter case, check the box marked IsProperty.