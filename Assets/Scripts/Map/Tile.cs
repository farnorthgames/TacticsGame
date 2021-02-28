using System;
using TMPro;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class Tile : MonoBehaviour
    {
        [SerializeField] public TileNode tileNode;
    }
}