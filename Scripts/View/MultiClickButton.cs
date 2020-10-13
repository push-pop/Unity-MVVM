using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityMVVM.View
{
    [RequireComponent(typeof(Button))]
    public class MultiClickButton : MonoBehaviour
    {
        public UnityEvent OnMultiClick { get; set; } = new UnityEvent();
        public int numClicks = 3;
        public float clickTimeout = 1f;

        int _clicks;
        float timeout;
        Button b;

        // Use this for initialization
        void Awake()
        {
            b = GetComponent<Button>();
            if (!b) return;

            b.onClick.AddListener(() =>
            {
                timeout += clickTimeout;
                _clicks++;
            });
        }

        // Update is called once per frame
        void Update()
        {
            timeout = Mathf.Max(-.1f, timeout - Time.deltaTime);

            if (_clicks == numClicks)
            {
                OnMultiClick.Invoke();
                _clicks = 0;
                timeout = -.1f;
            }

            if (timeout < 0)
            {
                _clicks = 0;
            }
        }
    }
}