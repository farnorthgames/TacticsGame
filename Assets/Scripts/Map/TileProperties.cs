
namespace Map
{
    public static class TileProperties
    {
        public enum TileTerrain
        {
            GRASS,
            DIRT,
            QUICKSAND,
            SAND,
            STONE,
            WOOD,
            WATER,
            DEEP_WATER,
            SWAMP,
            DEEP_SWAMP,
            LAVA,
        }
        
        public enum TilePassThrough
        {
            WALKABLE,
            NON_WALKABLE
        }
    }
}