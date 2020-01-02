namespace JStuff.AnimationSpace
{
    /// <summary>
    /// A simple type class that specifies the animation key state. Used for abilities. (0-infinity)
    /// </summary>
    public class AnimationKey
    {
        public int Val
        {
            get;
            private set;
        }

        public AnimationKey(int i)
        {
            Val = i;
        }

        
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        public static bool operator ==(AnimationKey val0, AnimationKey val1)
        {
            return val0.Val == val1.Val;
        }
        
        public static bool operator !=(AnimationKey val0, AnimationKey val1)
        {
            return val0.Val != val1.Val;
        }
        
        public static bool operator ==(AnimationKey val0, int val1)
        {
            return val0.Val == val1;
        }
        
        public static bool operator !=(AnimationKey val0, int val1)
        {
            return val0.Val != val1;
        }
    }
}