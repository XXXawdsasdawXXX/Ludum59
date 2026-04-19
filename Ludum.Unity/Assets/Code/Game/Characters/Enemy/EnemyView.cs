using System;
using System.Linq;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.World;
using Cysharp.Threading.Tasks;
using PolyNav;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyView : Character, IInitializeListener
    {

        [field: SerializeField] public EnemyRender Render { get; private set; }
        [field: SerializeField] public PolyNavAgent Agent { get; private set; }
        [ShowInInspector, ReadOnly] public EEnemyType Type { get; private set; }
        [ShowInInspector, ReadOnly] public EnemyModel Model { get; private set; }
        private MapView _mapView;
        
        
        public UniTask GameInitialize()
        {
            _mapView = Container.Instance.GetView<MapView>();
            return UniTask.CompletedTask;
        }
        
        public override void InitializeComponents()
        {
            EnemyMovement enemyMovement = new(this);
            Components.Add(typeof(EnemyMovement), enemyMovement);
            
            enemyMovement.Condition.Add(() => true);
        }

        public void SetType(EEnemyType type)
        {
            Type = type;
            
            Model = Container.Instance.GetConfiguration<EnemiesConfiguration>()
                .Models
                .FirstOrDefault(m => m.Type == Type);

            if (Model == null)
            {
                Debug.LogError($"Enemy with type {Type} has not model");
                return;
            }
            
            Agent.maxSpeed = Model.Speed;
            Agent.map = _mapView.Maps[Math.Min(_mapView.Maps.Length - 1, Model.Size)];
            
            Render.SetModel(Model);
        }
    }
}