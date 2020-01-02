using System;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.EventSpace
{
    /// <summary>
    /// Class that ties events to an object:IEventOwner. When an event is executed,
    /// the IEventOwner will be passed, as well as some data:object to the subscribed
    /// methods.
    /// </summary>
    /// <typeparam name="SType"></typeparam>
    public class EventAdministrator<SType> where SType : EventType
    {
        private IEventOwner source;

        public delegate void EventMethod(IEventOwner source, object data = null);

        private Dictionary<string, EventMethod> eventDictionary;

        public EventAdministrator(IEventOwner source)
        {
            this.source = source;
            eventDictionary = new Dictionary<string, EventMethod>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="method">Of type (Player, object):void</param>
        public void Subscribe(SType eventType, EventMethod method)
        {

            if (!eventDictionary.ContainsKey(eventType.Val))
            {
                eventDictionary.Add(eventType.Val, method);
            }
            else
            {
                eventDictionary[eventType.Val] += method;
            }
        }

        /// <summary>
        /// Unsubscribe an item from an event type.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="method"></param>
        public void UnSubscribe(SType eventType, EventMethod method)
        {
            if (!eventDictionary.ContainsKey(eventType.Val))
                throw new Exception("Cannot unsubscribe an element of a dictionary event that doesn't exist.");

            if (eventDictionary[eventType.Val] == null)
                throw new Exception("The unsubscribed event is already null.");

            eventDictionary[eventType.Val] -= method; // NOTE: don't use delegate subtraction, if the subtracted delegate contains
                                                // more than 1 method.
            Debug.Log("Unsubscribed.");
            if (eventDictionary[eventType.Val] == null)
                eventDictionary.Remove(eventType.Val);
        }

        /// <summary>
        /// Execute event.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="data">The data passed to subscribed items.</param>
        public void ExecuteEvent(SType eventType, object data = null)
        {
            if (!eventDictionary.ContainsKey(eventType.Val))
                return;

            if (eventDictionary[eventType.Val] == null)
                throw new Exception("Empty delegate which is not removed.");
            //Debug.Log("Executing event. Source =" + source + ". " + (source == null));
            eventDictionary[eventType.Val](this.source, data);
        }
    }
}

