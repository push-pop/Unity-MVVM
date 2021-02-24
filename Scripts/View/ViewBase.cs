using UnityEngine;

namespace UnityMVVM.View
{
    [AddComponentMenu("Unity-MVVM/Views/ViewBase")]
    public class ViewBase : MonoBehaviour, IView
    {
        public Visibility ElementVisibility
        {
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    SetVisibility(_visibility, _isInitialValue);
                }

                _isInitialValue = false;
            }
            get
            {
                return Visibility.Hidden;
            }
        }
        [SerializeField]
        Visibility _visibility;

        [SerializeField]
        bool _collapse = false;

        bool _isInitialValue = true;

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


        public virtual float Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        [SerializeField]
        float _alpha = 1f;





        public Coroutine animationRoutine;

        [SerializeField]
        protected float _fadeTime = AnimationDefaults.FadeTime;

        protected virtual void SetInitialVisibility()
        {
            switch (_visibility)
            {
                case Visibility.Visible:
                    Alpha = 1;
                    break;
                case Visibility.Hidden:
                    Alpha = 0f;
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

        public virtual void SetVisibility(Visibility visibility, bool isInitialValue)
        {
            if (isInitialValue)
                SetInitialVisibility();

            switch (visibility)
            {
                case Visibility.Visible:
                    gameObject.SetActive(true);
                    this.CancelAnimation();
                    this.FadeIn(fadeTime: _fadeTime);
                    break;
                case Visibility.Hidden:
                    gameObject.SetActive(true);
                    this.CancelAnimation();
                    this.FadeOut(fadeTime: _fadeTime);
                    break;
                case Visibility.Collapsed:
                    this.FadeOut(() =>
                    {
                        gameObject.SetActive(false);
                    }, fadeTime: _fadeTime);
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
}
