using Code.UI.Base;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Code.UI.Windows
{
    public class AbilityView : UIView
    {
        [field: SerializeField] public Image Fill { get; private set; }
        [field: SerializeField] public Image Background { get; private set; }

    }
}