using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Samples.UsingViews
{
    public class ViewModel : ViewModelBase
    {
        public bool IsViewOpen
        {
            get { return _isViewOpen; }
            set
            {
                if (value != _isViewOpen)
                {
                    _isViewOpen = value;
                    NotifyPropertyChanged(nameof(IsViewOpen));
                }
            }
        }

        [SerializeField]
        private bool _isViewOpen;

        public void ToggleViewVisibility()
        {
            IsViewOpen = !IsViewOpen;
        }
    }
}
