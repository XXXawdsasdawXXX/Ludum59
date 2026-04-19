using System;
using System.Numerics;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Tools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Code.Game
{
    public class PlayerInput : IService, IUpdateListener
    {
        private const string HORIZONTAL_AXIS_NAME = "Horizontal";
        private const string VERTICAL_AXIS_NAME = "Vertical";
        
        private const KeyCode RADAR_KEY = KeyCode.Alpha1;

        public Action RadarPressed;
        public Vector2 Forward { get; private set; }

        
        private float cooldown;
        private const float cooldownMax = 0.3f;
        
        public void GameUpdate()
        {
            Vector2 input = new(
                Input.GetAxisRaw(HORIZONTAL_AXIS_NAME), 
                Input.GetAxisRaw(VERTICAL_AXIS_NAME));

            if (Forward != input)
            {
                Forward = input;
            }

            cooldown -= Time.deltaTime;
            if (Input.GetKeyDown(RADAR_KEY) && cooldown < 0) 
            {
                Debug.Log("press radar");
                RadarPressed?.Invoke();
                cooldown = cooldownMax;
            }
        }
    }
}
