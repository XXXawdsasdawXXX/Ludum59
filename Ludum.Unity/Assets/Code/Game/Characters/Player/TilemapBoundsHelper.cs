using FoW;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Game.Characters.Player
{
    public class TilemapBoundsHelper : MonoBehaviour
    {
        public Tilemap tilemap;
        public FogOfWarTeam fog;

        void Start()
        {
            tilemap.CompressBounds();
            Bounds bounds = tilemap.localBounds;
            Debug.Log($"Size: {bounds.size}");
            Debug.Log($"Center: {bounds.center}");
            Debug.Log($"Map Size для FogOfWar: {Mathf.Max(bounds.size.x, bounds.size.y)}");
            Debug.Log($"Map Offset X: {bounds.center.x}  Y: {bounds.center.y}");

            fog.mapSize = Mathf.Max(bounds.size.x, bounds.size.y);
            fog.mapOffset = bounds.center;
        }
    }
}