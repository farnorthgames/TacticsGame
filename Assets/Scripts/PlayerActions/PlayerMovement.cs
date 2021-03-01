using System;
using System.Collections.Generic;
using Map;
using UnityEngine;
using WorldActions;

namespace PlayerActions
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum PlayerState
        {
            STANDING,
            MOVING,
            JUMPING,
            PRONE
        }
        
        public PlayerMovement Instance { get; private set; }

        [SerializeField] public static PlayerState _playerState { get; private set; }

        [SerializeField] private GridMap _gridMap;

        [SerializeField] private GameObject _playerModel;
        
        [SerializeField] private TileSelection _tileSelection;

        [SerializeField] private TileNode _currentTileNode;

        [SerializeField] private List<TileNode> _currentPath;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _gridMap = GridMap.Instance;
            _playerModel = GetComponentInChildren<Transform>().gameObject;
            _currentTileNode = _gridMap.GetNodeFromVector(transform.position);
            _tileSelection = GetComponent<TileSelection>();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && _playerState == PlayerState.STANDING)
            {
                StartPlayerMove();
            }
            
            if (_playerState == PlayerState.MOVING)
            {
                MovePlayer();
            }
        }

        private void StartPlayerMove()
        {
            var node = _tileSelection.GetSelectedNode();
            
            if (node == null)
                return;

            _gridMap.EndPosition = new Vector3(node.x, node.y, node.z);
            _currentPath = _gridMap.StartPathfinding();

            if (_currentPath.Count < 1)
                _tileSelection.wasActive = false;

            // _playerState = PlayerState.MOVING;
        }

        private void MovePlayer()
        {
            
        }

        private void EndPlayerMovement()
        {
            _currentTileNode = _gridMap.GetNodeFromVector(transform.position);
        }
    }  
}