using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Samples.SelectableCollectionView
{
    [System.Serializable]
    public class ObservableListFloat : ObservableCollection<float> { }

    public class OtherViewModel : ViewModelBase
    {
        public ButtonModel SelectedModel
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
        private ButtonModel _selectedModel;

        public ObservableRangeCollection<ButtonModel> Collection { get; set; } = new ObservableRangeCollection<ButtonModel>();

        void Start()
        {

            for (int i = 0; i < 10; i++)
            {
                Collection.Add(new ButtonModel()
                {
                    color = Color.gray,
                    label = $"Item {i}"
                });
            }
        }
    }
}
