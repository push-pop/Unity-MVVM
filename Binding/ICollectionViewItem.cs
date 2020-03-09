using System;

namespace UnityMVVM.Model
{
    public interface ICollectionViewItem
    {
        void Init(IModel model);
        void UpdateItem(IModel model);
        void Cleanup();
        void SetSelected(bool v);
        void ToggleSelected();
        IModel Model { get; set; }
        bool IsSelected { get; set; }
    }
}