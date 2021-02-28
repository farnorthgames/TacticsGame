using Map;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    private Color _initColour;
    private void Start()
    {
        _initColour = GetComponent<Renderer>().material.color;
    }

    private void OnMouseEnter()
    {
        _initColour = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.blue;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = _initColour;
    }

    private void OnMouseUp()
    {
        var map = GridMap.Instance;
        var tile = GetComponent<Tile>().tileNode;
        map.EndPosition = new Vector3(tile.x, tile.y, tile.z);
        map.StartPathfinding();
    }
}
