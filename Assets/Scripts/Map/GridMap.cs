using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class GridMap : MonoBehaviour
    {
        public static GridMap Instance { get; private set; }
        
        private int minX, maxX;
        private int minY, maxY;
        private int minZ, maxZ;
        
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }

        public TileNode[,,] grid;

        private Color _initialColor;

        private void Awake()
        {
            Instance = this;

            StartPosition = new Vector3(15, 4, 30);

            var tiles = GameObject.FindWithTag("WorldOverlay").GetComponentsInChildren<Tile>();

            var nodesList = new List<TileNode>();

            foreach (var tile in tiles)
            {
                nodesList.Add(tile.tileNode);
            }
            
            _initialColor = nodesList[0].worldObject.GetComponent<Renderer>().material.color;

            var nodes = nodesList.ToArray();
            // Debug.Log(nodes.Length);

            var worldBounds = GameObject.FindWithTag("Ground").GetComponent<MeshRenderer>().bounds;
            minX = (int)worldBounds.min.x;
            maxX = (int)worldBounds.max.x;
        
            minY = (int)worldBounds.min.y;
            maxY = (int)worldBounds.max.y;
        
            minZ = (int)worldBounds.min.z;
            maxZ = (int)worldBounds.max.z;

            var xLength = (int)worldBounds.extents.x * 2 + 1;
            var yLength = (int)worldBounds.extents.y * 2 + 1;
            var zLength = (int)worldBounds.extents.z * 2 + 1;
            
            grid = new TileNode[xLength, yLength, zLength];
            
            // Debug.Log($"{xLength}, {yLength}, {zLength}");
            
            // Debug.Log(grid.Length);
            
            // Debug.Log($"{minX}, {maxX}, {minY}, {maxY}, {minZ}, {maxZ}");
            
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    for (var z = minZ; z <= maxZ; z++)
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

            if (x <= maxX && x >= minX && 
                y <= maxY && y >= minY && 
                z <= maxZ && z >= minZ)
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

        public void StartPathfinding()
        {
            foreach (var tileNode in grid)
            {
                if (tileNode == null)
                    continue;
                    
                if (tileNode.worldObject.GetComponent<Renderer>().material.color == Color.cyan)
                    tileNode.worldObject.GetComponent<Renderer>().material.color = _initialColor;
            }
            
            var pathfinder = new Pathfinder();

            // Avoidance test here.
            // grid[1, 0, 1].passThrough = TileProperties.TilePassThrough.NON_WALKABLE;

            // Pass the target node.
            var startNode = GetNodeFromVector(StartPosition);
            var endNode = GetNodeFromVector(EndPosition);
                
            // Find path.
            var path = pathfinder.FindPath(startNode, endNode);
                
            // Change colour of tile for each object passed through.
            startNode.worldObject.GetComponent<Renderer>().material.color = Color.cyan;
            foreach (var node in path)
            {
                node.worldObject.GetComponent<Renderer>().material.color = Color.cyan;
            }
        }
    }
}