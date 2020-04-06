using System;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Samples.PropertyPaths
{
    public class PropertyPathsViewModel : ViewModelBase
    {
        [System.Serializable]
        public class NestedPropSrc : IEquatable<NestedPropSrc>
        {
            public float floatField;
            public int intField;

            public bool Equals(NestedPropSrc other)
            {
                other = other as NestedPropSrc;

                return floatField == other.floatField && intField == other.intField;
            }
        }

        public NestedPropSrc NestedProp
        {
            get { return _nestedProp; }
            set
            {
                if (value != _nestedProp)
                {
                    _nestedProp = value;
                    NotifyPropertyChanged(nameof(NestedProp));
                }
            }
        }

        [SerializeField]
        private NestedPropSrc _nestedProp = new NestedPropSrc();

        private void Update()
        {
            NestedProp.floatField = Mathf.Sin(Time.time * 2);
            NotifyPropertyChanged(nameof(NestedProp));
        }

    }
}