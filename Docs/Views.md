# Views

Views are what we display to the user. They can be interactive or simply informational. It's also how we move through the User Experience of an app by cycling in and out with changing data.

## ViewBase
Custom Views can be created by inheriting from `ViewBase`. ViewBase types all have a Visibility Enum property to hold ViewStates (`Visible`, `Hidden`, `Collapsed`.) Collapsed in the context of Unity means that the GameObject gets deactivated after fading out. 

ViewBase implements a simple fade animation by default. Create a custom view to define behaviours for Visibility state transitions.

## CanvasView
CanvasView is a type of view that fades a Canvas element in and out. This is the easiest way to do basic transitions between screens of an App.

## CollectionView
Use CollectionView to create, edit, and delete Prefabs based on the contents of an `ObservableCollection` or `ObservableRangeCollection`. Inherit from CollectionViewBase to set custom data as needed.