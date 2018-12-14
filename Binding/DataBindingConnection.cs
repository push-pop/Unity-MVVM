using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;

namespace UnityMVVM
{
    namespace Binding
    {

        [Serializable]
        public class DataBindingConnection : IDisposable
        {
            object PropertyOwner;

            public string PropertyName;

            public Action PropertyChangedAction;

            public bool isDisposed = false;

            public DataBindingConnection()
            { }

            public DataBindingConnection(BindTarget src, Action action)
            {
                PropertyName = src.propertyName;
                PropertyOwner = src.propertyOwner;

                var notifyChange = PropertyOwner as INotifyPropertyChanged;
                if (notifyChange != null)
                    notifyChange.PropertyChanged += NotifyChange_PropertyChanged;

                PropertyChangedAction = action;
            }

            public DataBindingConnection(object owner, string propertyName, Action action)
            {
                PropertyName = propertyName;
                PropertyOwner = owner;
                PropertyChangedAction = action;

                var notifyChange = PropertyOwner as INotifyPropertyChanged;

                if (notifyChange != null)
                    notifyChange.PropertyChanged += NotifyChange_PropertyChanged;
            }

            public void AddHandler(Action action)
            {
                PropertyChangedAction = action;
            }

            internal void ClearHandler()
            {
                PropertyChangedAction = null;
            }

            public static string GetName<T>(Expression<Func<T>> e)
            {
                var member = (MemberExpression)e.Body;
                return member.Member.Name;
            }

            public static object GetOwner<T>(Expression<Func<T>> e)
            {
                var member = (MemberExpression)e.Body;
                return member.Expression.Type;
            }

            //public static T GetPropValue(object src, string propName)
            //{
            //    return (T)src.GetType().GetProperty(propName).GetValue(src, null);
            //}

            private void NotifyChange_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == PropertyName)
                    PropertyChangedAction?.Invoke();// ((T)Convert.ChangeType(GetPropValue(sender, e.PropertyName), typeof(T)));
            }

            public void Dispose()
            {
                Dispose(true);
            }

            void Dispose(bool disposing)
            {
                if (isDisposed)
                    return;

                if (disposing && PropertyOwner != null)
                {
                    var notifyPropertyChanged = PropertyOwner as INotifyPropertyChanged;
                    if (notifyPropertyChanged != null)
                    {
                        notifyPropertyChanged.PropertyChanged -= NotifyChange_PropertyChanged;
                    }

                    PropertyOwner = null;
                }

                isDisposed = true;
            }

        }
    }
}