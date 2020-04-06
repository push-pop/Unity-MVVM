using System;
using UnityMVVM.Model;

namespace UnityMVVM.View
{
    public interface ICollectionViewItem
    {
        IModel Model { get; set; }
        int Index { get; set; }
        void Init(IModel model, int idx);
        void Update(IModel model, int newIdx);
        Type ModelType { get; set; }
    }

    public interface ICollectionViewItem<T> : ICollectionViewItem
        where T : IModel
    {
        void InitItem(T model, int idx);
        void UpdateItem(T model, int newIdx);
    }
}