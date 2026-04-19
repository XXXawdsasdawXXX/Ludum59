using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Enemy;
using Code.Tools;

namespace Code.Game.Characters.Player.Abilities
{
    public class PlayerStan : ICharacterComponent, ISubscriber
    {
        public Condition Condition { get; } = new Condition();
        
        private readonly EnemySpawner _enemySpawner;
        private readonly PlayerModel _model;
        private readonly PlayerInput _input;

        public PlayerStan(PlayerView view)
        {
            _model = view.Model;
         
            _enemySpawner = Container.Instance.GetService<EnemySpawner>();
            _input = Container.Instance.GetService<PlayerInput>();
        }


        public void Subscribe()
        {
            
        }

        public void Unsubscribe()
        {
        
        }
    }
}