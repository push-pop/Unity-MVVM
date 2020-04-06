using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityMVVM.Model;
using UnityMVVM.View;

namespace UnityMVVM.Samples.SelectableCollectionView
{
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
        public Action<object, object> OnSelected { get; set; }
        public Action<object, object> OnDeselected { get; set; }

        private void Awake()
        {
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
            GetComponent<Image>().color = model.color;
        }
    }
}