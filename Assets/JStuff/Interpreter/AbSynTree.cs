using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.Interpreter
{
    public abstract class AbSynTree
    {
        //public abstract object Execute();
        public Dictionary<string, object> table;
        public abstract class Element
        {
            public abstract object Execute(object context = null);
        };
    }
}
