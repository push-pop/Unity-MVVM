using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEditor;
using UnityMVVM.Binding;
using UnityMVVM.Model;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(CollectionViewSource), true)]
    public class CollectionViewSourceEditor : DataBindingBaseEditor
    {
        public int _srcIndex = -1;
        public int _selectedItemIdx = -1;

        SerializedList _srcCollectionNames = new SerializedList("SrcCollectionName");
        SerializedList _selectedItemNames = new SerializedList("SelectedItemName");

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _srcCollectionNames.Init(serializedObject);
            _selectedItemNames.Init(serializedObject);
        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();
            GUIUtils.BindingField("Source Collection", _srcCollectionNames);
            GUIUtils.BindingField("Selected Item", _selectedItemNames);
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();

            _srcCollectionNames.UpdateProperty();
            _selectedItemNames.UpdateProperty();
        }

        protected override void SetupDropdownIndices()
        {
            base.SetupDropdownIndices();

            _srcCollectionNames.SetupIndex();
            _selectedItemNames.SetupIndex();
        }

        protected override void CollectPropertyLists()
        {
            base.CollectPropertyLists();

            if (_viewModelChanged)
            {
                _srcCollectionNames.Value = null;
                _selectedItemNames.Value = null;
            }

            _srcCollectionNames.Clear();
            _selectedItemNames.Clear();



            _srcCollectionNames.Values
                = ViewModelProvider.GetViewModelPropertyList<INotifyCollectionChanged>(_viewModelProp.Value);

            var collectionName = _srcCollectionNames.Value;
            if (!string.IsNullOrEmpty(collectionName))
            {
                var list = new List<string>();


                var listType = ViewModelProvider
              .GetViewModelType(_viewModelProp.Value)
              .GetProperty(collectionName)
              .PropertyType.GenericTypeArguments.FirstOrDefault();

                list.Add("--");
                list.AddRange(ViewModelProvider.GetViewModelPropertyList(_viewModelProp.Value, listType));

                _selectedItemNames.Values = list;
            }

        }

    }
}