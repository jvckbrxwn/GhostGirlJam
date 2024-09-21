using UnityEngine;

namespace PickUpObjects.Scriptables
{
	[CreateAssetMenu(fileName = "Name?", menuName = "Pick Up Item/Item", order = 1)]
	public class PickUpItemData : ScriptableObject
	{
		[SerializeField] private new string name;
		[SerializeField] private Sprite sprite;
		[SerializeField] private string hint;

		public string Name => name;
		public Sprite Sprite => sprite;
		public string Hint => hint;
	}
}