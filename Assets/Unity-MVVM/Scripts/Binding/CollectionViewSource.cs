﻿using System;
using System.Collections;
using System.Collections.Specialized;
using UnityEngine;

namespace UnityMVVM.Binding
{
    [AddComponentMenu("Unity MVVM/Bindings/Colection View Source")]
    public class CollectionViewSource : DataBindingBase
    {
        INotifyCollectionChanged srcCollection;

        public string SrcCollectionName;

        public Action<int, IList> OnElementsAdded;
        public Action<int, IList> OnElementsRemoved;
        public Action<int, IList> OnCollectionReset;
        public Action<int, IList> OnElementUpdated;

        BindTarget src;

        DataBindingConnection _conn;

        public override bool KeepConnectionAliveOnDisable => true;

        public int Count
        {
            get
            {
                return (src.GetValue() as IList).Count;
            }
        }

        public override bool IsBound { get => _isBound; }
        bool _isBound = false;

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
            if (IsBound)
                return;

            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for collection {1}", ViewModelName, SrcCollectionName);

                return;
            }

            src = new BindTarget(_viewModel, SrcCollectionName);
            srcCollection = src.GetValue() as INotifyCollectionChanged;

            if (srcCollection != null && !IsBound)
                srcCollection.CollectionChanged += CollectionChanged;

            _isBound = true;

        }

        public override void UnregisterDataBinding()
        {
            if (srcCollection != null && IsBound)
                srcCollection.CollectionChanged -= CollectionChanged;

            _isBound = false;
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
        }

        protected override void OnDestroy()
        {
            if (_conn != null && !_conn.isDisposed)
                _conn.Dispose();

            base.OnDestroy();
        }
    }
}
