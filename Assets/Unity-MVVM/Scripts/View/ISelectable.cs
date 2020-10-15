using System;

namespace UnityMVVM.Model
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
        void SetSelected(bool selected);
        Action<object, object> OnSelected { get; set; }
        Action<object, object> OnDeselected { get; set; }
    }
}