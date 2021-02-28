using System.Linq;
using Map;
using UnityEngine;
using UnityEditor;
using static Map.TileProperties;

// ReSharper disable once CheckNamespace
namespace EditorScripts
{
    public class GridWorld : Editor
    {
        private static GameObject _obj;

        private static int _worldWidth = 31;
        private static int _worldLength = 31;
        private static float _yOffSet = 0.001f;

        private static Vector3 _objPos;
        private static Vector3 _pos;
        private static Quaternion _rot;

        private static GameObject _cellPrefab;
        private static GameObject _parentObject;

        private static LayerMask _mask;

        private static GridMap _map;

        private static bool _hasHit;

        private static RaycastHit _currentHit;
    
        [MenuItem("TacticsGame/Generate UI Grid")]
        private static void SetupUIGrid()
        {
            // Force update on Collider mesh before creating grid map.
            var ground = GameObject.FindWithTag("Ground");
            var collider = ground.GetComponent<MeshCollider>();
            collider.sharedMesh = ground.GetComponent<MeshFilter>().sharedMesh;
            
            _mask = 1 << LayerMask.NameToLayer("Terrain");
            _cellPrefab = Resources.Load("Prefabs/CellQuad") as GameObject;
            
            if (_cellPrefab == null)
            {
                Debug.Log("Prefab not found - please check the configuration");
                return;
            }
            
            _parentObject = GameObject.FindGameObjectsWithTag("WorldOverlay").FirstOrDefault();
            
            DestroyImmediate(_parentObject);
            
            _parentObject = new GameObject();
            _parentObject.AddComponent<GridMap>();
            _parentObject.transform.position = Vector3.zero;
            _parentObject.name = "WorldOverlay";
            _parentObject.tag = "WorldOverlay";

            _map = _parentObject.GetComponent<GridMap>();

            GenerateUIGrid();
        }

        private static void GenerateUIGrid()
        {
            for (var i = 0; i < _worldWidth; i++)
            {
                for (var j = 0; j < _worldLength; j++)
                {
                    InitializeTileObject(i, j);
                    
                    _hasHit = Physics.Raycast(_objPos, Vector3.down, out _currentHit, int.MaxValue, _mask);

                    if (!_hasHit)
                        continue;

                    CreateTileObject();
                    
                    while (_hasHit)
                    {
                        var newPos = _currentHit.point + new Vector3(0, -_yOffSet, 0);
                        _hasHit = Physics.Raycast(newPos, Vector3.down, out var hit, int.MaxValue, _mask);

                        if (!_hasHit)
                            break;
                        
                        _currentHit = hit;
                        
                        InitializeTileObject(i, j);

                        CreateTileObject();
                    }
                }
            }
        }

        private static void InitializeTileObject(int newX, int newZ)
        {
            _pos.Set(newX, 100f, newZ);
            _obj = Instantiate(_cellPrefab, _pos, _cellPrefab.transform.rotation, _parentObject.transform);
            _obj.tag = "Tile";

            _objPos = _obj.transform.position;
        }

        private static void CreateTileObject()
        {
            _rot = Quaternion.LookRotation(-_currentHit.normal, Vector3.up);
            _obj.transform.rotation = _rot;
                    
            _pos.Set(_objPos.x, _currentHit.point.y + _yOffSet, _objPos.z);
            _obj.transform.position = _pos;
                    
            var scale = Vector3.one;
                    
            if (_obj.transform.localEulerAngles.x > 44.9 && _obj.transform.localEulerAngles.x < 45.1)
            {
                scale.y = 1.44f;
                _obj.transform.localScale = scale;
            }
            
            // FindTileType(); // <- Find Type type here.
                    
            var tile = _obj.GetComponent<Tile>().tileNode;

            tile.x = (int)_pos.x;
            tile.y = (int)(_pos.y);
            tile.z = (int)_pos.z;

            // tile.hCost = 1;
            
            tile.worldObject = _obj;
            tile.terrainType = TileTerrain.GRASS;

            _obj.name = $"[W: {tile.x}] [H: {tile.y}] [L: {tile.z}]";
        }

        private static void FindTileType()
        {
            // Physics.Raycast(_pos, Vector3.down, out var hit, int.MaxValue, _mask);

            // Need to find type type here. This will not be as simple as I thought due to multiple textures 
            // fading together on one object.  I.E Could be two active textures on the same spot.  Textures don't
            // seem to be accessible from the material via a raycast hit.
        }
    }
}