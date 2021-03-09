using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityMVVM.Binding;
using UnityMVVM.Model;

namespace UnityMVVM.View
{
    [RequireComponent(typeof(CollectionViewSource)), AddComponentMenu("Unity-MVVM/Views/CollectionViewBase")]
    public class CollectionViewBase : ViewBase, IDataBinding
    {
        public string SrcCollectionName
        {
            get => _src?.SrcCollectionName;
        }

        public string ViewModelName
        {
            get => _src?.ViewModelName;
        }

        public string SelectedItemName;
        public string SelectedItemsName;

        [SerializeField]
        protected GameObject _listItemPrefab;

        protected List<GameObject> InstantiatedItems = new List<GameObject>();

        [SerializeField]
        protected CollectionViewSource _src;

        BindTarget _selectedItemsSrc;
        System.Collections.ObjectModel.ObservableCollection<IModel> _dstCollection;

        [SerializeField]
        bool CanSelectMultiple = false;

        private DataBindingConnection _conn;
        public bool IsBound
        {
            get => _isBound;
            protected set => _isBound = true;
        }
        public UnityEvent OnSelectionChanged { get; set; } = new UnityEvent();

        public List<IModel> SelectedItems
        {
            get => _selectedItems;
            set => _selectedItems = value;
        }

        public IModel SelectedItem
        {
            get => _selectedItems.FirstOrDefault();
            set
            {
                if (value != null)
                    _selectedItems = new List<IModel>() { value };
                else
                    _selectedItems.Clear();
            }
        }

        List<IModel> _selectedItems = new List<IModel>();
        private Delegate _changeDelegate;
        UnityEventBinder _binder = new UnityEventBinder();
        private bool _isBound;

        protected void Awake()
        {
            //Turn off prefabs that might be left
            foreach (Transform item in transform)
            {
                item.gameObject.SetActive(false);
            }

            if (_src != null)
            {
                _src.OnElementsAdded += AddElements;
                _src.OnElementsRemoved += RemoveElements;
                _src.OnCollectionReset += ResetView;
                _src.OnElementUpdated += UpdateElement;
            }

            RegisterDataBinding();
        }

        void UpdateSelectedItem(IModel selected, bool isSelected)
        {
            var items = InstantiatedItems.Select(e => e.GetComponent<ICollectionViewItem>()).ToList();

            SelectedItem = isSelected ? selected : null;

            items.ForEach(e =>
            {
                var model = e.Model;
                if (model == null)
                    Debug.LogError("Model is null");
                if (e is ISelectable && model != null)
                    (e as ISelectable).IsSelected = _selectedItems.Contains(model);
            });

            if (isSelected && _dstCollection != null && !_dstCollection.Contains(selected))
                _dstCollection.Add(selected);

            else if (!isSelected && _dstCollection != null && _dstCollection.Contains(selected))
                _dstCollection.Remove(selected);



            OnSelectionChanged?.Invoke();
        }

        private void UpdateSelectedItems(List<IModel> selected, bool isSelected)
        {
            var items = InstantiatedItems.Select(e => e.GetComponent<ICollectionViewItem>()).ToList();

            if (isSelected)
                SelectedItems = SelectedItems.Union(selected).ToList();
            else
                SelectedItems = SelectedItems.Intersect(selected).ToList();

            items.ForEach(e =>
            {
                var model = e.Model;
                if (e is ISelectable && model != null)
                    (e as ISelectable).IsSelected = _selectedItems.Contains(model);

            });

            OnSelectionChanged?.Invoke();
        }

        public override void SetVisibility(Visibility visibility, bool isInitialVisibility)
        {
            if (isInitialVisibility)
                SetInitialVisibility();

            switch (visibility)
            {
                case Visibility.Visible:
                    UpdateChildVisibilities(visibility);
                    gameObject.SetActive(true);
                    this.CancelAnimation();
                    this.FadeIn(fadeTime: _fadeTime);
                    break;
                case Visibility.Hidden:
                    UpdateChildVisibilities(visibility);
                    gameObject.SetActive(true);
                    this.CancelAnimation();
                    this.FadeOut(fadeTime: _fadeTime);
                    break;
                case Visibility.Collapsed:
                    this.FadeOut(() =>
                    {
                        UpdateChildVisibilities(visibility);
                        gameObject.SetActive(false);
                    }, fadeTime: _fadeTime);
                    break;
                default:
                    break;
            }

        }

        void UpdateChildVisibilities(Visibility v)
        {
            foreach (var item in InstantiatedItems)
            {
                item.SetActive(v.Equals(Visibility.Collapsed) ? false : true);
            }
        }


        protected void OnDestroy()
        {
            if (_src != null)
            {
                _src.OnElementsAdded -= AddElements;
                _src.OnElementsRemoved -= RemoveElements;
                _src.OnCollectionReset -= ResetView;
                _src.OnElementUpdated -= UpdateElement;
            }
        }

        protected virtual void InitItem(GameObject go, object item, int index)
        {
            var model = (item as IModel);
            if (model != null)
            {
                var it = go.GetComponent<ICollectionViewItem>();
                if (it != null)
                {
                    it.Init(model, index);
                }

                if (it is ISelectable)
                {
                    var selectableItem = it as ISelectable;
                    selectableItem.OnSelected += (caller, m) =>
                    {
                        UpdateSelectedItem((m as IModel), true);
                    };

                    selectableItem.OnDeselected += (caller, m) =>
                    {
                        UpdateSelectedItem((m as IModel), false);
                    };
                }

            }
        }


