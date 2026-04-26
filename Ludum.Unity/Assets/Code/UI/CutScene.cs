using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

namespace Code.UI
{
    public class CutScene : MonoBehaviour
    {
        [Header("Cutscene Settings")]
        [SerializeField] private float _animationDuration;

        [Header("FMOD Events")]
        [SerializeField] private EventReference event1;
        [SerializeField] private EventReference event2;
        [SerializeField] private EventReference event3;
        [SerializeField] private EventReference event4;
        [SerializeField] private EventReference event5;
        [SerializeField] private EventReference event6;
        [SerializeField] private EventReference event7;

        private async void OnEnable()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_animationDuration + 2));
            SceneManager.LoadSceneAsync(sceneBuildIndex: 1);
        }

        public void PlayEvent1() => RuntimeManager.PlayOneShot(event1, transform.position);
        public void PlayEvent2() => RuntimeManager.PlayOneShot(event2, transform.position);
        public void PlayEvent3() => RuntimeManager.PlayOneShot(event3, transform.position);
        public void PlayEvent4() => RuntimeManager.PlayOneShot(event4, transform.position);
        public void PlayEvent5() => RuntimeManager.PlayOneShot(event5, transform.position);
        public void PlayEvent6() => RuntimeManager.PlayOneShot(event6, transform.position);
        public void PlayEvent7() => RuntimeManager.PlayOneShot(event7, transform.position);
    }
}