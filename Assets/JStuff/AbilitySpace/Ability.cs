using JStuff.EventSpace;

namespace JStuff.AbilitySpace
{
    public abstract class Ability
    {
        protected delegate void ScribeDelegate(EventType type, 
                                                EventAdministrator<EventType>.EventMethod method);

        public virtual void Subscribe(IEventOwner owner)
        {
            this.Scribe(owner, owner.GetEventAdministrator().Subscribe);
        }

        public virtual void UnSubscribe(IEventOwner owner)
        {
            this.Scribe(owner, owner.GetEventAdministrator().UnSubscribe);
        }

        protected abstract void Scribe(IEventOwner player, ScribeDelegate method);
    }
}