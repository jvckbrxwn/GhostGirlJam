using InteractableObjects;
using InteractableObjects.Base;
using Player;
using Player.Movement;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;

namespace Managers
{
	public class PlayerManager : MonoBehaviour, IManager
	{
		[SerializeField] private PlayerData playerData;
		[SerializeField] private PlayerMovement playerMovement;

		public string PlayerTag => playerData.Tag;

		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		private void Start()
		{
			Init();
		}

		public void Init()
		{ }

		public void InteractWith(IInteractable component)
		{
			component.Interact();
		}

		public void SetMovementState(bool state)
		{
			playerMovement.SetMovementState(state);
		}

		public void TranslateTo(DoorComponent door)
		{
			TranslateTo(door.transform.position);
			door.Room.ChangeRoom();
		}

		private void TranslateTo(Vector3 position)
		{
			playerMovement.gameObject.transform.position = position;
		}
	}
}