using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVVM.View
{
    public interface IView
    {
        void Show();
        void Hide();
        void SetVisibility(Visibility visibility);
    }
}