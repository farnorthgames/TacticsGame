using Map;
using TMPro;
using UnityEngine;

namespace UIActions
{
	public class TilePanel : MonoBehaviour
	{
		public static TilePanel Instance { get; private set; }

		[SerializeField] private TMP_Text _tileElementType;
		
		[SerializeField] private TMP_Text _tileType;

		private string _nullString = "---"; 

		private void Awake()
		{
			Instance = this;
		}

		public void UpdateTilePanel(TileNode node)
		{
			if (node == null)
			{
				_tileElementType.text = _nullString;
				_tileType.text = _nullString;
				return;
			}
			
			_tileElementType.text = node.terrainType.ToString().ToUpper();
			_tileType.text = node.passThrough.ToString().ToUpper();
			
			// Debug.Log(node.terrainType.ToString().ToUpper());
			// Debug.Log(node.passThrough.ToString().ToUpper());
		}
	}
}