using Code.Core.GameLoop;
using Code.Core.Pools;
using Cysharp.Threading.Tasks;
using MPUIKIT;
using UnityEngine;

namespace Code.UI.World
{
    public class UIEnemyMarker : UIComponent, IInitializeListener
    {
        private const float MAX_SIZE = 0.2f;
        private const float DURATION = 1.5f;
        
        [SerializeField] private MPImage _mpImage;
        
        public UniTask GameInitialize()
        {
            Circle circle = _mpImage.Circle;
            circle.Radius = 0;
            _mpImage.Circle = circle;
            
            return UniTask.CompletedTask;
        }

        public async UniTaskVoid Show()
        {
            float start = _mpImage.Circle.Radius;
            float time = 0f;

            while (time < DURATION)
            {
                time += Time.deltaTime;
                float t = time / DURATION;

                Circle circle = _mpImage.Circle;
                circle.Radius = Mathf.Lerp(start, MAX_SIZE, t);
                _mpImage.Circle = circle;

                await UniTask.Yield();
            }
        }
    }
}