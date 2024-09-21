using Managers;
using Player;
using ServiceLocator;
using UnityEngine;

namespace Objects.Room.HiddenObjects
{
	public class HiddenObjectController : MonoBehaviour
	{
		private PlayerManager playerManager;
		
		private void Start()
		{
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
			Init();
		}

		private void Init()
		{
			gameObject.SetActive(playerManager.CurrentState == PlayerStateType.Ghost);
			playerManager.StatesController.PlayerStateChanged += OnPlayerStateChanged;
		}

		private void OnPlayerStateChanged(PlayerStateType state)
		{
			gameObject.SetActive(state == PlayerStateType.Ghost);
		}
	}
}