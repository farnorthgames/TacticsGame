using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class GridMap : MonoBehaviour
    {
        public static GridMap Instance { get; private set; }
        
        private int _minX, _maxX;
        private int _minY, _maxY;
        private int _minZ, _maxZ;
        
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }

        public TileNode[,,] grid;

        private Color _initialColor;

        public List<TileNode> Path { get; private set; }

        private void Awake()
        {
            Instance = this;

            StartPosition = new Vector3(15, 4, 30);

            var tiles = GameObject.FindWithTag("WorldOverlay").GetComponentsInChildren<Tile>(true);

            var nodesList = new List<TileNode>();

            foreach (var tile in tiles)
            {
                nodesList.Add(tile.tileNode);
            }
            
            _initialColor = nodesList[0].worldObject.GetComponent<Renderer>().material.color;

            var nodes = nodesList.ToArray();
            // Debug.Log(nodes.Length);

            var worldBounds = GameObject.FindWithTag("Ground").GetComponent<MeshRenderer>().bounds;
            _minX = (int)worldBounds.min.x;
            _maxX = (int)worldBounds.max.x;
        
            _minY = (int)worldBounds.min.y;
            _maxY = (int)worldBounds.max.y;
        
            _minZ = (int)worldBounds.min.z;
            _maxZ = (int)worldBounds.max.z;

            var xLength = (int)worldBounds.extents.x * 2 + 1;
            var yLength = (int)worldBounds.extents.y * 2 + 1;
            var zLength = (int)worldBounds.extents.z * 2 + 1;
            
            grid = new TileNode[xLength, yLength, zLength];
            
            for (var x = _minX; x <= _maxX; x++)
            {
                for (var y = _minY; y <= _maxY; y++)
                {
                    for (var z = _minZ; z <= _maxZ; z++)
                    {
                        foreach (var node in nodes)
                        {
                            if (x == node.x &&
                                y == node.y &&
                                z == node.z)
                            {
                                grid[x, y, z] = node;
                            }
                        }
                    }
                }
            }

            foreach (var tileNode in grid)
            {
                if (tileNode == null)
                    continue;
                    
                if (tileNode.passThrough == TileProperties.TilePassThrough.NON_WALKABLE)
                    tileNode.worldObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        public TileNode GetTileNode(int x, int y, int z)
        {
            TileNode node = null;

            if (x <= _maxX && x >= _minX && 
                y <= _maxY && y >= _minY && 
                z <= _maxZ && z >= _minZ)
            {
                node = grid[x, y, z];
            }

            return node;
        }

        public TileNode GetNodeFromVector(Vector3 position)
        {
            var x = Mathf.RoundToInt(position.x);
            var y = Mathf.RoundToInt(position.y);
            var z = Mathf.RoundToInt(position.z);

            return GetTileNode(x, y, z);
        }

        public List<TileNode> StartPathfinding()
        {
            foreach (var tileNode in grid)
            {
                if (tileNode == null)
                    continue;

                if (tileNode.renderer.material.color != Color.cyan)
                    continue;
                
                tileNode.renderer.material.color = _initialColor;
                tileNode.worldObject.SetActive(false);
            }
            
            var pathfinder = new Pathfinder();

            // Avoidance test here.
            // grid[1, 0, 1].passThrough = TileProperties.TilePassThrough.NON_WALKABLE;

            // Pass the target node.
            var startNode = GetNodeFromVector(StartPosition);
            var endNode = GetNodeFromVector(EndPosition);
                
            // Find path.
            Path = pathfinder.FindPath(startNode, endNode);
                
            // Change colour of tile for each object passed through.
            
            startNode.worldObject.SetActive(true);
            startNode.renderer.material.color = Color.cyan;
            
            foreach (var node in Path)
            {
                node.worldObject.SetActive(true);
                node.renderer.material.color = Color.cyan;
            }

            if (Path.Count >= 1) 
                return Path;
            
            startNode.renderer.material.color = _initialColor;
            startNode.worldObject.SetActive(false);

            return Path;
        }
    }
}