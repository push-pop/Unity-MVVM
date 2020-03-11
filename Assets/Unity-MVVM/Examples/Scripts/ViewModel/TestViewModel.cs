using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Examples
{
        public enum ApplicationState
        {
            State1,
            State2,
            State3
        }

        public class TestViewModel : ViewModelBase
        {

            public float NewFloatProp
            {
                get { return _newFloatField; }
                set
                {
                    if (value != _newFloatField)
                    {
                        _newFloatField = value;
                        NotifyPropertyChanged(nameof(NewFloatProp));
                    }
                }
            }

            [SerializeField]
            private float _newFloatField;


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

            public Color Color2
            {
                get
                {
                    return _color2;
                }
                set
                {
                    if (value != _color2)
                    {
                        _color2 = value;
                        NotifyPropertyChanged(nameof(Color2));
                    }
                }
            }


            [SerializeField]
            Color _color2;



            public Color Color3
            {
                get
                {
                    return _color;
                }
                set
                {
                    if (value != _color3)
                    {
                        _color3 = value;
                        NotifyPropertyChanged(nameof(Color3));
                    }
                }
            }

            [SerializeField]
            Color _color3;

            [SerializeField]
            Color _color4;

            public Color Color4
            {
                get
                {
                    return _color4;
                }
                set
                {
                    if (value != _color4)
                    {
                        _color4 = value;
                        NotifyPropertyChanged(nameof(Color4));
                    }
                }
            }

            [SerializeField]
            Color _color5;

            public Color Color5
            {
                get
                {
                    return _color5;
                }
                set
                {
                    if (value != _color5)
                    {
                        _color5 = value;
                        NotifyPropertyChanged(nameof(Color5));
                    }
                }
            }

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



            // Use this for initialization
            void Start()
            {
                Text = System.DateTime.Now.ToShortTimeString();

                StartCoroutine(StateChangeRoutine());
                StartCoroutine(BoolChangeRoutine());
            }

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

            // Update is called once per frame
            void Update()
            {
                if (System.DateTime.Now.Second % 5 == 0)
                {
                    Text = System.DateTime.Now.ToShortTimeString();
                }
            }

            public void ChangeColor()
            {
                Debug.Log("Change Color Button Handler");
                Color = Random.ColorHSV();
            }

            public void SetText(string text)
            {
                Text = text;
            }
        }
    }
