using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnityStringEvent : UnityEvent<string> { }

public class ToDoEntryInput : MonoBehaviour
{
    [SerializeField]
    Button _submitButton;

    [SerializeField]
    InputField _inputField;

    public UnityStringEvent OnEntrySumbitted { get; set; } = new UnityStringEvent();

    private void Awake()
    {
        _submitButton.onClick.AddListener(() =>
        {
            OnEntrySumbitted?.Invoke(_inputField.text);
            _inputField.text = "";
        });
    }

    private void OnValidate()
    {
        if (_submitButton == null)
            _submitButton = GetComponentInChildren<Button>();

        if (_inputField == null)
            _inputField = GetComponentInChildren<InputField>();
    }
}
