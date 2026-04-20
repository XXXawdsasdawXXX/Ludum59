using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyAttack : ICharacterComponent, ISubscriber
    {
        public Condition Condition { get; } = new Condition();
        private readonly EnemyView _view;
        private readonly PlayerSpawner _playerSpawner;

        
        public EnemyAttack(EnemyView enemyView)
        {
            _view = enemyView;
            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();
        }

        public void Subscribe()
        {
            _view.AttackTrigger.OnTrigger.SubscribeToValue(_attack);
        }

        public void Unsubscribe()
        {
            _view.AttackTrigger.OnTrigger.UnsubscibeFromValue(_attack);
        }

        private void _attack(bool value)
        {
            if (value && !_view.Model.Attack.PropertyValue)
            {
                _view.Model.Attack.PropertyValue = true;
                _playerSpawner.Player.Model.Health.PropertyValue--;
            }
            
            if(!value && _view.Model.Attack.PropertyValue)
            {
                _view.Model.Attack.PropertyValue = false;
            }
        }
    }
}