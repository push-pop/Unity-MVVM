using System;
using System.Reflection;
using UnityEngine.Events;

namespace UnityMVVM.Binding
{
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

        internal static MethodInfo GetHandler(Type[] args)
        {
            var argCount = args.Length;
            string m = "";
            for (int i = 0; i < args.Length; i++)
            {
                m += args[i].Name;
            }
            m += "Handler";

            return typeof(UnityEventBinder).GetMethod(m);
        }

        public Action OnChange = null;

        public static Type GetDelegateType(Type[] argTypes)
        {
            var argCount = argTypes.Length;
            Type constructed;

            if (argTypes.Length > 0)
            {

                Type generic = typeof(UnityAction<>);
                constructed = generic.MakeGenericType(argTypes);
            }
            else
                constructed = typeof(UnityAction);

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

        #region Handlers

        public void Handler()
        {
            OnChange?.Invoke();
        }

        public void StringHandler(string val)
        {
            OnChange?.Invoke();
        }

        public void SingleHandler(float val)
        {
            OnChange?.Invoke();
        }

        public void IntHandler(int val)
        {
            OnChange?.Invoke();
        }

        public void BooleanHandler(bool val)
        {
            OnChange?.Invoke();
        }

        public void StringStringHandler(string val1, string val2)
        {
            OnChange?.Invoke();
        }
        #endregion
    }
}
