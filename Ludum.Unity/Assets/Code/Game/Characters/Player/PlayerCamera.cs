using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerCamera : ICharacterComponent, IUpdateListener
    {
        private static Vector3 _camera_offset = new Vector3(0, 0, -10);

        public Condition Condition { get; private set; } = new();

        private readonly Camera _camera;
        private readonly PlayerConfiguration _playerConfiguration;
        private readonly Transform _transform;

        public PlayerCamera(PlayerView playerView)
        {
            _transform = playerView.transform;
            _camera = Camera.main;
            
            /*if (_camera != null)
            {
                _camera.transform.SetParent(_transform);
            }*/
        }

        public void GameUpdate()
        {
            _camera.transform.position = _transform.position + _camera_offset;
        }
    }
}