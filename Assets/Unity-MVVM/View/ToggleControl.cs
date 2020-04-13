using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityMVVM.View
{
    public class UnityBoolEvent : UnityEvent<bool> { }
    [RequireComponent(typeof(Toggle))]
    public class ToggleControl : MonoBehaviour
    {
        Toggle _toggle;

        public bool IsOn
        {
            get => _toggle.isOn;
            set => _toggle.isOn = value;
        }

        public UnityBoolEvent OnValueChanged { get; set; } = new UnityBoolEvent();

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(new UnityAction<bool>((newVal) => { OnValueChanged.Invoke(newVal); }));
        }
    }
}