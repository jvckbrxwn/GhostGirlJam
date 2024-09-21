using System;
using Managers;
using Player;
using ServiceLocator;
using UnityEngine;

namespace Objects.Room.HiddenObjects
{
	[Serializable]
	public struct HiddenObjectData
	{
		public int index;
		public string passwordPart;
	}

	public class HiddenObjectController : MonoBehaviour
	{
		[SerializeField] private HiddenObjectData hiddenObjectData;

		private PlayerManager playerManager;
		private InventoryManager inventoryManager;

		private void Start()
		{
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
			inventoryManager = ServiceManager.Instance.GetManager<InventoryManager>();
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

		private void OnBecameVisible()
		{
			inventoryManager.AddPasswordPart(hiddenObjectData.index, hiddenObjectData.passwordPart);
		}
	}
}