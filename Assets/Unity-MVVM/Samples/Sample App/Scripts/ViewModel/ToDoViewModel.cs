using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Samples.SampleApp.ViewModel
{
    public class ToDoViewModel : ViewModelBase
    {
        public int ItemCount => ToDoItems.Count;

        public ObservableCollection<ToDoItem> ToDoItems { get; set; } = new ObservableCollection<ToDoItem>();

        public void AddToDoListItem(string text)
        {
            ToDoItems.Add(new ToDoItem()
            {
                Label = text,
                IsComplete = false,

            });
        }

        private void Awake()
        {
            ToDoItems.CollectionChanged += ToDoItems_CollectionChanged;
        }

        private void ToDoItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(ItemCount));
        }
    }
}