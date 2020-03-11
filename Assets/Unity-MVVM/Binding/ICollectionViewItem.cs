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
        bool IsSelected { get; set; }
        Action<IModel> OnSelected { get; set; }
        Action<IModel> OnDeselected { get; set; }

        //void OnSelectedChange(Action callback);
    }
}