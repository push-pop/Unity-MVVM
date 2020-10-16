# ViewModels

### Intro
A ViewModel (or VM) is what holds the data that will be presented on a view. It contains all the properties that can be bound to view elements. All ViewModels inherit from INotifyPropertyChanged which alerts the system when data changes and a UI element needs to be updated.

## Writing your own ViewModel
You can use as many or as fiew ViewModels as you want in your application. Typically you would create one for each screen on an app: `Screen1ViewModel` or `SettingsPageViewModel` for example. A ViewModel should inherit from [`ViewModelBase`](../Assets/Unity-MVVM/Scripts/ViewModel/ViewModelBase.cs) which implements `INotifyPropertyChanged` and provides a method alerting subscribers of changes.

When writing a ViewModel, you want to think about what sort of data is needed to be provided to your view and create bindable properties for each element or collection of elements. 

## Anatomy of a Bindable Property

Any data that needs to be updated in the view will take a form similar to this:

```
public string Text
{
  get { return _text; } // Getter returns backing field
  set
  {
      // If value has changed
      if (value != _text) {

          // Set backing field
          _text = value;    

          // Alert subscribers to change in Property with name of property
          NotifyPropertyChanged(nameof(Text)); 
    }
  }
}

// Backing field
[SerializeField]
string _text;
```
## Visual Studio Code Snippet
Included in Unity-MVVM is a Visual Studio [code snippet](../Assets/Unity-MVVM/VS/bindprop.snippet) which is helpful for writing bindable properties in your ViewModel. To install on Windows, open the .snippet file with Visual Studio.

Once this is installed type bindprop + (Tabx2) to create a bindable property snippet