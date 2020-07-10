# Unity-MVVM
Simple, Lightweight MVVM Framework for Unity3D

For a background on MVVM checkout the [Wikipedia page](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel)

The goal of this project is to implement some of the concepts I knew and loved from developing WPF applications, mainly that they be very quick to build, and easy to maintain. It also attempts to solve some of the Unity-specific pain points I've uncovered over the last few years working with Unity.

## Add to a project
This project now supports Unity's package manager. The easiest way to get Unity-MVVM + Samples and keep it up to date is to add it in the package manager window. Click the + icon at the top left and select "Add from git url." Then copy+paste the https url from this repo and make sure to add the #upm to get the right branch: https://github.com/push-pop/Unity-MVVM.git#upm

The library will be automatically added to your project, and you can download samples from the package manager to see demonstrated usage.

## MVVM Architechture

MVVM stands for Model-View-ViewModel. It is a software application architechture designed to decouple view logic from business logic when building software. This is good practice for a number of reasons including reusability, maintainability, and speed of development.

### Model
A model is simply a structure of data. Sometimes a model will reflect real world types (i.e. car, desk) and sometimes it will reflect types from web server, etc...

### View
A view is the presentation layer of an application. In Unity-MVVM, this can be a Unity UI element like button or text field. In addition, you can use the same logic for 3D gameobjects. Views should handle their own transitions (animations etc...) based on state changes in the ViewModel.

### ViewModel
A ViewModel (or VM) is what holds the data that will be presented on a view. It contains all the properties that can be bound to view elements. All ViewModels inherit from INotifyPropertyChanged which alerts the system when data changes and a UI element needs to be updated.

### Information Flow
Generally speaking, with MVVM, Models don't know anything, Views don't care about anything, and ViewModels hold everything together.

What this means is that ViewModels subscribe to data providers (HTTP Requests, Services, etc...) and notify Views when data has been changed. This is called a DataBinding. By default, this is a One-Way relationshop (i.e. ViewModel can update data on a View, but not the other way around.

Sometimes, Views will need to change data (i.e. an input field.) This can be done with a Two-Way Databinding relationship. When you update a field in a UI, the ViewModel will handle that change and forward it to the relevant service or provider.

# Building an Application with Unity-MVVM

## Writing your own ViewModel
You can use as many or as fiew ViewModels as you want in your application. Typically you would create one for each screen on an app: `Screen1ViewModel` or `SettingsPageViewModel` for example. A ViewModel should inherit from `ViewModelBase` which implements `INotifyPropertyChanged` and provides a method alerting subscribers of changes.

When writing a ViewModel, you want to think about what sort of data is needed to be provided to your view and create bindable properties for each element or collection of elements. 

##### Anatomy of a Bindable Property

```
public string Text
{
  get { return _text; } // Getter returns backing field
  set
  {
    if (value != _text) // Only update field if value is changed
    {
      _text = value;    // Set backing field
      NotifyPropertyChanged(nameof(Text)); // Alert subscribers to change in Property with name of property
    }
  }
}

[SerializeField]
string _text;
```
##### Visual Studio Code Snippet
Included in Unity-MVVM is a Visual Studio [code snippet](./Assets/Unity-MVVM/VS/bindprop.snippet) which is helpful for writing bindable properties in your ViewModel. To install on Windows, open the .snippet file with Visual Studio.

Once this is installed type bindprop + (Tabx2) to create a bindable property snippet

## Adding Views

### CollectionView
Use CollectionView to create, edit, and delete Prefabs based on the contents of an `ObservableCollection` or `ObservableRangeCollection`. Inherit from CollectionViewBase to set custom data as needed.

### Creating Custom Views
Custom Views can be created by inheriting from `ViewBase`. ViewBase types all have a Visibility Enum property to hold ViewStates (Visible, Hidden, Collapsed.) ViewBase implements a simple fade animation by default. Create a custom view to define behaviours for Visibility state transitions.

## Binding To Views
To bind a ViewModel to a View, there are a series of MonoBehaviours you can add directly on the GameObject. They have custom editor scripts which allow you to bind to ViewModel Properties without writing any code

### OneWayDataBinding
This is the most basic binding between a ViewModel and a View. Dropdowns allow you to select the VM and the property which you want to subscribe to changes. Component is the destination component which you will be updating. DstProp is a dropdown which allows you to pick the Property which will be updated on the View. 

### TwoWayDataBinding
This is the same as a OneWayDataBinding except that it also allows you to select a UnityEvent to subscribe to which will update the Data from the View to the VM

**Important: Defualt DataBindings disconnect when GameObjects are Disabled. Do not use OneWayDataBinding Or TwoWayDataBinding to turn GameObjects on and off, use the specialized types below**

### DataBoundActivator
This is a OneWayDataBinding which automatically targets the IsActive Property on the GameObject. It also does not disable when the GameObject is disabled. Use this to turn GameObjects on and off.

### VisibilityBinding
This is a OneWayDataBinding which automatically targets the Visibility Property on a Componenty inheriting from `ViewBase`. It also does not disable when the GameObject is disabled. Use this to trigger In/Out transitions in your custom Views

### CollectionViewSource
This is a OneWayDataBinding which binds to an `ObservableCollection` or `ObservableRangeCollection`. Use this with a `CollectionView` to create arrays of elements on the scene.

## Converters
Sometimes you want to bind to a property in the ViewModel, but you want that to be altered before it is set on the View Property. You can use a Converter to make a string all lowecase/uppercase with `TextTransformationConverter` or check if an enum equals a certain value with `EnumToBoolConverter`. Some Converters have parameters so you can tailor the output to a specific Property.

### Writing your own Converter
You can write your own converters by inheriting from `ValueConverterBase`. You then need to implement abstract methods `Convert` and `ConvertBack` (ConvertBack is only used by TwoWayDataBindings)

## Event Binding
Sometimes you want to bind an event on the View to a method or property on your ViewModel. Unity-MVVM provides two ways to do this

### EventBinding
This allows you to bind a UnityEvent from a view to a method on your ViewModel. The binding expects a method with the same signature as the UnityEvent. i.e. if the event is a `UnityEvent<string>` you need to bind to a `void MethodName(sting argName)` in your VM.

### EventPropertyBinding
This allows you to bind a UnityEvent from a view to a property on your ViewModel. In the Value field, you can specify a value to set the property to **OR** the name of a property on your view to set from. In the latter case, check the box marked IsProperty.

### Limitations // Upcoming features
- No way to filter with CollectionViewSource
- No Code Snippet template for OSX
- No way to access indexed properties on CollectionViewSource
- Need cleaner way to bind property with `EventPropertyBinding`





Open to feedback and PRs


MIT License - do with it what you will, just don't blame me if it doesn't work :)

Copyright (c) 2018 Nate Turley
turley.nate@gmail.com
