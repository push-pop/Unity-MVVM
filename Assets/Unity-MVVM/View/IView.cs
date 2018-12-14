using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView  {
    void Show();
    void Hide();
    void SetVisibility(Visibility visibility);
}
