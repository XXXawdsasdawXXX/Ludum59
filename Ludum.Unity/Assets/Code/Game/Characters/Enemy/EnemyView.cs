using System.Linq;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.World;
using PolyNav;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyView : Character, ISubscriber
    {
        [field: SerializeField] public EnemyRender Render { get; private set; }
        [field: SerializeField] public PolyNavAgent Agent { get; private set; }
        [ShowInInspector, ReadOnly] public EEnemyType Type { get; private set; }
        [ShowInInspector, ReadOnly] public EnemyModel Model { get; private set; }

        [SerializeField] private Trigger _rangeTrigger;
        [SerializeField] private Trigger _milleTrigger;
        [SerializeField] private Trigger _attackTrigger;
        
        private MapView _mapView;
        
        public void SetType(EEnemyType type)
        {
            Type = type;
            
            Model = Container.Instance.GetConfiguration<EnemiesConfiguration>()
                .Models
                .FirstOrDefault(m => m.Type == Type)?
                .Clone();

            if (Model == null)
            {
                Debug.LogError($"Enemy with type {Type} has not model");
                return;
            }
            
            Render.SetModel(Model);
        }

        public override void InitializeComponents()
        {
            EnemyMovement enemyMovement = new(this, transform.position);
            Components.Add(typeof(EnemyMovement), enemyMovement);

            EnemyAnimation enemyAnimation = new(this);
            Components.Add(typeof(EnemyAnimation), enemyAnimation);
            
            enemyMovement.Condition.Add(() => Model.Follow.PropertyValue || Model.AbilityAgro.PropertyValue);
            enemyMovement.Condition.Add(() => !Model.Stan.PropertyValue);
            enemyMovement.Condition.Add(() => !_attackTrigger.OnTrigger.PropertyValue);
        }
        
        public void Subscribe()
        {
            _milleTrigger.OnTrigger.SubscribeToValue(_onMilleTriggerEnter);
            _rangeTrigger.OnTrigger.SubscribeToValue(_onRangeTriggerEnter);
            _attackTrigger.OnTrigger.SubscribeToValue(_onAttackTriggerEnter);
        }

        public void Unsubscribe()
        {
            _milleTrigger.OnTrigger.UnsubscibeFromValue(_onMilleTriggerEnter);
            _rangeTrigger.OnTrigger.UnsubscibeFromValue(_onRangeTriggerEnter);
            _attackTrigger.OnTrigger.UnsubscibeFromValue(_onAttackTriggerEnter);
        }

        private void _onRangeTriggerEnter(bool value)
        {
            if (Model.Follow.PropertyValue && !value)
            {
                Model.Follow.PropertyValue = false;
            }
        }

        private void _onMilleTriggerEnter(bool value)
        {
            if (value)
            {
                Model.Follow.PropertyValue = true;
            }
        }

        private void _onAttackTriggerEnter(bool value)
        {
            Model.Attack.PropertyValue = value;
        }
    }
}