using Code.Core.GameLoop;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Windows.Radar
{
    public class RadarView : UIView
    {
        [field: SerializeField] public MonoPool<UIRadarMarker> MarkerPool;
        [field: SerializeField] public UIRadarMarker MainCircle { get; private set; }
    }
}