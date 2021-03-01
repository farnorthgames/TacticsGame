using System;
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

        public Renderer renderer;

        public int moveValue;

        public TileTerrain terrainType;
    
        public TilePassThrough passThrough;
        
        public TileNode()
        {
            x = 0;
            y = 0;
            z = 0;
            
            hCost = 0;
            gCost = 0;
            
            parentNode = null;
            worldObject = null;
            renderer = null;
            
        }

        public void SetTileElement(TileTerrain type)
        {
            switch (type)
            {
                case TileTerrain.GRASS:
                    terrainType = TileTerrain.GRASS;
                    moveValue = 1;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.DIRT:
                    terrainType = TileTerrain.DIRT;
                    moveValue = 1;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.STONE:
                    terrainType = TileTerrain.STONE;
                    moveValue = 1;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.WOOD:
                    terrainType = TileTerrain.WOOD;
                    moveValue = 1;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.SAND:
                    terrainType = TileTerrain.SAND;
                    moveValue = 1;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.WATER:
                    terrainType = TileTerrain.WATER;
                    moveValue = 2;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.SWAMP:
                    terrainType = TileTerrain.SWAMP;
                    moveValue = 2;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.QUICKSAND:
                    terrainType = TileTerrain.QUICKSAND;
                    moveValue = 4;
                    passThrough = TilePassThrough.WALKABLE;
                    break;
                case TileTerrain.DEEP_WATER:
                    terrainType = TileTerrain.DEEP_WATER;
                    moveValue = 0;
                    passThrough = TilePassThrough.NON_WALKABLE;
                    break;
                case TileTerrain.DEEP_SWAMP:
                    terrainType = TileTerrain.DEEP_SWAMP;
                    moveValue = 0;
                    passThrough = TilePassThrough.NON_WALKABLE;
                    break;
                case TileTerrain.LAVA:
                    terrainType = TileTerrain.LAVA;
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
                   $"{nameof(renderer)}: {renderer}, " +
                   $"{nameof(moveValue)}: {moveValue}, " +
                   $"{nameof(terrainType)}: {terrainType}, " +
                   $"{nameof(passThrough)}: {passThrough}, " +
                   $"{nameof(FCost)}: {FCost}";
        }
    }
}