using PolyNav;
using UnityEngine;

namespace Code.Game.World
{
    public static class PolyNavMapExtensions
    {
        public static bool GetRandomPoint(this PolyNavMap map, out Vector2 result, int maxAttempts = 100)
        {
            var points = GetMasterBounds(map);
        
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector2 candidate = new(
                    Random.Range(points.min.x, points.max.x),
                    Random.Range(points.min.y, points.max.y)
                );

                if (map.PointIsValid(candidate))
                {
                    result = candidate;
                    return true;
                }
            }

            result = Vector2.zero;
            return false;
        }
        
        private static Bounds GetMasterBounds(this PolyNavMap map)
        {
            var collider = map.GetComponent<PolygonCollider2D>();
    
            if (collider == null)
                return new Bounds(map.transform.position, Vector2.one * 10f);

            // считаем bounds вручную из точек полигона
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            for (int i = 0; i < collider.pathCount; i++)
            {
                Vector2[] points = collider.GetPath(i);
                foreach (Vector2 p in points)
                {
                    // переводим в мировые координаты
                    Vector2 world = map.transform.TransformPoint(p);
                    if (world.x < minX) minX = world.x;
                    if (world.y < minY) minY = world.y;
                    if (world.x > maxX) maxX = world.x;
                    if (world.y > maxY) maxY = world.y;
                }
            }

            Vector2 center = new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
            Vector2 size = new Vector2(maxX - minX, maxY - minY);
            return new Bounds(center, size);
        }
    }
}