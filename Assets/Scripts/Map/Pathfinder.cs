using System;
using System.Collections.Generic;
using UnityEngine;
using static Map.TileProperties;

namespace Map
{
    public class Pathfinder
    {
        private GridMap _gridMap;

        public List<TileNode> FindPath(TileNode startNode, TileNode endNode)
        {
            _gridMap = GridMap.Instance;

            return CalculatePath(startNode, endNode);
        }

        private List<TileNode> CalculatePath(TileNode startNode, TileNode endNode)
        {
            // Start A* Algorithm here.
            
            // The found path. This list will contain all the nodes along the path to the endNode.
            var foundPath = new List<TileNode>();
            
            // Two lists, one for nodes that still need to be checked, and one for nodes already checked.
            var openNodes = new List<TileNode>();
            var closedNodes = new HashSet<TileNode>();
            
            // Start adding to the open set.  First node to check is the starting node.
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                var currentNode = openNodes[0];

                // This loop could be optimised later (Heap?) if necessary.
                for (var i = 0; i < openNodes.Count; i++)
                {
                    // Check for costs of current node
                    if (openNodes[i].FCost < currentNode.FCost ||
                        Math.Abs(openNodes[i].FCost - currentNode.FCost) < Mathf.Epsilon &&
                        openNodes[i].hCost < currentNode.hCost)
                    {
                        // Assign a new current node.
                        if (!currentNode.Equals(openNodes[i]))
                        {
                            currentNode = openNodes[i];
                        }
                    }
                }
                
                // Add current node to checked list and remove from already checked list.
                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                // If current node is the end node retrace and return path.
                if (currentNode.Equals(endNode))
                {
                    foundPath = RetracePath(startNode, currentNode);
                    break;
                }
                
                // If the current node is not the end node then start checking neighbours nodes.
                foreach (var neighbourNode in GetNeighbours(currentNode, true))
                {
                    if (!closedNodes.Contains(neighbourNode))
                    {
                        // Create new movement cost for neighbour node.
                        var newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourNode);

                        // If cost is lower than the neighbour's cost.
                        if (newMovementCostToNeighbour < neighbourNode.gCost || !openNodes.Contains(neighbourNode))
                        {
                            // Calculate the new costs.
                            neighbourNode.gCost = newMovementCostToNeighbour;
                            neighbourNode.hCost = GetDistance(neighbourNode, endNode);

                            // Assign the parent node.
                            neighbourNode.parentNode = currentNode;

                            // Add the neighbour node to the open set.
                            if (!openNodes.Contains(neighbourNode))
                            {
                                openNodes.Add(neighbourNode);
                            }
                        }
                    }
                }
            }
            return foundPath;
        }

        private List<TileNode> RetracePath(TileNode startNode, TileNode endNode)
        {
            // Retrace the path from the end node to the start node.
            var path = new List<TileNode>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                // Going backwards by getting the parent nodes.
                currentNode = currentNode.parentNode;
            }
            
            // Reverse list so now path is starting at the start node.
            path.Reverse();
            
            return path;
        }

        private List<TileNode> GetNeighbours(TileNode node, bool getVerticalNeighbours = false)
        {
            // Start adding neighbours.
            var retList = new List<TileNode>();

            for (var x = -1; x <= 1; x++)
            {
                for (var yIndex = 0; yIndex <= 0; yIndex++) // Use -1 Character jump limit here for index numbers.
                {
                    for (var z = -1; z <= 1; z++)
                    {
                        var y = yIndex;
                        
                        // If there's no 3D pathfinding.
                        if (!getVerticalNeighbours)
                        {
                            yIndex = 0;
                        }

                        // Remove Diagonal Movement.
                        if (x == 0 && y == 0 && z == 0 || 
                            x == 1 && z == 1 || 
                            x == -1 && z == 1 || 
                            x == 1 && z == -1 || 
                            x == -1 && z == -1)
                        {
                            continue;
                        }
                        
                        var searchPosition = new TileNode();

                        searchPosition.x = node.x + x;
                        searchPosition.y = node.y + y;
                        searchPosition.z = node.z + z;

                        var newNode = GetNeighbourNode(searchPosition, true); // node could be used here.

                        if (newNode != null)
                        {
                            retList.Add(newNode);
                        }
                    }
                }
            }
            
            return retList;
        }

        private TileNode GetNeighbourNode(TileNode adjacentPosition, bool searchTopDown) // TileNode currentNode could be sued here.
        {
            // This is the core of the A* algorithm.
            // Can add all the checks here to tweak the algorithm until the desired result is achieved.

            TileNode returnNode = null;

            // Get the node adjacent to this adjacent position.
            var node = _gridMap.GetTileNode(adjacentPosition.x, adjacentPosition.y, adjacentPosition.z);

            // If it's not null and is walkable we can return it.
            if (node != null && node.passThrough == TilePassThrough.WALKABLE)
            {
                returnNode = node;
            }
            else if (searchTopDown) // If 3D search is active.
            {
                // If the 2d adjacent nodes are not usable.
                adjacentPosition.y -= 1;
                var bottomNode = _gridMap.GetTileNode(adjacentPosition.x, adjacentPosition.y, adjacentPosition.z);
                
                // If there is a bottom node and is passable.
                if (bottomNode != null && bottomNode.passThrough == TilePassThrough.WALKABLE)
                {
                    returnNode = bottomNode;
                }
                else //  If there is no bottom node or it's not passable check top node.
                {
                    adjacentPosition.y += 2;
                    var topNode = _gridMap.GetTileNode(adjacentPosition.x, adjacentPosition.y, adjacentPosition.z);
                    
                    if (topNode != null && topNode.passThrough == TilePassThrough.WALKABLE)
                    {
                        returnNode = topNode;
                    }
                }
            }
            
            // Could use current nodes here to check for diagonal movement if needed.
            
            // Could add more checks here for specific case scenarios.
            
            return returnNode;
        }

        private float GetDistance(TileNode currentNode, TileNode neighbourNode)
        {
            // Find the distance between each node.

            var x = Mathf.Abs(currentNode.x - neighbourNode.x);
            var y = Mathf.Abs(currentNode.y - neighbourNode.y);
            var z = Mathf.Abs(currentNode.z - neighbourNode.z);

            // There are some questions here....
            if (x > z)
            {
                return 14 * z + 10 * (x - z) + 10 * y;
            }
            
            return 14 * x + 10 * (z - x) + 10 * y;
        }
    }
}