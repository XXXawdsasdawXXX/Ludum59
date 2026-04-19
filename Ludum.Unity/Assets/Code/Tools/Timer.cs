
using System;

namespace Code.Tools
{
    public class Timer
    {
        public event Action Updated;
        public float Max { get; private set; }
        public float Current { get; private set; }

        public Timer(float max)
        {
            Max = max;
        }

        public bool Update(float value)
        {
            Current += value;

            Updated?.Invoke();
            
            if (Current < Max)
            {
                return false;
            }
            
            return true;
        }

        public void Finish()
        {
            Current = Max;
            
            Updated?.Invoke();
        }
        
        public void Reset()
        {
            Current = 0;
            
            Updated?.Invoke();
        }

        public bool AreMet()
        {
            return Current >= Max;
        }

        public void SetMaxTime(float max)
        {
            Max = max;
        }
    }
}