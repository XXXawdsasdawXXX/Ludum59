using System;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Code.Game.Characters.Player
{
    public class PlayerInput : IService, IUpdateListener
    {
        private const string HORIZONTAL_AXIS_NAME = "Horizontal";
        private const string VERTICAL_AXIS_NAME = "Vertical";
        
        private const KeyCode STAN_KEY = KeyCode.Alpha1    ;
        private const KeyCode RADAR_KEY = KeyCode.Alpha2;
        private const KeyCode PATH_KEY = KeyCode.Alpha3;

        public Action RadarPressed;
        public Action StanPressed;
        public Vector2 Forward { get; private set; }
        

        public void GameUpdate()
        {
            Vector2 input = new(
                Input.GetAxisRaw(HORIZONTAL_AXIS_NAME), 
                Input.GetAxisRaw(VERTICAL_AXIS_NAME));

            if (Forward != input)
            {
                Forward = input;
            }
            
            if (Input.GetKeyDown(RADAR_KEY)) 
            {
                RadarPressed?.Invoke();
            }
            
            if (Input.GetKeyDown(STAN_KEY)) 
            {
                StanPressed?.Invoke();
            }
        }
    }
}
