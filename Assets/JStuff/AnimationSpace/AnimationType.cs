namespace JStuff.AnimationSpace
{
    /// <summary>
    /// A simple type class that specifies the animation state type. (E.G. walk, idle, melee attack)
    /// </summary>
    public class AnimationType
    {
        public string Val
        {
            get;
            private set;
        }

        public AnimationPlayType Type
        {
            get;
            private set;
        }

        public AnimationType(string s, AnimationPlayType type)
        {
            Val = s;
            Type = type;
        }
        
        
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        public static bool operator ==(AnimationType val0, AnimationType val1)
        {
            return val0.Val == val1.Val;
        }
        
        public static bool operator !=(AnimationType val0, AnimationType val1)
        {
            return val0.Val != val1.Val;
        }
        
        public static bool operator ==(string val0, AnimationType val1)
        {
            return val0 == val1.Val;
        }
        
        public static bool operator !=(string val0, AnimationType val1)
        {
            return val0 != val1.Val;
        }
        
        public static bool operator ==(AnimationType val0, string val1)
        {
            return val0.Val == val1;
        }
        
        public static bool operator !=(AnimationType val0, string val1)
        {
            return val0.Val != val1;
        }
    }
}