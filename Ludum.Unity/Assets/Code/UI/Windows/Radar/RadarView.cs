using Code.Core.Pools;
using Code.UI.World;
using MPUIKIT;
using UnityEngine;

namespace Code.UI.Windows
{
    public class RadarView : UIView
    {
        [field: SerializeField] public MonoPool<UIEnemyMarker> MarkerPool;

        [field: SerializeField] public MPImage MainCircle { get; private set; }
    }
}