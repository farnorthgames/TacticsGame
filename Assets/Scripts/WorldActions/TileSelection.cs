using Map;
using UIActions;
using UnityEngine;

namespace WorldActions
{
    public class TileSelection : MonoBehaviour
    {
        private GridMap _gridMap;

        private Ray _ray;
        private Camera _mainCamera;
        private TileNode _node;
        private TilePanel _tilePanel;

        public bool wasActive;
        private void Start()
        {
            _gridMap = GridMap.Instance;
            _tilePanel = TilePanel.Instance;
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(_ray, out var hit))
            {
                if (!wasActive)
                   _node?.worldObject.SetActive(false);

                _node = null;
                _tilePanel.UpdateTilePanel(_node);
                return;
            }

            if (!wasActive) // If not already active then deactivate.
            {
                _node?.worldObject.SetActive(false);
            }

            _node = _gridMap.GetNodeFromVector(hit.point);
            _tilePanel.UpdateTilePanel(_node);
            
            if (_node == null)
                return;

            wasActive = _node.worldObject.activeSelf;

            _node.worldObject.SetActive(true);
        }

        public TileNode GetSelectedNode()
        {
            wasActive = true;
            return _node;
        }
    }
}