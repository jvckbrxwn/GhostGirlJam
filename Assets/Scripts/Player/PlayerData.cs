using UnityEngine;

namespace Player
{
	public class PlayerData : MonoBehaviour
	{
		[field: SerializeReference] public string Tag => gameObject.tag;
		[field: SerializeReference] public bool MovementState { get; set; } = true;
	}
}