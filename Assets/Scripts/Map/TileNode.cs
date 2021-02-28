using System;
using JetBrains.Annotations;
using UnityEngine;
using static Map.TileProperties;

namespace Map
{
    [Serializable]
    public class TileNode
    {
        public int x, y, z;

        public float hCost;

        public float gCost;
        public float FCost => gCost + hCost;

        public TileNode parentNode;

        public GameObject worldObject;

        public int moveValue;
    
        public TileTerrain? terrainType;
    
        public TilePassThrough passThrough;
        
        public TileNode(TileTerrain? terrainType)
        {
            x = 0;
            y = 0;
            z = 0;
            
            hCost = 0;
            gCost = 0;
            
            parentNode = null;
            worldObject = null;

            if (terrainType == null)
                terrainType = TileTerrain.GRASS;   
            
            this.terrainType = terrainType;  
            
            switch (terrainType)
            {
                case TileTerrain.GRASS: 
                case TileTerrain.DIRT:
                case TileTerrain.STONE:
                case TileTerrain.WOOD:
                case TileTerrain.SAND:
                    moveValue = 1;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.WATER:
                case TileTerrain.SWAMP:
                    moveValue = 2;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.QUICKSAND:
                    moveValue = 4;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.DEEP_WATER:
                case TileTerrain.DEEP_SWAMP:
                case TileTerrain.LAVA:
                    moveValue = 0;
                    passThrough = TilePassThrough.NON_WALKABLE;
                    break;
                default:
                    throw new Exception($"No Terrain Type is Set.");
            }
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, " +
                   $"{nameof(y)}: {y}, " +
                   $"{nameof(z)}: {z}, " +
                   $"{nameof(hCost)}: {hCost}, " +
                   $"{nameof(gCost)}: {gCost}, " +
                   $"{nameof(parentNode)}: {parentNode}, " +
                   $"{nameof(worldObject)}: {worldObject}, " +
                   $"{nameof(moveValue)}: {moveValue}, " +
                   $"{nameof(terrainType)}: {terrainType}, " +
                   $"{nameof(passThrough)}: {passThrough}, " +
                   $"{nameof(FCost)}: {FCost}";
        }
    }
}