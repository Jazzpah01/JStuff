using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.EventSpace
{
    public abstract class EventType
    {
        public string Val
        {
            get;
            protected set;
        }

        public EventType(string s)
        {
            Val = s;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        public static bool operator ==(EventType val0, EventType val1)
        {
            return val0.Val == val1.Val;
        }

        public static bool operator !=(EventType val0, EventType val1)
        {
            return val0.Val != val1.Val;
        }

        public static bool operator ==(string val0, EventType val1)
        {
            return val0 == val1.Val;
        }

        public static bool operator !=(string val0, EventType val1)
        {
            return val0 != val1.Val;
        }

        public static bool operator ==(EventType val0, string val1)
        {
            return val0.Val == val1;
        }

        public static bool operator !=(EventType val0, string val1)
        {
            return val0.Val != val1;
        }
    }
}