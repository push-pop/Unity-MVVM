using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Samples.MVVMTest
{
    public enum ApplicationState
    {
        State1,
        State2,
        State3
    }

    public class TestViewModel : ViewModelBase
    {
        #region Binding Properties
        public ApplicationState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    NotifyPropertyChanged(nameof(State));
                }
            }
        }

        [SerializeField]
        ApplicationState _state;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    NotifyPropertyChanged(nameof(Text));
                }
            }


        }

        [SerializeField]
        string _text;

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                if (value != _color)
                {
                    _color = value;
                    NotifyPropertyChanged(nameof(Color));
                }
            }
        }


        [SerializeField]
        Color _color;

        public bool TwoWayBool
        {
            get { return _twoWayBool; }
            set
            {
                if (value != _twoWayBool)
                {
                    _twoWayBool = value;
                    NotifyPropertyChanged(nameof(TwoWayBool));
                }
            }
        }

        [SerializeField]
        private bool _twoWayBool;

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

        public ObservableCollection<TextItemModel> TestCollection { get; set; } = new ObservableCollection<TextItemModel>();

        #endregion

        #region public Methods
        public void ChangeColor()
        {
            Debug.Log("Change Color Button Handler");
            Color = Random.ColorHSV();
        }

        public void SetText(string text)
        {
            Text = text;
        }
        #endregion

        #region Unity Lifecycle
        void Start()
        {
            Text = System.DateTime.Now.ToShortTimeString();

            StartCoroutine(AddToCollection());

            StartCoroutine(StateChangeRoutine());
            StartCoroutine(BoolChangeRoutine());
        }

        void Update()
        {
            BoolProp = (System.DateTime.Now.Second % 2 == 0);

            if (System.DateTime.Now.Second % 5 == 0)
                Text = System.DateTime.Now.ToShortTimeString();
        }
        #endregion

        #region CoRoutines
        IEnumerator StateChangeRoutine()
        {
            while (true)
            {
                State = (ApplicationState)((int)(_state + 1) % 3);
                yield return new WaitForSeconds(3f);
            }
        }

        IEnumerator BoolChangeRoutine()
        {
            while (true)
            {
                TwoWayBool = !TwoWayBool;
                yield return new WaitForSeconds(5f);
            }

        }

        IEnumerator AddToCollection()
        {
            while (true)
            {
                var toRemove = new TextItemModel()
                {
                    message = "I'm going to get removed",
                    color = Random.ColorHSV()
                };

                var toStay = new TextItemModel()
                {
                    message = "I'm going to stay",
                    color = Random.ColorHSV()
                };

                var toChange = new TextItemModel()
                {
                    message = "I'm Going to Change",
                    color = Random.ColorHSV()
                };


                TestCollection.Add(toRemove);
                TestCollection.Add(toStay);
                TestCollection.Add(toChange);

                yield return new WaitForSeconds(2f);

                TestCollection.Remove(toRemove);
                TestCollection[TestCollection.IndexOf(toChange)] = new TextItemModel()
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
        #endregion
    }
}
