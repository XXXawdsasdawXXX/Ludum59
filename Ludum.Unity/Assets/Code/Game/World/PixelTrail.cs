using System.Collections.Generic;
using Code.Core.ServiceLocator;
using Code.Tools;
using PolyNav;
using UnityEngine;

namespace Code.Game.World
{
    public class PixelTrail : MonoBehaviour, IService
    {
        [SerializeField] private PolyNavAgent _agent;
        [SerializeField] private Material _material;
        [SerializeField] private SpriteRenderer _source;
        [SerializeField] private float _spawnInterval = 0.05f;
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField] private int _poolSize = 20;

        private float _timer;
        private Queue<SpriteRenderer> _pool = new();
        private List<(SpriteRenderer sr, float timeLeft)> _active = new();

        private Timer _durationTimer = new();
        private bool _isActive;

        public void Activate(float duration,Vector3 startPosition, Vector2 position)
        {
            _agent.transform.position = startPosition;
            _agent.gameObject.SetActive(true);
            _durationTimer.Start(duration);
            _isActive = true;
            _agent.SetDestination(position);
        }
        
        void Awake()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                var go = new GameObject("TrailGhost");
                go.transform.SetParent(transform);
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sortingLayerName = _source.sortingLayerName;
                sr.sortingOrder = _source.sortingOrder - 1;
                sr.material = _material;
                go.SetActive(false);
                _pool.Enqueue(sr);
            }
        }

        void Update()
        {
            if (_durationTimer.IsFinish() )
            {
                if (!_isActive)
                {
                    return;
                }

           
                // обновляем затухание
                for (int i = _active.Count - 1; i >= 0; i--)
                {
                    var (sr, timeLeft) = _active[i];
                    float newTime = timeLeft - Time.deltaTime;

                    sr.gameObject.SetActive(false);
                    _pool.Enqueue(sr);
                    _active.RemoveAt(i);
                }
                
                _agent.gameObject.SetActive(false);
                _isActive = false;
            }
            
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _timer = _spawnInterval;
                _spawnGhost();
            }

            // обновляем затухание
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var (sr, timeLeft) = _active[i];
                float newTime = timeLeft - Time.deltaTime;

                if (newTime <= 0f)
                {
                    sr.gameObject.SetActive(false);
                    _pool.Enqueue(sr);
                    _active.RemoveAt(i);
                }
                else
                {
                    Color c = sr.color;
                    c.a = newTime / _fadeDuration;
                    sr.color = c;
                    _active[i] = (sr, newTime);
                }
            }
        }

        private void _spawnGhost()
        {
            if (_pool.Count == 0) return;

            var sr = _pool.Dequeue();
            sr.sprite = _source.sprite;
            sr.flipX = _source.flipX;
            sr.color = new Color(1f, 1f, 1f, 1f);

            sr.transform.position = _source.transform.position;
            sr.transform.localScale = _source.transform.localScale;

            sr.gameObject.SetActive(true);
            _active.Add((sr, _fadeDuration));
        }
    }
}