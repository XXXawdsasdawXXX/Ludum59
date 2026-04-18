using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Data;
using Code.Game.Map;
using Code.Tools;
using Cysharp.Threading.Tasks;
using FoW;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerFog : ICharacterComponent, IUpdateListener
    {
        public Condition Condition { get; private set; } = new();

        private readonly FogView _fogMap;
        private readonly Transform _transform;
        private readonly FogOfWarUnit _fogUnit;

        private readonly RangedFloat _reloadCooldown = new(20f, 40f);
        private float _currentCooldownTime;
        private float _timer;
        private bool _isReloading;

        public PlayerFog(PlayerView playerView)
        {
            _fogMap = Container.Instance.GetView<FogView>();
            _transform = playerView.transform;
            _fogUnit = playerView.FogOfWarUnit;
        }

        public void GameUpdate()
        {
            _timer += Time.deltaTime;

            if (_timer >= _currentCooldownTime && !_isReloading)
            {
                _runFlashlight().Forget();

                _fogMap.SetOffset(_transform.position);

                _timer = 0;
                
                _currentCooldownTime = _reloadCooldown.GetRandomValue();
            }
        }
        
        private async UniTaskVoid _runFlashlight()
        {
            _isReloading = true;

            float baseBrightness = 0.3f;

            while (_isReloading)
            {
                float duration = Random.Range(0.5f, 2f);
                float amplitude = Random.Range(0.05f, 0.2f);

                float time = 0f;

                while (time < duration)
                {
                    time += Time.deltaTime;

                    float wave = Mathf.Sin(time * 2f); // плавная волна
                    float target = baseBrightness + wave * amplitude;

                    _fogUnit.brightness = target;

                    await UniTask.Yield();
                }
                
                _fogMap.SetOffset(_transform.position);
                
                if (Random.value < 0.4f)
                {
                    int flickers = Random.Range(2, 6);

                    for (int i = 0; i < flickers; i++)
                    {
                        _fogUnit.brightness = Random.Range(0.2f, 1f);
                        await UniTask.Delay(Random.Range(20, 80));
                    }

                    _fogUnit.brightness = baseBrightness;
                }

                _isReloading = false;
            }
            
        }
    }
}