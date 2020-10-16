
# MVVM Architechture

MVVM stands for Model-View-ViewModel. It is a software application architechture designed to decouple view logic from business logic when building software. This is good practice for a number of reasons including reusability, maintainability, and speed of development.

## Model
A model is simply a structure of data. Sometimes a model will reflect real world types (i.e. car, desk) and sometimes it will reflect types from web server, etc...

## View
A view is the presentation layer of an application. In Unity-MVVM, this can be a Unity UI element like button or text field. In addition, you can use the same logic for 3D gameobjects. Views should handle their own transitions (animations etc...) based on state changes in the ViewModel.

## ViewModel
A ViewModel (or VM) is what holds the data that will be presented on a view. It contains all the properties that can be bound to view elements. All ViewModels inherit from INotifyPropertyChanged which alerts the system when data changes and a UI element needs to be updated.

## Information Flow
Generally speaking, with MVVM, Models don't know anything, Views don't care about anything, and ViewModels hold everything together.

What this means is that ViewModels subscribe to data providers (HTTP Requests, Services, etc...) and notify Views when data has been changed. This is called a DataBinding. By default, this is a One-Way relationshop (i.e. ViewModel can update data on a View, but not the other way around.

Sometimes, Views will need to change data (i.e. an input field.) This can be done with a Two-Way Databinding relationship. When you update a field in a UI, the ViewModel will handle that change and forward it to the relevant service or provider.