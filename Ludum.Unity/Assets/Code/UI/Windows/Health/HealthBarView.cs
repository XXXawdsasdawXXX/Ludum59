using Code.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.Health
{
    public class HealthBarView : UIView
    {
        [field: SerializeField] public Image Hand { get; private set; }
        [field: SerializeField] public Sprite[] HandStates { get; private set; }
    }
}