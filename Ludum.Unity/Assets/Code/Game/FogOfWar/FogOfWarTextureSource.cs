using UnityEngine;

namespace Code.Game.FogOfWar
{
    [RequireComponent(typeof(Camera))]
    public class FogOfWarTextureSource : MonoBehaviour
    {
        public Camera targetCamera;

        Camera _fogCamera;
        RenderTexture _renderTexture = null;

        void Awake()
        {
            _fogCamera = GetComponent<Camera>();

            // Follow Dynamic Resolution scaling on both cameras
            _fogCamera.allowDynamicResolution = true;
            if (targetCamera != null)
                targetCamera.allowDynamicResolution = true;
        }

        void OnDisable()
        {
            if (targetCamera != null)
            {
                if (targetCamera.targetTexture == _renderTexture)
                    targetCamera.targetTexture = null;
                targetCamera.enabled = false;
            }
        }

        void OnDestroy()
        {
            DestroyRenderTexture();
        }

        void DestroyRenderTexture()
        {
            if (_renderTexture != null)
            {
                _renderTexture.Release();
                Destroy(_renderTexture);
                _renderTexture = null;
            }
        }

        void LateUpdate()
        {
            if (targetCamera == null)
                return;

            targetCamera.enabled = true;

            // Recreate RT if size changed (tracks DR via pixelWidth/Height)
            if (_renderTexture == null ||
                _renderTexture.width != _fogCamera.pixelWidth ||
                _renderTexture.height != _fogCamera.pixelHeight)
            {
                DestroyRenderTexture();
                _renderTexture = new RenderTexture(_fogCamera.pixelWidth, _fogCamera.pixelHeight, 16)
                {
                    // IMPORTANT: follow Dynamic Resolution scaling
                    useDynamicScale = true
                };
            }

            if (targetCamera.targetTexture != _renderTexture)
                targetCamera.targetTexture = _renderTexture;
        }
    }

    [RequireComponent(typeof(Camera))]
    [System.Obsolete("FogOfWarClearFog has been renamed to " + nameof(FogOfWarTextureSource))]
    public class FogOfWarClearFog : FogOfWarTextureSource { }
}
