using Code.UI.Base;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Code.UI.Windows
{
    public class RadarAbilityView : UIView
    {
        [field: SerializeField] public Image Fill { get; private set; }
        [field: SerializeField] public Image Background { get; private set; }

    }
}