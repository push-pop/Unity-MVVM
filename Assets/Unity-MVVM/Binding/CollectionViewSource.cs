using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Reflection;
using UnityMVVM.Util;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityMVVM
{
    namespace Binding
    {

        public class CollectionViewSource : DataBindingBase
        {

            INotifyCollectionChanged srcCollection;

            [HideInInspector]
            public List<string> SrcCollections = new List<string>();

            [HideInInspector]
            public string SrcCollectionName;

            public Action<int, IList> OnElementsAdded;
            public Action<int, IList> OnElementsRemoved;
            public Action<int, IList> OnCollectionReset;
            public Action<int, IList> OnElementUpdated;

            BindTarget src;

            public override void RegisterDataBinding()
            {
                if (_viewModel == null)
                {
                    Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for collection {1}", ViewModelName, SrcCollectionName);

                    return;
                }

                src = new BindTarget(_viewModel, SrcCollectionName);
                srcCollection = src.GetValue() as INotifyCollectionChanged;

                if (srcCollection != null)
                    srcCollection.CollectionChanged += CollectionChanged;
            }

            public override void UnregisterDataBinding()
            {
                if (srcCollection != null)
                    srcCollection.CollectionChanged -= CollectionChanged;
            }

            protected virtual void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        OnElementsAdded?.Invoke(e.NewStartingIndex, e.NewItems);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        OnElementsRemoved?.Invoke(e.OldStartingIndex, e.OldItems);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        OnElementUpdated.Invoke(e.NewStartingIndex, e.NewItems);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        OnCollectionReset?.Invoke(e.NewStartingIndex, e.NewItems);
                        break;
                    default:
                        break;
                }
            }

            protected override void UpdateBindings()
            {
                base.UpdateBindings();

                if (!string.IsNullOrEmpty(ViewModelName))
                {
                    var props = ViewModelProvider.GetViewModelProperties(ViewModelName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                    SrcCollections = props.Where(prop =>
                            typeof(INotifyCollectionChanged).IsAssignableFrom(prop.PropertyType)
                            && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                        ).Select(e => e.Name).ToList();


                }
            }

#if UNITY_EDITOR
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

                    //_srcIndex = myClass.SrcCollections.IndexOf(_srcNameProp.stringValue);
                }

            }
#endif
        }
    }
}