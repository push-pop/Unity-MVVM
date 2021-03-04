using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityMVVM.Binding;
using UnityMVVM.Model;
using UnityMVVM.View;

namespace UnityMVVM.Samples.SelectableCollectionView
{
    public class ButtonItemEvent : UnityEvent<ButtonModel> { }
    public class ButtonItem : CollectionViewItemBase<ButtonModel>,
        ISelectable
    {
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    SetSelected(value);
                }
            }
        }

        bool _isSelected = false;

        public List<GameObject> _selectedItems;
        public List<GameObject> _unselectedItems;

        public UnityEvent OnClick { get; set; } = new UnityEvent();
        public ButtonItemEvent OnDelete { get; set; } = new ButtonItemEvent();
        public Action<object, object> OnSelected { get; set; }
        public Action<object, object> OnDeselected { get; set; }

        [SerializeField]
        Button _deleteButton;

        private void Awake()
        {
            _deleteButton.onClick.AddListener(() =>
            {
                OnDelete?.Invoke(Model);
            });

            GetComponent<Button>().onClick.AddListener(new UnityAction(() =>
            {
                OnClick?.Invoke();
                if (!IsSelected)
                    OnSelected?.Invoke(this, Model);
                else
                    OnDeselected?.Invoke(this, Model);
            }));
        }


        public override void InitItem(ButtonModel model, int idx)
        {
            gameObject.name = $"Item {idx}";
        }

        public void SetSelected(bool v)
        {
            foreach (var item in _selectedItems)
            {
                item.SetActive(v);
            }
            foreach (var item in _unselectedItems)
            {
                item.SetActive(!v);
            }
        }


        public override void UpdateItem(ButtonModel model, int newIdx)
        {
            Model = model;
            var bindings = GetComponentsInChildren<CollectionItemBinding>(true);
            foreach (var item in bindings)
                item.UpdateFromSource();
            //GetComponent<Image>().color = model.color;
            //GetComponent<>
        }
    }
}