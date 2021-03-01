using UnityEngine;

namespace TurnActions
{
	public class TurnManager : MonoBehaviour
	{
		public static TurnManager Instance { get; private set; }
		
		private void Awake()
		{
			Instance = this;
		}
	}
}