using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityMVVM.Model
{
    public class CollectionViewItemSelectedEvent : UnityEvent<IModel> { }
    public abstract class CollectionViewItemBase : MonoBehaviour, ICollectionViewItem
    {
        public abstract IModel Model { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                SetSelected(value);
            }

        }
        bool _isSelected;
        public abstract void Cleanup();
        public abstract void Init(IModel model);
        public abstract void UpdateItem(IModel model);
        public abstract void SetSelected(bool v);

        public abstract void ToggleSelected();
        public bool Equals(ICollectionViewItem other)
        {
            return other.Model.Equals(Model) && other.Equals(this);
        }

        public Action<IModel> OnSelected { get; set; }
        public Action<IModel> OnDeselected { get; set; }

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

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                SetSelected(value);
            }
        }
        bool _isSelected;

        public abstract void Cleanup();
        public virtual void Init(IModel model)
        {
            Model = model;
        }
        public abstract void SetSelected(bool v);
        public abstract void UpdateItem(IModel model);

        public Action<IModel> OnSelected { get; set; }
        public Action<IModel> OnDeselected { get; set; }
    }
}