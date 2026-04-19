using Code.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.HealthBar
{
    public class EnergyBarView : UIView
    {
        [field: SerializeField] public Image Energy { get; private set; }
    }
}