        protected virtual void ResetView(int newStartingIndex, IList newItems)
        {
            foreach (Transform t in transform)
                GameObject.Destroy(t.gameObject);

            InstantiatedItems.Clear();
        }

        //
        // Override this method to create the gameobject that will spawn in your CollectionView
        //
        protected virtual GameObject CreateCollectionItem(object ListItem, Transform parent)
        {
            var go = GameObject.Instantiate(_listItemPrefab, transform);

            return go;
        }

        protected virtual void AddElements(int newStartingIndex, IList newItems)
        {
            int idx = 0;
            var gameObjects = new List<GameObject>(newItems.Count);
            foreach (var item in newItems)
            {
                var go = CreateCollectionItem(item, transform);
                go.transform.SetSiblingIndex(newStartingIndex + idx);

                gameObjects.Add(go);

                InitItem(go, item, newStartingIndex + idx);
                idx++;
            }

            InstantiatedItems.InsertRange(newStartingIndex, gameObjects);
        }

        protected virtual void UpdateElement(int index, IList newItems)
        {
            int idx = index;

            if (idx < 0)
                idx = 0;

            foreach (var item in newItems)
            {
                //var go = InstantiatedItems[idx];
                //var view = go.GetComponent<ICollectionViewItem>();
                //var newModel = item as IModel;
                //if (view != null && newModel != null)
                //    view.Update(newModel);

                //idx++;

                //    var model = view.Model;
                //    var newModel = newItems;

                //    //view.Model = newItems[]



                //    //go.transform.SetSiblingIndex(newStartingIndex);

                //    gameObjects.Add(go);

                //    InitItem(go, item, idx);
                //    idx++;
                //}

                //InstantiatedItems.InsertRange(newStartingIndex, gameObjects);
            }
        }

        protected virtual void RemoveElements(int oldStartingIndex, IList oldItems)
        {
            for (int i = oldStartingIndex; i < oldStartingIndex + oldItems.Count; i++)
            {
                if (i < InstantiatedItems.Count)
                {
                    GameObject.Destroy(InstantiatedItems[i]);
                    InstantiatedItems[i] = null;
                }
            }

            InstantiatedItems.RemoveRange(oldStartingIndex, oldItems.Count);
        }
        private void OnValidate()
        {
            _src = GetComponent<CollectionViewSource>();
        }

        public void RegisterDataBinding()
        {

            //if (!CanSelectMultiple)
            //{
            SelectedItemName = SelectedItemName.Replace("--", "");
            if (!string.IsNullOrEmpty(SelectedItemName))
            {

                _conn = new DataBindingConnection(gameObject,
                      new BindTarget(_src.ViewModelSrc, SelectedItemName),
                      new BindTarget(this, nameof(SelectedItem)));

                _conn.OnSrcUpdated();
                _conn.Bind();

                OnSelectionChanged.AddListener(_conn.DstUpdated);

            }

            //}

            //else
            //{
            //    _conn = new DataBindingConnection(gameObject,
            //         new BindTarget(_src.ViewModelSrc, SelectedItemsName),
            //         new BindTarget(this, nameof(SelectedItems)));

            //    BindChangeEvent();
            //}
            //if (!(string.IsNullOrEmpty(SelectedItemName) && string.IsNullOrEmpty(SelectedItemsName)) && _conn == null)
            //{
            //    if (CanSelectMultiple)
            //        _conn = new DataBindingConnection(gameObject,
            //            new BindTarget(_src.ViewModelSrc, SelectedItemsName),
            //            new BindTarget(this, nameof(SelectedItems)));
            //    else
            //        _conn = new DataBindingConnection(gameObject,
            //            new BindTarget(_src.ViewModelSrc, SelectedItemName),
            //            new BindTarget(this, nameof(SelectedItem)));

            //    BindChangeEvent();

            //    _conn.OnSrcUpdated();
            //    _conn.Bind();
            //}
        }

        //private void HandleSelectedItemsSrcChange(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    var newItems = (List<IModel>)e.NewItems;
        //    var oldItems = (List<IModel>)e.OldItems;
        //    switch (e.Action)
        //    {
        //        case NotifyCollectionChangedAction.Add:
        //            UpdateSelectedItems(newItems, true);
        //            break;
        //        case NotifyCollectionChangedAction.Move:
        //            break;
        //        case NotifyCollectionChangedAction.Remove:
        //            UpdateSelectedItems(oldItems, false);
        //            break;
        //        case NotifyCollectionChangedAction.Replace:
        //            break;
        //        case NotifyCollectionChangedAction.Reset:
        //            break;
        //        default:
        //            break;
        //    }
        //}


        //private void BindChangeEvent()
        //{
        //    if (CanSelectMultiple)
        //    {
        //        //_selectedItemsSrc = new BindTarget(_src.ViewModelSrc, SelectedItemsName);
        //        var collChange = (_conn.SrcTarget.GetValue() as INotifyCollectionChanged);
        //        _dstCollection = _conn.SrcTarget.GetValue() as ObservableCollection<IModel>;

        //        collChange.CollectionChanged += HandleSelectedItemsSrcChange;
        //    }

        //}

        public void UnregisterDataBinding()
        {
            //if (!CanSelectMultiple)
            OnSelectionChanged.RemoveListener(_conn.DstUpdated);
            _conn.Unbind();

        }
    }
}


