using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityMVVM.Model;

namespace UnityMVVM.Examples
{
    public class ButtonItem : CollectionViewItemBase<DataModel>
    {
        public bool IsSelected
        {
            set
            {
                Debug.Log(value);
            }
        }

        public override IModel Model
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

        public List<GameObject> _selectedItems;

        public UnityEvent OnClick { get; set; } = new UnityEvent();

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(new UnityAction(() =>
            {
                OnClick?.Invoke();
            }));
        }

        public override void Cleanup()
        {

        }

        public override void Init(IModel model)
        {
            var data = model as DataModel;
            Model = model;

            GetComponent<Image>().color = data.color;
            GetComponentInChildren<Text>().text = data.message;
        }

        public override void SetSelected(bool v)
        {
            foreach (var item in _selectedItems)
            {
                item.SetActive(v);
            }
        }

        public override void UpdateItem(IModel model)
        {
        }

    }
}