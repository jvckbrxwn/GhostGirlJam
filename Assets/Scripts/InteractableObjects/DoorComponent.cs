using System;
using Cysharp.Threading.Tasks;
using InteractableObjects.Base;
using Managers;
using Objects.Room;
using ServiceLocator;
using UnityEngine;

namespace InteractableObjects
{
	public class DoorComponent : MonoBehaviour, IInteractable
	{
		[SerializeField] private DoorComponent connectedDoor;

		private RoomComponent roomComponent;
		private PlayerManager playerManager;
		private TransitionManager transitionManager;
		private bool canInteract = false;

		public RoomComponent Room => roomComponent;

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			if (connectedDoor is not null)
			{
				// Draws a blue line from this transform to the target
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(transform.position, connectedDoor.transform.position);
			}
		}
#endif

		private void Start()
		{
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
			transitionManager = ServiceManager.Instance.GetManager<TransitionManager>();

			transitionManager.BeforeInteract += OnBeforeInteract;
			transitionManager.AfterInteract += OnAfterInteract;
		}

		private void Update()
		{
			if (canInteract && Input.GetKeyDown(KeyCode.E))
			{
				playerManager.InteractWith(this);
			}
		}

		public void Interact()
		{
			if (connectedDoor is null)
			{
				Debug.Assert(connectedDoor is null, "connectedDoor is null", this);
				return;
			}

			transitionManager.MoveTo(connectedDoor).Forget();
		}

		public void SetRoomComponent(RoomComponent room)
		{
			roomComponent = room;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(playerManager.PlayerTag))
			{
				canInteract = true;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(playerManager.PlayerTag))
			{
				canInteract = false;
			}
		}

		private void OnBeforeInteract()
		{
			playerManager.SetMovementState(false);
		}

		private void OnAfterInteract()
		{
			playerManager.SetMovementState(true);
		}
	}
}