using System.Collections.Specialized;
using UnityEditor;
using UnityMVVM.Binding;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(CollectionViewSource), true)]
    public class CollectionViewSourceEditor : DataBindingBaseEditor
    {
        SerializedList _srcCollectionNames = new SerializedList("SrcCollectionName");
        //SerializedList _selectedItemNames = new SerializedList("SelectedItemName");
        //SerializedList _selectedItemCollectionNames = new SerializedList("SelectedItemsName");
        //SerializedProperty _canSelectMultipleProp;

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();

            _srcCollectionNames.Init(serializedObject);
        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();
            GUIUtils.BindingField("Source Collection", _srcCollectionNames);
            
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();

            _srcCollectionNames.UpdateProperty();
        }

        protected override void SetupDropdownIndices()
        {
            base.SetupDropdownIndices();

            _srcCollectionNames.SetupIndex();
        }

        protected override void CollectPropertyLists()
        {
            base.CollectPropertyLists();

            if (_viewModelChanged)
            {
                _srcCollectionNames.Value = null;
               
            }

            _srcCollectionNames.Clear();


            _srcCollectionNames.Values
                = ViewModelProvider.GetViewModelPropertyList<INotifyCollectionChanged>(_viewModelProp.Value);

          

        }

    }
}