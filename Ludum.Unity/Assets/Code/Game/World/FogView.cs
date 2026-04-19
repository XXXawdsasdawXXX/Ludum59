using Code.Core.ServiceLocator;
using Code.Game.FogOfWar;
using UnityEngine;

namespace Code.Game.World
{
    public class FogView : MonoBehaviour, IView
    {
        [SerializeField] private FogOfWarTeam _team;
      
        private Transform _player;


        public void SetOffset(Vector2 position)
        {
            _team.mapOffset = position;
            _team.Reinitialize();
        }
    }
}