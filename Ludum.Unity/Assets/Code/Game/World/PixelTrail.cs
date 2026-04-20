using System.Collections.Generic;
using Code.Core.ServiceLocator;
using Code.Tools;
using PolyNav;
using UnityEngine;

namespace Code.Game.World
{
    /// <summary>
    /// Рисует призрачный след через Graphics.RenderMesh с GPU Instancing —
    /// все копии спрайта за один draw call независимо от их количества.
    /// Материал должен поддерживать GPU Instancing (галка Enable GPU Instancing).
    /// </summary>
    public class PixelTrail : MonoBehaviour, IService
    {
        [SerializeField] private PolyNavAgent _agent;
        [SerializeField] private SpriteRenderer _source;

        [Header("Trail")]
        [SerializeField] private float _spawnInterval  = 0.05f;
        [SerializeField] private float _fadeDuration   = 0.5f;
        [SerializeField] private int   _maxGhosts      = 40;
        [SerializeField] private Color _ghostTint      = new Color(0.5f, 0.8f, 1f, 1f);

        // Материал с GPU Instancing — Sprites/Default + Enable GPU Instancing
        [SerializeField] private Material _instancedMaterial;

        private struct Ghost
        {
            public Matrix4x4 Matrix;   // позиция + масштаб
            public Vector4   Color;    // rgba для instancing
            public float     TimeLeft;
        }

        private readonly List<Ghost> _ghosts = new();
        private float  _timer;
        private Timer  _durationTimer = new();
        private bool   _isActive;

        private ParticleSystem.EmitParams _emitParams;

        [SerializeField] private ParticleSystem _particles;
        // Instancing буферы
        private Mesh              _spriteMesh;
        private MaterialPropertyBlock _mpb;
        private Matrix4x4[]       _matrices;
        private Vector4[]         _colors;

        // ── IService / Activate ───────────────────────────────────────────────

        public void Activate(float duration, Vector3 from, Vector2 to)
        {
            _agent.transform.position = from;
            _agent.gameObject.SetActive(true);
            _durationTimer.Start(duration);
            _isActive = true;
            _agent.SetDestination(to);
            
        }

        private void Update()
        {
            if (_durationTimer.IsFinish() && _isActive)
            {
                _agent.gameObject.SetActive(false);
                _isActive = false;
            }

            if (!_isActive) return;

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = _spawnInterval;
                EmitParticle();
            }
        }

        private void EmitParticle()
        {
            if (_source.sprite == null) return;

            _emitParams.position = _source.transform.position;
            _emitParams.startColor = _ghostTint;
            _emitParams.startSize =  _particles.main.startSize.constant;

            _particles.Emit(_emitParams, 1);
        }

        // ── Unity ─────────────────────────────────────────────────────────────

        /*private void Awake()
        {
            _mpb      = new MaterialPropertyBlock();
            _matrices = new Matrix4x4[_maxGhosts];
            _colors   = new Vector4[_maxGhosts];
            
            _instancedMaterial = new Material(_instancedMaterial);
            _instancedMaterial.enableInstancing = true;

            _mpb      = new MaterialPropertyBlock();
            _matrices = new Matrix4x4[_maxGhosts];
            _colors   = new Vector4[_maxGhosts];
        }

        private void Update()
        {
            // Завершение эффекта
            if (_durationTimer.IsFinish() && _isActive)
            {
                _agent.gameObject.SetActive(false);
                _isActive = false;
            }

            // Спавн нового ghost-а пока активны
            if (_isActive)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _timer = _spawnInterval;
                    SpawnGhost();
                }
            }

            // Обновляем затухание и собираем данные для instancing
            UpdateGhosts();
        }

        private void LateUpdate()
        {
            if (_ghosts.Count == 0) return;
            DrawGhosts();
        }

        // ── Ghost logic ───────────────────────────────────────────────────────

        private void SpawnGhost()
        {
            if (_ghosts.Count >= _maxGhosts) return;
            if (_source.sprite == null) return;

            // Берём текущую матрицу трансформа источника
            Matrix4x4 matrix = Matrix4x4.TRS(
                _source.transform.position,
                _source.transform.rotation,
                // Учитываем flipX через отрицательный scale X
                new Vector3(
                    _source.transform.lossyScale.x * (_source.flipX ? -1f : 1f),
                    _source.transform.lossyScale.y,
                    1f));

            _ghosts.Add(new Ghost
            {
                Matrix   = matrix,
                TimeLeft = _fadeDuration,
                Color    = new Vector4(_ghostTint.r, _ghostTint.g, _ghostTint.b, _ghostTint.a)
            });
        }

        private void UpdateGhosts()
        {
            for (int i = _ghosts.Count - 1; i >= 0; i--)
            {
                Ghost g = _ghosts[i];
                g.TimeLeft -= Time.deltaTime;

                if (g.TimeLeft <= 0f)
                {
                    _ghosts.RemoveAt(i);
                    continue;
                }

                // Плавное затухание alpha
                float t = g.TimeLeft / _fadeDuration;
                g.Color.w = t * _ghostTint.a;

                _ghosts[i] = g;
            }
        }

        private void DrawGhosts()
        {
            // Перестраиваем mesh если спрайт сменился
            if (_spriteMesh == null || _source.sprite != null)
                _spriteMesh = BuildMeshFromSprite(_source.sprite);

            if (_spriteMesh == null) return;

            int count = Mathf.Min(_ghosts.Count, _maxGhosts);

            for (int i = 0; i < count; i++)
            {
                _matrices[i] = _ghosts[i].Matrix;
                _colors[i]   = _ghosts[i].Color;
            }

            // Передаём цвета через instanced property
            _mpb.SetVectorArray("_Color", _colors);

            var renderParams = new RenderParams(_instancedMaterial)
            {
                matProps       = _mpb,
                shadowCastingMode  = UnityEngine.Rendering.ShadowCastingMode.Off,
                receiveShadows = false,
                layer          = gameObject.layer
            };

            Graphics.RenderMeshInstanced(renderParams, _spriteMesh, 0, _matrices, count);
        }

        // ── Mesh builder ──────────────────────────────────────────────────────

        /// <summary>
        /// Строит Mesh из Sprite с учётом pivot и PPU —
        /// геометрически совпадает с тем, что рисует SpriteRenderer.
        /// </summary>
        private static Mesh BuildMeshFromSprite(Sprite sprite)
        {
            if (sprite == null) return null;

            var mesh = new Mesh { name = "GhostSpriteMesh" };

            // Sprite уже хранит вершины в локальных координатах (units)
            Vector2[] spriteVerts = sprite.vertices;
            Vector3[] verts       = new Vector3[spriteVerts.Length];
            for (int i = 0; i < spriteVerts.Length; i++)
                verts[i] = spriteVerts[i];

            mesh.vertices  = verts;
            mesh.uv        = sprite.uv;

            // ushort[] → int[]
            ushort[] spriteTriangles = sprite.triangles;
            int[]    tris            = new int[spriteTriangles.Length];
            for (int i = 0; i < spriteTriangles.Length; i++)
                tris[i] = spriteTriangles[i];

            mesh.triangles = tris;
            mesh.RecalculateBounds();
            return mesh;
        }*/
    }
}