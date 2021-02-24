using UnityEngine;

namespace UnityMVVM.View
{
    [RequireComponent(typeof(CanvasGroup)), AddComponentMenu("Unity-MVVM/Views/CanvasView")]
    public class CanvasView : ViewBase
    {
        public override float Alpha
        {
            get
            {
                return cg.alpha;
            }
            set
            {
                cg.alpha = value;
            }
        }

        public CanvasGroup cg
        {
            get
            {
                if (_cg == null)
                    _cg = GetComponent<CanvasGroup>();
                if (_cg == null)
                    _cg = gameObject.AddComponent<CanvasGroup>();
                return _cg;
            }
        }

        private CanvasGroup _cg;

        public override void SetVisibility(Visibility visibility, bool isInitialValue)
        {
            base.SetVisibility(visibility, isInitialValue);

            switch (visibility)
            {
                case Visibility.Visible:
                    cg.blocksRaycasts = true;
                    break;
                case Visibility.Hidden:
                case Visibility.Collapsed:
                    cg.blocksRaycasts = false;
                    break;
                default:
                    break;
            }
        }
    }
}
