
namespace Code.Tools
{
    public class Timer
    {
        public float Max { get; private set; }
        public float Current { get; private set; }

        public Timer(float max)
        {
            Max = max;
        }

        public bool Update(float value)
        {
            Current += value;

            if (Current < Max)
            {
                return false;
            }
            
            return true;
        }

        public void Finish()
        {
            Current = Max;
        }
        
        public void Reset()
        {
            Current = 0;
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