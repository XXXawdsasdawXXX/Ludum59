using UnityEngine;
using UnityEngine.Events;

namespace Code.UI.Windows
{
    public class PressAnyKeyCurtain : MonoBehaviour
    {
        [SerializeField] private UnityEvent _pressAnyKey;

        private void Update()
        {
            if (Input.anyKey)
            {
                _pressAnyKey?.Invoke();
            }
        }
    }
}