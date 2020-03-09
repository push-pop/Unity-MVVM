using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Reflection;
using UnityMVVM.Util;
using UnityMVVM.Model;

namespace UnityMVVM.Binding
{
    public class CollectionViewSource : DataBindingBase
    {
        INotifyCollectionChanged srcCollection;

        public string SrcCollectionName;
        public string SelectedItemName;

        public Action<int, IList> OnElementsAdded;
        public Action<int, IList> OnElementsRemoved;
        public Action<int, IList> OnCollectionReset;
        public Action<int, IList> OnElementUpdated;

        public Action<IModel> OnSelectedItemUpdated;

        public IModel SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                if (value != _selectedItem)
                {
                    _selectedItem = value;
                    Debug.Log("Selected Item Changed" + value);

                    OnSelectedItemUpdated?.Invoke(value);
                }
            }
        }

        IModel _selectedItem;


        BindTarget src;

        DataBindingConnection _conn;

        bool isBound = false;

        public override bool KeepConnectionAliveOnDisable => true;

        public int Count
        {
            get
            {
                return (src.GetValue() as IList).Count;
            }
        }


        public object this[int key]
        {
            get
            {
                var list = (src.GetValue() as IList);
                return list[key];
            }
            set
            {
                (src.GetValue() as IList)[key] = value;
            }
        }

        public override void RegisterDataBinding()
        {
            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for collection {1}", ViewModelName, SrcCollectionName);

                return;
            }

            src = new BindTarget(_viewModel, SrcCollectionName);
            srcCollection = src.GetValue() as INotifyCollectionChanged;

            if (srcCollection != null && !isBound)
                srcCollection.CollectionChanged += CollectionChanged;

            if (!string.IsNullOrEmpty(SelectedItemName) && _conn == null)
            {
                _conn = new DataBindingConnection(gameObject, new BindTarget(_viewModel, SelectedItemName), new BindTarget(this, nameof(SelectedItem)));
                _conn.OnSrcUpdated();
                _conn.Bind();
            }

            isBound = true;
        }

        public override void UnregisterDataBinding()
        {
            if (srcCollection != null && isBound)
                srcCollection.CollectionChanged -= CollectionChanged;
        }

        protected virtual void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnElementsAdded?.Invoke(e.NewStartingIndex, e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnElementsRemoved?.Invoke(e.OldStartingIndex, e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    OnElementUpdated.Invoke(e.NewStartingIndex, e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    OnCollectionReset?.Invoke(e.NewStartingIndex, e.NewItems);
                    break;
                default:
                    break;
            }

            OnSelectedItemUpdated?.Invoke(SelectedItem);
        }

        protected override void OnDestroy()
        {
            if (_conn != null && !_conn.isDisposed)
                _conn.Dispose();

            base.OnDestroy();
        }
    }
}
