using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Visibility
{
    Visible,
    Hidden,
    Invisible
};

public class ViewBase : MonoBehaviour, IView
{
    [SerializeField]
    Image img;

    public Visibility ElementVisibility
    {
        set
        {
            _visibility = value;
            SetVisibility(value);
        }
        get
        {
            return Visibility.Hidden;
        }
    }
    [SerializeField]
    Visibility _visibility;


    CanvasGroup cg;
    private float targetAlpha = 1f;

    public virtual void Hide()
    {
    }

    public virtual void SetVisibility(Visibility visibility)
    {
        switch (visibility)
        {
            case Visibility.Visible:
                gameObject.SetActive(true);
                targetAlpha = 1f;
                break;
            case Visibility.Hidden:
                gameObject.SetActive(false);
                targetAlpha = 0f;
                break;
            case Visibility.Invisible:
                gameObject.SetActive(true);
                targetAlpha = 0f;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (cg == null)
            cg = GetComponent<CanvasGroup>();

        cg.alpha = Mathf.Lerp(cg.alpha, targetAlpha, .2f);
    }


    public virtual void Show()
    {
    }
}
