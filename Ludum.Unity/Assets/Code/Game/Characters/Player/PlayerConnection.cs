using System;
using System.Threading;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.World;
using Code.Tools;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerConnection : ICharacterComponent, ISubscriber
    {
        private readonly PlayerView _view;
        private readonly MachineSpawner _machineSpawner;
        public Condition Condition { get; } = new Condition();

        public MachineView _currentMachine ;

        private CancellationTokenSource _waitInput;
        private readonly PlayerInput _input;

        public PlayerConnection(PlayerView view)
        {
            _view = view;

            _machineSpawner = Container.Instance.GetService<MachineSpawner>();
            _input = Container.Instance.GetService<PlayerInput>();
        }

        public void Subscribe()
        {
            _machineSpawner.Spawned += _subscribeToMachine;
        }

        public void Unsubscribe()
        {
            _machineSpawner.Spawned -= _subscribeToMachine;
            if (_currentMachine != null)
            {
         
            }
        }

        private void _subscribeToMachine(MachineView obj)
        {
            if (_currentMachine != null)
            {
                _currentMachine.InteractionTrigger.OnTrigger.SubscribeToValue(_updateState);
            }

            _currentMachine = obj;

        }

        private void _updateState(bool obj)
        {
            if (obj && Condition.AreMet())
            {
                _waitInput?.Cancel();
                _waitInput = new CancellationTokenSource();
                _waitInputAsync(_waitInput.Token).Forget();
            }
            else if (!obj)
            {
                _waitInput?.Cancel();
            }
        }

        private async UniTaskVoid _waitInputAsync(CancellationToken ct)
        {
            try
            {
                await UniTask.WaitUntil(() => _input.InteractionPressed, cancellationToken: ct);
           
                _view.Model.Connecting.PropertyValue = true;
                
                await _moveToConnectionPoint();
                
                await _playAnimation();

                _currentMachine.IsConnected.PropertyValue = true;
                _view.Model.Connecting.PropertyValue = false;
            }
            catch (OperationCanceledException)
            {
                // игрок вышел из зоны — нормальная отмена, ничего не делаем
            }
        }


        private async UniTask _moveToConnectionPoint()
        {
            try
            {
                var target = _currentMachine.ConnectPoint.position;

                while (Vector3.Distance(_view.transform.position, target) > 0.05f)
                {
                    var dir = (target - _view.transform.position).normalized;

                    _view.Rigidbody2D.velocity = dir * 1f;

                    await UniTask.DelayFrame(1);
                }

                _view.Rigidbody2D.velocity = Vector2.zero;

                _view.Model.Connecting.PropertyValue = true;
            }
            catch (OperationCanceledException)
            {
                _view.Rigidbody2D.velocity = Vector2.zero;
            }
        }

        private async UniTask _playAnimation()
        {
            _view.Renderer.PlayConnection();
            await UniTask.Delay(TimeSpan.FromSeconds(0.6f));
            _currentMachine.Render.PlayConnect();
        }
    }
}