using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVVM.Util
{
    public class AnimationDispatcher : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance == null)
            {
                _instance = new GameObject("AnimationDispatcher").AddComponent<AnimationDispatcher>();
                DontDestroyOnLoad(_instance.gameObject);
            }
        }

        public static void Stop(IEnumerator routine)
        {
            _instance.StopCoroutine(routine);
        }

        public static void Stop(Coroutine routine)
        {
            _instance.StopCoroutine(routine);
        }    

        public static void StopAll()
        {
            _instance.StopAllCoroutines();
        }

        public static Coroutine DispatchCoroutine(string methodName)
        {
            return _instance.StartCoroutine(methodName);
        }

        public static Coroutine DispatchCoroutine(string methodName, object value)
        {
            return _instance.StartCoroutine(methodName, value);
        }

        public static Coroutine DispatchCoroutine(IEnumerator routine)
        {
            return _instance.StartCoroutine(routine);
        }

        static AnimationDispatcher _instance;
    }
}