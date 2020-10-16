using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Samples.BasicUsage
{
    public class ViewModel : ViewModelBase
    {
        #region Binding Properties
        public int FrameCount
        {
            get { return _frameCount; }
            set
            {
                if (value != _frameCount)
                {
                    _frameCount = value;
                    NotifyPropertyChanged(nameof(FrameCount));
                }
            }
        }

        [SerializeField]
        private int _frameCount;

        public float CurrTime
        {
            get { return _currTime; }
            set
            {
                if (value != _currTime)
                {
                    _currTime = value;
                    NotifyPropertyChanged(nameof(CurrTime));
                }
            }
        }

        [SerializeField]
        private float _currTime;

        public bool IsMouseClicked
        {
            get { return _isMouseClicked; }
            set
            {
                if (value != _isMouseClicked)
                {
                    _isMouseClicked = value;
                    NotifyPropertyChanged(nameof(IsMouseClicked));
                }
            }
        }

        [SerializeField]
        private bool _isMouseClicked;

        public int CurrPage
        {
            get { return _currPage; }
            set
            {
                if (value != _currPage)
                {
                    _currPage = value;
                    NotifyPropertyChanged(nameof(CurrPage));
                }
            }
        }

        [SerializeField]
        private int _currPage;

        public int IntSliderVal
        {
            get { return _intSliderVal; }
            set
            {
                if (value != _intSliderVal)
                {
                    _intSliderVal = value;
                    NotifyPropertyChanged(nameof(IntSliderVal));
                }
            }
        }

        [SerializeField]
        private int _intSliderVal;


        public float FloatSliderVal
        {
            get { return _floatSliderVal; }
            set
            {
                if (value != _floatSliderVal)
                {
                    _floatSliderVal = value;
                    NotifyPropertyChanged(nameof(FloatSliderVal));
                }
            }
        }

        [SerializeField]
        private float _floatSliderVal;

        public string StringValue
        {
            get { return _stringValue; }
            set
            {
                if (value != _stringValue)
                {
                    _stringValue = value;
                    NotifyPropertyChanged(nameof(StringValue));
                }
            }
        }

        [SerializeField]
        private string _stringValue;


        public bool BoolVal
        {
            get { return _boolVal; }
            set
            {
                if (value != _boolVal)
                {
                    _boolVal = value;
                    NotifyPropertyChanged(nameof(BoolVal));
                }
            }
        }

        [SerializeField]
        private bool _boolVal;

        public DateTime Now
        {
            get { return _now; }
            set
            {
                if (value != _now)
                {
                    _now = value;
                    NotifyPropertyChanged(nameof(Now));
                }
            }
        }

        [SerializeField]
        private DateTime _now;

        public Color DemoBackgroundColor
        {
            get { return _demoBackgroundColor; }
            set
            {
                if (value != _demoBackgroundColor)
                {
                    _demoBackgroundColor = value;
                    NotifyPropertyChanged(nameof(DemoBackgroundColor));
                }
            }
        }

        [SerializeField]
        private Color _demoBackgroundColor = new Color(192f / 255f, 100f / 255f, 67f / 255f);


        public TimeSpan TimerVal
        {
            get { return _timerVal; }
            set
            {
                if (value != _timerVal)
                {
                    _timerVal = value;
                    NotifyPropertyChanged(nameof(TimerVal));
                }
            }
        }

        [SerializeField]
        private TimeSpan _timerVal;


        #endregion

        #region public Methods
        public void SetRandomBackgroundColor()
        {
            DemoBackgroundColor = UnityEngine.Random.ColorHSV();
        }

        public void ResetFrameCounter()
        {
            FrameCount = 0;
        }

        public void NextPage()
        {
            CurrPage++;
        }

        public void PreviousPage()
        {
            CurrPage--;
        }
        #endregion

        private void Update()
        {
            FrameCount++;// = Time.frameCount;
            CurrTime = Time.time;
            IsMouseClicked = Input.GetMouseButton(0);
            Now = DateTime.Now;
            TimerVal += TimeSpan.FromSeconds(Time.deltaTime);
        }
    }
}
