using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI
{
    public class CutScene : MonoBehaviour
    {
        [SerializeField] private float _animationDuration;

        private async void Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_animationDuration));

            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
}