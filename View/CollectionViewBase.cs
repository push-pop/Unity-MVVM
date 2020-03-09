using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityMVVM.Binding;
using UnityMVVM.Model;

namespace UnityMVVM
{
    namespace View
    {
        [RequireComponent(typeof(CollectionViewSource))]
        public class CollectionViewBase : ViewBase
        {
            [SerializeField]
            protected GameObject _listItemPrefab;



            protected List<GameObject> InstantiatedItems = new List<GameObject>();

            [SerializeField]
            bool _canSelectMultiple = false;

            [SerializeField]
            protected CollectionViewSource _src;

            public UnityEvent<IModel> OnSelectionChanged { get; set; }

            public List<ICollectionViewItem> SelectedItems
            {
                get =>
                    InstantiatedItems
                        .Select(e => e.GetComponent<ICollectionViewItem>())
                        .Where(e => e.IsSelected).ToList();
            }
            // Use this for initialization
            protected void Awake()
            {
                if (_src != null)
                {
                    _src.OnElementsAdded += AddElements;
                    _src.OnElementsRemoved += RemoveElements;
                    _src.OnCollectionReset += ResetView;
                    _src.OnElementUpdated += UpdateElement;
                }

                _src.OnSelectedItemUpdated += UpdateSelectedItem;
            }

            private void UpdateSelectedItem(IModel obj)
            {
                var items = InstantiatedItems.Select(e => e.GetComponent<ICollectionViewItem>()).ToList();

                if (_canSelectMultiple)
                    items.Where(e => e.Model == obj).FirstOrDefault()?.ToggleSelected();
                else
                {
                    foreach (var item in items)
                        if (item != null)
                            item.IsSelected = (item.Model == obj);
                }
            }

            protected virtual void UpdateElement(int index, IList newItems)
            {

            }

            public override void SetVisibility(Visibility visibility)
            {
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
                    var it = go.GetComponent<ICollectionViewItem>() as ICollectionViewItem;
                    if (it != null)
                    {
                        it.Model = model;
                        it.Init(model);
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

            protected virtual void AddElement(int index, object newItem)
            {
                var go = CreateCollectionItem(newItem, transform);
                go.transform.SetSiblingIndex(index);

                InstantiatedItems.Insert(index, go);
            }

            protected virtual void AddElements(int newStartingIndex, IList newItems)
            {
                int idx = 0;
                var gameObjects = new List<GameObject>(newItems.Count);
                foreach (var item in newItems)
                {
                    var go = CreateCollectionItem(item, transform);
                    go.transform.SetSiblingIndex(newStartingIndex);

                    gameObjects.Add(go);

                    InitItem(go, item, idx);
                    idx++;
                }

                InstantiatedItems.InsertRange(newStartingIndex, gameObjects);
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
        }
    }
}


