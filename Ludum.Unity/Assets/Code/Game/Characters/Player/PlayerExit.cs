using System;
using System.Threading;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Door;
using Code.Game.World;
using Code.Tools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Progress = Game.Progress;

namespace Code.Game.Characters.Player
{
    public class PlayerExit : ICharacterComponent, ISubscriber
    {
        private readonly PlayerView _view;
        private readonly DoorSpawner _machineSpawner;

        public event Action Exited;
        public Condition Condition { get; } = new Condition();

        public DoorView _currentMachine;

        private CancellationTokenSource _waitInput;
        private readonly PlayerInput _input;

        public PlayerExit(PlayerView view)
        {
            _view = view;

            _machineSpawner = Container.Instance.GetService<DoorSpawner>();
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
                _currentMachine.InteractionTrigger.OnTrigger.UnsubscibeFromValue(_updateState);
            }
        }

        private void _subscribeToMachine(DoorView obj)
        {
            if (_currentMachine != null)
            {
                _currentMachine.InteractionTrigger.OnTrigger.UnsubscibeFromValue(_updateState);
            }

            _currentMachine = obj;
            _currentMachine.InteractionTrigger.OnTrigger.SubscribeToValue(_updateState);
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
                _currentMachine.InteractionIcon.gameObject.SetActive(true);
                
                await UniTask.WaitUntil(() => _input.InteractionPressed, cancellationToken: ct);

                _currentMachine.InteractionIcon.gameObject.SetActive(false);
                
                _view.Model.Connecting.PropertyValue = true;

                await _moveToConnectionPoint();

                _currentMachine.IsConnected.PropertyValue = true;
                _view.Model.Connecting.PropertyValue = false;

                Progress.Attempt++;
                Exited?.Invoke();
            }
            catch (OperationCanceledException)
            {
                _currentMachine.InteractionIcon.gameObject.SetActive(false);
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


    }
}