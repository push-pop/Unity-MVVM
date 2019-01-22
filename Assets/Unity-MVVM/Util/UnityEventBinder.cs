using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventBinder
{
    public delegate void ParamsAction(params object[] arguments);

    public static MethodInfo GetAddListener(object e)
    {
        return e.GetType().GetMethod("AddListener");
    }

    public static MethodInfo GetRemoveListener(object e)
    {
        return e.GetType().GetMethod("RemoveListener");
    }



    public static void BindEventWithArgs(object e, ParamsAction callback)
    {
        var args = e.GetType().GetGenericArguments();
        foreach (var arg in args)
        {
            Debug.Log(arg);
        }

        if (e as UnityEvent<string> != null)
            (e as UnityEvent<string>).AddListener(new UnityAction<string>((newval) => { callback?.Invoke(newval); }));
        else if (e as UnityEvent<int> != null)
            (e as UnityEvent<int>).AddListener(new UnityAction<int>((newVal) => { callback?.Invoke(newVal); }));
        else if (e as UnityEvent<bool> != null)
            (e as UnityEvent<bool>).AddListener(new UnityAction<bool>((newVal) => { callback?.Invoke(newVal); }));
        else if (e as UnityEvent != null)
            (e as UnityEvent).AddListener(new UnityAction(() => { callback?.Invoke(); }));
        else
        {
            Debug.LogError("Couldn't bind UnityEvent");
            return;
        }

        Debug.Log("Bound event");
    }


    internal static MethodInfo GetHandler(Type[] args)
    {
        var argCount = args.Length;
        string m = "";
        if (argCount == 0)
            m = "NoArgHandler";
        else if (argCount == 1)
        {
            var t = args[0];
            if (t.Equals(typeof(string)))
                m = "StringHandler";
            else if (t.Equals(typeof(int)))
                m = "IntHandler";
            else if (t.Equals(typeof(float)))
                m = "FloatHandler";
        }

        return typeof(UnityEventBinder).GetMethod(m);
    }

    public Action OnChange = null;

    public static Type GetDelegateType(Type[] argTypes)
    {
        var argCount = argTypes.Length;

        Type generic = typeof(UnityAction<>);
        Type constructed = generic.MakeGenericType(argTypes);

        return constructed;
    }

    public static Delegate GetDelegate(object owner, Type[] args)
    {
        var mInfo = UnityEventBinder.GetHandler(args);
        var delegateType = UnityEventBinder.GetDelegateType(args);

        return Delegate.CreateDelegate(delegateType, owner, mInfo);
    }

    public Delegate GetDelegate<T>(int argCount)
    {
        Delegate d = null;

        var mInfo = typeof(UnityEventBinder).GetMethod("OneArgHandler");

        if (argCount == 1)
            d = Delegate.CreateDelegate(typeof(Func<object>), this, "OneArgHandler");

        return d;
    }

    public void BindDelegate(int argCount)
    {
        OnChange?.Invoke();
    }

    public void NoArgHandler()
    {
        OnChange?.Invoke();
    }

    public void StringHandler(string val)
    {
        OnChange?.Invoke();
    }

    public void FloatHandler(float val)
    {
        OnChange?.Invoke();
    }

    public void IntHandler(int val)
    {
        OnChange?.Invoke();
    }

    public void TwoArgHandler(object val1, object val2)
    {
        OnChange?.Invoke();
    }



    public static void BindEvent(object e, Action callback)
    {
        if (e as UnityEvent<string> != null)
            (e as UnityEvent<string>).AddListener(new UnityAction<string>((newval) => { callback?.Invoke(); }));
        else if (e as UnityEvent<int> != null)
            (e as UnityEvent<int>).AddListener(new UnityAction<int>((newVal) => { callback?.Invoke(); }));
        else if (e as UnityEvent<bool> != null)
            (e as UnityEvent<bool>).AddListener(new UnityAction<bool>((newVal) => { callback?.Invoke(); }));
        else if (e as UnityEvent<float> != null)
            (e as UnityEvent<float>).AddListener(new UnityAction<float>((newVal) => { callback?.Invoke(); }));
        else if (e as UnityEvent != null)
            (e as UnityEvent).AddListener(new UnityAction(() => { callback?.Invoke(); }));
        else
        {
            Debug.LogError("Couldn't bind UnityEvent");
            return;
        }

    }
}
