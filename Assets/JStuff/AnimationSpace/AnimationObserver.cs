using UnityEngine;

namespace JStuff.AnimationSpace
{
    public class AnimationObserver: MonoBehaviour
    {
        public delegate void KeyFrameStateEvent(AnimationKey keyframe);
        public event KeyFrameStateEvent keyFrameStateEvent;

        /// <summary>
        /// Subscribes a function to the observer pattern.
        /// </summary>
        /// <param name="keyFrameStateEvent">Parameter function: (AnimationKey):void</param>
        public void Subscribe(KeyFrameStateEvent keyFrameStateEvent)
        {
            this.keyFrameStateEvent += keyFrameStateEvent;
        }

        public void Observe(int keyframeInt)
        {
            if (keyFrameStateEvent == null)
                return;

            keyFrameStateEvent(new AnimationKey(keyframeInt));
        }
    }
}