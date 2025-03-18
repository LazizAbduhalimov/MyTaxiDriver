using LGrid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Scripts.Game
{
    public class MapIniterForAStart : MonoBehaviour
    {
        public Tile Tile;
        public GameObject Obsticle; 
        
        public void Start()
        {
            var obstacles = FindObjectsOfType<Obstacle>();
            var tiles = FindObjectsOfType<Tile>();
            foreach (var tile in tiles)
            {
                Map.Instance.CreateCell(Vector3Int.RoundToInt(tile.transform.position));
            }
            
            foreach (var obstacle in obstacles)
            {
                var position = Vector3Int.RoundToInt(obstacle.transform.position);
                if (Map.Instance.IsCellExists(position, out var cellOccupied))
                {
                    cellOccupied.IsOccupied = true;
                }
            }
        }

        [ContextMenu("Create map")]
        public void CreateMap30x30() => CreateMap(30, 30);

        private void CreateMap(int x, int z)
        {
            var tilesParent = new GameObject("Tiles").transform;
            var obstaclesParent = new GameObject("Obstacles").transform;
            for (var i = -x; i <= x; i++)
            {
                for (var j = -z; j <= z; j++)
                {
                    var cellPosition = new Vector3Int(i, 0, j);
                    Instantiate(Tile, cellPosition, Quaternion.identity, tilesParent);
                    if (i == 0 && j == 0) continue;
                    if (Random.Range(1, 10) == 1)
                    {
                        Instantiate(Obsticle, cellPosition, Quaternion.identity, obstaclesParent);
                    }
                }    
            }
        }
    }
}