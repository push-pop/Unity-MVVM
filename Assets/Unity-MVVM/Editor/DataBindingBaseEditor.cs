using System.Collections.Generic;
using UnityEditor;
using UnityMVVM.Binding;
using UnityMVVM.Util;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(DataBindingBase), true)]
    public class DataBindingBaseEditor : MVVMBaseEditor
    {
        public string ViewModelName { get => _viewModelProp.Value; }

        public int _viewModelIdx = 0;
        protected List<string> _viewModels = new List<string>();

        protected SerializedList _viewModelProp = new SerializedList("ViewModelName");

        protected bool _viewModelChanged { get; set; }

        protected override void OnEnable()
        {
            _viewModels = ViewModelProvider.Viewmodels;

            CollectSerializedProperties();

            CollectPropertyLists();

            serializedObject.ApplyModifiedProperties();

            UpdateSerializedProperties();
        }

        protected override void CollectSerializedProperties()
        {
            _viewModelProp.Init(serializedObject);
        }

        protected void DrawViewModelDrawer()
        {
            if (string.IsNullOrEmpty(_viewModelProp.Value))
                GUIUtils.Message("No ViewModels. Maybe you should make one!", MessageType.Error);
            else
              _viewModelChanged =  GUIUtils.ViewModelField(_viewModelProp);
        }

        protected override void DrawChangeableElements()
        {
            DrawViewModelDrawer();
        }

        protected override void UpdateSerializedProperties()
        {
            _viewModelProp.UpdateProperty();
        }

        protected override void SetupDropdownIndices()
        {
            _viewModelProp.SetupIndex();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _viewModelChanged = false;

        }

        protected override void CollectPropertyLists()
        {
            _viewModelProp.Values = ViewModelProvider.Viewmodels;
        }
    }
}
