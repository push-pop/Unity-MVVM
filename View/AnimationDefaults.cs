//#define UNITY_MVVM_LEANTWEEN

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVVM.View
{
    public class AnimationDefaults
    {
        public const float FadeTime = .2f;
#if UNITY_MVVM_LEANTWEEN
    public const LeanTweenType Ease = LeanTweenType.easeInCubic;
#endif
    }

    public static class AnimationExt
    {
#if UNITY_MVVM_LEANTWEEN

    public static void FadeIn(this CanvasGroup cg, Action callback = null,float fadeTime = AnimationDefaults.FadeTime, LeanTweenType ease = AnimationDefaults.Ease)
    {
        FadeTo(cg, 1f, callback, fadeTime, ease);
    }

    public static void FadeOut(this CanvasGroup cg,Action callback = null, float fadeTime = AnimationDefaults.FadeTime, LeanTweenType ease = AnimationDefaults.Ease)
    {
        FadeTo(cg, 0f, callback, fadeTime, ease);
    }

    public static void FadeTo(this CanvasGroup cg, float target, Action callback = null, float fadeTime = AnimationDefaults.FadeTime, LeanTweenType ease = AnimationDefaults.Ease)
    {
        LeanTween.alphaCanvas(cg, target, fadeTime).setEase(ease).setOnComplete(callback);
    }

    public static void CancelAnimation(this CanvasGroup cg)
    {
        LeanTween.cancel(cg.gameObject);
    }

#else

        public static void FadeIn(this ViewBase vb, Action callback = null, float fadeTime = AnimationDefaults.FadeTime)
        {
            vb.FadeTo(1f, callback, fadeTime);
        }

        public static void FadeOut(this ViewBase vb, Action callback = null, float fadeTime = AnimationDefaults.FadeTime)
        {
            vb.FadeTo(0f, callback, fadeTime);
        }

        public static void FadeTo(this ViewBase vb, float target, Action callback = null, float fadeTime = AnimationDefaults.FadeTime)
        {
            vb.CancelAnimation();

            vb.animationRoutine = vb.StartCoroutine(vb.FadeRoutine(target, fadeTime, callback));
        }

        public static void CancelAnimation(this ViewBase vb)
        {
            if (vb.animationRoutine != null)
            {
                vb.StopCoroutine(vb.animationRoutine);
                vb.animationRoutine = null;
            }
        }

        static IEnumerator FadeRoutine(this ViewBase vb, float target, float time, Action callback)
        {
            float start = vb.Alpha;
            for (float t = 0f; t < time; t += Time.deltaTime)
            {
                float normalizedTime = t / time;
                //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
                vb.Alpha = Mathf.Lerp(start, target, normalizedTime);
                yield return null;
            }
            vb.Alpha = target;

            callback?.Invoke();
        }

#endif
    }
}