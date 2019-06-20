using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityMVVM.ViewModel;
using UnityMVVM.Examples;

namespace UnityMVVM
{
    namespace Examples
    {
        [System.Serializable]
        public class ObservableListFloat : ObservableCollection<float> { }

        public class OtherViewModel : ViewModelBase
        {
            public int IntProp
            {
                get
                {
                    return _intField;
                }
                set
                {
                    if (value != _intField)
                    {
                        _intField = value;
                        NotifyPropertyChanged(nameof(IntProp));
                    }
                }
            }

            [SerializeField]
            int _intField;

            public bool BoolProp
            {
                get
                {
                    return _boolField;
                }
                set
                {
                    if (value != _boolField)
                    {
                        _boolField = value;
                        NotifyPropertyChanged(nameof(BoolProp));
                    }
                }
            }

            [SerializeField]
            bool _boolField;


            public ObservableCollection<DataModel> TestCollection
            {
                get
                {
                    return _testCollection;
                }
            }
            public ObservableCollection<DataModel> _testCollection = new ObservableCollection<DataModel>();


            public DataModel SelectedModel
            {
                get { return _selectedModel; }
                set
                {
                    if (value != _selectedModel)
                    {
                        _selectedModel = value;
                        NotifyPropertyChanged(nameof(SelectedModel));
                    }
                }
            }

            [SerializeField]
            private DataModel _selectedModel;


            public ObservableRangeCollection<DataModel> Collection2 { get; set; } = new ObservableRangeCollection<DataModel>();

            // Use this for initialization
            void Start()
            {
                StartCoroutine(AddToCollection());

                for (int i = 0; i < 10; i++)
                {
                    Collection2.Add(new DataModel()
                    {
                        color = Color.gray,
                        message = string.Format("SelectableItem {0}", i)
                    });
                }
            }

            // Update is called once per frame
            void Update()
            {
                IntProp = Mathf.FloorToInt(Time.timeSinceLevelLoad);
                BoolProp = (System.DateTime.Now.Second % 2 == 0);

            }

            IEnumerator AddToCollection()
            {
                while (true)
                {
                    var toRemove = new DataModel()
                    {
                        message = "I'm going to get removed",
                        color = Random.ColorHSV()
                    };

                    var toStay = new DataModel()
                    {
                        message = "I'm going to stay",
                        color = Random.ColorHSV()
                    };

                    var toChange = new DataModel()
                    {
                        message = "I'm Going to Change",
                        color = Random.ColorHSV()
                    };


                    TestCollection.Add(toRemove);
                    TestCollection.Add(toStay);
                    TestCollection.Add(toChange);

                    yield return new WaitForSeconds(2f);

                    TestCollection.Remove(toRemove);
                    TestCollection[TestCollection.IndexOf(toChange)] = new DataModel()
                    {
                        message = "Told ya ;)",
                        color = Random.ColorHSV()
                    };

                    yield return new WaitForSeconds(2f);

                    if (TestCollection.Count > 5)
                    {
                        TestCollection.Clear();
                    }

                }
            }
        }
    }
}

