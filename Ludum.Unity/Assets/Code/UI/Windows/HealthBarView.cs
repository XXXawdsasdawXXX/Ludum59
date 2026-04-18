using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
    public class HealthBarView : UIView
    {
        [field: SerializeField] public Image Health { get; private set; }
    }
}