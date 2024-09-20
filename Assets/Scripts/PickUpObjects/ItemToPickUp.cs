using InteractableObjects.Base;
using Managers;
using PickUpObjects.Scriptables;
using ServiceLocator;
using UnityEngine;

namespace Objects.Room.PickUpObjects
{
	public enum PickUpType
	{
		Chokolade,
	}
	
	[RequireComponent(typeof(SpriteRenderer))]
	public class ItemToPickUp : MonoBehaviour, IPickupable, IInteractable
	{
		[SerializeField] private PickUpType type;
		[SerializeField] private PickUpItemData pickUpItemData;

		private TooltipManager tooltipManager;
		private InventoryManager inventoryManager;
		private SpriteRenderer spriteRenderer;

		private string playerTag;
		private bool canInteract;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			tooltipManager = ServiceManager.Instance.GetManager<TooltipManager>();
			inventoryManager = ServiceManager.Instance.GetManager<InventoryManager>();
			playerTag = ServiceManager.Instance.GetManager<PlayerManager>().PlayerTag;
		}

		private void Update()
		{
			if (canInteract && Input.GetKeyDown(KeyCode.E))
			{
				PickUp();
			}
		}

		public void PickUp()
		{
			inventoryManager.AddItem(type, MemberwiseClone() as IPickupable);
			//for now, it will be destroyable, I think here should be a bug :))
			Destroy(gameObject);
		}

		public void Interact()
		{
			PickUp();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(playerTag))
			{
				canInteract = true;
				tooltipManager.ShowUseTooltip(transform, other.transform, spriteRenderer.bounds.size.magnitude);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(playerTag))
			{
				canInteract = false;
				tooltipManager.HideUseTooltip();
			}
		}
	}
}