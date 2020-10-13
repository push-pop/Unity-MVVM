using System;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Model;
using System.Linq;

namespace UnityMVVM.View
{
    public abstract class CollectionViewItemBase<T> : MonoBehaviour, ICollectionViewItem<T>
    where T : class, IModel
    {
        public virtual T Model
        {
            get
            {
                return _model;
            }
            private set
            {
                _model = value;
            }
        }

        IModel ICollectionViewItem.Model { get => Model; set => Model = (T)value; }
        public Type ModelType { get => typeof(T); set { } }


        public virtual int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }


        int _index = -1;

        T _model;

        public abstract void InitItem(T model, int idx);
        public abstract void UpdateItem(T model, int newIdx);

        void ICollectionViewItem.Init(IModel model, int idx)
        {
            Model = model as T;
            Index = idx;

            InitItem(model as T, idx);
            var bindings = GetComponentsInChildren<CollectionItemBinding>(true);
            foreach (var item in bindings)
            {
                item.RegisterDataBinding(model);
            }
        }

        void ICollectionViewItem.Update(IModel model, int newIdx)
        {
            UpdateItem(model as T, newIdx);
        }

        void ICollectionViewItem<T>.InitItem(T model, int idx)
        {
            throw new NotImplementedException();
        }

        void ICollectionViewItem<T>.UpdateItem(T model, int newIdx)
        {
            throw new NotImplementedException();
        }
    }
}