using System.Linq;
using UnityEditor;
using UnityMVVM.Binding;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(CollectionViewSource), true)]
    public class CollectionViewSourceEditor : DataBindingBaseEditor
    {
        public int _srcIndex = 0;

        SerializedProperty _srcNameProp;

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();
            _srcNameProp = serializedObject.FindProperty("SrcCollectionName");
        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();

            var myClass = target as CollectionViewSource;
            EditorGUILayout.LabelField("Source Collection");

            _srcIndex = EditorGUILayout.Popup(_srcIndex, myClass.SrcCollections.ToArray());

        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();
            var myClass = target as CollectionViewSource;
            myClass.SrcCollectionName = _srcIndex > -1 ?
                myClass.SrcCollections[_srcIndex] : null;
        }

        public override void OnInspectorGUI()
        {

            var myClass = target as CollectionViewSource;

            _srcIndex = myClass.SrcCollections.IndexOf(_srcNameProp.stringValue);

            if (_srcIndex < 0 && myClass.SrcCollections.Count > 0)
            {
                _srcIndex = 0;
                myClass.SrcCollectionName = myClass.SrcCollections.FirstOrDefault();
            }
            base.OnInspectorGUI();

        }

    }
}