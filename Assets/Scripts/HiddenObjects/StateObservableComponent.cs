using Managers;
using Player;
using ServiceLocator;
using UnityEngine;

namespace Objects.Room.HiddenObjects
{
	//dummy observable without different realizations
	public class StateObservableComponent : MonoBehaviour
	{
		[HideInInspector, SerializeField] private new Collider2D collider2D;

		private PlayerManager playerManager;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (collider2D is null)
			{
				collider2D = GetComponent<Collider2D>();
			}
		}
#endif

		private void Start()
		{
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
			playerManager.StatesController.PlayerStateChanged += StatesControllerOnPlayerStateChanged;
		}

		private void StatesControllerOnPlayerStateChanged(PlayerStateType state)
		{
			collider2D.enabled = state == PlayerStateType.Girl;
		}
	}
}