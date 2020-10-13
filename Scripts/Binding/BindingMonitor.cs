using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace UnityMVVM.Binding
{

    public class BindingMonitor
    {
        public static IEnumerable<DataBindingConnection> ActiveConnections
        {
            get
            {
                return Connections?.Where(c => c.IsBound);
            }
        }
        public static IEnumerable<DataBindingConnection> InactiveConnections
        {
            get
            {
                return Connections?.Where(c => !c.IsBound);
            }
        }

        public static List<DataBindingConnection> Connections = new List<DataBindingConnection>();

        public static void RegisterConnection(DataBindingConnection c)
        {
            Connections.Add(c);
        }

        public static void UnRegisterConnection(DataBindingConnection c)
        {
            Connections.Remove(c);
        }

        public static void Reset()
        {
            Connections = new List<DataBindingConnection>();
        }
    }
}