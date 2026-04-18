using Code.Core.ServiceLocator;
using FoW;
using UnityEngine;

namespace Code.Game.Map
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