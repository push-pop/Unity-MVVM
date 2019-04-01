using System;

namespace UnityMVVM.Model
{
    public interface ICollectionViewItem
    {
        void Init(IModel model);
        void UpdateItem(IModel model);
        void Cleanup();
        void SetSelected(bool v);
        IModel Model { get; set; }
    }
}