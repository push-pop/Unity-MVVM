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

        public ButtonModel ToDelete
        {
            get => new ButtonModel();
            set
            {
                Debug.Log($"Delete {value.label}");

                Collection.Remove(value);
            }
        }

        public void AddButton()
        {
            Collection.Add(new ButtonModel()
            {
                color = Color.grey,
                label = $"Item {Collection.Count}"
            });
        }

        public void UpdateLabels()
        {
            var color = Color.black;
            var gradient = new Gradient()
            {
                colorKeys = new GradientColorKey[] { new GradientColorKey(Color.blue, 0f), new GradientColorKey(Color.red, 1f) }
                , mode = GradientMode.Blend
            };

            for (int i = 0; i < Collection.Count; i++)
            {
                Collection[i] = new ButtonModel()
                {
                    color = gradient.Evaluate((float)i/(float)Collection.Count),
                    label = $"Item {i}"
                };
            }
        }

        public void DeleteButton(ButtonModel model)
        {
            Collection.Remove(model);
        }

        public ObservableRangeCollection<ButtonModel> Collection { get; set; } = new ObservableRangeCollection<ButtonModel>();

        void Start()
        {
            var models = new List<ButtonModel>();
            for (int i = 0; i < 10; i++)
            {
                models.Add(new ButtonModel()
                {
                    color = Color.grey,
                    label = $"Item {i}"
                });
            }

            Collection.AddRange(models);
        }
    }
}
