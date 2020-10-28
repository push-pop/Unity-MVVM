using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Util;

public enum MessageType
{
    info,
    warning,
    error
}
public struct PopupMessage
{
    public string Message;
    public MessageType Type;
}

public class PopupProvider : Singleton<PopupProvider>
{
    public Action<PopupMessage> OnShowPopup;
    public Action OnHidePopup;

    public void ShowMessage(string message, MessageType type, float time = 3f)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(new PopupMessage()
        {
            Message = message,
            Type = type
        }, time));
    }

    IEnumerator ShowMessageRoutine(PopupMessage p, float duration)
    {
        OnShowPopup?.Invoke(p);
        yield return new WaitForSeconds(duration);
        OnHidePopup?.Invoke();
    }
}
