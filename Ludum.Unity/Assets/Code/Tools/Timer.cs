using System;
using UnityEngine;

namespace Code.Tools
{
    public class Timer
    {
        public event Action Finished;
        public float Duration { get; private set; }
        public float Max => _endTime;
        public float Current => Time.time;

        private float _startTime;
        private float _endTime;
        
        public bool IsFinish()
        {
            return Current >= Max;
        }

        public void Reset()
        {
            _startTime = 0;
            _endTime = 0;
        }

        public void Start(float duration)
        {
            _startTime = Time.time;
            _endTime = _startTime + duration;
            Duration = duration;
        }
    }
}