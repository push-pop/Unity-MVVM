using System;
using UnityEngine;

namespace UnityMVVM.Model
{
    public abstract class CollectionViewItemBase : MonoBehaviour, ICollectionViewItem
    {
        public abstract IModel Model { get; set; }

        public abstract void Cleanup();
        public abstract void Init(IModel model);
        public abstract void UpdateItem(IModel model);
        public abstract void SetSelected(bool v);
        public bool Equals(ICollectionViewItem other)
        {
            return other.Model.Equals(Model);
        }
    }

    public abstract class CollectionViewItemBase<T> : MonoBehaviour, ICollectionViewItem
        where T : class, IModel
    {
        public virtual IModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }
        IModel _model;

        public abstract void Cleanup();
        public virtual void Init(IModel model)
        {
            Model = model;
        }
        public abstract void SetSelected(bool v);
        public abstract void UpdateItem(IModel model);
    }
}