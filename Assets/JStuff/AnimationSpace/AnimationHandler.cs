using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace JStuff.AnimationSpace
{
    public enum AnimationPlayType
    {
        Interrupt,
        Loop
    }
    
    /// <summary>
    /// MonoBehavior class for handling animations.
    /// Assumption: The animator controller has a variable "speed" that is multiplied to the animations.
    /// </summary>
    public class AnimationHandler: MonoBehaviour {
        //private GameObject gameObject;
        private Animator animator;
        private AnimationObserver observer;

        //public AnimationHandler(Animator animator)
        //{
        //    this.animator = animator;
        //}

        void Start()
        {
            if (animator == null)
            {
                animator = gameObject.GetComponent<Animator>();
                if (animator == null)
                {
                    animator = gameObject.AddComponent<Animator>();
                }
            }
        }

        public void SetAnimatorController(AnimatorController animatorController)
        {
            this.animator.runtimeAnimatorController = animatorController;
        }

        public void SetAnimatorController(string animatorController)
        {
            this.animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>
                                                                ("Animations/"+animatorController);
        }

        public void Subscribe(AnimationObserver.KeyFrameStateEvent method)
        {
            if (observer == null)
                observer = gameObject.AddComponent<AnimationObserver>();

            observer.Subscribe(method);
        }

        public void PlayAnimation(float speed)
        {
            animator.SetFloat("speed", speed);
        }

        public void PlayAnimation(AnimationType animationType, float speed)
        {
            //Debug.Log(this.animator.GetCurrentAnimatorStateInfo(0).IsName(animationType.Val));
            
            animator.SetFloat("speed", speed);
            
            if (animationType.Type == AnimationPlayType.Interrupt || !this.animator.GetCurrentAnimatorStateInfo(0).IsName(animationType.Val))
                animator.Play(animationType.Val, -1, 0f);
        }
    }
}