using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.EventSpace
{
    public interface IEventOwner
    {
        EventAdministrator<EventType> GetEventAdministrator();
    }
}