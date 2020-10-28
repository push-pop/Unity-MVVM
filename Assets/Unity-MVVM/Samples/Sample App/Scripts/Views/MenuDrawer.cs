using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.View;

namespace UnityMVVM.Samples.SampleApp.View
{
    public class MenuDrawer : ViewBase
    {
        [SerializeField]
        RectTransform _button;

        [SerializeField]
        RectTransform _self;

        public bool IsMenuOpen
        {
            set
            {
                StopAllCoroutines();

                StartCoroutine(AnimateMenuRoutine(value));
            }
        }
        IEnumerator AnimateMenuRoutine(bool isMenuOpen)
        {
            float xTarget = isMenuOpen ? 0 : -_self.sizeDelta.x;
            float rotZTarget = isMenuOpen ? 90 : 30;

            var posTarget = new Vector3(xTarget, _self.anchoredPosition.y);
            var rotTarget = Quaternion.Euler(0f, 0f, rotZTarget);

            float t = 0.0f;

            while (t <= 1.0f)

            //while (!Mathf.Approximately(xTarget, _self.localPosition.x) ||
            //  !Mathf.Approximately(rotZTarget, _button.localRotation.eulerAngles.z))
            {
                _self.anchoredPosition = Vector2.Lerp(_self.anchoredPosition, posTarget, Mathf.Clamp01(t));
                _button.localRotation = Quaternion.Slerp(_button.localRotation, rotTarget, Mathf.Clamp01(t));

                t += Time.deltaTime*.2f;
                
                yield return null;
            }

        }

        private void OnValidate()
        {
            _self = GetComponent<RectTransform>();
        }
    }


}