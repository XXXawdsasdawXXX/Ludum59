using System;
using Code.Core.GameLoop;
using Code.Tools;
using PolyNav;

namespace Code.Game.Characters.Enemy
{
    public class EnemyAnimation : ICharacterComponent, IUpdateListener, ISubscriber
    {
        public Condition Condition { get; } = new();
        
        private readonly PolyNavAgent _agent;
        private readonly EnemyRender _render;
        
        private float _forward;
        private readonly EnemyView _view;

        
        public EnemyAnimation(EnemyView view)
        {
            _view = view;
            _render = view.Render;
            _agent = view.Agent;
        }
        
        public void Subscribe()
        {
            _view.Model.Stan.SubscribeToValue(_render.SetStan);
            _view.Model.Attack.SubscribeToValue(_render.SetAttack);
        }

        public void Unsubscribe()
        {
            _view.Model.Stan.UnsubscibeFromValue(_render.SetStan);
            _view.Model.Attack.UnsubscibeFromValue(_render.SetAttack);
        }

        public void GameUpdate()
        {
            float forward = _agent.CurrentVelocity.x;
                
            if (forward != 0 && Math.Abs(forward - _forward) > 0.1f)
            {
                _render.FlipX(forward < 0);
                _forward = forward; 
            }
            
            _render.SetSpeed(_agent.currentSpeed * _agent.maxSpeed);
        }
    }
}