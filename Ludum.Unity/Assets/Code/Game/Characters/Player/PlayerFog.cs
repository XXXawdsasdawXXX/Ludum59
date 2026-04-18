using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Map;
using Code.Tools;
using FoW;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerFog : ICharacterComponent, IUpdateListener
    {
        private readonly FogView _team;
        private readonly Transform _transfomr;
        public Condition Condition { get; private set; } = new();

        public PlayerFog(PlayerView playerView)
        {
            _team = Container.Instance.GetView<FogView>();
            _transfomr = playerView.transform;
        }
        
        public void GameUpdate()
        {
            
             _team.SetOffset(_transfomr);
        }
    }
}