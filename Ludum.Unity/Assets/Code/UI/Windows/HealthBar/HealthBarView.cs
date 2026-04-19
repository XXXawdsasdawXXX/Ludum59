using Code.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.HealthBar
{
    public class HealthBarView : UIView
    {
        [field: SerializeField] public Image Health { get; private set; }
    }
}