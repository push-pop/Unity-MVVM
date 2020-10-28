using System;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Samples.SampleApp.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (value != _currentTime)
                {
                    _currentTime = value;
                    NotifyPropertyChanged(nameof(CurrentTime));
                }
            }
        }

        [SerializeField]
        private DateTime _currentTime = DateTime.Now;

        public bool IsMenuOpen
        {
            get { return _isMenuOpen; }
            set
            {
                if (value != _isMenuOpen)
                {
                    _isMenuOpen = value;
                    NotifyPropertyChanged(nameof(IsMenuOpen));
                }
            }
        }

        [SerializeField]
        private bool _isMenuOpen;

        public AppState AppState
        {
            get { return _appState; }
            set
            {
                if (value != _appState)
                {
                    _appState = value;
                    NotifyPropertyChanged(nameof(AppState));
                }
            }
        }

        [SerializeField]
        private AppState _appState;


        public PopupMessage Popup
        {
            get { return _popup; }
            set
            {
                _popup = value;
                NotifyPropertyChanged(nameof(Popup));
            }
        }

        [SerializeField]
        private PopupMessage _popup;

        public bool IsPopupVisible
        {
            get { return _isPopupVisible; }
            set
            {
                if (value != _isPopupVisible)
                {
                    _isPopupVisible = value;
                    NotifyPropertyChanged(nameof(IsPopupVisible));
                }
            }
        }

        [SerializeField]
        private bool _isPopupVisible;



        public void ToggleMenu()
        {
            IsMenuOpen = !IsMenuOpen;
        }

        private void Start()
        {
            TimeProvider.Instance.OnTimeUpdated += (time) =>
            {
                CurrentTime = time;
            };

            PopupProvider.Instance.OnShowPopup += (msg) =>
            {
                Popup = msg;
                IsPopupVisible = true;
            };

            PopupProvider.Instance.OnHidePopup += () => IsPopupVisible = false;
        }
    }
}
