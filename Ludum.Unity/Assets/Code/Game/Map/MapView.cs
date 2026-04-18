using Code.Core.ServiceLocator;
using PolyNav;
using UnityEngine;

namespace Code.Game.Map
{
    public class MapView : MonoBehaviour, IView
    {
        [field: SerializeField] public PolyNavMap[] Maps { get; private set; }
    }
}