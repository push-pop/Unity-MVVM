using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityMVVM.View;

namespace UnityMVVM.Examples
{
        public class CustomTypeCollectionView : CollectionViewBase
        {
            protected override void UpdateElement(int index, IList newItems)
            {
                var item = newItems[0] as DataModel;

                var go = InstantiatedItems[index];

                go.GetComponentInChildren<Text>().text = item.message;
                go.GetComponent<Image>().color = item.color;
            }

            protected override GameObject CreateCollectionItem(object ListItem, Transform parent)
            {
                var customItem = ListItem as DataModel;

                var go = GameObject.Instantiate(_listItemPrefab, transform);

                go.transform.SetAsLastSibling();

                go.GetComponentInChildren<Text>().text = customItem.message;
                go.GetComponent<Image>().color = customItem.color;

                return go;
            }
    }
}

