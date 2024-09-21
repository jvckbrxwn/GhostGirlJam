using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using InteractableObjects.Base;
using Managers;
using Objects.Room;
using Objects.Room.PickUpObjects;
using ServiceLocator;
using UnityEngine;

namespace InteractableObjects
{
	public class DoorComponent : MonoBehaviour, IInteractable
	{
		[SerializeField] private DoorComponent connectedDoor;
		[SerializeField] private List<PickUpType> requireItemsToOpen;

		private RoomComponent roomComponent;
		private PlayerManager playerManager;
		private TransitionManager transitionManager;
		private InventoryManager inventoryManager;
		private TooltipManager tooltipManager;
		private SpriteRenderer spriteRenderer;

		private bool canInteract = false;
		private bool fullyOpen = true;

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

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
			transitionManager = ServiceManager.Instance.GetManager<TransitionManager>();
			tooltipManager = ServiceManager.Instance.GetManager<TooltipManager>();
			inventoryManager = ServiceManager.Instance.GetManager<InventoryManager>();

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
			if (!requireItemsToOpen.All(item => inventoryManager.HasItem(item)))
			{
				Debug.Log("Show tooltip that door is closed");
				return;
			}

			if (connectedDoor is null)
			{
				Debug.Assert(connectedDoor is null, "connectedDoor is null", this);
				return;
			}

			transitionManager.MoveTo(connectedDoor).Forget();
			fullyOpen = true;
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
				tooltipManager.ShowUseTooltip(transform, other.transform, spriteRenderer.bounds.size.magnitude / 2f);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(playerManager.PlayerTag))
			{
				canInteract = false;
				tooltipManager.HideUseTooltip();
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