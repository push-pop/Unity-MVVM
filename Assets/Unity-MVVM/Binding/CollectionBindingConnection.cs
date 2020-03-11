using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVVM.Binding
{
    public class CollectionBindingConnection : MonoBehaviour
    {
        [SerializeField]
        CollectionViewSource src;

        [SerializeField]
        int index;

        [SerializeField]
        string PropertyName;

        private void Update()
        {
            if (src.Count > 0)
            {
                foreach (var item in src[index].GetType().GetProperties())
                {
                    Debug.Log(item.PropertyType);
                }
            }
        }
    }
}