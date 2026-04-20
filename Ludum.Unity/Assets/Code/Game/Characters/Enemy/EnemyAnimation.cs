using System;
using Code.Core.GameLoop;
using Code.Tools;
using PolyNav;

namespace Code.Game.Characters.Enemy
{
    public class EnemyAnimation : ICharacterComponent, IUpdateListener
    {
        public Condition Condition { get; } = new();
        
        private readonly PolyNavAgent _agent;
        private readonly EnemyRender _render;
        
        private float _forward;
        
        public EnemyAnimation(EnemyView view)
        {
            _render = view.Render;
            _agent = view.Agent;
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