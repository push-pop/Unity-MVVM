using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;
using UnityMVVM.Util;
using UnityMVVM.View;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(CollectionViewBase), true)]
    public class CollectionViewEditor : MVVMBaseEditor
    {
        SerializedProperty _srcCollectionProp;
        SerializedList _selectedItemNames = new SerializedList("SelectedItemName");
        SerializedList _selectedItemCollectionNames = new SerializedList("SelectedItemsName");
        SerializedProperty _canSelectMultipleProp;
        SerializedProperty _listItemPrefabProp;

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _selectedItemNames.Init(serializedObject);
            _selectedItemCollectionNames.Init(serializedObject);

            _srcCollectionProp = serializedObject.FindProperty("_src");
            _canSelectMultipleProp = serializedObject.FindProperty("CanSelectMultiple");
            _listItemPrefabProp = serializedObject.FindProperty("_listItemPrefab");
        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();
            GUIUtils.ObjectField("Source Collection", _srcCollectionProp, typeof(CollectionViewSource));
            GUIUtils.ObjectField("List Item Prefab", _listItemPrefabProp, typeof(GameObject));
            //GUIUtils.ToggleField("Can Select Multiple", _canSelectMultipleProp);
            if (_canSelectMultipleProp.boolValue)
                GUIUtils.BindingField("Selected Item List", _selectedItemCollectionNames);
            else
                GUIUtils.BindingField("Selected Item", _selectedItemNames);
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();

            _selectedItemNames.UpdateProperty();
            _selectedItemCollectionNames.UpdateProperty();
        }
        protected override void SetupDropdownIndices()
        {
            base.SetupDropdownIndices();

            _selectedItemNames.SetupIndex();
            _selectedItemCollectionNames.SetupIndex();
        }

        protected override void CollectPropertyLists()
        {
            var myClass = target as CollectionViewBase;

            base.CollectPropertyLists();

            _selectedItemNames.Clear();
            _selectedItemCollectionNames.Clear();

            var collectionName = myClass.SrcCollectionName;
            if (!string.IsNullOrEmpty(collectionName))
            {
                var list = new List<string>();

                var listType = ViewModelProvider
              .GetViewModelType(myClass.ViewModelName)
              .GetProperty(collectionName)
              .PropertyType
              .GenericTypeArguments
              .FirstOrDefault();

                var listCollectionType = typeof(ObservableCollection<>)
                    .MakeGenericType(listType);


                list.Add("--");
                list.AddRange(ViewModelProvider.GetViewModelPropertyList(myClass.ViewModelName,
                    _canSelectMultipleProp.boolValue ? listCollectionType : listType));

                if (_canSelectMultipleProp.boolValue)
                    _selectedItemCollectionNames.Values = list;
                else
                    _selectedItemNames.Values = list;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}