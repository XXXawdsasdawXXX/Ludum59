using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.World;
using Code.UI.Base;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Windows.Progress
{
    public class ProgressPresenter : UIPresenter<ProgressView>, IInitializeListener, ISubscriber
    {
        private MachineSpawner _machineSpawner;
        private MachineView _currentMachine;

        private int _stage;
        
        public UniTask GameInitialize()
        {
            _machineSpawner = Container.Instance.GetService<MachineSpawner>();
            
            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            _machineSpawner.Spawned += _subscribeToMachine;
        }

        public void Unsubscribe()
        {   
            if (_currentMachine != null)
            {
                _currentMachine.IsConnected.UnsubscibeFromValue(_updateProgress);
            }
        }

        private void _subscribeToMachine(MachineView obj)
        {
            if (_currentMachine != null)
            {
                _currentMachine.IsConnected.UnsubscibeFromValue(_updateProgress);
            }

            _currentMachine = obj;
            _currentMachine.IsConnected.SubscribeToValue(_updateProgress);
        }

        private void _updateProgress(bool isConnected)
        {
            if (!isConnected)
            {
                return;
            }

            if (_stage < view.Stages.Length)
            {
                view.Stages[_stage].DOColor(Color.white, 0.6f).SetEase(Ease.OutBack);
                _stage++;
            }
        }
    }
}