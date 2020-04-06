using UnityEditor;
using UnityMVVM.Binding;
using UnityMVVM.View;
using System.Linq;
using UnityEngine;
using UnityMVVM.Extensions;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CollectionItemBinding), true)]
    public class CollectionItemBindingEditor : MVVMBaseEditor
    {
        SerializedProperty _srcViewProp;
        SerializedProperty _converterProp;
        SerializedProperty _dstViewProp;
        SerializedProperty _srcProp;

        SerializedList _srcNames = new SerializedList("_srcProp");
        SerializedList _srcPaths = new SerializedList("_srcPath");

        SerializedList _dstNames = new SerializedList("_dstProp");
        SerializedList _dstPaths = new SerializedList("_dstPath");


        ICollectionViewItem collectionView;

        protected override void CollectSerializedProperties()
        {
            _srcViewProp = serializedObject.FindProperty("_srcView");

            _converterProp = serializedObject.FindProperty("_converter");
            _dstViewProp = serializedObject.FindProperty("_dstView");

            _srcProp = serializedObject.FindProperty("_srcProp");

            _srcNames.Init(serializedObject);
            _srcPaths.Init(serializedObject);

            _dstNames.Init(serializedObject);
            _dstPaths.Init(serializedObject);
        }

        protected override void DrawChangeableElements()
        {
            if (collectionView == null)
                collectionView = (target as Component).gameObject.GetComponentInParent<ICollectionViewItem>();

            if (collectionView == null)
                GUIUtils.Message("No CollectionView in Parent", MessageType.Error);
            else
            {
                var modelType = collectionView.ModelType;

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Src Model", GUIUtils.labelOptions);

                EditorGUILayout.LabelField(modelType.Name);

                if (UnityEngine.GUILayout.Button("Open"))
                {
                    var str = AssetDatabase.FindAssets(modelType.Name).FirstOrDefault();
                    var path = AssetDatabase.GUIDToAssetPath(str);
                    var asset = EditorGUIUtility.Load(path);
                    AssetDatabase.OpenAsset(asset);
                }

                EditorGUILayout.EndHorizontal();

                GUIUtils.BindingField("Source Property", _srcNames, _srcPaths);
            }

            GUIUtils.ObjectField("Dst View", _dstViewProp);

            GUIUtils.BindingField("Dest Property", _dstNames, _dstPaths);

            GUIUtils.ObjectField("Converter", _converterProp);
        }

        protected override void SetupDropdownIndices()
        {
            _srcNames.SetupIndex();
            _srcPaths.SetupIndex();

            _dstNames.SetupIndex();
            _dstPaths.SetupIndex();
        }

        protected override void UpdateSerializedProperties()
        {
            _srcNames.UpdateProperty();
            _srcPaths.UpdateProperty();

            _dstNames.UpdateProperty();
            _dstPaths.UpdateProperty();
        }

        protected override void CollectPropertyLists()
        {
            if (collectionView == null)
                collectionView = (target as Component).gameObject.GetComponentInParent<ICollectionViewItem>();

            _srcNames.Clear();
            _srcPaths.Clear();
            _dstNames.Clear();
            _dstPaths.Clear();

            var dstView = _dstViewProp.objectReferenceValue as Component;
            if (dstView)
            {
                _dstNames.Values = dstView.GetBindablePropertyList(needsGetter: false);
                _dstPaths.Values = dstView.GetPropertiesAndFieldsList(_dstNames.Value);
            }
            else
            {
                _dstNames.Value = null;
                _dstPaths.Value = null;
            }

            if (collectionView != null)
            {
                var modelType = collectionView.ModelType;
                _srcNames.Values = modelType.GetBindablePropertyNames(needsSetter: false);

                var propType = modelType.GetProperty(_srcNames.Value).PropertyType;
                if (propType != null)
                    _srcPaths.Values = propType.GetNestedFields();

            }

        }
    }
}