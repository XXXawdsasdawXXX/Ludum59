using System;
using Code.Core.ServiceLocator;
using FoW;
using UnityEngine;

namespace Code.Game.Map
{
    public class FogView : MonoBehaviour, IView
    {
        [SerializeField] private FogOfWarTeam _team;
        private Transform _player;

        private const float Cooldown = 10;

        private float timer = 0;
        public void SetOffset(Transform player)
        {
            _player = player;

            /*
            _team.mapOffset = new Vector2(
                player.position.x - _team.mapSize / 2f,
                player.position.y - _team.mapSize / 2f
            );
            */


            /*_team.mapOffset = new Vector2(
                player.position.x  / 2f,
                player.position.y  / 2f
            );*/
            
          //  _team.mapOffset = player.position;
        }

        private void LateUpdate()
        {
            timer += Time.deltaTime;

            if (timer >= Cooldown)
            {
                      
                _team.mapOffset = _player.position;
                _team.Reinitialize();
                timer = 0;
            }
      

        }
    }
}