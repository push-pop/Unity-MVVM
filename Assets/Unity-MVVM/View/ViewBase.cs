using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Visibility
{
    Visible,
    Hidden,
    Collapsed
};

public class ViewBase : MonoBehaviour, IView
{
    public Visibility ElementVisibility
    {
        set
        {
            if (_visibility != value)
            {
                _visibility = value;
                SetVisibility(_visibility);
            }
        }
        get
        {
            return Visibility.Hidden;
        }
    }
    [SerializeField]
    Visibility _visibility;

    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }
    RectTransform _rectTransform;
    CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        SetInitialVisibility();
    }

    private void SetInitialVisibility()
    {
        switch (_visibility)
        {
            case Visibility.Visible:
                cg.alpha = 1;
                break;
            case Visibility.Hidden:
                cg.alpha = 0f;
                break;
            case Visibility.Collapsed:
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public virtual void Hide()
    {
    }

    public virtual void SetVisibility(Visibility visibility)
    {
        switch (visibility)
        {
            case Visibility.Visible:
                gameObject.SetActive(true);
                cg.CancelAnimation();
                cg.FadeIn();
                break;
            case Visibility.Hidden:
                gameObject.SetActive(true);
                cg.CancelAnimation();
                cg.FadeOut();
                break;
            case Visibility.Collapsed:
                cg.FadeOut(() =>
                {
                    gameObject.SetActive(false);
                });
                break;
            default:
                break;
        }
    }

    public virtual void Show()
    {

    }

    public virtual void UpdateView()
    {

    }
}
